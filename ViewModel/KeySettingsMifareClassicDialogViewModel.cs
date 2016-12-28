﻿using GalaSoft.MvvmLight;
using MvvmDialogs.ViewModels;
using System;
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
	public class KeySettingsMifareClassicDialogViewModel : ViewModelBase, IUserDialogViewModel
	{
		
		private readonly string[] cmbbxItems = {"DataBlock 0", "DataBlock 1", "DataBlock 2", "All DataBlocks"};
		private int selectionIndex = 3;
		
		private bool isClassicAuthInfo = true;
		
		private MifareClassicAccessBitsModel sab;
		
		private readonly ObservableCollection<sourceForSectorTrailerDataGrid> displaySourceForSectorTrailerDataGrid;
		private readonly ObservableCollection<sourceForLongDataBlockDataGrid> displaySourceForLongDataBlockDataGrid;
		private readonly ObservableCollection<sourceForShortDataBlockDataGrid> displaySourceForShortDataBlockDataGrid;
		
		private ObservableCollection<object> displaySourceForDataBlock0DataGrid;
		private ObservableCollection<object> displaySourceForDataBlock1DataGrid;
		private ObservableCollection<object> displaySourceForDataBlock2DataGrid;
		private ObservableCollection<object> displaySourceForCombinedDataBlockDataGrid;

		
		private sourceForSectorTrailerDataGrid _selectedSectorTrailerAccessBitsItem;
		private object _selectedDataBlockAccessBitsItem;
		
		public KeySettingsMifareClassicDialogViewModel(bool isModal = true)
		{
			sab = new MifareClassicAccessBitsModel();
			
			displaySourceForSectorTrailerDataGrid = new ObservableCollection<sourceForSectorTrailerDataGrid>();
			foreach(string accessConditions in sab.GetSectorTrailerAccessConditions){
				displaySourceForSectorTrailerDataGrid.Add(new sourceForSectorTrailerDataGrid(accessConditions));
			}
			
			displaySourceForLongDataBlockDataGrid = new ObservableCollection<sourceForLongDataBlockDataGrid>();
			foreach(string accessConditions in sab.GetDataBlockAccessConditions){
				displaySourceForLongDataBlockDataGrid.Add(new sourceForLongDataBlockDataGrid(accessConditions));
			}
			
			displaySourceForShortDataBlockDataGrid = new ObservableCollection<sourceForShortDataBlockDataGrid>();
			foreach(string accessConditions in sab.GetShortDataBlockAccessConditions){
				if(!String.IsNullOrEmpty(accessConditions))
					displaySourceForShortDataBlockDataGrid.Add(new sourceForShortDataBlockDataGrid(accessConditions));
			}
			this.IsModal = isModal;
		}

		#region SelectedItem
		
		public sourceForSectorTrailerDataGrid SelectedSectorTrailerAccessBitsRow
		{
			get { return _selectedSectorTrailerAccessBitsItem; }
			set	{ _selectedSectorTrailerAccessBitsItem = value;
				if(_selectedSectorTrailerAccessBitsItem.getReadAccessCond == "Key A or B"){
					displaySourceForCombinedDataBlockDataGrid = null;
					displaySourceForDataBlock0DataGrid = null;
					displaySourceForDataBlock1DataGrid = null;
					displaySourceForDataBlock2DataGrid = null;
					displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForLongDataBlockDataGrid);
					displaySourceForDataBlock0DataGrid = new ObservableCollection<object>(displaySourceForLongDataBlockDataGrid);
					displaySourceForDataBlock1DataGrid = new ObservableCollection<object>(displaySourceForLongDataBlockDataGrid);
					displaySourceForDataBlock2DataGrid = new ObservableCollection<object>(displaySourceForLongDataBlockDataGrid);
				}
				else{
					displaySourceForCombinedDataBlockDataGrid = null;
					displaySourceForDataBlock0DataGrid = null;
					displaySourceForDataBlock1DataGrid = null;
					displaySourceForDataBlock2DataGrid = null;
					displaySourceForCombinedDataBlockDataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
					displaySourceForDataBlock0DataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
					displaySourceForDataBlock1DataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
					displaySourceForDataBlock2DataGrid = new ObservableCollection<object>(displaySourceForShortDataBlockDataGrid);
				}

				RaisePropertyChanged("DataBlockSource");
			}
		}
		
		public object SelectedDataBlockAccessBitsRow
		{
			get { return _selectedDataBlockAccessBitsItem; }
			set	{ _selectedDataBlockAccessBitsItem = value;}
		}


		public string SelectedDataBlockItem{
			get { return cmbbxItems[selectionIndex];}
			set { selectionIndex = Array.IndexOf(cmbbxItems, value); RaisePropertyChanged("DataBlockSource");}
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
		
		public Action<KeySettingsMifareClassicDialogViewModel> OnOk { get; set; }
		public Action<KeySettingsMifareClassicDialogViewModel> OnCancel { get; set; }
		public Action<KeySettingsMifareClassicDialogViewModel> OnCloseRequest { get; set; }

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
		
		public ObservableCollection<sourceForSectorTrailerDataGrid> SectorTrailerSource{
			get { return displaySourceForSectorTrailerDataGrid;}
		}
		
		public bool IsClassicAuthInfoEnabled{
			get { return isClassicAuthInfo;}
			set { isClassicAuthInfo = value; RaisePropertyChanged("IsClassicAuthInfoEnabled");}
		}
		
		public bool IsDesfireAuthInfoEnabled{
			get { return !isClassicAuthInfo;}
			set { isClassicAuthInfo = !value; RaisePropertyChanged("IsDesfireAuthInfoEnabled");}
		}
		
		public ObservableCollection<object> DataBlockSource{
			get { switch(selectionIndex){
					case 3:
						return displaySourceForCombinedDataBlockDataGrid;
					case 2:
						return displaySourceForDataBlock2DataGrid;
					case 1:
						return displaySourceForDataBlock1DataGrid;
					case 0:
						return displaySourceForDataBlock0DataGrid;
					default:
						return null;
				}
			}
		}
		
		public ObservableCollection<string> DataBlockSelection{
			get {return new ObservableCollection<string>(cmbbxItems);}
		}
		
		public string SectorAccessBitsAsString{
			get {return sab.DecodedSectorTrailerAccessBits;}
			set { sab.decodeSectorTrailer(value);}
		}
		
		public sourceForSectorTrailerDataGrid SelectedSectorTrailerAccessBitsItem{
			get {return _selectedSectorTrailerAccessBitsItem;}
			set {_selectedSectorTrailerAccessBitsItem = value;}
		}
	}
}
