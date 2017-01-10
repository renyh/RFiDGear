﻿using GalaSoft.MvvmLight;
using MvvmDialogs.ViewModels;
using RFiDGear.DataSource;
using System;
using System.Security;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace RFiDGear.ViewModel
{
	/// <summary>
	/// Description of KeySettingsMifareClassicDialogViewModel.
	/// </summary>
	public class MifareAuthSettingsDialogViewModel : ViewModelBase, IUserDialogViewModel
	{
		private DatabaseReaderWriter databaseReaderWriter;
		private object selectedTreeViewViewModel;
		
		private readonly string[] cmbbxItems = {
			"DataBlock 0",
			"DataBlock 1",
			"DataBlock 2",
			"All DataBlocks"
		};
		
		private readonly string[] keyCmbbxItems = {
			"No. 0",
			"No. 1",
			"No. 2",
			"No. 3",
			"No. 4",
			"No. 5",
			"No. 6",
			"No. 7",
			"No. 8",
			"No. 9",
			"No. 10",
			"No. 11",
			"No. 12",
			"No. 13",
			"No. 14",
			"No. 15"
		};
		
		private int selectionIndex = 3;
		
		private bool isClassicAuthInfo = true;
		
		private readonly ObservableCollection<MifareClassicAccessBitsSectorTrailerDataGridModel> displaySourceForSectorTrailerDataGrid;
		private readonly ObservableCollection<MifareClassicAccessBitsLongDataBlockDataGridModel> displaySourceForLongDataBlockDataGrid;
		private readonly ObservableCollection<MifareClassicAccessBitsShortDataBlockDataGridModel> displaySourceForShortDataBlockDataGrid;
		
		private ObservableCollection<object> displaySourceForCombinedDataBlockDataGrid;
		
		private MifareClassicAccessBitsSectorTrailerDataGridModel _selectedSectorTrailerAccessBitsItem;
		private object selectedCombinedDataBlockAccessBitsDataGridItem;
		private object selectedDataBlock0AccessBitsDataGridItem;
		private object selectedDataBlock1AccessBitsDataGridItem;
		private object selectedDataBlock2AccessBitsDataGridItem;
		
		public MifareAuthSettingsDialogViewModel(object treeViewViewModel, bool isModal = true)
		{
			databaseReaderWriter = new DatabaseReaderWriter();
			selectedTreeViewViewModel = treeViewViewModel;
			
			displaySourceForSectorTrailerDataGrid = new ObservableCollection<MifareClassicAccessBitsSectorTrailerDataGridModel>();
			displaySourceForLongDataBlockDataGrid = new ObservableCollection<MifareClassicAccessBitsLongDataBlockDataGridModel>();
			displaySourceForShortDataBlockDataGrid = new ObservableCollection<MifareClassicAccessBitsShortDataBlockDataGridModel>();
			
			foreach (string accessConditions in MifareClassicAccessBitsModel.sectorTrailerAB) {
				displaySourceForSectorTrailerDataGrid.Add(new MifareClassicAccessBitsSectorTrailerDataGridModel(accessConditions));
			}

			foreach (string accessConditions in MifareClassicAccessBitsModel.dataBlockAB) {
				displaySourceForLongDataBlockDataGrid.Add(new MifareClassicAccessBitsLongDataBlockDataGridModel(accessConditions));
			}
			

			foreach (string accessConditions in MifareClassicAccessBitsModel.dataBlockABs) {
				if (!String.IsNullOrEmpty(accessConditions))
					displaySourceForShortDataBlockDataGrid.Add(new MifareClassicAccessBitsShortDataBlockDataGridModel(accessConditions));
			}
			
			// select the default items from Database
			displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
			
			foreach (MifareClassicAccessBitsSectorTrailerDataGridModel item in displaySourceForSectorTrailerDataGrid) {
				if (item.GetSectorAccessBitsFromHumanReadableFormat() ==
				    (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.sectorAccessBits.DecodedSectorTrailerAccessBits) //"N,A,A,A,A,A"
					_selectedSectorTrailerAccessBitsItem = item;
			}
			
			foreach (MifareClassicAccessBitsShortDataBlockDataGridModel item in displaySourceForCombinedDataBlockDataGrid) {
				if (item.GetShortDataBlockAccessBitsFromHumanReadableFormat() ==
				    (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.sectorAccessBits.DecodedDataBlock0AccessBits) //"A,A,A,A"
					selectedCombinedDataBlockAccessBitsDataGridItem = item;
			}
			this.IsModal = isModal;
		}

		#region SelectedItem
		
		public MifareClassicAccessBitsSectorTrailerDataGridModel SelectedSectorTrailerAccessBitsRow {
			get {
				if (_selectedSectorTrailerAccessBitsItem.getReadAccessCond == "Key A or B") {
					displaySourceForCombinedDataBlockDataGrid = null;
					displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForLongDataBlockDataGrid);
					;
				} else {
					displaySourceForCombinedDataBlockDataGrid = null;
					displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
				}
				
				return _selectedSectorTrailerAccessBitsItem;
			}
			
			set {
				_selectedSectorTrailerAccessBitsItem = value;
				if (_selectedSectorTrailerAccessBitsItem.getReadAccessCond == "Key A or B") {
					displaySourceForCombinedDataBlockDataGrid = null;
					displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForLongDataBlockDataGrid);
				} else {
					displaySourceForCombinedDataBlockDataGrid = null;
					displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
				}

				RaisePropertyChanged("DataBlockSource");
			}
		}
		
		public object SelectedDataBlockAccessBitsRow {
			get {
				switch (selectionIndex) {
					case 0:
						return selectedDataBlock0AccessBitsDataGridItem;
					case 1:
						return selectedDataBlock1AccessBitsDataGridItem;
					case 2:
						return selectedDataBlock2AccessBitsDataGridItem;
					case 3:
						return selectedCombinedDataBlockAccessBitsDataGridItem;
				}
				return null;
			}
			set {
				switch (selectionIndex) {
					case 0:
						selectedDataBlock0AccessBitsDataGridItem = value;
						break;
					case 1:
						selectedDataBlock1AccessBitsDataGridItem = value;
						break;
					case 2:
						selectedDataBlock2AccessBitsDataGridItem = value;
						break;
					case 3:
						selectedCombinedDataBlockAccessBitsDataGridItem = value;
						break;
				}
				// TODO build sectorTrailer depended on selectedindex of datablock selection
				string test = selectedCombinedDataBlockAccessBitsDataGridItem.GetType().ToString();
				switch (selectedCombinedDataBlockAccessBitsDataGridItem.GetType().ToString()) {
					case "RFiDGear.DataSource.MifareClassicAccessBitsShortDataBlockDataGridModel":
						string test2 = (selectedCombinedDataBlockAccessBitsDataGridItem as MifareClassicAccessBitsShortDataBlockDataGridModel).GetType().ToString();
						break;
				}
			}
		}


		public string SelectedDataBlockItem {
			get { return cmbbxItems[selectionIndex]; }
			set {
				selectionIndex = Array.IndexOf(cmbbxItems, value);
				RaisePropertyChanged("DataBlockSource");
				RaisePropertyChanged("SelectedDataBlockAccessBitsRow");
			}
		}
		
		#endregion //SelectedItem
		
		#region IUserDialogViewModel Implementation

		public bool IsModal { get; private set; }
		public virtual void RequestClose()
		{
			if (this.OnCloseRequest != null)
				OnCloseRequest(this);
			else
				Close();
		}
		public event EventHandler DialogClosing;

		public ICommand ApplyCommand { get { return new RelayCommand(Ok); } }
		protected virtual void Ok()
		{
			if (this.OnOk != null)
				this.OnOk(this);
			else
				Close();
		}
		
		public ICommand ExitCommand { get { return new RelayCommand(Cancel); } }
		protected virtual void Cancel()
		{
			if (this.OnCancel != null)
				this.OnCancel(this);
			else
				Close();
		}
		
		public Action<MifareAuthSettingsDialogViewModel> OnOk { get; set; }
		public Action<MifareAuthSettingsDialogViewModel> OnCancel { get; set; }
		public Action<MifareAuthSettingsDialogViewModel> OnCloseRequest { get; set; }

		public void Close()
		{
			if (this.DialogClosing != null)
				this.DialogClosing(this, new EventArgs());
		}

		public void Show(IList<IDialogViewModel> collection)
		{
			collection.Add(this);
		}
		
		#endregion IUserDialogViewModel Implementation
		
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler KeySettingsPropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.KeySettingsPropertyChanged != null)
				this.KeySettingsPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion // INotifyPropertyChanged Members
		
		private string _Caption;
		public string Caption {
			get { return _Caption; }
			set {
				_Caption = value;
				RaisePropertyChanged(() => this.Caption);
			}
		}
		
		public ObservableCollection<MifareClassicAccessBitsSectorTrailerDataGridModel> SectorTrailerSource {
			get { return displaySourceForSectorTrailerDataGrid; }
		}
		
		public bool IsClassicAuthInfoEnabled {
			get { return isClassicAuthInfo; }
			set {
				isClassicAuthInfo = value;
				RaisePropertyChanged("IsClassicAuthInfoEnabled");
			}
		}
		
		public bool IsDesfireAuthInfoEnabled {
			get { return !isClassicAuthInfo; }
			set {
				isClassicAuthInfo = !value;
				RaisePropertyChanged("IsDesfireAuthInfoEnabled");
			}
		}
		
		public ObservableCollection<object> DataBlockSource {
			get {
				switch (selectionIndex) {
					case 3:
						(_selectedSectorTrailerAccessBitsItem as MifareClassicAccessBitsSectorTrailerDataGridModel)
							.ProcessSectorTrailerEncoding((selectedCombinedDataBlockAccessBitsDataGridItem as MifareClassicAccessBitsShortDataBlockDataGridModel).GetShortDataBlockAccessBitsFromHumanReadableFormat()
							                              , (selectedCombinedDataBlockAccessBitsDataGridItem as MifareClassicAccessBitsShortDataBlockDataGridModel).GetShortDataBlockAccessBitsFromHumanReadableFormat()
							                              , (selectedCombinedDataBlockAccessBitsDataGridItem as MifareClassicAccessBitsShortDataBlockDataGridModel).GetShortDataBlockAccessBitsFromHumanReadableFormat());
						return displaySourceForCombinedDataBlockDataGrid;
					case 2:
						return displaySourceForCombinedDataBlockDataGrid;
					case 1:
						return displaySourceForCombinedDataBlockDataGrid;
					case 0:
						return displaySourceForCombinedDataBlockDataGrid;
					default:
						return null;
				}
			}
		}
		
		public ObservableCollection<string> DataBlockSelection {
			get { return new ObservableCollection<string>(cmbbxItems); }
		}
		
		public ObservableCollection<string> KeySelection {
			get { return new ObservableCollection<string>(keyCmbbxItems); }
		}
		
		public int KeySelectionComboboxIndex {
			get {
				if (selectedTreeViewViewModel is TreeViewChildNodeViewModel)
					return (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.mifareClassicSectorNumber;
				else
					return 0;
			}
		}
		
		public bool? IsFixedKeyNumber {
			get {
				if (selectedTreeViewViewModel is TreeViewChildNodeViewModel)
					return false;
				else
					return true;
			}
		}
		
		// TODO Add Keys to KeySetup Dialog. Keys are published as follows: 1. add sector trailer to db. 2. add st to model in databasereaderwriter class
		public string selectedClassicKeyAKey {
			get { return (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.sectorAccessBits.sectorKeyAKey; }
			set { (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.sectorAccessBits.sectorKeyAKey = value; }
		}
		
		public string selectedClassicKeyBKey {
			get { return (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.sectorAccessBits.sectorKeyBKey; }
			set { (selectedTreeViewViewModel as TreeViewChildNodeViewModel)._sectorModel.sectorAccessBits.sectorKeyBKey = value; }
		}
		
		public object ViewModelContext {
			get { return selectedTreeViewViewModel; }
			set { selectedTreeViewViewModel = value; }
		}
		
		public MifareClassicAccessBitsSectorTrailerDataGridModel SelectedSectorTrailerAccessBitsItem {
			get { return _selectedSectorTrailerAccessBitsItem; }
			set { _selectedSectorTrailerAccessBitsItem = value; }
		}
	}
}
