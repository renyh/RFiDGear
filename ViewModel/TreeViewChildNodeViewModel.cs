﻿/*
 * Created by SharpDevelop.
 * User: rotts
 * Date: 26.11.2016
 * Time: 23:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using RFiDGear;

namespace RFiDGear.ViewModel
{
	/// <summary>
	/// Description of TreeViewChildNodeViewModel.
	/// </summary>
	public class TreeViewChildNodeViewModel : INotifyPropertyChanged
	{
		
		#region Data

		readonly ObservableCollection<TreeViewGrandChildNodeViewModel> _children;
		readonly TreeViewParentNodeViewModel _parent;
		
		static object _selectedItem;

		bool _isExpanded;
		bool _isSelected;

		#endregion // Data
		
		readonly Model.MifareClassicSectorModel _sector;
		readonly Model.MifareDesfireAppModel _appID;
		readonly CARD_TYPE _cardType;
		readonly ViewModel.RelayCommand _cmdReadSectorWithDefaults;
		readonly string _parentUid;
		
		readonly List<MenuItem> ContextMenuItems;

		bool? isAuth;
		
		public TreeViewChildNodeViewModel(Model.MifareClassicSectorModel sector, TreeViewParentNodeViewModel parent, CARD_TYPE cardType, int sectorNumber)
		{
			_sector = sector;
			_sector.mifareClassicSectorNumber = sectorNumber;
			_cardType = cardType;
			_parent = parent;
			
			_cmdReadSectorWithDefaults = new ViewModel.RelayCommand(ReadSectorWithDefaults);
			
			ContextMenuItems = new List<MenuItem>();
			ContextMenuItems.Add(new MenuItem() {
			                     	Header = "Read Sector using default Configuration",
			                     	Command = _cmdReadSectorWithDefaults
			                     });
			
			ContextMenuItems.Add(new MenuItem() {
			                     	Header = "Edit Authentication Settings and Read Sector",
			                     	Command = _cmdReadSectorWithDefaults
			                     });			
			
			_children = new ObservableCollection<TreeViewGrandChildNodeViewModel>();
			
			LoadChildren();
		}

		public TreeViewChildNodeViewModel(Model.MifareDesfireAppModel appID, TreeViewParentNodeViewModel parentUID, CARD_TYPE cardType)
		{
			_appID = appID;
			_cardType = cardType;
			_parentUid = parentUID.UidNumber;
		}

		
		public ObservableCollection<TreeViewGrandChildNodeViewModel> Children
		{
			get { return _children; }
		}
		
		#region SelectedItem
		
		public object SelectedItem
		{
			get { return _selectedItem; }
			private set
			{
				if (_selectedItem != value)
				{
					_selectedItem = value;
				}
			}
		}
		
		#endregion //SelectedItem
		
		#region IsExpanded

		/// <summary>
		/// Gets/sets whether the TreeViewItem
		/// associated with this object is expanded.
		/// </summary>
		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				if (value != _isExpanded)
				{
					_isExpanded = value;
					this.OnPropertyChanged("IsExpanded");
				}

				// Expand all the way up to the root.
				if (_isExpanded && _parent != null)
					_parent.IsExpanded = true;
			}
		}

		#endregion // IsExpanded

		#region IsSelected

		/// <summary>
		/// Gets/sets whether the TreeViewItem
		/// associated with this object is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (value != _isSelected)
				{
					_isSelected = value;
					OnPropertyChanged("IsSelected");
					
					SelectedItem = this;
				}
			}
		}

		#endregion // IsSelected

		#region Parent

		public TreeViewParentNodeViewModel Parent
		{
			get { return _parent; }
		}

		#endregion // Parent


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion // INotifyPropertyChanged Members
		
		public ICommand ReadSectorCommand {get { return new RelayCommand(ReadSectorWithDefaults); }}
		public void ReadSectorWithDefaults() {
			Messenger.Default.Send<NotificationMessage<string>>(
				new NotificationMessage<string>(this, "TreeViewChildNode", "ReadSectorsWithDefaults")
			);
		}
		
		public List<MenuItem> ContextMenu {
			get { return ContextMenuItems; }
		}
		
		public string SectorNumberDisplayItem {
			get { return String.Format("Sector: [{0}]", _sector.mifareClassicSectorNumber); }
		}
		
		public int SectorNumber {
			get { return _sector.mifareClassicSectorNumber; }
			set { _sector.mifareClassicSectorNumber = value; }
		}
		
		public int BlockCount {
			get { return _sector.mifareClassicSectorNumber > 31 ? 16 : 4; }
		}

		public bool? IsAuthenticated {
			get {return isAuth;}
			set { isAuth = value; OnPropertyChanged("IsAuthenticated");}
		}
		
		public string ParentUid {
			get { return _parentUid; }
		}

		public void LoadChildren()
		{
			switch (_cardType) {
				case CARD_TYPE.CT_CLASSIC_1K:
					{
						for (int i = 0; i < 4; i++) {
							_children.Add(new ViewModel.TreeViewGrandChildNodeViewModel(new Model.MifareClassicDataBlockModel(i), this, _cardType, _sector.mifareClassicSectorNumber));
						}
					}
					break;
					
				case CARD_TYPE.CT_CLASSIC_2K:
					{
						for (int i = 0; i < 4; i++) {
							_children.Add(new ViewModel.TreeViewGrandChildNodeViewModel(new Model.MifareClassicDataBlockModel(i), this, _cardType, _sector.mifareClassicSectorNumber));
						}
					}
					break;
					
				case CARD_TYPE.CT_CLASSIC_4K:
					{
						if (SectorNumber < 32) {
							for (int i = 0; i < 4; i++) {
								_children.Add(new ViewModel.TreeViewGrandChildNodeViewModel(new Model.MifareClassicDataBlockModel(i), this, _cardType, _sector.mifareClassicSectorNumber));
							}
						} else {
							
							for (int i = 0; i < 16; i++) {
								_children.Add(new ViewModel.TreeViewGrandChildNodeViewModel(new Model.MifareClassicDataBlockModel(i), this, _cardType, _sector.mifareClassicSectorNumber));
							}
						}
					}
					break;
			}
			//foreach (Model.chipMifareClassicDataBlock dataBlock in Definitions.MifareClassicLayout.GetMifareClassicDataBlocks(_sector))
			//   base.Children.Add(new ViewModel.TreeViewGrandChildNodeViewModel(dataBlock, this));
		}
	}
}
