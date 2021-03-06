﻿/*
 * Created by SharpDevelop.
 * Date: 10/11/2017
 * Time: 22:15
 *
 */

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MefMvvm.SharedContracts;
using MefMvvm.SharedContracts.ViewModel;
using MvvmDialogs.ViewModels;

using RFiDGear.DataAccessLayer;
using RFiDGear.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace RFiDGear.ViewModel
{
	/// <summary>
	/// Description of MifareClassicSetupViewModel.
	/// </summary>
	public class MifareClassicSetupViewModel : ViewModelBase, IUserDialogViewModel
	{
		#region fields

		private ObservableCollection<MifareClassicDataBlockModel> dataBlock_AccessBits = new ObservableCollection<MifareClassicDataBlockModel>
			(new[]
			 {
			 	new MifareClassicDataBlockModel(0,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB),

			 	new MifareClassicDataBlockModel(1,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicDataBlockModel(2,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicDataBlockModel(3,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB),

			 	new MifareClassicDataBlockModel(4,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB),

			 	new MifareClassicDataBlockModel(5,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicDataBlockModel(6,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicDataBlockModel(7,
			 	                                SectorTrailer_DataBlock.BlockAll,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                                AccessCondition_MifareClassicSectorTrailer.NotAllowed)
			 });

		private ObservableCollection<MifareClassicSectorModel> sectorTrailer_AccessBits = new ObservableCollection<MifareClassicSectorModel>
			(new[]
			 {
			 	new MifareClassicSectorModel(0,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA),

			 	new MifareClassicSectorModel(1,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB),

			 	new MifareClassicSectorModel(2,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicSectorModel(3,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicSectorModel(4,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA),

			 	new MifareClassicSectorModel(5,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed),

			 	new MifareClassicSectorModel(6,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyB),

			 	new MifareClassicSectorModel(7,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			 	                             AccessCondition_MifareClassicSectorTrailer.NotAllowed)
			 });

		private MifareClassicChipModel chipModel;
		private MifareClassicSectorModel sectorModel;
		private MifareClassicDataBlockModel dataBlock0;
		private MifareClassicDataBlockModel dataBlock1;
		private MifareClassicDataBlockModel dataBlock2;
		private MifareClassicDataBlockModel dataBlockCombined;

		private DatabaseReaderWriter databaseReaderWriter;
		private SettingsReaderWriter settings = new SettingsReaderWriter();

		private byte madGPB = 0xC1;
		
		#endregion fields

		#region constructors
		
		/// <summary>
		///
		/// </summary>
		public MifareClassicSetupViewModel()
		{
			chipModel = new MifareClassicChipModel(string.Format("Task Description: {0}", SelectedTaskDescription), CARD_TYPE.Mifare4K);

			sectorModel = new MifareClassicSectorModel(4,
			                                           AccessCondition_MifareClassicSectorTrailer.NotAllowed,
			                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
			                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA);

			sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block0, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
			sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block1, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
			sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block2, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
			sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.BlockAll, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));

			childNodeViewModelFromChip = new RFiDChipChildLayerViewModel(sectorModel, this);
			childNodeViewModelTemp = new RFiDChipChildLayerViewModel(sectorModel, this);
			
			Selected_Sector_AccessCondition = sectorTrailer_AccessBits[4];
			Selected_DataBlock_AccessCondition = dataBlock_AccessBits[0];
			
			MifareClassicKeys = CustomConverter.GenerateStringSequence(0,16).ToArray();
			
			SelectedClassicSectorCurrent = "0";
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="_selectedSetupViewModel"></param>
		/// <param name="_dialogs"></param>
		public MifareClassicSetupViewModel(object _selectedSetupViewModel, ObservableCollection<IDialogViewModel> _dialogs)
		{
			try
			{
				MefHelper.Instance.Container.ComposeParts(this); //Load Plugins
				
				databaseReaderWriter = new DatabaseReaderWriter();
				
				chipModel = new MifareClassicChipModel(string.Format("Task Description: {0}", SelectedTaskDescription), CARD_TYPE.Mifare4K);

				sectorModel = new MifareClassicSectorModel(4,
				                                           AccessCondition_MifareClassicSectorTrailer.NotAllowed,
				                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
				                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
				                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
				                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
				                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA);

				sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block0, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
				sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block1, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
				sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block2, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
				sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.BlockAll, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));

				sectorModel.AccessBitsAsString = settings.DefaultSpecification.MifareClassicDefaultSectorTrailer;

				sectorModel.SectorNumber = selectedClassicSectorCurrentAsInt;

				childNodeViewModelFromChip = new RFiDChipChildLayerViewModel(sectorModel, this);
				childNodeViewModelTemp = new RFiDChipChildLayerViewModel(sectorModel, this);

				Selected_DataBlock_AccessCondition = dataBlock_AccessBits[0];
				Selected_Sector_AccessCondition = sectorTrailer_AccessBits[4];
				
				if(_selectedSetupViewModel is MifareClassicSetupViewModel)
				{
					PropertyInfo[] properties = typeof(MifareClassicSetupViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

					foreach (PropertyInfo p in properties)
					{
						// If not writable then cannot null it; if not readable then cannot check it's value
						if (!p.CanWrite || !p.CanRead) { continue; }

						MethodInfo mget = p.GetGetMethod(false);
						MethodInfo mset = p.GetSetMethod(false);

						// Get and set methods have to be public
						if (mget == null) { continue; }
						if (mset == null) { continue; }

						p.SetValue(this, p.GetValue(_selectedSetupViewModel));
					}
				}
				
				else
				{
					DataBlockIsCombinedToggleButtonIsChecked = true;
					
					IsValidSectorTrailer = true;

					ClassicKeyAKeyCurrent = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[0].AccessBits.Split(new[] { ',', ';' })[0];
					ClassicKeyBKeyCurrent = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[0].AccessBits.Split(new[] { ',', ';' })[2];

					ClassicKeyAKeyTarget = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[0].AccessBits.Split(new[] { ',', ';' })[0];
					ClassicKeyBKeyTarget = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[0].AccessBits.Split(new[] { ',', ';' })[2];
					
					IsWriteFromMemoryToChipChecked = true;

					SelectedTaskIndex = "0";
					SelectedTaskDescription = "Enter a Description";
				}
				
				MifareClassicKeys = CustomConverter.GenerateStringSequence(0,39).ToArray();
				MADVersions = CustomConverter.GenerateStringSequence(1,3).ToArray();
				MADSectors = CustomConverter.GenerateStringSequence(1,39).ToArray();
				AppNumber = "1";
				
				SelectedClassicSectorCurrent = "0";
				SelectedMADSector = "1";
				SelectedMADVersion = "1";
				
				ClassicMADKeyAKeyCurrent = "FFFFFFFFFFFF";
				ClassicMADKeyBKeyCurrent = "FFFFFFFFFFFF";
				ClassicMADKeyAKeyTarget = "FFFFFFFFFFFF";
				ClassicMADKeyBKeyTarget = "FFFFFFFFFFFF";
				
				IsValidClassicKeyAKeyCurrent = null;
				IsValidClassicKeyBKeyCurrent = null;
				IsValidClassicMADKeyAKeyCurrent = null;
				IsValidClassicMADKeyBKeyCurrent = null;
				IsValidClassicKeyAKeyTarget = null;
				IsValidClassicKeyBKeyTarget = null;
				IsValidClassicMADKeyAKeyTarget = null;
				IsValidClassicMADKeyBKeyTarget = null;
				IsValidAppNumber = null;
				
				UseMAD = false;
				
				HasPlugins = items != null ? items.Any() : false;
				
				if (HasPlugins)
					SelectedPlugin = Items.FirstOrDefault();
				
			}
			catch (Exception e)
			{
				LogWriter.CreateLogEntry(string.Format("{0}: {1}; {2}", DateTime.Now, e.Message, e.InnerException != null ? e.InnerException.Message : ""));
			}
			
		}

		#endregion
		
		#region AccessBitsTab

		/// <summary>
		///
		/// </summary>
		public SectorTrailer_DataBlock Selected_DataBlockType
		{
			get { return selected_DataBlockType; }
			set
			{
				selected_DataBlockType = value;
				RaisePropertyChanged("Selected_DataBlock_AccessCondition");
				RaisePropertyChanged("Selected_DataBlockType");
			}
		} private SectorTrailer_DataBlock selected_DataBlockType;

		/// <summary>
		///
		/// </summary>
		public bool IsValidSelectedAccessBitsTaskIndex
		{
			get
			{
				//classicKeyAKeyCurrent = SectorTrailer.Split(',')[0];
				return isValidSelectedAccessBitsTaskIndex;
			}
			set
			{
				isValidSelectedAccessBitsTaskIndex = value;
				RaisePropertyChanged("IsValidSelectedAccessBitsTaskIndex");
			}
		} private bool isValidSelectedAccessBitsTaskIndex;

		/// <summary>
		///
		/// </summary>
		public bool IsAccessBitsEditTabEnabled
		{
			get { return isAccessBitsEditTabEnabled; }
			set
			{
				isAccessBitsEditTabEnabled = value;
				RaisePropertyChanged("IsAccessBitsEditTabEnabled");
			}
		} private bool isAccessBitsEditTabEnabled;

		/// <summary>
		///
		/// </summary>
		[XmlIgnore]
		public ObservableCollection<MifareClassicSectorModel> SectorTrailerSource
		{
			get { return sectorTrailer_AccessBits; }
		}

		/// <summary>
		///
		/// </summary>
		[XmlIgnore]
		public ObservableCollection<MifareClassicDataBlockModel> DataBlockSource
		{
			get
			{
				return dataBlock_AccessBits;
			}
		}

		/// <summary>
		///
		/// </summary>
		public MifareClassicDataBlockModel Selected_DataBlock_AccessCondition
		{
			get
			{
				switch (Selected_DataBlockType)
				{
					case SectorTrailer_DataBlock.Block0:
						return dataBlock0;

					case SectorTrailer_DataBlock.Block1:
						return dataBlock1;

					case SectorTrailer_DataBlock.Block2:
						return dataBlock2;

					default:
						return dataBlockCombined;
				}
			}

			set
			{
				switch (Selected_DataBlockType)
				{
					case SectorTrailer_DataBlock.Block0:
						dataBlock0 = value;
						sectorModel.DataBlock[0].Cx = value.Cx;
						sectorModel.DataBlock[0].Read_DataBlock = value.Read_DataBlock;
						sectorModel.DataBlock[0].Write_DataBlock = value.Write_DataBlock;
						sectorModel.DataBlock[0].Increment_DataBlock = value.Increment_DataBlock;
						sectorModel.DataBlock[0].Decrement_DataBlock = value.Decrement_DataBlock;
						break;

					case SectorTrailer_DataBlock.Block1:
						dataBlock1 = value;
						sectorModel.DataBlock[1].Cx = value.Cx;
						sectorModel.DataBlock[1].Read_DataBlock = value.Read_DataBlock;
						sectorModel.DataBlock[1].Write_DataBlock = value.Write_DataBlock;
						sectorModel.DataBlock[1].Increment_DataBlock = value.Increment_DataBlock;
						sectorModel.DataBlock[1].Decrement_DataBlock = value.Decrement_DataBlock;
						break;

					case SectorTrailer_DataBlock.Block2:
						dataBlock2 = value;
						sectorModel.DataBlock[2].Cx = value.Cx;
						sectorModel.DataBlock[2].Read_DataBlock = value.Read_DataBlock;
						sectorModel.DataBlock[2].Write_DataBlock = value.Write_DataBlock;
						sectorModel.DataBlock[2].Increment_DataBlock = value.Increment_DataBlock;
						sectorModel.DataBlock[2].Decrement_DataBlock = value.Decrement_DataBlock;
						break;

					default:
						dataBlockCombined = value;

						dataBlock0 = value;
						sectorModel.DataBlock[0].Cx = value.Cx;
						sectorModel.DataBlock[0].Read_DataBlock = value.Read_DataBlock;
						sectorModel.DataBlock[0].Write_DataBlock = value.Write_DataBlock;
						sectorModel.DataBlock[0].Increment_DataBlock = value.Increment_DataBlock;
						sectorModel.DataBlock[0].Decrement_DataBlock = value.Decrement_DataBlock;

						dataBlock1 = value;
						sectorModel.DataBlock[1].Cx = value.Cx;
						sectorModel.DataBlock[1].Read_DataBlock = value.Read_DataBlock;
						sectorModel.DataBlock[1].Write_DataBlock = value.Write_DataBlock;
						sectorModel.DataBlock[1].Increment_DataBlock = value.Increment_DataBlock;
						sectorModel.DataBlock[1].Decrement_DataBlock = value.Decrement_DataBlock;

						dataBlock2 = value;
						sectorModel.DataBlock[2].Cx = value.Cx;
						sectorModel.DataBlock[2].Read_DataBlock = value.Read_DataBlock;
						sectorModel.DataBlock[2].Write_DataBlock = value.Write_DataBlock;
						sectorModel.DataBlock[2].Increment_DataBlock = value.Increment_DataBlock;
						sectorModel.DataBlock[2].Decrement_DataBlock = value.Decrement_DataBlock;
						break;
				}

				encodeSectorTrailer(ref sectorModel);

				RaisePropertyChanged("Selected_DataBlock_AccessCondition");
				RaisePropertyChanged("SectorTrailer");
			}
		}

		/// <summary>
		///
		/// </summary>
		public MifareClassicSectorModel Selected_Sector_AccessCondition
		{
			get
			{
				return selected_Sector_AccessCondition;
			}
			set
			{
				//TODO: create sector string
				selected_Sector_AccessCondition = value;

				sectorModel.Read_AccessCondition_MifareClassicSectorTrailer = value.Read_AccessCondition_MifareClassicSectorTrailer;
				sectorModel.Write_AccessCondition_MifareClassicSectorTrailer = value.Write_AccessCondition_MifareClassicSectorTrailer;
				sectorModel.Read_KeyA = value.Read_KeyA;
				sectorModel.Write_KeyA = value.Write_KeyA;
				sectorModel.Read_KeyB = value.Read_KeyB;
				sectorModel.Write_KeyB = value.Write_KeyB;
				sectorModel.Cx = value.Cx;

				encodeSectorTrailer(ref sectorModel);

				RaisePropertyChanged("Selected_Sector_AccessCondition");
				RaisePropertyChanged("SectorTrailer");
			}
		} private MifareClassicSectorModel selected_Sector_AccessCondition;

		/// <summary>
		///
		/// </summary>
		public string SectorTrailer
		{
			get
			{
				return sectorModel.AccessBitsAsString; //CustomConverter.encodeSectorTrailer(sector);
			}
			set
			{
				sectorModel.AccessBitsAsString = value.ToUpper();
				IsValidSectorTrailer = !decodeSectorTrailer(sectorModel.AccessBitsAsString, ref selected_Sector_AccessCondition);
				if (!IsValidSectorTrailer)
					return;
				RaisePropertyChanged("Selected_Sector_AccessCondition");
				RaisePropertyChanged("Selected_DataBlock_AccessCondition");
			}
		}
		
		/// <summary>
		///
		/// </summary>
		public bool IsValidSectorTrailer
		{
			get { return isValidSectorTrailer; }
			set
			{
				isValidSectorTrailer = value;
				RaisePropertyChanged("IsValidSectorTrailer");
			}
		} private bool isValidSectorTrailer;

		#endregion AccessBitsTab

		#region MADEditor
		
		[XmlIgnore]
		public string[] MADVersions { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string SelectedMADVersion
		{
			get
			{
				return selectedMADVersion;
			}
			set
			{
				if(byte.TryParse(value, out selectedMADVersionAsByte))
				{
					selectedMADVersion = value;
					
					madGPB = (byte)((madGPB &= 0xFC) | selectedMADVersionAsByte);
				}

				RaisePropertyChanged("SelectedMADVersion");
			}
			
		}
		private string selectedMADVersion;
		private byte selectedMADVersionAsByte;

		
		[XmlIgnore]
		public string[] MADSectors { get; set; }
		
		/// <summary>
		/// Change MAD GPB
		/// </summary>
		public bool IsMultiApplication
		{
			get { return isMultiApplication; }
			set
			{
				isMultiApplication = value;
				if (value)
					madGPB |= 0x40;
				else
					madGPB &= 0xBF;
				RaisePropertyChanged("IsMultiApplication");
			}
		} private bool isMultiApplication;
		
		/// <summary>
		/// Do authenticate to MAD or not before performing a write operation?
		/// </summary>
		public bool UseMadAuth
		{
			get
			{
				return useMADAuth;
			}
			set
			{
				useMADAuth = value;
				RaisePropertyChanged("UseMadAuth");
			}
		} private bool useMADAuth;
		
		/// <summary>
		///
		/// </summary>
		public string FileSize
		{
			get { return fileSize; }
			set
			{
				fileSize = value;
				IsValidFileSize = (int.TryParse(value, out fileSizeAsInt) && fileSizeAsInt <= 4200);
				
				if(IsValidFileSize != false)
				{
					if(childNodeViewModelFromChip.Children.Any(x => x.MifareClassicMAD != null))
					{
						try
						{
							childNodeViewModelFromChip.Children.Single().MifareClassicMAD = new MifareClassicMADModel(new byte[fileSizeAsInt], appNumberAsInt);
							childNodeViewModelTemp.Children.Single().MifareClassicMAD = new MifareClassicMADModel(new byte[fileSizeAsInt], appNumberAsInt);
							
							childNodeViewModelTemp.Children.Single().RequestRefresh();
							childNodeViewModelFromChip.Children.Single().RequestRefresh();
						}
						catch
						{
							
						}
						
					}
					else
					{
						childNodeViewModelFromChip.Children.Add(new RFiDChipGrandChildLayerViewModel(new MifareClassicMADModel(new byte[fileSizeAsInt], appNumberAsInt),null));
						childNodeViewModelTemp.Children.Add(new RFiDChipGrandChildLayerViewModel(new MifareClassicMADModel(new byte[fileSizeAsInt], appNumberAsInt), null));
					}
				}
				
				RaisePropertyChanged("FileSize");
			}
		}
		private string fileSize;
		private int fileSizeAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidFileSize
		{
			get
			{
				return isValidFileSize;
			}
			set
			{
				isValidFileSize = value;
				RaisePropertyChanged("IsValidFileSize");
			}
		} private bool? isValidFileSize;
		
		/// <summary>
		///
		/// </summary>
		public string AppNumber
		{
			get { return appNumber; }
			set
			{
				appNumber = value;
				IsValidAppNumber = (int.TryParse(value, out appNumberAsInt) && appNumberAsInt <= 0xFFFF);
				RaisePropertyChanged("AppNumberNew");
			}
		}
		private string appNumber;
		private int appNumberAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidAppNumber
		{
			get
			{
				return isValidAppNumber;
			}
			set
			{
				isValidAppNumber = value;
				RaisePropertyChanged("IsValidAppNumber");
			}
		}
		private bool? isValidAppNumber;
		
		/// <summary>
		/// 
		/// </summary>
		public string SelectedMADSector
		{
			get { return selectedMADSector; }
			
			set
			{
				if(int.TryParse(value,out selectedMADSectorAsInt))
					selectedMADSector = value;
				RaisePropertyChanged("SelectedMADSector");
			}
		}
		private string selectedMADSector;
		private int selectedMADSectorAsInt;
		
		/// <summary>
		///
		/// </summary>
		public string ClassicMADKeyAKeyCurrent
		{
			get
			{
				return classicMADKeyAKeyCurrent;
			}
			set
			{
				classicMADKeyAKeyCurrent = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();
				
				IsValidClassicMADKeyAKeyCurrent = (CustomConverter.IsInHexFormat(classicMADKeyAKeyCurrent) && classicMADKeyAKeyCurrent.Length == 12);

				RaisePropertyChanged("ClassicMADKeyAKeyCurrent");
			}
		}
		private string classicMADKeyAKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicMADKeyAKeyCurrent
		{
			get
			{
				return isValidClassicMADKeyAKeyCurrent;
			}
			set
			{
				isValidClassicMADKeyAKeyCurrent = value;
				RaisePropertyChanged("IsValidClassicMADKeyAKeyCurrent");
			}
		}
		private bool? isValidClassicMADKeyAKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public string ClassicMADKeyBKeyCurrent
		{
			get
			{
				return classicMADKeyBKeyCurrent;
			}
			set
			{
				classicMADKeyBKeyCurrent = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();

				IsValidClassicMADKeyBKeyCurrent = (CustomConverter.IsInHexFormat(classicMADKeyBKeyCurrent) && classicMADKeyBKeyCurrent.Length == 12);
				
				RaisePropertyChanged("ClassicMADKeyBKeyCurrent");
			}
		}
		private string classicMADKeyBKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicMADKeyBKeyCurrent
		{
			get
			{
				return isValidClassicMADKeyBKeyCurrent;
			}
			set
			{
				isValidClassicMADKeyBKeyCurrent = value;
				RaisePropertyChanged("IsValidClassicMADKeyBKeyCurrent");
			}
		}
		private bool? isValidClassicMADKeyBKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public string ClassicMADKeyAKeyTarget
		{
			get
			{
				return classicMADKeyAKeyTarget;
			}
			set
			{
				classicMADKeyAKeyTarget = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();
				
				IsValidClassicMADKeyAKeyTarget = (CustomConverter.IsInHexFormat(classicMADKeyAKeyTarget) && classicMADKeyAKeyTarget.Length == 12);

				RaisePropertyChanged("ClassicMADKeyAKeyTarget");
			}
		}
		private string classicMADKeyAKeyTarget;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicMADKeyAKeyTarget
		{
			get
			{
				return isValidClassicMADKeyAKeyTarget;
			}
			set
			{
				isValidClassicMADKeyAKeyTarget = value;
				RaisePropertyChanged("IsValidClassicMADKeyAKeyTarget");
			}
		}
		private bool? isValidClassicMADKeyAKeyTarget;

		/// <summary>
		///
		/// </summary>
		public string ClassicMADKeyBKeyTarget
		{
			get
			{
				return classicMADKeyBKeyTarget;
			}
			set
			{
				classicMADKeyBKeyTarget = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();

				IsValidClassicMADKeyBKeyTarget = (CustomConverter.IsInHexFormat(classicMADKeyBKeyTarget) && classicMADKeyBKeyTarget.Length == 12);
				
				RaisePropertyChanged("ClassicMADKeyBKeyTarget");
			}
		}
		private string classicMADKeyBKeyTarget;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicMADKeyBKeyTarget
		{
			get
			{
				return isValidClassicMADKeyBKeyTarget;
			}
			set
			{
				isValidClassicMADKeyBKeyTarget = value;
				RaisePropertyChanged("IsValidClassicMADKeyBKeyTarget");
			}
		}
		private bool? isValidClassicMADKeyBKeyTarget;

		/// <summary>
		///
		/// </summary>
		public string ClassicKeyAKeyTarget
		{
			get
			{
				return classicKeyAKeyTarget;
			}
			set
			{
				classicKeyAKeyTarget = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();
				
				IsValidClassicKeyAKeyTarget = (CustomConverter.IsInHexFormat(classicKeyAKeyTarget) && classicKeyAKeyTarget.Length == 12);

				RaisePropertyChanged("ClassicKeyAKeyTarget");
			}
		}
		private string classicKeyAKeyTarget;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicKeyAKeyTarget
		{
			get
			{
				return isValidClassicKeyAKeyTarget;
			}
			set
			{
				isValidClassicKeyAKeyTarget = value;
				RaisePropertyChanged("IsValidClassicKeyAKeyTarget");
			}
		}
		private bool? isValidClassicKeyAKeyTarget;

		/// <summary>
		///
		/// </summary>
		public string ClassicKeyBKeyTarget
		{
			get
			{
				return classicKeyBKeyTarget;
			}
			set
			{
				classicKeyBKeyTarget = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();

				IsValidClassicKeyBKeyTarget = (CustomConverter.IsInHexFormat(classicKeyBKeyTarget) && classicKeyBKeyTarget.Length == 12);
				
				RaisePropertyChanged("ClassicKeyBKeyTarget");
			}
		}
		private string classicKeyBKeyTarget;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicKeyBKeyTarget
		{
			get
			{
				return isValidClassicKeyBKeyTarget;
			}
			set
			{
				isValidClassicKeyBKeyTarget = value;
				RaisePropertyChanged("IsValidClassicKeyBKeyTarget");
			}
		}
		private bool? isValidClassicKeyBKeyTarget;

		#endregion
		
		#region KeySetup

		[XmlIgnore]
		public string[] MifareClassicKeys { get; set;}
		
		/// <summary>
		/// 
		/// </summary>
		public bool UseMAD
		{
			get { return useMAD; }
			set
			{
				useMAD = value;
				if(UseMAD)
				{
					ChildNodeViewModelTemp.Children.Clear();
					ChildNodeViewModelFromChip.Children.Clear();
					
					ChildNodeViewModelFromChip.Children.Add(new RFiDChipGrandChildLayerViewModel(new MifareClassicMADModel(0,1),this));
					
					ChildNodeViewModelTemp.Children.Add(new RFiDChipGrandChildLayerViewModel(new MifareClassicMADModel(0,1),this));
				}
				else
				{
					ChildNodeViewModelTemp.Children.Clear();
					ChildNodeViewModelFromChip.Children.Clear();
					
					sectorModel = new MifareClassicSectorModel(4,
					                                           AccessCondition_MifareClassicSectorTrailer.NotAllowed,
					                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
					                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
					                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
					                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA,
					                                           AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA);

					sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block0, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
					sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block1, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
					sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.Block2, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));
					sectorModel.DataBlock.Add(new MifareClassicDataBlockModel(0, SectorTrailer_DataBlock.BlockAll, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB, AccessCondition_MifareClassicSectorTrailer.Allowed_With_KeyA_Or_KeyB));

					childNodeViewModelFromChip = new RFiDChipChildLayerViewModel(sectorModel, this);
					childNodeViewModelTemp = new RFiDChipChildLayerViewModel(sectorModel, this);
					
					RaisePropertyChanged("ChildNodeViewModelFromChip");
					RaisePropertyChanged("ChildNodeViewModelTemp");
				}
				
				RaisePropertyChanged("UseMAD");
				RaisePropertyChanged("UseMADInvert");
			}
		}
		private bool useMAD;
		
		/// <summary>
		/// 
		/// </summary>
		public bool UseMADInvert
		{
			get { return !UseMAD;}
		}
		
		/// <summary>
		///
		/// </summary>
		public bool IsClassicAuthInfoEnabled
		{
			get { return isClassicAuthInfoEnabled; }
			set
			{
				isClassicAuthInfoEnabled = value;
				RaisePropertyChanged("IsClassicAuthInfoEnabled");
			}
		}
		private bool isClassicAuthInfoEnabled = false;

		/// <summary>
		///
		/// </summary>
		public bool IsClassicKeyEditingEnabled
		{
			get { return isClassicKeyEditingEnabled; }
			set
			{
				isClassicKeyEditingEnabled = value;
				RaisePropertyChanged("IsClassicKeyEditingEnabled");
			}
		}
		private bool isClassicKeyEditingEnabled;

		/// <summary>
		///
		/// </summary>
		public bool IsValidSelectedKeySetupTaskIndex
		{
			get
			{
				//classicKeyAKeyCurrent = SectorTrailer.Split(',')[0];
				return isValidSelectedKeySetupTaskIndex;
			}
			set
			{
				isValidSelectedKeySetupTaskIndex = value;
				RaisePropertyChanged("IsValidSelectedKeySetupTaskIndex");
			}
		}
		private bool isValidSelectedKeySetupTaskIndex;

		/// <summary>
		///
		/// </summary>
		public string ClassicKeyAKeyCurrent
		{
			get
			{
				return classicKeyAKeyCurrent;
			}
			set
			{
				classicKeyAKeyCurrent = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();

				IsValidClassicKeyAKeyCurrent = (CustomConverter.IsInHexFormat(classicKeyAKeyCurrent) && classicKeyAKeyCurrent.Length == 12);
				
				if (IsValidClassicKeyAKeyCurrent != false && SelectedTaskType == TaskType_MifareClassicTask.ChangeDefault)
				{
					string currentSectorTrailer = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[selectedClassicKeyANumberCurrentAsInt].AccessBits;
					currentSectorTrailer = string.Join(",", new[]
					                                   {
					                                   	classicKeyAKeyCurrent,
					                                   	currentSectorTrailer.Split(new[] {',',';'})[1],
					                                   	currentSectorTrailer.Split(new[] {',',';'})[2]
					                                   });

					settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[selectedClassicKeyANumberCurrentAsInt] = new MifareClassicDefaultKeys(selectedClassicKeyANumberCurrentAsInt, currentSectorTrailer);
				}
				else if (IsValidClassicKeyAKeyCurrent != false)
					sectorModel.KeyA = classicKeyAKeyCurrent;

				RaisePropertyChanged("ClassicKeyAKeyCurrent");
			}
		}
		private string classicKeyAKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicKeyAKeyCurrent
		{
			get
			{
				return isValidClassicKeyAKeyCurrent;
			}
			set
			{
				isValidClassicKeyAKeyCurrent = value;
				RaisePropertyChanged("IsValidClassicKeyAKeyCurrent");
			}
		}
		private bool? isValidClassicKeyAKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public string ClassicKeyBKeyCurrent
		{
			get
			{
				return classicKeyBKeyCurrent;
			}
			set
			{
				classicKeyBKeyCurrent = value.Length > 12 ? value.ToUpper().Remove(12, value.Length - 12) : value.ToUpper();

				IsValidClassicKeyBKeyCurrent = (CustomConverter.IsInHexFormat(classicKeyBKeyCurrent) && classicKeyBKeyCurrent.Length == 12);
				if (IsValidClassicKeyBKeyCurrent != false && SelectedTaskType == TaskType_MifareClassicTask.ChangeDefault)
				{
					string currentSectorTrailer = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[selectedClassicKeyBNumberCurrentAsInt].AccessBits;
					currentSectorTrailer = string.Join(",", new[]
					                                   {
					                                   	currentSectorTrailer.Split(new[] {',',';'})[0],
					                                   	currentSectorTrailer.Split(new[] {',',';'})[1],
					                                   	classicKeyBKeyCurrent
					                                   });

					settings.DefaultSpecification.MifareClassicDefaultSecuritySettings[selectedClassicKeyBNumberCurrentAsInt] = new MifareClassicDefaultKeys(selectedClassicKeyBNumberCurrentAsInt, currentSectorTrailer);
				}
				else if (IsValidClassicKeyBKeyCurrent != false)
					sectorModel.KeyB = classicKeyBKeyCurrent;

				RaisePropertyChanged("ClassicKeyBKeyCurrent");
			}
		}
		private string classicKeyBKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidClassicKeyBKeyCurrent
		{
			get
			{
				return isValidClassicKeyBKeyCurrent;
			}
			set
			{
				isValidClassicKeyBKeyCurrent = value;
				RaisePropertyChanged("IsValidClassicKeyBKeyCurrent");
			}
		}
		private bool? isValidClassicKeyBKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public string SelectedClassicKeyANumberCurrent
		{
			get
			{
				return selectedClassicKeyANumberCurrent;
			}
			set
			{
				if(int.TryParse(value, out selectedClassicKeyANumberCurrentAsInt))
				{
					selectedClassicKeyANumberCurrent = value;
					RaisePropertyChanged("SelectedClassicKeyANumberCurrent");
				}
//				if(SelectedTaskType == TaskType_MifareClassicTask.ChangeDefault)
//				{
//					ClassicKeyAKeyCurrent = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings.
//						First(x => x.KeyType == SelectedClassicKeyANumberCurrent).AccessBits.Split(new[] { ',', ';' })[0];
//				}
			}
		}
		private string selectedClassicKeyANumberCurrent;
		private int selectedClassicKeyANumberCurrentAsInt;

		/// <summary>
		///
		/// </summary>
		public string SelectedClassicKeyBNumberCurrent
		{
			get
			{
				return selectedClassicKeyBNumberCurrent;
			}
			set
			{
				if(int.TryParse(value, out selectedClassicKeyBNumberCurrentAsInt))
				{
					selectedClassicKeyBNumberCurrent = value;
					RaisePropertyChanged("SelectedClassicKeyBNumberCurrent");
				}

//				ClassicKeyBKeyCurrent = settings.DefaultSpecification.MifareClassicDefaultSecuritySettings.
//					First(x => x.KeyType == SelectedClassicKeyBNumberCurrent).AccessBits.Split(new[] { ',', ';' })[2];

			}
		}
		private string selectedClassicKeyBNumberCurrent;
		private int selectedClassicKeyBNumberCurrentAsInt;

		/// <summary>
		///
		/// </summary>
		public string SelectedClassicSectorCurrent
		{
			get
			{
				return selectedClassicSectorCurrent;
			}
			set
			{
				//sectorModel.SectorNumber = (int)selectedClassicSectorCurrent;
				try
				{
					//parentNodeViewModelFromChip.Children.First(x => x.SectorNumber == (int)selectedClassicSectorCurrent).SectorNumber = (int)value;
				}
				catch
				{
				}
				//(parentNodeViewModel as RFiDChipParentLayerViewModel).Children.First(x => x.SectorNumber == (int)SelectedClassicSectorCurrent).IsTask = true;

				if(int.TryParse(value, out selectedClassicSectorCurrentAsInt))
				{
					selectedClassicSectorCurrent = value;
					RaisePropertyChanged("SelectedClassicSectorCurrent");
				}
			}
		}
		private string selectedClassicSectorCurrent;
		private int selectedClassicSectorCurrentAsInt;

		public bool DataBlockIsCombinedToggleButtonIsChecked
		{
			get { return dataBlockIsCombinedToggleButtonIsChecked; }
			set
			{
				dataBlockIsCombinedToggleButtonIsChecked = value;

				if (value)
					Selected_DataBlockType = SectorTrailer_DataBlock.BlockAll;

				RaisePropertyChanged("SelectedDataBlockItem");
				RaisePropertyChanged("DataBlockIsCombinedToggleButtonIsChecked");
				RaisePropertyChanged("DataBlockSelectionComboBoxIsEnabled");
				RaisePropertyChanged("DataBlockSelection");
			}
		}
		private bool dataBlockIsCombinedToggleButtonIsChecked;

		#endregion KeySetup

		#region DataExplorer

		/// <summary>
		///
		/// </summary>
		public DataExplorer_DataBlock SelectedDataBlockToReadWrite
		{
			get { return selectedDataBlockToReadWrite; }
			set
			{
				selectedDataBlockToReadWrite = value;
				foreach (RFiDChipGrandChildLayerViewModel gCNVM in ChildNodeViewModelTemp.Children)
					gCNVM.IsFocused = false;

				ChildNodeViewModelTemp.Children.First(x => x.DataBlockNumber == (int)SelectedDataBlockToReadWrite).IsFocused = true;

				RaisePropertyChanged("SelectedDataBlockToReadWrite");
			}
		}
		private DataExplorer_DataBlock selectedDataBlockToReadWrite;

		/// <summary>
		///
		/// </summary>
		public bool IsWriteFromMemoryToChipChecked
		{
			get { return isWriteFromMemoryToChipChecked; }
			set
			{
				isWriteFromMemoryToChipChecked = value;
				RaisePropertyChanged("IsWriteFromMemoryToChipChecked");
			}
		}
		private bool isWriteFromMemoryToChipChecked;

		/// <summary>
		///
		/// </summary>
		public bool IsWriteFromFileToChipChecked
		{
			get { return isWriteFromFileToChipChecked; }
			set
			{
				isWriteFromFileToChipChecked = value;
				RaisePropertyChanged("IsWriteFromFileToChipChecked");
			}
		}
		private bool isWriteFromFileToChipChecked;

		/// <summary>
		///
		/// </summary>
		public bool IsDataExplorerEditTabEnabled
		{
			get { return isDataExplorerEditTabEnabled; }
			set
			{
				isDataExplorerEditTabEnabled = value;
				RaisePropertyChanged("IsDataExplorerEditTabEnabled");
			}
		}
		private bool isDataExplorerEditTabEnabled;

		/// <summary>
		///
		/// </summary>
		public RFiDChipChildLayerViewModel ChildNodeViewModelFromChip
		{
			get { return childNodeViewModelFromChip; }
			set { childNodeViewModelFromChip = value; }
		}
		private RFiDChipChildLayerViewModel childNodeViewModelFromChip;

		/// <summary>
		///
		/// </summary>
		public RFiDChipChildLayerViewModel ChildNodeViewModelTemp
		{
			get { return childNodeViewModelTemp; }
			set { childNodeViewModelTemp = value; }
		}
		private RFiDChipChildLayerViewModel childNodeViewModelTemp;

		#endregion DataExplorer

		#region Plugins

		/// <summary>
		/// True if Count of Imported Views is > 0; See constructor
		/// </summary>
		[XmlIgnore]
		public bool HasPlugins
		{
			get {
				return hasPlugins;
			}
			set
			{
				hasPlugins = value;
				RaisePropertyChanged("HasPlugins");
			}
		} private bool hasPlugins;
		
		/// <summary>
		/// Selected Plugin in ComboBox
		/// </summary>
		[XmlIgnore]
		public object SelectedPlugin
		{
			get {return selectedPlugin;}
			set {
				selectedPlugin = value;
				RaisePropertyChanged("SelectedPlugin");
			}
		} private object selectedPlugin;
		
		/// <summary>
		/// Imported Views by URI
		/// </summary>
		[XmlIgnore]
		[ImportMany()]
		public Lazy<IUIExtension, IUIExtensionDetails>[] Items
		{
			get
			{
				return items;
			}
			
			set
			{
				items = (from g in value
				         orderby g.Metadata.SortOrder, g.Metadata.Name
				         select g).ToArray();
				
				RaisePropertyChanged("Items");
			}
		} private Lazy<IUIExtension, IUIExtensionDetails>[] items;
		#endregion
		
		#region General Properties

		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		public ERROR TaskErr { get; set; }

		/// <summary>
		///
		/// </summary>
		public bool? IsTaskCompletedSuccessfully
		{
			get { return isTaskCompletedSuccessfully; }
			set
			{
				isTaskCompletedSuccessfully = value;
				RaisePropertyChanged("IsTaskCompletedSuccessfully");
			}
		}
		private bool? isTaskCompletedSuccessfully;

		/// <summary>
		///
		/// </summary>
		[XmlIgnore]
		public string StatusText
		{
			get { return statusText; }
			set
			{
				statusText = value;
				RaisePropertyChanged("StatusText");
			}
		}
		private string statusText;

		/// <summary>
		///
		/// </summary>
		public string SelectedTaskIndex
		{
			get
			{
				//classicKeyAKeyCurrent = SectorTrailer.Split(',')[0];
				return selectedAccessBitsTaskIndex;
			}
			set
			{
				selectedAccessBitsTaskIndex = value;
				IsValidSelectedTaskIndex = int.TryParse(value, out selectedTaskIndexAsInt);
			}
		}
		private string selectedAccessBitsTaskIndex;

		/// <summary>
		///
		/// </summary>
		public int SelectedTaskIndexAsInt
		{ get { return selectedTaskIndexAsInt; } }
		private int selectedTaskIndexAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidSelectedTaskIndex
		{
			get
			{
				return isValidSelectedTaskIndex;
			}
			set
			{
				isValidSelectedTaskIndex = value;
				RaisePropertyChanged("IsValidSelectedTaskIndex");
			}
		}
		private bool? isValidSelectedTaskIndex;

		/// <summary>
		///
		/// </summary>
		public TaskType_MifareClassicTask SelectedTaskType
		{
			get
			{
				//classicKeyAKeyCurrent = SectorTrailer.Split(',')[0];
				return selectedAccessBitsTaskType;
			}
			set
			{
				selectedAccessBitsTaskType = value;
				switch (value)
				{
					case TaskType_MifareClassicTask.None:
						IsClassicKeyEditingEnabled = false;
						IsClassicAuthInfoEnabled = true;
						IsAccessBitsEditTabEnabled = true;
						break;

					case TaskType_MifareClassicTask.ReadData:
						IsClassicKeyEditingEnabled = false;
						IsClassicAuthInfoEnabled = true;
						IsAccessBitsEditTabEnabled = true;
						IsDataExplorerEditTabEnabled = true;
						break;

					case TaskType_MifareClassicTask.WriteData:
						IsClassicKeyEditingEnabled = false;
						IsClassicAuthInfoEnabled = true;
						IsAccessBitsEditTabEnabled = true;
						IsDataExplorerEditTabEnabled = true;
						break;

					case TaskType_MifareClassicTask.ChangeDefault:
						IsClassicKeyEditingEnabled = false;
						IsClassicAuthInfoEnabled = true;
						IsAccessBitsEditTabEnabled = false;
						break;
				}
				RaisePropertyChanged("SelectedTaskType");
			}
		}
		private TaskType_MifareClassicTask selectedAccessBitsTaskType;

		/// <summary>
		///
		/// </summary>
		public string SelectedTaskDescription
		{
			get
			{
				return selectedAccessBitsTaskDescription;
			}
			set
			{
				selectedAccessBitsTaskDescription = value;
				RaisePropertyChanged("SelectedTaskDescription");
			}
		}
		private string selectedAccessBitsTaskDescription;

		/// <summary>
		/// 
		/// </summary>
		[XmlIgnore]
		public SettingsReaderWriter Settings
		{
			get { return settings; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool DataBlockSelectionComboBoxIsEnabled
		{
			get { return !dataBlockIsCombinedToggleButtonIsChecked; }
		}

		#endregion General Properties

		#region Commands

		/// <summary>
		/// 
		/// </summary>
		public ICommand ReadDataCommand { get { return new RelayCommand(OnNewReadDataCommand); } }
		private void OnNewReadDataCommand()
		{
			//Mouse.OverrideCursor = Cursors.Wait;
			TaskErr = ERROR.Empty;
			
			Task classicTask =
				new Task(() =>
				         {
				         	using (RFiDDevice device = RFiDDevice.Instance)
				         	{
				         		if(device != null && device.ReadChipPublic() == ERROR.NoError)
				         		{
				         			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);
				         			
				         			if(!useMAD)
				         			{
				         				if (device.ReadMiFareClassicSingleSector(
				         					selectedClassicSectorCurrentAsInt,
				         					ClassicKeyAKeyCurrent,
				         					ClassicKeyBKeyCurrent) == ERROR.NoError)
				         				{
				         					
				         					childNodeViewModelFromChip.SectorNumber = selectedClassicSectorCurrentAsInt;
				         					childNodeViewModelTemp.SectorNumber = selectedClassicSectorCurrentAsInt;
				         					
				         					StatusText = StatusText + string.Format("{0}: Success for Sector: {1}\n", DateTime.Now, selectedClassicSectorCurrentAsInt);
				         					
				         					for (int i = 0; i < device.Sector.DataBlock.Count; i++)
				         					{
				         						childNodeViewModelFromChip.Children[i].DataBlockNumber = i;
				         						childNodeViewModelTemp.Children[i].DataBlockNumber = i;
				         						
				         						childNodeViewModelFromChip.Children[i].MifareClassicDataBlock.DataBlockNumberChipBased = device.Sector.DataBlock.First(x => x.DataBlockNumberSectorBased == i).DataBlockNumberChipBased;
				         						
				         						if (device.Sector.DataBlock[i].IsAuthenticated)
				         						{
				         							StatusText = StatusText + string.Format("{0}: \tSuccess for Blocknumber: {1} Data: {2}\n", DateTime.Now, device.Sector.DataBlock[i].DataBlockNumberChipBased, CustomConverter.HexToString(device.Sector.DataBlock[i].Data));
				         							childNodeViewModelFromChip.Children.First(x => x.DataBlockNumber == i).MifareClassicDataBlock.Data = device.Sector.DataBlock[i].Data;
				         							childNodeViewModelFromChip.Children.First(x => x.DataBlockNumber == i).RequestRefresh();
				         							
				         							childNodeViewModelTemp.Children.First(x => x.DataBlockNumber == i).MifareClassicDataBlock.Data = device.Sector.DataBlock[i].Data;
				         							childNodeViewModelTemp.Children.First(x => x.DataBlockNumber == i).RequestRefresh();
				         							
				         							TaskErr = ERROR.NoError;
				         						}
				         						else
				         							StatusText = StatusText + string.Format("{0}: \tBut: unable to authenticate to sector: {1}, DataBlock: {2} using specified Keys\n", DateTime.Now, selectedClassicSectorCurrentAsInt, device.Sector.DataBlock[i - 1].DataBlockNumberChipBased);
				         					}
				         					
				         					TaskErr = ERROR.NoError;


				         				}
				         				
				         				else
				         				{
				         					StatusText = StatusText + string.Format("{0}: Unable to Authenticate to Sector: {1} using specified Keys\n", DateTime.Now, selectedClassicSectorCurrentAsInt);
				         					TaskErr = ERROR.AuthenticationError;
				         					return;
				         				}
				         			}
				         			
				         			else
				         			{
				         				if (device.ReadMiFareClassicSingleSector(
				         					0,
				         					ClassicMADKeyAKeyCurrent,
				         					ClassicMADKeyBKeyCurrent) == ERROR.NoError)
				         				{
				         					
				         					StatusText = StatusText + string.Format("{0}: Successfully Authenticated to MAD\n", DateTime.Now);
				         					
				         					ChildNodeViewModelFromChip.Children.FirstOrDefault().MifareClassicMAD.MADApp = appNumberAsInt;
				         					ChildNodeViewModelTemp.Children.FirstOrDefault().MifareClassicMAD.MADApp = appNumberAsInt;
				         					
				         					if(device.ReadMiFareClassicWithMAD(appNumberAsInt, ClassicKeyAKeyCurrent, ClassicKeyBKeyCurrent, ClassicMADKeyAKeyCurrent ,ClassicMADKeyBKeyCurrent, fileSizeAsInt) == ERROR.NoError)
				         					{
				         						ChildNodeViewModelFromChip.Children.FirstOrDefault().MifareClassicMAD.Data = device.MifareClassicData;
				         						ChildNodeViewModelTemp.Children.FirstOrDefault().MifareClassicMAD.Data = device.MifareClassicData;
				         						
				         						ChildNodeViewModelTemp.Children.Single().RequestRefresh();
				         						ChildNodeViewModelFromChip.Children.Single().RequestRefresh();
				         						
				         						StatusText = StatusText + string.Format("{0}: Successfully Read Data from MAD\n", DateTime.Now);
				         					}

				         					
				         					TaskErr = ERROR.NoError;
				         					

				         				}
				         				
				         				else
				         				{
				         					StatusText = StatusText + string.Format("{0}: Unable to Authenticate to MAD Sector using specified MAD Key(s)\n", DateTime.Now);
				         					TaskErr = ERROR.AuthenticationError;
				         					return;
				         				}
				         			}


				         		}
				         		
				         		else
				         		{
				         			TaskErr = ERROR.DeviceNotReadyError;
				         			return;
				         		}

				         		RaisePropertyChanged("ChildNodeViewModelTemp");
				         		
				         		return;
				         	}
				         });

			classicTask.Start();

			classicTask.ContinueWith((x) =>
			                         {
			                         	//Mouse.OverrideCursor = null;

			                         	if (TaskErr == ERROR.NoError)
			                         	{
			                         		IsTaskCompletedSuccessfully = true;
			                         	}
			                         	else
			                         	{
			                         		IsTaskCompletedSuccessfully = false;
			                         	}
			                         });
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand WriteDataCommand { get { return new RelayCommand(OnNewWriteDataCommand); } }
		private void OnNewWriteDataCommand()
		{
			//Mouse.OverrideCursor = Cursors.Wait;

			TaskErr = ERROR.Empty;

			Task classicTask =
				new Task(() =>
				         {
				         	using (RFiDDevice device = RFiDDevice.Instance)
				         	{
				         		StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);
				         		
				         		if(!UseMAD)
				         		{
				         			if (device != null)
				         			{
				         				childNodeViewModelFromChip.SectorNumber = selectedClassicSectorCurrentAsInt;
				         				childNodeViewModelTemp.SectorNumber = selectedClassicSectorCurrentAsInt;
				         				
				         				if (device.WriteMiFareClassicSingleBlock(childNodeViewModelFromChip.Children[(int)SelectedDataBlockToReadWrite].MifareClassicDataBlock.DataBlockNumberChipBased,
				         				                                         ClassicKeyAKeyCurrent,
				         				                                         ClassicKeyBKeyCurrent,
				         				                                         childNodeViewModelTemp.Children[(int)SelectedDataBlockToReadWrite].MifareClassicDataBlock.Data) == ERROR.NoError)
				         				{
				         					StatusText = StatusText + string.Format("{0}: \tSuccess for Blocknumber: {1} Data: {2}\n",
				         					                                        DateTime.Now,
				         					                                        childNodeViewModelFromChip.Children[(int)SelectedDataBlockToReadWrite].DataBlockNumber,
				         					                                        CustomConverter.HexToString(childNodeViewModelTemp.Children[(int)SelectedDataBlockToReadWrite].MifareClassicDataBlock.Data));
				         					TaskErr = ERROR.NoError;
				         				}
				         				else
				         					TaskErr = ERROR.AuthenticationError;

				         			}
				         			else
				         			{
				         				StatusText = "Unable to Auth";
				         				TaskErr = ERROR.DeviceNotReadyError;
				         			}
				         		}

				         		else
				         		{
				         			if (device.ReadMiFareClassicSingleSector(
				         				0,
				         				ClassicMADKeyAKeyCurrent,
				         				ClassicMADKeyBKeyCurrent) == ERROR.NoError)
				         			{
				         				
				         				StatusText = StatusText + string.Format("{0}: Successfully Authenticated to MAD\n", DateTime.Now);
				         				
				         				ChildNodeViewModelFromChip.Children.FirstOrDefault().MifareClassicMAD.MADApp = appNumberAsInt;
				         				ChildNodeViewModelTemp.Children.FirstOrDefault().MifareClassicMAD.MADApp = appNumberAsInt;
				         				
				         				
				         				
				         				if(device.WriteMiFareClassicWithMAD(appNumberAsInt, selectedMADSectorAsInt,
				         				                                    ClassicMADKeyAKeyCurrent ,ClassicMADKeyBKeyCurrent,
				         				                                    ClassicMADKeyAKeyTarget, ClassicMADKeyBKeyTarget,
				         				                                    ClassicKeyAKeyCurrent, ClassicKeyBKeyCurrent,
				         				                                    ClassicKeyAKeyTarget, ClassicKeyBKeyTarget,
				         				                                    ChildNodeViewModelTemp.Children.Single(x => x.MifareClassicMAD.MADApp == appNumberAsInt).MifareClassicMAD.Data, 
				         				                                    madGPB, UseMadAuth) == ERROR.NoError)
				         				{
				         					StatusText = StatusText + string.Format("{0}: Wrote n bytes to MAD ID x\n", DateTime.Now);
				         				}

				         				
				         				TaskErr = ERROR.NoError;
				         				

				         			}
				         			
				         			else
				         			{
				         				StatusText = StatusText + string.Format("{0}: Unable to Authenticate to MAD Sector using specified MAD Key(s)\n", DateTime.Now);
				         				TaskErr = ERROR.AuthenticationError;
				         				return;
				         			}
				         		}
				         	}
				         });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				classicTask.ContinueWith((x) =>
				                         {
				                         	//Mouse.OverrideCursor = null;

				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         }); //TaskScheduler.FromCurrentSynchronizationContext()

				classicTask.Start();
			}

			return;
		}

		#endregion Commands

		#region IUserDialogViewModel Implementation

		[XmlIgnore]
		public bool IsModal { get; private set; }
		
		public virtual void RequestClose()
		{
			if (this.OnCloseRequest != null)
				OnCloseRequest(this);
			else
				Close();
		}

		public event EventHandler DialogClosing;

		public ICommand OKCommand { get { return new RelayCommand(Ok); } }

		protected virtual void Ok()
		{
			if (this.OnOk != null)
				this.OnOk(this);
			else
				Close();
		}

		public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }

		protected virtual void Cancel()
		{
			if (this.OnCancel != null)
				this.OnCancel(this);
			else
				Close();
		}

		public ICommand AuthCommand { get { return new RelayCommand(Auth); } }

		protected virtual void Auth()
		{
			if (this.OnAuth != null)
				this.OnAuth(this);
			else
				Close();
		}

		[XmlIgnore]
		public Action<MifareClassicSetupViewModel> OnOk { get; set; }

		[XmlIgnore]
		public Action<MifareClassicSetupViewModel> OnCancel { get; set; }

		[XmlIgnore]
		public Action<MifareClassicSetupViewModel> OnAuth { get; set; }

		[XmlIgnore]
		public Action<MifareClassicSetupViewModel> OnCloseRequest { get; set; }

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

		#region Localization

		/// <summary>
		/// localization strings
		/// </summary>
		public string LocalizationResourceSet { get; set; }
		
		[XmlIgnore]
		public string Caption
		{
			get { return _Caption; }
			set
			{
				_Caption = value;
				RaisePropertyChanged("Caption");
			}
		} private string _Caption;

		#endregion Localization

		#region Extensions

		/// <summary>
		/// turns a given byte or string sector trailer to a access bits selection
		/// </summary>
		/// <param name="st"></param>
		/// <param name="_sector"></param>
		/// <returns></returns>
		private bool decodeSectorTrailer(byte[] st, ref MifareClassicSectorModel _sector)
		{
			uint C1x, C2x;

			LibLogicalAccess.SectorAccessBits sab;

			uint tmpAccessBitCx;

			if (CustomConverter.SectorTrailerHasWrongFormat(st))
			{
				_sector = null;
				return true;
			}

			#region getAccessBitsForSectorTrailer

			C1x = st[1];
			C2x = st[2];

			C1x &= 0xF0;
			C1x >>= 7;
			C1x &= 0x01;

			sab.d_sector_trailer_access_bits.c1 = (short)C1x;

			C2x >>= 2;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 1;
			tmpAccessBitCx &= 0x01;

			sab.d_sector_trailer_access_bits.c2 = (short)tmpAccessBitCx;

			C1x |= C2x;
			C2x >>= 3;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 2;
			tmpAccessBitCx &= 0x01;

			sab.d_sector_trailer_access_bits.c3 = (short)tmpAccessBitCx;

			C1x &= 0x03;
			C1x |= C2x;
			C1x &= 0x07; //now we have C1³ + C2³ + C3³ as integer in C1x see mifare manual
//
			//            if (C1x == 4)
			//                isTransportConfiguration = true;
			//            else
			//                isTransportConfiguration = false;

			_sector = sectorTrailer_AccessBits[(int)C1x];

			#endregion getAccessBitsForSectorTrailer

			#region getAccessBitsForDataBlock2

			C1x = st[1];
			C2x = st[2];

			C1x &= 0xF0;
			C1x >>= 6;
			C1x &= 0x01;

			sab.d_data_block2_access_bits.c1 = (short)C1x;

			C2x >>= 1;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 1;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block2_access_bits.c2 = (short)tmpAccessBitCx;

			C1x |= C2x;
			//C2 &= 0xF8;
			C2x >>= 3;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 2;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block2_access_bits.c3 = (short)tmpAccessBitCx;

			C1x &= 0x03;
			C1x |= C2x;
			C1x &= 0x07;

			//if(isTransportConfiguration)
			//	decodedBlock2AccessBits = dataBlockABs[C1x];
			//else
			//	decodedBlock2AccessBits = dataBlockAB[C1x];

			dataBlock2 = dataBlock_AccessBits[(int)C1x];
			//dataBlock2_AccessCondition = new DataBlock_AccessCondition(2, sab.d_data_block2_access_bits.c1, sab.d_data_block2_access_bits.c2, sab.d_data_block2_access_bits.c3);

			#endregion getAccessBitsForDataBlock2

			#region getAccessBitsForDataBlock1

			C1x = st[1];
			C2x = st[2];

			C1x &= 0xF0;
			C1x >>= 5;
			C1x &= 0x01;

			sab.d_data_block1_access_bits.c1 = (short)C1x;

			C1x |= C2x;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 1;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block1_access_bits.c2 = (short)tmpAccessBitCx;

			C2x >>= 3;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 2;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block1_access_bits.c3 = (short)tmpAccessBitCx;

			C1x &= 0x03;
			C1x |= C2x;
			C1x &= 0x07;

			//if(isTransportConfiguration)
			//	decodedBlock1AccessBits = dataBlockABs[C1x];
			//else
			//	decodedBlock1AccessBits = dataBlockAB[C1x];
			dataBlock1 = dataBlock_AccessBits[(int)C1x];

			#endregion getAccessBitsForDataBlock1

			#region getAccessBitsForDataBlock0

			C1x = st[1];
			C2x = st[2];

			C1x &= 0xF0;
			C1x >>= 4;
			C1x &= 0x01;

			tmpAccessBitCx = C1x;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block0_access_bits.c1 = (short)C1x;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block0_access_bits.c2 = (short)C1x;

			C2x <<= 1;
			C1x |= C2x;
			C2x >>= 3;

			tmpAccessBitCx = C2x;
			tmpAccessBitCx >>= 2;
			tmpAccessBitCx &= 0x01;

			sab.d_data_block0_access_bits.c3 = (short)C1x;

			C2x &= 0xFC;
			C1x &= 0x03;
			C1x |= C2x;
			C1x &= 0x07;

			dataBlock0 = dataBlock_AccessBits[(int)C1x];

			if (dataBlock0 == dataBlock1 && dataBlock1 == dataBlock2)
			{
				dataBlockCombined = dataBlock0;
				Selected_DataBlockType = SectorTrailer_DataBlock.BlockAll;
				DataBlockIsCombinedToggleButtonIsChecked = true;
			}
			else
			{
				Selected_DataBlockType = SectorTrailer_DataBlock.Block0;
				DataBlockIsCombinedToggleButtonIsChecked = false;
			}

			#endregion getAccessBitsForDataBlock0

			return false;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="st"></param>
		/// <param name="_sector"></param>
		/// <returns></returns>
		private bool decodeSectorTrailer(string st, ref MifareClassicSectorModel _sector)
		{
			byte[] _bytes = new byte[255];
			int discarded = 0;

			string[] sectorTrailer = st.Split(new[] { ',', ';' });
			if (sectorTrailer.Count() != 3 ||
			    !(CustomConverter.IsInHexFormat(sectorTrailer[1]) && sectorTrailer[1].Length == 8) ||
			    !(CustomConverter.IsInHexFormat(sectorTrailer[0]) && sectorTrailer[0].Length == 12) ||
			    !(CustomConverter.IsInHexFormat(sectorTrailer[2]) && sectorTrailer[2].Length == 12))
				return true;

			_bytes = CustomConverter.GetBytes(sectorTrailer[1], out discarded);

			if (!decodeSectorTrailer(_bytes, ref _sector))
				return false;
			else
				return true;
		}

		/// <summary>
		/// Converts a given selection for either sector
		/// access bits or datablock access bits to the equivalent 3 bytes sector trailer
		/// </summary>
		/// <param name="_sector"></param>
		/// <returns></returns>
		private bool encodeSectorTrailer(ref MifareClassicSectorModel _sector)
		{
			byte[] st = new byte[4] { 0x00, 0x00, 0x00, 0xC3 };

			uint sectorAccessBitsIndex = (uint)_sector.Cx; //_sector.Sector_AccessCondition.Cx;
			uint dataBlock0AccessBitsIndex = (uint)_sector.DataBlock[0].Cx; //(uint)dataBlock_AccessBits.IndexOf(_sector); //_sector.DataBlock0_AccessCondition.Cx;
			uint dataBlock1AccessBitsIndex = (uint)_sector.DataBlock[1].Cx; //_sector.DataBlock1_AccessCondition.Cx;
			uint dataBlock2AccessBitsIndex = (uint)_sector.DataBlock[2].Cx; //_sector.DataBlock2_AccessCondition.Cx;

			// DataBlock 0 = C1/0; C2/0; C3/0

			st[1] |= (byte)((dataBlock0AccessBitsIndex & 0x01) << 4);   // C1/0
			st[2] |= (byte)((dataBlock0AccessBitsIndex & 0x02) >> 1);   // C2/0
			st[2] |= (byte)((dataBlock0AccessBitsIndex & 0x04) << 2);   // C3/0

			// DataBlock 1 = C1/1; C2/1; C3/1

			st[1] |= (byte)((dataBlock1AccessBitsIndex & 0x01) << 5);   // C1/1
			st[2] |= (byte)(dataBlock1AccessBitsIndex & 0x02);          // C2/1
			st[2] |= (byte)((dataBlock1AccessBitsIndex & 0x04) << 3);   // C3/1

			// DataBlock 2 = C1/2; C2/2; C3/2

			st[1] |= (byte)((dataBlock2AccessBitsIndex & 0x01) << 6);   // C1/2
			st[2] |= (byte)((dataBlock2AccessBitsIndex & 0x02) << 1);   // C2/2
			st[2] |= (byte)((dataBlock2AccessBitsIndex & 0x04) << 4);   // C3/2

			// SectorAccessBits = C1/3; C2/3; C3/3

			st[1] |= (byte)((sectorAccessBitsIndex & 0x01) << 7);   // C1/3
			st[2] |= (byte)((sectorAccessBitsIndex & 0x02) << 2);   // C2/3
			st[2] |= (byte)((sectorAccessBitsIndex & 0x04) << 5);   // C3/3

			st = CustomConverter.buildSectorTrailerInvNibble(st);
			string[] stAsString;

			if (!string.IsNullOrWhiteSpace(_sector.AccessBitsAsString))
				stAsString = _sector.AccessBitsAsString.Split(new[] { ',', ';' });
			else
				stAsString = new string[] { "FFFFFFFFFFFF", "FF0780C3", "FFFFFFFFFFFF" };

			stAsString[1] = CustomConverter.HexToString(st);
			_sector.AccessBitsAsString = string.Join(",", stAsString);
			return false;
		}

		
		#endregion Extensions
	}
}