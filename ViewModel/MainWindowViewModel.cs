﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MvvmDialogs.ViewModels;
using RedCell.Diagnostics.Update;
using RFiDGear.DataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace RFiDGear.ViewModel
{
	/// <summary>
	/// Description of MainWindowViewModel.
	/// </summary>
	public class MainWindowViewModel : ViewModelBase
	{
		private ReaderSetupModel readerSetup;
		private DatabaseReaderWriter databaseReaderWriter;

		private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
		public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }
		
		private SourceForMifareDataBlocks currentBlock;
		private ObservableCollection<SourceForMifareDataBlocks> DataSourceForMifareClassicDataBlock = new ObservableCollection<SourceForMifareDataBlocks>();
		private ObservableCollection<object> gridSource;
		private ObservableCollection<TreeViewParentNodeViewModel> _uids = new ObservableCollection<TreeViewParentNodeViewModel>();
		
		private List<Model.MifareClassicUidModel> mifareClassicUidModels = new List<RFiDGear.Model.MifareClassicUidModel>();
		private List<Model.MifareDesfireUidModel> mifareDesfireViewModels = new List<RFiDGear.Model.MifareDesfireUidModel>();
		
		private Updater updater = new Updater();
		
		public MainWindowViewModel()
		{
			readerSetup = new ReaderSetupModel(null);
			databaseReaderWriter = new DatabaseReaderWriter();

			Messenger.Default.Register<NotificationMessage<string>>(
				this, nm => {
					// Processing the Message
					switch (nm.Notification) {
						case "ReadAllSectors":
							ReadSectorsWithDefaultConfig((TreeViewParentNodeViewModel)nm.Sender);
							break;
						case "DeleteMe":
							databaseReaderWriter.databaseUIDs.Remove(nm.Content);
							_uids.Remove((TreeViewParentNodeViewModel)nm.Sender);
							break;
						case "EditAuthInfoAndReadAllSectors":
							NewSectorTrailerEditDialog((TreeViewParentNodeViewModel)nm.Sender);
							break;
						case "EditAuthAndModifySector":
							NewSectorTrailerEditDialog((TreeViewChildNodeViewModel)nm.Sender);
							break;
						case "ReadSectorWithDefaults":
							break;
					}
				});
			
			Messenger.Default.Register<TreeViewChildNodeViewModel>(this, NewSectorTrailerEditDialog);
			Messenger.Default.Register<TreeViewGrandChildNodeViewModel>(this, ShowDataBlockInDataGrid);
			
			//updater.StartMonitoring();
			//any dialog boxes added in the constructor won't appear until DialogBehavior.DialogViewModels gets bound to the Dialogs collection.
		}

		private void ReadSectorsWithDefaultConfig(TreeViewParentNodeViewModel selectedPnVM)
		{
			
			Mouse.OverrideCursor = Cursors.Wait;
			
			TreeViewParentNodeViewModel vm = selectedPnVM;
			
			foreach (TreeViewParentNodeViewModel treeViewPnVM in _uids) {
				treeViewPnVM.IsExpanded = false;
				if (treeViewPnVM.UidNumber == selectedPnVM.UidNumber) {
					selectedPnVM.IsExpanded = true;
					foreach (TreeViewChildNodeViewModel cnVM in treeViewPnVM.Children) {
						readerSetup.readMiFareClassicSingleSector(cnVM.SectorNumber, cnVM.SectorNumber);
						cnVM.IsAuthenticated = readerSetup.sectorSuccesfullyAuth;
						foreach (TreeViewGrandChildNodeViewModel gcVM in cnVM.Children) {
							gcVM.IsAuthenticated = readerSetup.dataBlockSuccesfullyAuth[(((cnVM.SectorNumber + 1) * cnVM.BlockCount) - (cnVM.BlockCount - gcVM.DataBlockNumber))];
							gcVM.DataBlockContent = readerSetup.currentSector[gcVM.DataBlockNumber];
						}
					}
				}
			}
			
			Mouse.OverrideCursor = Cursors.Arrow;
		}
		
		private void ShowDataBlockInDataGrid(TreeViewGrandChildNodeViewModel selectedGCN)
		{
			DataSourceForMifareClassicDataBlock.Clear();
			for (int i = 0; i < selectedGCN.DataBlockContent.Length; i++) {
				DataSourceForMifareClassicDataBlock.Add(new SourceForMifareDataBlocks(selectedGCN.DataBlockContent, i));
			}
			gridSource = new ObservableCollection<object>(DataSourceForMifareClassicDataBlock);
			RaisePropertyChanged("DataGridSource");
		}
		
		public ICommand SectorTrailerEditDialogCommand { get { return new RelayCommand(GetTreeViewSelection); } }
		void GetTreeViewSelection(){
			
		}
		
		public void NewSectorTrailerEditDialog(TreeViewChildNodeViewModel sectorVM)
		{
			this.Dialogs.Add(new MifareAuthSettingsDialogViewModel() {
			                 	
			                 	Caption = ResourceLoader.getResource("messageBoxRestartRequiredCaption"),

			                 	OnOk = (sender) => {
			                 		sender.Close();
			                 	},

			                 	OnCancel = (sender) => {
			                 		sender.Close();

			                 	},

			                 	OnCloseRequest = (sender) => {
			                 		sender.Close();
			                 	}
			                 });
		}

		public void NewSectorTrailerEditDialog(TreeViewParentNodeViewModel sectorVM)
		{
			bool isClassicCard;
			
			if(sectorVM.CardType == CARD_TYPE.CT_CLASSIC_1K || sectorVM.CardType == CARD_TYPE.CT_CLASSIC_2K || sectorVM.CardType == CARD_TYPE.CT_CLASSIC_4K)
				isClassicCard = true;
			else
				isClassicCard = false;
			
			this.Dialogs.Add(new MifareAuthSettingsDialogViewModel() {
			                 	
			                 	Caption = ResourceLoader.getResource("messageBoxRestartRequiredCaption"),
			                 	IsClassicAuthInfoEnabled = isClassicCard,

			                 	OnOk = (sender) => {
			                 		sender.Close();
			                 	},

			                 	OnCancel = (sender) => {
			                 		sender.Close();

			                 	},

			                 	OnCloseRequest = (sender) => {
			                 		sender.Close();
			                 	}
			                 });
		}
		
		#region Resource getters
		
		public string MenuItem_FileHeader {
			get { return ResourceLoader.getResource("menuItemFileHeader"); }
		}
		
		public string MenuItem_ExitHeader {
			get { return ResourceLoader.getResource("menuItemExitHeader"); }
		}
		
		public string MenuItem_EditHeader {
			get { return ResourceLoader.getResource("menuItemEditHeader"); }
		}
		
		public string MenuItem_OptionsHeader {
			get { return ResourceLoader.getResource("menuItemOptionsHeader"); }
		}
		
		public string MenuItem_ReaderSettingsHeader {
			get { return ResourceLoader.getResource("menuItemReaderSettingsHeader"); }
		}
		#endregion
		
		#region Commands
		public ICommand ReadChipCommand { get { return new RelayCommand(OnNewReadChipCommand); } }
		public void OnNewReadChipCommand()
		{
			if (!String.IsNullOrEmpty(new ReaderSetupModel(null).GetChipUID)) {
				if (new ReaderSetupModel(null).SelectedReader != "N/A") {
					if (!databaseReaderWriter.databaseUIDs.Contains(new ReaderSetupModel(null).GetChipUID)) {
						databaseReaderWriter.databaseUIDs.Add(new ReaderSetupModel(null).GetChipUID);
						RaisePropertyChanged("TreeViewParentNode");
					}
				}
			} else {
				if (new ReaderSetupModel(null).SelectedReader != "N/A") {
					if (!databaseReaderWriter.databaseUIDs.Contains(new ReaderSetupModel(null).GetChipUID)) {
						databaseReaderWriter.databaseUIDs.Add(new ReaderSetupModel(null).GetChipUID);
						RaisePropertyChanged("TreeViewParentNode");
					}
				}
			}
		}
		
		public ICommand CloseAllCommand { get { return new RelayCommand(OnCloseAll); } }
		public void OnCloseAll()
		{
			this.Dialogs.Clear();
		}

		void DeleteNode()
		{


		}
		
		public ICommand SwitchLanguageToGerman { get { return new RelayCommand(SetGermanLanguage); } }
		public void SetGermanLanguage()
		{
			if (ResourceLoader.getLanguage() != "german") {
				ResourceLoader.setLanguage("german");
				this.OnNewLanguageChangedDialog();
			}

		}
		
		public ICommand SwitchLanguageToEnglish { get { return new RelayCommand(SetEnglishLanguage); } }
		public void SetEnglishLanguage()
		{
			if (ResourceLoader.getLanguage() != "english") {
				ResourceLoader.setLanguage("english");
				this.OnNewLanguageChangedDialog();
			}

		}
		
		public void OnNewLanguageChangedDialog()
		{
			this.Dialogs.Add(new CustomDialogViewModel {
			                 	Message = ResourceLoader.getResource("messageBoxRestartRequiredMessage"),
			                 	Caption = ResourceLoader.getResource("messageBoxRestartRequiredCaption"),

			                 	OnOk = (sender) => {
			                 		sender.Close();
			                 		App.Current.Shutdown();
			                 	},

			                 	OnCancel = (sender) => {
			                 		sender.Close();

			                 	},

			                 	OnCloseRequest = (sender) => {
			                 		sender.Close();
			                 	}
			                 });
		}
		
		public ICommand NewReaderSetupDialogCommand { get { return new RelayCommand(OnNewReaderSetupDialog); } }
		public void OnNewReaderSetupDialog()
		{
			this.Dialogs.Add(new ReaderSetupDialogViewModel {
			                 	Caption = "RFiDGear Reader Setup",
			                 	
			                 	OnOk = (sender) => {
			                 		
			                 		//currentReaderSetup = new RFiDReaderSetup(sender.SelectedReader);
			                 		
			                 		sender.Close();
			                 		
			                 	},

			                 	OnCancel = (sender) => {
			                 		sender.Close();
			                 	},

			                 	OnCloseRequest = (sender) => {
			                 		sender.Close();
			                 	}
			                 });
		}
		
		public ICommand NewModalDialogCommand { get { return new RelayCommand(OnNewModalDialog); } }
		public void OnNewModalDialog()
		{
			this.Dialogs.Add(new CustomDialogViewModel {
			                 	Message = "Hello World!",
			                 	Caption = "Modal Dialog Box",

			                 	OnOk = (sender) => {
			                 		sender.Close();
			                 		new MessageBoxViewModel("You pressed ok!", "Bye bye!").Show(this.Dialogs);
			                 	},

			                 	OnCancel = (sender) => {
			                 		sender.Close();
			                 		new MessageBoxViewModel("You pressed cancel!", "Bye bye!").Show(this.Dialogs);
			                 	},

			                 	OnCloseRequest = (sender) => {
			                 		sender.Close();
			                 		new MessageBoxViewModel("You clicked the caption bar close button!", "Bye bye!").Show(this.Dialogs);
			                 	}
			                 });
		}

		public ICommand NewModelessDialogCommand { get { return new RelayCommand(OnNewModelessDialog); } }
		public void OnNewModelessDialog()
		{
			var confirmClose = new Action<CustomDialogViewModel>((sender) => {
			                                                     	if (new MessageBoxViewModel {
			                                                     	    	Caption = "Closing",
			                                                     	    	Message = "Are you sure you want to close this window?",
			                                                     	    	Buttons = MessageBoxButton.YesNo,
			                                                     	    	Image = MessageBoxImage.Question
			                                                     	    }
			                                                     	    .Show(this.Dialogs) == MessageBoxResult.Yes)
			                                                     		sender.Close();
			                                                     });

			new CustomDialogViewModel(false) {
				Message = "Hello World!",
				Caption = "Modeless Dialog Box",
				OnOk = confirmClose,
				OnCancel = confirmClose,
				OnCloseRequest = confirmClose
			}.Show(this.Dialogs);
		}

		public ICommand NewMessageBoxCommand { get { return new RelayCommand(OnNewMessageBox); } }
		public void OnNewMessageBox()
		{
			new MessageBoxViewModel {
				Caption = "Message Box",
				Message = "This is a message box!",
				Image = MessageBoxImage.Information
			}.Show(this.Dialogs);
		}

		public ICommand NewOpenFileDialogCommand { get { return new RelayCommand(OnNewOpenFileDialog); } }
		public void OnNewOpenFileDialog()
		{
			var dlg = new OpenFileDialogViewModel {
				Title = "Select a file (I won't actually do anything with it)",
				Filter = "All files (*.*)|*.*",
				Multiselect = false
			};
			
			if (dlg.Show(this.Dialogs))
				new MessageBoxViewModel { Message = "You selected the following file: " + dlg.FileName + "." }.Show(this.Dialogs);
			else
				new MessageBoxViewModel { Message = "You didn't select a file." }.Show(this.Dialogs);
		}

		public ICommand CloseApplication { get { return new RelayCommand(OnCloseRequest); } }
		public void OnCloseRequest()
		{
			App.Current.Shutdown();
		}
		
		public bool RadioButtonEnglishLanguageSelectedState {
			get {
				if (ResourceLoader.getLanguage() == "german")
					return false;
				else
					return true;
			}
			set {
				if (ResourceLoader.getLanguage() == "german")
					value = false;
				RaisePropertyChanged("RadioButtonEnglishLanguageSelectedState");
			}
		}
		#endregion
		
		public ObservableCollection<object> DataGridSource {
			get { return gridSource; }
		}
		
		public object SelectedDataGridItem {
			get { return currentBlock; }
			set { currentBlock = (SourceForMifareDataBlocks)value;
			}
		}
		public ObservableCollection<TreeViewParentNodeViewModel> TreeViewParentNode {
			get {
				CustomConverter converter = new CustomConverter();
				if (!String.IsNullOrEmpty(readerSetup.GetChipUID)) {
					
					if (!databaseReaderWriter.databaseUIDs.Contains(readerSetup.GetChipUID))
					{
						databaseReaderWriter.databaseUIDs.Add(readerSetup.GetChipUID);
					}
					
					switch (readerSetup.GetChipType) {
						case "Mifare1K":
							_uids.Add(new TreeViewParentNodeViewModel(new Model.MifareClassicUidModel(readerSetup.GetChipUID, CARD_TYPE.CT_CLASSIC_1K), CARD_TYPE.CT_CLASSIC_1K));
							break;
							
						case "Mifare2K":
							_uids.Add(new TreeViewParentNodeViewModel(new Model.MifareClassicUidModel(readerSetup.GetChipUID, CARD_TYPE.CT_CLASSIC_2K), CARD_TYPE.CT_CLASSIC_2K));
							break;
							
						case "Mifare4K":
							_uids.Add(new TreeViewParentNodeViewModel(new Model.MifareClassicUidModel(readerSetup.GetChipUID, CARD_TYPE.CT_CLASSIC_4K), CARD_TYPE.CT_CLASSIC_4K));
							break;
							
						case "DESFireEV1":
							_uids.Add(new TreeViewParentNodeViewModel(new Model.MifareDesfireUidModel(readerSetup.GetChipUID), CARD_TYPE.CT_DESFIRE_EV1));
							break;
					}
					
					foreach(TreeViewParentNodeViewModel pnVM in _uids)
					{
						databaseReaderWriter.WriteDatabase(pnVM.mifareClassicUidModel);
					}
					return _uids;
				}
				return null;
			}
		}
		
		public bool RadioButtonGermanLanguageSelectedState {
			get {
				if (ResourceLoader.getLanguage() == "german")
					return true;
				else
					return false;
			}
			set {
				if (ResourceLoader.getLanguage() == "english")
					value = false;
				RaisePropertyChanged("RadioButtonGermanLanguageSelectedState");
			}
		}

	}
}
