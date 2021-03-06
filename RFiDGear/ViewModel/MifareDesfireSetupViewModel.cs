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
using LibLogicalAccess;
using RFiDGear.DataAccessLayer;
using RFiDGear.Model;
using RFiDGear.ViewModel;
using System;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace RFiDGear.ViewModel
{
	/// <summary>
	/// Description of MifareDesfireSetupViewModel.
	/// </summary>
	public class MifareDesfireSetupViewModel : ViewModelBase, IUserDialogViewModel
	{
		private SettingsReaderWriter settings = new SettingsReaderWriter();

		private MifareDesfireChipModel chip;
		private MifareDesfireAppModel app;
		private DESFireAccessRights accessRights;

		public ERROR TaskErr { get; set; }

		/// <summary>
		///
		/// </summary>
		public MifareDesfireSetupViewModel()
		{
			accessRights = new DESFireAccessRights();
			chip = new MifareDesfireChipModel(string.Format("Task Description: {0}", SelectedTaskDescription), CARD_TYPE.DESFire);
			app = new MifareDesfireAppModel(0);
			
			childNodeViewModelFromChip = new RFiDChipChildLayerViewModel(app, null, CARD_TYPE.DESFire, new ObservableCollection<IDialogViewModel>(), true);
			childNodeViewModelTemp = new RFiDChipChildLayerViewModel(app, null, CARD_TYPE.DESFire, new ObservableCollection<IDialogViewModel>(), true);
			
			MifareDesfireKeys = CustomConverter.GenerateStringSequence(0,16).ToArray();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="_selectedSetupViewModel"></param>
		/// <param name="_dialogs"></param>
		public MifareDesfireSetupViewModel(object _selectedSetupViewModel, ObservableCollection<IDialogViewModel> _dialogs)
		{
			//dialogs = _dialogs;

			try
			{
				MefHelper.Instance.Container.ComposeParts(this); //Load Plugins
				
				chip = new MifareDesfireChipModel(string.Format("Task Description: {0}", ""), CARD_TYPE.DESFire);
				app = new MifareDesfireAppModel(0);
				
				childNodeViewModelFromChip = new RFiDChipChildLayerViewModel(app, null, CARD_TYPE.DESFire, _dialogs, true);
				childNodeViewModelTemp = new RFiDChipChildLayerViewModel(app, null, CARD_TYPE.DESFire, _dialogs, true);
				
				MifareDesfireKeys = CustomConverter.GenerateStringSequence(0,16).ToArray();
				
				if(_selectedSetupViewModel is MifareDesfireSetupViewModel)
				{
					PropertyInfo[] properties = typeof(MifareDesfireSetupViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

					foreach (PropertyInfo p in properties.Where(x => x.PropertyType != items.GetType()))
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
					DesfireMasterKeyCurrent = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardCardMasterKey).Key;
					SelectedDesfireMasterKeyEncryptionTypeCurrent = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardCardMasterKey).EncryptionType;

					DesfireMasterKeyTarget = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardApplicationMasterKey).Key;
					SelectedDesfireMasterKeyEncryptionTypeTarget = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardCardMasterKey).EncryptionType;

					DesfireAppKeyCurrent = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardApplicationMasterKey).Key;
					SelectedDesfireAppKeyEncryptionTypeCurrent = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardApplicationMasterKey).EncryptionType;
					SelectedDesfireAppKeyNumberCurrent = "0";
					
					DesfireAppKeyTarget = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardApplicationMasterKey).Key;
					SelectedDesfireAppKeyEncryptionTypeTarget = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardApplicationMasterKey).EncryptionType;

					DesfireReadKeyCurrent = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardReadKey).Key;
					SelectedDesfireReadKeyNumber = "1";
					
					DesfireWriteKeyCurrent = settings.DefaultSpecification.MifareDesfireDefaultSecuritySettings.First(x => x.KeyType == KeyType_MifareDesFireKeyType.DefaultDesfireCardWriteKey).Key;
					SelectedDesfireWriteKeyNumber = "1";
					
					accessRights = new DESFireAccessRights();
					
					AppNumberNew = "1";
					AppNumberCurrent = "0";
					AppNumberTarget = "0";
					
					SelectedDesfireAppKeyNumberTarget = "1";
					SelectedDesfireAppMaxNumberOfKeys = "1";

					IsValidDesfireMasterKeyCurrent = null;
					IsValidDesfireMasterKeyTarget = null;

					IsValidDesfireAppKeyCurrent = null;
					IsValidDesfireAppKeyTarget = null;

					IsValidAppNumberCurrent = null;
					IsValidAppNumberTarget = null;
					IsValidAppNumberNew = null;

					IsValidDesfireReadKeyCurrent = null;
					IsValidDesfireWriteKeyCurrent = null;
					
					SelectedTaskIndex = "0";
					SelectedTaskDescription = "Enter a Description for this Task";
					
					FileNumberCurrent = "0";
					FileSizeCurrent = "16";
					
					IsValidFileNumberCurrent = null;
					IsValidFileSizeCurrent = null;
					
					IsDesfireFileAuthoringTabEnabled = false;
					IsDataExplorerEditTabEnabled = false;
					IsDesfirePICCAuthoringTabEnabled = false;
					IsDesfireAuthenticationTabEnabled = false;
					IsDesfireAppAuthenticationTabEnabled = false;
					IsDesfireAppAuthoringTabEnabled = false;
					IsDesfireAppCreationTabEnabled = false;
				}
				
				HasPlugins = items != null ? items.Any() : false;
				
				if (HasPlugins)
					SelectedPlugin = Items.FirstOrDefault();
				
			}
			catch (Exception e)
			{
				
			}

			
		}

		
		#region Dialogs
		[XmlIgnore]
		public ObservableCollection<IDialogViewModel> Dialogs { get { return dialogs; }}
		private ObservableCollection<IDialogViewModel> dialogs = new ObservableCollection<IDialogViewModel>();
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

		/// <summary>
		/// 
		/// </summary>
//		[XmlIgnore]
//		private ObservableCollection<IDialogViewModel> dialogs = new ObservableCollection<IDialogViewModel>();
//		public ObservableCollection<IDialogViewModel> Dialogs
//		{
//			get { return dialogs; }
//			set { dialogs = value; }
//		}
		
		#endregion Dialogs
		
		#region Key Properties Card Master

		[XmlIgnore]
		public string[] MifareDesfireKeys { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public DESFireKeyType SelectedDesfireMasterKeyEncryptionTypeCurrent
		{
			get { return selectedDesfireMasterKeyEncryptionTypeCurrent; }
			set
			{
				selectedDesfireMasterKeyEncryptionTypeCurrent = value;
				RaisePropertyChanged("SelectedDesfireMasterKeyEncryptionTypeCurrent");
			}
		}
		private DESFireKeyType selectedDesfireMasterKeyEncryptionTypeCurrent;

		/// <summary>
		/// 
		/// </summary>
		public string DesfireMasterKeyCurrent
		{
			get { return desfireMasterKeyCurrent; }
			set
			{
				try
				{
					desfireMasterKeyCurrent = value.ToUpper().Remove(32);
				}
				catch
				{
					desfireMasterKeyCurrent = value.ToUpper();
				}
				IsValidDesfireMasterKeyCurrent = (CustomConverter.IsInHexFormat(desfireMasterKeyCurrent) && desfireMasterKeyCurrent.Length == 32);
				RaisePropertyChanged("DesfireMasterKeyCurrent");
			}
		}
		private string desfireMasterKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidDesfireMasterKeyCurrent
		{
			get
			{
				return isValidDesfireMasterKeyCurrent;
			}
			set
			{
				isValidDesfireMasterKeyCurrent = value;
				RaisePropertyChanged("IsValidDesfireMasterKeyCurrent");
			}
		}
		private bool? isValidDesfireMasterKeyCurrent;

		/// <summary>
		/// 
		/// </summary>
		public DESFireKeyType SelectedDesfireMasterKeyEncryptionTypeTarget
		{
			get { return selectedDesfireMasterKeyEncryptionTypeTarget; }
			set
			{
				selectedDesfireMasterKeyEncryptionTypeTarget = value;
				RaisePropertyChanged("SelectedDesfireMasterKeyEncryptionTypeCurrent");
			}
		}
		private DESFireKeyType selectedDesfireMasterKeyEncryptionTypeTarget;

		/// <summary>
		///
		/// </summary>
		public string DesfireMasterKeyTarget
		{
			get { return desfireMasterKeyTarget; }
			set
			{
				try
				{
					desfireMasterKeyTarget = value.ToUpper().Remove(32);
				}
				catch
				{
					desfireMasterKeyTarget = value.ToUpper();
				}

				IsValidDesfireMasterKeyTarget = (
					CustomConverter.IsInHexFormat(desfireMasterKeyTarget) &&
					desfireMasterKeyTarget.Length == 32);
				RaisePropertyChanged("DesfireMasterKeyTarget");
			}
		}
		private string desfireMasterKeyTarget;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidDesfireMasterKeyTarget
		{
			get
			{
				return isValidDesfireMasterKeyTarget;
			}
			set
			{
				isValidDesfireMasterKeyTarget = value;
				RaisePropertyChanged("IsValidDesfireMasterKeyTarget");
			}
		}
		private bool? isValidDesfireMasterKeyTarget;

		#endregion Key Properties Card Master

		#region Key Properties App Master

		#region App Creation

		/// <summary>
		///
		/// </summary>
		public DESFireKeyType SelectedDesfireAppKeyEncryptionTypeCreateNewApp
		{
			get { return selectedCreateDesfireAppKeyEncryptionTypeCurrent; }
			set
			{
				selectedCreateDesfireAppKeyEncryptionTypeCurrent = value;
				RaisePropertyChanged("SelectedDesfireAppKeyEncryptionTypeCreateNewApp");
			}
		}
		private DESFireKeyType selectedCreateDesfireAppKeyEncryptionTypeCurrent;

		/// <summary>
		///
		/// </summary>
		public string SelectedDesfireAppMaxNumberOfKeys
		{
			get
			{
				return selectedDesfireAppMaxNumberOfKeys;
			}
			set
			{
				if(int.TryParse(value, out selectedDesfireAppMaxNumberOfKeysAsInt))
				{
					selectedDesfireAppMaxNumberOfKeys = value;
				}
				RaisePropertyChanged("SelectedDesfireAppMaxNumberOfKeys");
			}
		}
		private string selectedDesfireAppMaxNumberOfKeys;
		private int selectedDesfireAppMaxNumberOfKeysAsInt;

		/// <summary>
		///
		/// </summary>
		public AccessCondition_MifareDesfireAppCreation SelectedDesfireAppKeySettingsCreateNewApp
		{
			get { return selectedDesfireAppKeySettingsTarget; }
			set
			{
				selectedDesfireAppKeySettingsTarget = value;
				//selectedDesfireAppKeySettingsTarget |= DESFireKeySettings.KS_ALLOW_CHANGE_MK;
				RaisePropertyChanged("SelectedDesfireAppKeySettingsCreateNewApp");
			}
		}
		private AccessCondition_MifareDesfireAppCreation selectedDesfireAppKeySettingsTarget;

		/// <summary>
		///
		/// </summary>
		public string AppNumberNew
		{
			get { return appNumberNew; }
			set
			{
				try
				{
					appNumberNew = value.ToUpper().Remove(32);
				}
				catch
				{
					appNumberNew = value.ToUpper();
				}
				IsValidAppNumberNew = (int.TryParse(value, out appNumberNewAsInt) && appNumberNewAsInt <= 0xFFFFFF);
				RaisePropertyChanged("AppNumberNew");
			}
		}
		private string appNumberNew;

		/// <summary>
		///
		/// </summary>
		public int AppNumberNewAsInt
		{ get { return appNumberNewAsInt; } }
		private int appNumberNewAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidAppNumberNew
		{
			get
			{
				return isValidAppNumberNew;
			}
			set
			{
				isValidAppNumberNew = value;
				RaisePropertyChanged("IsValidAppNumberNew");
			}
		}
		private bool? isValidAppNumberNew;

		/// <summary>
		///
		/// </summary>
		public bool IsAllowChangeMKChecked
		{
			get { return isAllowChangeMKChecked; }
			set
			{
				isAllowChangeMKChecked = value;
				RaisePropertyChanged("IsAllowChangeMKChecked");
			}
		}
		private bool isAllowChangeMKChecked;

		/// <summary>
		///
		/// </summary>
		public bool IsAllowListingWithoutMKChecked
		{
			get { return isAllowListingWithoutMKChecked; }
			set
			{
				isAllowListingWithoutMKChecked = value;
				RaisePropertyChanged("IsAllowListingWithoutMKChecked");
			}
		}
		private bool isAllowListingWithoutMKChecked;

		/// <summary>
		///
		/// </summary>
		public bool IsAllowCreateDelWithoutMKChecked
		{
			get { return isAllowCreateDelWithoutMKChecked; }
			set
			{
				isAllowCreateDelWithoutMKChecked = value;
				RaisePropertyChanged("IsAllowCreateDelWithoutMKChecked");
			}
		}
		private bool isAllowCreateDelWithoutMKChecked;

		/// <summary>
		///
		/// </summary>
		public bool IsAllowConfigChangableChecked
		{
			get { return isAllowConfigChangableChecked; }
			set
			{
				isAllowConfigChangableChecked = value;
				RaisePropertyChanged("IsAllowConfigChangableChecked");
			}
		}
		private bool isAllowConfigChangableChecked;

		#endregion App Creation

		#region Key Properties App Master Current

		/// <summary>
		///
		/// </summary>
		public DESFireKeyType SelectedDesfireAppKeyEncryptionTypeCurrent
		{
			get { return selectedDesfireAppKeyEncryptionTypeCurrent; }
			set
			{
				selectedDesfireAppKeyEncryptionTypeCurrent = value;
				RaisePropertyChanged("SelectedDesfireAppKeyEncryptionTypeCurrent");
			}
		} private DESFireKeyType selectedDesfireAppKeyEncryptionTypeCurrent;

		/// <summary>
		///
		/// </summary>
		public string SelectedDesfireAppKeyNumberCurrent
		{
			get
			{
				return selectedDesfireAppKeyNumberCurrent;
			}
			set
			{
				if(int.TryParse(value, out selectedDesfireAppKeyNumberCurrentAsInt))
				{
					selectedDesfireAppKeyNumberCurrent = value;
				}
				RaisePropertyChanged("SelectedDesfireAppKeyNumberCurrent");
			}
		}
		private string selectedDesfireAppKeyNumberCurrent;
		private int selectedDesfireAppKeyNumberCurrentAsInt;

		/// <summary>
		///
		/// </summary>
		public string DesfireAppKeyCurrent
		{
			get { return desfireAppKeyCurrent; }
			set
			{
				try
				{
					desfireAppKeyCurrent = value.ToUpper().Remove(32);
				}
				catch
				{
					desfireAppKeyCurrent = value.ToUpper();
				}
				IsValidDesfireAppKeyCurrent = (CustomConverter.IsInHexFormat(desfireAppKeyCurrent) && desfireAppKeyCurrent.Length == 32);
				RaisePropertyChanged("DesfireAppKeyCurrent");
			}
		} private string desfireAppKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidDesfireAppKeyCurrent
		{
			get
			{
				return isValidDesfireAppKeyCurrent;
			}
			set
			{
				isValidDesfireAppKeyCurrent = value;
				RaisePropertyChanged("IsValidDesfireAppKeyCurrent");
			}
		} private bool? isValidDesfireAppKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public string AppNumberCurrent
		{
			get { return appNumberCurrent; }
			set
			{
				try
				{
					appNumberCurrent = value.ToUpper().Remove(32);
				}
				catch
				{
					appNumberCurrent = value.ToUpper();
				}
				IsValidAppNumberCurrent = (int.TryParse(value, out appNumberCurrentAsInt) && appNumberCurrentAsInt <= 65535);
				RaisePropertyChanged("AppNumberCurrent");
			}
		} private string appNumberCurrent;

		/// <summary>
		///
		/// </summary>
		public int AppNumberCurrentAsInt
		{ get { return appNumberCurrentAsInt; } } private int appNumberCurrentAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidAppNumberCurrent
		{
			get
			{
				return isValidAppNumberCurrent;
			}
			set
			{
				isValidAppNumberCurrent = value;
				RaisePropertyChanged("IsValidAppNumberCurrent");
			}
		} private bool? isValidAppNumberCurrent;

		#endregion Key Properties App Master Current

		#region Key Properties App Master Target

		/// <summary>
		///
		/// </summary>
		public DESFireKeyType SelectedDesfireAppKeyEncryptionTypeTarget
		{
			get { return selectedDesfireAppKeyEncryptionTypeTarget; }
			set
			{
				selectedDesfireAppKeyEncryptionTypeTarget = value;
				RaisePropertyChanged("SelectedDesfireAppKeyEncryptionTypeTarget");
			}
		} private DESFireKeyType selectedDesfireAppKeyEncryptionTypeTarget;

		/// <summary>
		///
		/// </summary>
		public string SelectedDesfireAppKeyNumberTarget
		{
			get
			{
				return selectedDesfireAppKeyNumberTarget;
			}
			set
			{
				if(int.TryParse(value, out selectedDesfireAppKeyNumberTargetAsInt))
				{
					selectedDesfireAppKeyNumberTarget = value;
				}
				RaisePropertyChanged("SelectedDesfireAppKeyNumberTarget");
			}
		}
		private string selectedDesfireAppKeyNumberTarget;
		private int selectedDesfireAppKeyNumberTargetAsInt;

		/// <summary>
		///
		/// </summary>
		public string DesfireAppKeyTarget
		{
			get { return desfireAppKeyTarget; }
			set
			{
				try
				{
					desfireAppKeyTarget = value.ToUpper().Remove(32);
				}
				catch
				{
					desfireAppKeyTarget = value.ToUpper();
				}

				IsValidDesfireAppKeyTarget = (
					CustomConverter.IsInHexFormat(desfireAppKeyTarget) &&
					desfireAppKeyTarget.Length == 32);
				RaisePropertyChanged("DesfireAppKeyTarget");
			}
		} private string desfireAppKeyTarget;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidDesfireAppKeyTarget
		{
			get
			{
				return isValidDesfireAppKeyTarget;
			}
			set
			{
				isValidDesfireAppKeyTarget = value;
				RaisePropertyChanged("IsValidDesfireAppKeyTarget");
			}
		} private bool? isValidDesfireAppKeyTarget;

		/// <summary>
		///
		/// </summary>
		public string AppNumberTarget
		{
			get { return appNumberTarget; }
			set
			{
				try
				{
					appNumberTarget = value.ToUpper().Remove(32);
				}
				catch
				{
					appNumberTarget = value.ToUpper();
				}
				IsValidAppNumberTarget = (int.TryParse(value, out appNumberTargetAsInt) && appNumberTargetAsInt <= 65535);
				RaisePropertyChanged("AppNumberTarget");
			}
		} private string appNumberTarget;

		/// <summary>
		///
		/// </summary>
		public int AppNumberTargetAsInt
		{ get { return appNumberTargetAsInt; } } private int appNumberTargetAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidAppNumberTarget
		{
			get
			{
				return isValidAppNumberTarget;
			}
			set
			{
				isValidAppNumberTarget = value;
				RaisePropertyChanged("IsValidAppNumberTarget");
			}
		} private bool? isValidAppNumberTarget;

		#endregion Key Properties App Master Target

		#endregion Key Properties App Master

		#region Key Properties File Master

		#region File Creation

		/// <summary>
		///
		/// </summary>
		public TaskAccessRights SelectedDesfireFileAccessRightReadWrite
		{
			get { return selectedDesfireFileAccessRightReadWrite; }
			set
			{
				selectedDesfireFileAccessRightReadWrite = value;
				RaisePropertyChanged("SelectedDesfireFileAccessRightReadWrite");
			}
		} private TaskAccessRights selectedDesfireFileAccessRightReadWrite;

		/// <summary>
		///
		/// </summary>
		public TaskAccessRights SelectedDesfireFileAccessRightChange
		{
			get { return selectedDesfireFileAccessRightChange; }
			set
			{
				selectedDesfireFileAccessRightChange = value;
				RaisePropertyChanged("SelectedDesfireFileAccessRightChange");
			}
		} private TaskAccessRights selectedDesfireFileAccessRightChange;

		/// <summary>
		///
		/// </summary>
		public TaskAccessRights SelectedDesfireFileAccessRightRead
		{
			get { return selectedDesfireFileAccessRightRead; }
			set
			{
				selectedDesfireFileAccessRightRead = value;
				RaisePropertyChanged("SelectedDesfireFileAccessRightRead");
			}
		} private TaskAccessRights selectedDesfireFileAccessRightRead;

		/// <summary>
		///
		/// </summary>
		public TaskAccessRights SelectedDesfireFileAccessRightWrite
		{
			get { return selectedDesfireFileAccessRightWrite; }
			set
			{
				selectedDesfireFileAccessRightWrite = value;
				RaisePropertyChanged("SelectedDesfireFileAccessRightWrite");
			}
		} private TaskAccessRights selectedDesfireFileAccessRightWrite;

		/// <summary>
		///
		/// </summary>
		public EncryptionMode SelectedDesfireFileCryptoMode
		{
			get { return selectedDesfireFileCryptoMode; }
			set
			{
				selectedDesfireFileCryptoMode = value;
				RaisePropertyChanged("SelectedDesfireFileCryptoMode");
			}
		} private EncryptionMode selectedDesfireFileCryptoMode;

		/// <summary>
		///
		/// </summary>
		public FileType_MifareDesfireFileType SelectedDesfireFileType
		{
			get { return selectedDesfireFileType; }
			set
			{
				selectedDesfireFileType = value;
				RaisePropertyChanged("SelectedDesfireFileType");
			}
		} private FileType_MifareDesfireFileType selectedDesfireFileType;

		/// <summary>
		///
		/// </summary>
		public string FileNumberCurrent
		{
			get { return fileNumberCurrent; }
			set
			{
				fileNumberCurrent = value;
				IsValidFileNumberCurrent = (int.TryParse(value, out fileNumberCurrentAsInt) && fileNumberCurrentAsInt <= 8000);
				RaisePropertyChanged("FileNumberCurrent");
			}
		} private string fileNumberCurrent;

		/// <summary>
		///
		/// </summary>
		public int FileNumberCurrentAsInt
		{ get { return fileNumberCurrentAsInt; } } private int fileNumberCurrentAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidFileNumberCurrent
		{
			get
			{
				return isValidFileNumberCurrent;
			}
			set
			{
				isValidFileNumberCurrent = value;
				RaisePropertyChanged("IsValidFileNumberCurrent");
			}
		} private bool? isValidFileNumberCurrent;

		/// <summary>
		///
		/// </summary>
		public string FileSizeCurrent
		{
			get { return fileSizeCurrent; }
			set
			{
				fileSizeCurrent = value;
				IsValidFileSizeCurrent = (int.TryParse(value, out fileSizeCurrentAsInt) && fileSizeCurrentAsInt <= 8000);
				
				if(IsValidFileSizeCurrent != false)
				{
					if(childNodeViewModelFromChip.Children.Any(x => x.DesfireFile != null))
					{
						try
						{
							childNodeViewModelFromChip.Children.Single(y => y.DesfireFile.FileID == FileNumberCurrentAsInt).DesfireFile = new MifareDesfireFileModel(new byte[FileSizeCurrentAsInt],0);
							childNodeViewModelTemp.Children.Single(y => y.DesfireFile.FileID == FileNumberCurrentAsInt).DesfireFile = new MifareDesfireFileModel(new byte[FileSizeCurrentAsInt],0);
						}
						catch
						{
							
						}
						
					}
					else
					{
						childNodeViewModelFromChip.Children.Add(new RFiDChipGrandChildLayerViewModel(new MifareDesfireFileModel(new byte[FileSizeCurrentAsInt],0), null));
						childNodeViewModelTemp.Children.Add(new RFiDChipGrandChildLayerViewModel(new MifareDesfireFileModel(new byte[FileSizeCurrentAsInt],0), null));
					}
				}
				
				RaisePropertyChanged("FileSizeCurrent");
			}
		} private string fileSizeCurrent;

		/// <summary>
		///
		/// </summary>
		public int FileSizeCurrentAsInt
		{ get { return fileSizeCurrentAsInt; } } private int fileSizeCurrentAsInt;

		/// <summary>
		///
		/// </summary>
		public bool? IsValidFileSizeCurrent
		{
			get
			{
				return isValidFileSizeCurrent;
			}
			set
			{
				isValidFileSizeCurrent = value;
				RaisePropertyChanged("IsValidFileSizeCurrent");
			}
		} private bool? isValidFileSizeCurrent;

		#endregion File Creation

		#endregion Key Properties File Master

		#region DataExplorer
		
		/// <summary>
		///
		/// </summary>
		public RFiDChipChildLayerViewModel ChildNodeViewModelFromChip
		{
			get { return childNodeViewModelFromChip; }
			set { childNodeViewModelFromChip = value; }
		} private RFiDChipChildLayerViewModel childNodeViewModelFromChip;

		/// <summary>
		///
		/// </summary>
		public RFiDChipChildLayerViewModel ChildNodeViewModelTemp
		{
			get { return childNodeViewModelTemp; }
			set {
				childNodeViewModelTemp = value;
				RaisePropertyChanged("ChildNodeViewModelTemp");
			}
		} private RFiDChipChildLayerViewModel childNodeViewModelTemp;
		
		/// <summary>
		///
		/// </summary>
		public RFiDChipGrandChildLayerViewModel GrandChildNodeViewModel
		{
			get { return ChildNodeViewModelTemp.Children.Count > 0 ? ChildNodeViewModelTemp.Children.Single(x => x.DesfireFile != null) : null; }
		}
		
		/// <summary>
		///
		/// </summary>
		public string DesfireReadKeyCurrent
		{
			get { return desfireReadKeyCurrent; }
			set
			{
				try
				{
					desfireReadKeyCurrent = value.ToUpper().Remove(32);
				}
				catch
				{
					desfireReadKeyCurrent = value.ToUpper();
				}

				IsValidDesfireReadKeyCurrent = (
					CustomConverter.IsInHexFormat(desfireReadKeyCurrent) &&
					desfireReadKeyCurrent.Length == 32);
				
				RaisePropertyChanged("DesfireReadKeyCurrent");
			}
		} private string desfireReadKeyCurrent;
		
		/// <summary>
		///
		/// </summary>
		public string SelectedDesfireReadKeyNumber
		{
			get
			{
				return  selectedDesfireReadKeyNumber;
			}
			set
			{
				if(int.TryParse(value, out selectedDesfireReadKeyNumberAsInt))
				{
					selectedDesfireReadKeyNumber = value;
				}
				RaisePropertyChanged("SelectedDesfireReadKeyNumber");
			}
		}
		private string  selectedDesfireReadKeyNumber;
		private int selectedDesfireReadKeyNumberAsInt;
		
		/// <summary>
		///
		/// </summary>
		public bool? IsValidDesfireReadKeyCurrent
		{
			get
			{
				return isValidDesfireReadKeyCurrent;
			}
			set
			{
				isValidDesfireReadKeyCurrent = value;
				RaisePropertyChanged("IsValidDesfireReadKeyCurrent");
			}
		} private bool? isValidDesfireReadKeyCurrent;

		/// <summary>
		///
		/// </summary>
		public string DesfireWriteKeyCurrent
		{
			get { return desfireWriteKeyCurrent; }
			set
			{
				try
				{
					desfireWriteKeyCurrent = value.ToUpper().Remove(32);
				}
				catch
				{
					desfireWriteKeyCurrent = value.ToUpper();
				}

				IsValidDesfireWriteKeyCurrent = (
					CustomConverter.IsInHexFormat(desfireWriteKeyCurrent) &&
					desfireWriteKeyCurrent.Length == 32);
				
				RaisePropertyChanged("DesfireWriteKeyCurrent");
			}
		} private string desfireWriteKeyCurrent;
		
		/// <summary>
		///
		/// </summary>
		public string SelectedDesfireWriteKeyNumber
		{
			get
			{
				return  selectedDesfireWriteKeyNumber;
			}
			set
			{
				if(int.TryParse(value, out selectedDesfireWriteKeyNumberAsInt))
				{
					selectedDesfireWriteKeyNumber = value;
				}
				RaisePropertyChanged("SelectedDesfireWriteKeyNumber");
			}
		}
		private string  selectedDesfireWriteKeyNumber;
		private int selectedDesfireWriteKeyNumberAsInt;
		
		/// <summary>
		///
		/// </summary>
		public bool? IsValidDesfireWriteKeyCurrent
		{
			get
			{
				return isValidDesfireWriteKeyCurrent;
			}
			set
			{
				isValidDesfireWriteKeyCurrent = value;
				RaisePropertyChanged("IsValidDesfireWriteKeyCurrent");
			}
		} private bool? isValidDesfireWriteKeyCurrent;
		
		/// <summary>
		/// 
		/// </summary>
		public DESFireKeyType SelectedDesfireReadKeyEncryptionType
		{
			get { return selectedDesfireReadKeyEncryptionType; }
			set
			{
				selectedDesfireReadKeyEncryptionType = value;
				RaisePropertyChanged("SelectedDesfireReadKeyEncryptionType");
			}
		} private DESFireKeyType selectedDesfireReadKeyEncryptionType;
		
		/// <summary>
		/// 
		/// </summary>
		public DESFireKeyType SelectedDesfireWriteKeyEncryptionType
		{
			get { return selectedDesfireWriteKeyEncryptionType; }
			set
			{
				selectedDesfireWriteKeyEncryptionType = value;
				RaisePropertyChanged("SelectedDesfireWriteKeyEncryptionType");
			}
		} private DESFireKeyType selectedDesfireWriteKeyEncryptionType;
		#endregion
		
		#region general props

		/// <summary>
		/// 
		/// </summary>
		public bool IsDesfireFileAuthoringTabEnabled
		{
			get { return isDesfireFileAuthoringTabEnabled; }
			set {
				isDesfireFileAuthoringTabEnabled = value;
				RaisePropertyChanged("IsDesfireFileAuthoringTabEnabled");
			}
		} private bool isDesfireFileAuthoringTabEnabled;

		/// <summary>
		///
		/// </summary>
		public bool IsDataExplorerEditTabEnabled
		{
			get { return isDataExplorerEditTabEnabled; }
			set {
				isDataExplorerEditTabEnabled = value;
				RaisePropertyChanged("IsDataExplorerEditTabEnabled");
			}
		} private bool isDataExplorerEditTabEnabled;

		/// <summary>
		///
		/// </summary>
		public bool IsDesfirePICCAuthoringTabEnabled
		{
			get { return isDesfirePICCAuthoringTabEnabled; }
			set {
				isDesfirePICCAuthoringTabEnabled = value;
				RaisePropertyChanged("IsDesfirePICCAuthoringTabEnabled");
			}
		} private bool isDesfirePICCAuthoringTabEnabled;
		
		/// <summary>
		///
		/// </summary>
		public bool IsDesfireAuthenticationTabEnabled
		{
			get { return isDesfireAuthenticationTabEnabled; }
			set {
				isDesfireAuthenticationTabEnabled = value;
				RaisePropertyChanged("IsDesfireAuthenticationTabEnabled");
			}
		} private bool isDesfireAuthenticationTabEnabled;

		/// <summary>
		///
		/// </summary>
		public bool IsDesfireAppAuthenticationTabEnabled
		{
			get { return isDesfireAppAuthenticationTabEnabled; }
			set {
				isDesfireAppAuthenticationTabEnabled = value;
				RaisePropertyChanged("IsDesfireAppAuthenticationTabEnabled");
			}
		} private bool isDesfireAppAuthenticationTabEnabled;
		
		/// <summary>
		///
		/// </summary>
		public bool IsDesfireAppAuthoringTabEnabled
		{
			get { return isDesfireAppAuthoringTabEnabled; }
			set {
				isDesfireAppAuthoringTabEnabled = value;
				RaisePropertyChanged("IsDesfireAppAuthoringTabEnabled");
			}
		} private bool isDesfireAppAuthoringTabEnabled;

		/// <summary>
		/// 
		/// </summary>
		public bool IsDesfireAppCreationTabEnabled
		{
			get { return isDesfireAppCreationTabEnabled; }
			set {
				isDesfireAppCreationTabEnabled = value;
				RaisePropertyChanged("IsDesfireAppCreationTabEnabled");
			}
		} private bool isDesfireAppCreationTabEnabled;
		
		/// <summary>
		/// 
		/// </summary>
		public SettingsReaderWriter Settings
		{
			get { return settings; }
		}

		/// <summary>
		///
		/// </summary>
		public TaskType_MifareDesfireTask SelectedTaskType
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
					case TaskType_MifareDesfireTask.None:
						IsDesfireFileAuthoringTabEnabled = false;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = false;
						IsDesfireAppAuthenticationTabEnabled = false;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;

					case TaskType_MifareDesfireTask.ApplicationKeyChangeover:
						IsDesfireFileAuthoringTabEnabled = false;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = true;
						IsDesfireAppAuthenticationTabEnabled = true;
						IsDesfireAppAuthoringTabEnabled = true;
						IsDesfireAppCreationTabEnabled = false;
						break;
						
					case TaskType_MifareDesfireTask.ChangeDefault:
						IsDesfireFileAuthoringTabEnabled = true;
						IsDataExplorerEditTabEnabled = true;
						IsDesfirePICCAuthoringTabEnabled = true;
						IsDesfireAuthenticationTabEnabled = true;
						IsDesfireAppAuthenticationTabEnabled = true;
						IsDesfireAppAuthoringTabEnabled = true;
						IsDesfireAppCreationTabEnabled = true;
						break;
						
					case TaskType_MifareDesfireTask.CreateApplication:
						IsDesfireFileAuthoringTabEnabled = false;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = true;
						IsDesfireAppAuthenticationTabEnabled = false;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = true;
						break;
						
					case TaskType_MifareDesfireTask.CreateFile:
						IsDesfireFileAuthoringTabEnabled = true;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = false;
						IsDesfireAppAuthenticationTabEnabled = true;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;
						
					case TaskType_MifareDesfireTask.DeleteApplication:
						IsDesfireFileAuthoringTabEnabled = false;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = true;
						IsDesfireAppAuthenticationTabEnabled = false;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = true;
						break;

					case TaskType_MifareDesfireTask.DeleteFile:
						IsDesfireFileAuthoringTabEnabled = true;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = false;
						IsDesfireAppAuthenticationTabEnabled = true;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;

					case TaskType_MifareDesfireTask.FormatDesfireCard:
						IsDesfireFileAuthoringTabEnabled = false;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = true;
						IsDesfireAppAuthenticationTabEnabled = false;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;

					case TaskType_MifareDesfireTask.PICCMasterKeyChangeover:
						IsDesfireFileAuthoringTabEnabled = false;
						IsDataExplorerEditTabEnabled = false;
						IsDesfirePICCAuthoringTabEnabled = true;
						IsDesfireAuthenticationTabEnabled = true;
						IsDesfireAppAuthenticationTabEnabled = false;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;
						
					case TaskType_MifareDesfireTask.ReadData:
						IsDesfireFileAuthoringTabEnabled = true;
						IsDataExplorerEditTabEnabled = true;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = false;
						IsDesfireAppAuthenticationTabEnabled = true;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;
						
					case TaskType_MifareDesfireTask.WriteData:
						IsDesfireFileAuthoringTabEnabled = true;
						IsDataExplorerEditTabEnabled = true;
						IsDesfirePICCAuthoringTabEnabled = false;
						IsDesfireAuthenticationTabEnabled = false;
						IsDesfireAppAuthenticationTabEnabled = true;
						IsDesfireAppAuthoringTabEnabled = false;
						IsDesfireAppCreationTabEnabled = false;
						break;
				}
				RaisePropertyChanged("SelectedTaskType");
			}
		}
		private TaskType_MifareDesfireTask selectedAccessBitsTaskType;

		/// <summary>
		///
		/// </summary>
		public string SelectedTaskIndex
		{
			get
			{
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
		public string SelectedTaskDescription
		{
			get
			{
				//classicKeyAKeyCurrent = SectorTrailer.Split(',')[0];
				return selectedAccessBitsTaskDescription;
			}
			set
			{
				selectedAccessBitsTaskDescription = value;
			}
		}
		private string selectedAccessBitsTaskDescription;

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
		}
		private bool isValidSelectedAccessBitsTaskIndex;

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

		#endregion general props

		#region Commands

		/// <summary>
		/// return new RelayCommand<RFiDDevice>((_device) => OnNewCreateAppCommand(_device));
		/// </summary>
		public ICommand CreateAppCommand { get { return new RelayCommand(OnNewCreateAppCommand); } }
		private void OnNewCreateAppCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask =
				new Task(() =>
				         {
				         	using (RFiDDevice device = RFiDDevice.Instance)
				         	{
				         		if (device != null)
				         		{
				         			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

				         			//Mouse.OverrideCursor = Cursors.Wait;

				         			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
				         			{
				         				if (IsValidAppNumberNew != false &&
				         				    device.AuthToMifareDesfireApplication(
				         				    	DesfireMasterKeyCurrent,
				         				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
				         				    	0) == ERROR.NoError)
				         				{
				         					StatusText += string.Format("{0}: Successfully Authenticated to App 0\n", DateTime.Now);

				         					DESFireKeySettings keySettings = DESFireKeySettings.KS_ALLOW_CHANGE_MK;
				         					keySettings = (DESFireKeySettings)SelectedDesfireAppKeySettingsCreateNewApp;

				         					keySettings |= IsAllowChangeMKChecked ? (DESFireKeySettings)1 : (DESFireKeySettings)0;
				         					keySettings |= IsAllowListingWithoutMKChecked ? (DESFireKeySettings)2 : (DESFireKeySettings)0;
				         					keySettings |= IsAllowCreateDelWithoutMKChecked ? (DESFireKeySettings)4 : (DESFireKeySettings)0;
				         					keySettings |= IsAllowConfigChangableChecked ? (DESFireKeySettings)8 : (DESFireKeySettings)0;

				         					if (device.CreateMifareDesfireApplication(
				         						DesfireMasterKeyCurrent,
				         						keySettings,
				         						SelectedDesfireMasterKeyEncryptionTypeCurrent,
				         						SelectedDesfireAppKeyEncryptionTypeCreateNewApp,
				         						selectedDesfireAppMaxNumberOfKeysAsInt,
				         						AppNumberNewAsInt) == ERROR.NoError)
				         					{
				         						StatusText += string.Format("{0}: Successfully Created AppID {1}\n", DateTime.Now, AppNumberNewAsInt);
				         						TaskErr = ERROR.NoError;
				         						return;
				         					}
				         					else
				         					{
				         						StatusText += "Unable to Create App";
				         						TaskErr = ERROR.AuthenticationError;
				         						return;
				         					}
				         				}
				         				else
				         				{
				         					StatusText += "Unable to Auth";
				         					TaskErr = ERROR.AuthenticationError;
				         					return;
				         				}
				         			}

				         			//RaisePropertyChanged("DataAsByteArray");

				         			//Mouse.OverrideCursor = null;
				         		}
				         		else
				         		{
				         			TaskErr = ERROR.DeviceNotReadyError;
				         			return;
				         		}
				         	}
				         });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}

		/// <summary>
		/// return new RelayCommand<RFiDDevice>((_device) => OnNewCreateAppCommand(_device));
		/// </summary>
		public ICommand CreateFileCommand { get { return new RelayCommand(OnNewCreateFileCommand); } }
		private void OnNewCreateFileCommand()
		{
			TaskErr = ERROR.Empty;

			accessRights.changeAccess = SelectedDesfireFileAccessRightChange;
			accessRights.readAccess = SelectedDesfireFileAccessRightRead;
			accessRights.writeAccess = SelectedDesfireFileAccessRightWrite;
			accessRights.readAndWriteAccess = SelectedDesfireFileAccessRightReadWrite;

			Task desfireTask =
				new Task(() =>
				         {
				         	using (RFiDDevice device = RFiDDevice.Instance)
				         	{
				         		if (device != null)
				         		{
				         			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

				         			//Mouse.OverrideCursor = Cursors.Wait;

				         			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
				         			{
				         				if (IsValidAppNumberNew != false &&
				         				    device.AuthToMifareDesfireApplication(
				         				    	DesfireAppKeyCurrent,
				         				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
				         				    	selectedDesfireAppKeyNumberCurrentAsInt,AppNumberCurrentAsInt) == ERROR.NoError)
				         				{
				         					StatusText += string.Format("{0}: Successfully Authenticated to App {1}\n", DateTime.Now, AppNumberCurrentAsInt);

				         					if (device.CreateMifareDesfireFile(DesfireAppKeyCurrent, SelectedDesfireAppKeyEncryptionTypeCurrent, SelectedDesfireFileType,
				         					                                   accessRights, SelectedDesfireFileCryptoMode, AppNumberNewAsInt, FileNumberCurrentAsInt, FileSizeCurrentAsInt) == ERROR.NoError)
				         					{
				         						StatusText += string.Format("{0}: Successfully Created FileNo: {1} with Size: {2} in AppID: {3}\n", DateTime.Now, FileNumberCurrentAsInt, FileSizeCurrentAsInt, AppNumberNewAsInt);
				         						TaskErr = ERROR.NoError;
				         						return;
				         					}
				         					else
				         					{
				         						StatusText += "Unable to Create File";
				         						TaskErr = ERROR.AuthenticationError;
				         						return;
				         					}
				         				}
				         				else
				         				{
				         					StatusText += "Unable to Auth";
				         					TaskErr = ERROR.AuthenticationError;
				         					return;
				         				}
				         			}

				         			//RaisePropertyChanged("DataAsByteArray");

				         			//Mouse.OverrideCursor = null;
				         		}
				         		else
				         		{
				         			TaskErr = ERROR.DeviceNotReadyError;
				         			return;
				         		}
				         	}
				         });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand ReadDataCommand { get { return new RelayCommand(OnNewReadDataCommand); } }
		private void OnNewReadDataCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask =
				new Task(() =>
				         {
				         	using (RFiDDevice device = RFiDDevice.Instance)
				         	{
				         		if (device != null)
				         		{
				         			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

				         			//Mouse.OverrideCursor = Cursors.Wait;

				         			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
				         			{
				         				if (IsValidAppNumberNew != false &&
				         				    device.AuthToMifareDesfireApplication(
				         				    	DesfireMasterKeyCurrent,
				         				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
				         				    	selectedDesfireAppKeyNumberCurrentAsInt,AppNumberCurrentAsInt) == ERROR.NoError)
				         				{
				         					StatusText += string.Format("{0}: Successfully Authenticated to App {1}\n", DateTime.Now, AppNumberCurrentAsInt);

				         					if (device.ReadMiFareDESFireChipFile(DesfireAppKeyCurrent, SelectedDesfireAppKeyEncryptionTypeCurrent,
				         					                                     DesfireReadKeyCurrent, SelectedDesfireReadKeyEncryptionType, selectedDesfireReadKeyNumberAsInt,
				         					                                     DesfireWriteKeyCurrent, SelectedDesfireWriteKeyEncryptionType, selectedDesfireWriteKeyNumberAsInt,
				         					                                     EncryptionMode.CM_ENCRYPT, FileNumberCurrentAsInt, AppNumberCurrentAsInt, FileSizeCurrentAsInt) == ERROR.NoError)
				         					{
				         						FileSizeCurrent = device.MifareDESFireData.Length.ToString();
				         						
				         						StatusText += string.Format("{0}: Successfully Read {2} Bytes Data from FileNo: {1} in AppID: {3}\n", DateTime.Now, FileNumberCurrentAsInt, FileSizeCurrentAsInt, AppNumberNewAsInt);
				         						
				         						childNodeViewModelFromChip.Children.Single(x => x.DesfireFile != null).DesfireFile = new MifareDesfireFileModel(device.MifareDESFireData, (byte)FileNumberCurrentAsInt);
				         						
				         						childNodeViewModelTemp.Children.Single(x => x.DesfireFile != null).DesfireFile = new MifareDesfireFileModel(device.MifareDESFireData, (byte)FileNumberCurrentAsInt);

				         						RaisePropertyChanged("ChildNodeViewModelTemp");
				         						RaisePropertyChanged("ChildNodeViewModelFromChip");
				         						
				         						TaskErr = ERROR.NoError;
				         						return;
				         					}
				         					else
				         					{
				         						StatusText += string.Format("{0}: Unable to Read File with FileID: {1}",DateTime.Now, FileNumberCurrentAsInt);
				         						TaskErr = ERROR.AuthenticationError;
				         						return;
				         					}
				         				}
				         				else
				         				{
				         					StatusText += string.Format("Unable to Auth");
				         					TaskErr = ERROR.AuthenticationError;
				         					return;
				         				}
				         			}

				         			//RaisePropertyChanged("DataAsByteArray");

				         			//Mouse.OverrideCursor = null;
				         		}
				         		else
				         		{
				         			TaskErr = ERROR.DeviceNotReadyError;
				         			return;
				         		}
				         	}
				         });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public ICommand GetDataFromFileCommand { get { return new RelayCommand(OnNewGetDataFromFileCommand); } }
		private void OnNewGetDataFromFileCommand() {
			var dlg = new OpenFileDialogViewModel
			{
				Title = ResourceLoader.getResource("windowCaptionOpenProject"),
				//Filter = ResourceLoader.getResource("filterStringSaveTasks"),
				Multiselect = false
			};

			if (dlg.Show(this.Dialogs) && dlg.FileName != null)
			{
				try {

					var data = File.ReadAllText(dlg.FileName);
					int err;


					childNodeViewModelTemp.Children.Single(x => x.DesfireFile != null).DesfireFile = new MifareDesfireFileModel((CustomConverter.GetBytes(data, out err)), 0);
					//.Data = new byte[(data.Length)/2];
					//childNodeViewModelTemp.Children.Single(x => x.DesfireFile != null).DesfireFile.Data = (CustomConverter.GetBytes(data, out err));

					RaisePropertyChanged("ChildNodeViewModelTemp");
					RaisePropertyChanged("ChildNodeViewModelFromChip");
				}
				catch (Exception e) {
					LogWriter.CreateLogEntry(string.Format("{0}: {1}; {2}", DateTime.Now, e.Message, e.InnerException != null ? e.InnerException.Message : ""));
				}
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public ICommand WriteDataCommand { get { return new RelayCommand(OnNewWriteDataCommand); } }
		private void OnNewWriteDataCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask =
				new Task(() =>
				         {
				         	using (RFiDDevice device = RFiDDevice.Instance)
				         	{
				         		if (device != null)
				         		{
				         			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

				         			//Mouse.OverrideCursor = Cursors.Wait;

				         			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
				         			{
				         				if (IsValidAppNumberNew != false &&
				         				    device.AuthToMifareDesfireApplication(
				         				    	DesfireMasterKeyCurrent,
				         				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
				         				    	selectedDesfireAppKeyNumberCurrentAsInt,AppNumberCurrentAsInt) == ERROR.NoError)
				         				{
				         					StatusText += string.Format("{0}: Successfully Authenticated to App {1}\n", DateTime.Now, AppNumberCurrentAsInt);

				         					if (device.WriteMiFareDESFireChipFile(DesfireMasterKeyCurrent, SelectedDesfireMasterKeyEncryptionTypeCurrent,
				         					                                      DesfireAppKeyCurrent, SelectedDesfireAppKeyEncryptionTypeCurrent,
				         					                                      DesfireReadKeyCurrent, SelectedDesfireReadKeyEncryptionType, selectedDesfireReadKeyNumberAsInt,
				         					                                      DesfireWriteKeyCurrent, SelectedDesfireWriteKeyEncryptionType, selectedDesfireWriteKeyNumberAsInt,
				         					                                      EncryptionMode.CM_ENCRYPT, FileNumberCurrentAsInt, AppNumberCurrentAsInt, childNodeViewModelTemp.Children.Single(x => x.DesfireFile != null).DesfireFile.Data) == ERROR.NoError)
				         					{
				         						StatusText += string.Format("{0}: Successfully Created FileNo: {1} with Size: {2} in AppID: {3}\n", DateTime.Now, FileNumberCurrentAsInt, FileSizeCurrentAsInt, AppNumberNewAsInt);
				         						TaskErr = ERROR.NoError;
				         						return;
				         					}
				         					else
				         					{
				         						StatusText += "Unable to Create File";
				         						TaskErr = ERROR.AuthenticationError;
				         						return;
				         					}
				         				}
				         				else
				         				{
				         					StatusText += "Unable to Auth";
				         					TaskErr = ERROR.AuthenticationError;
				         					return;
				         				}
				         			}

				         			//RaisePropertyChanged("DataAsByteArray");

				         			//Mouse.OverrideCursor = null;
				         		}
				         		else
				         		{
				         			TaskErr = ERROR.DeviceNotReadyError;
				         			return;
				         		}
				         	}
				         });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}
		
		/// <summary>
		///
		/// </summary>
		public ICommand ChangeAppKeyCommand { get { return new RelayCommand(OnNewChangeAppKeyCommand); } }
		private void OnNewChangeAppKeyCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask = new Task(() =>
			                            {
			                            	using (RFiDDevice device = RFiDDevice.Instance)
			                            	{
			                            		if (device != null)
			                            		{
			                            			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

			                            			//Mouse.OverrideCursor = Cursors.Wait;

			                            			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
			                            			{
			                            				if (IsValidAppNumberCurrent != false &&
			                            				    IsValidAppNumberTarget != false &&
			                            				    IsValidDesfireAppKeyTarget != false &&
			                            				    device.AuthToMifareDesfireApplication(
			                            				    	DesfireAppKeyCurrent, //DesfireMasterKeyCurrent
			                            				    	SelectedDesfireAppKeyEncryptionTypeCurrent, //SelectedDesfireMasterKeyEncryptionTypeCurrent
			                            				    	selectedDesfireAppKeyNumberCurrentAsInt , //SelectedDesfireAppKeyNumberCurrent
			                            				    	AppNumberCurrentAsInt) == ERROR.NoError)
			                            				{
			                            					StatusText += string.Format("{0}: Successfully Authenticated to AppID {1}\n", DateTime.Now, AppNumberCurrentAsInt);

			                            					if (device.ChangeMifareDesfireApplicationKey(DesfireAppKeyCurrent,
			                            					                                             selectedDesfireAppKeyNumberCurrentAsInt,
			                            					                                             SelectedDesfireAppKeyEncryptionTypeCurrent,
			                            					                                             DesfireAppKeyTarget,
			                            					                                             selectedDesfireAppKeyNumberTargetAsInt,
			                            					                                             SelectedDesfireAppKeyEncryptionTypeTarget,
			                            					                                             AppNumberCurrentAsInt, AppNumberTargetAsInt) == ERROR.NoError)
			                            					{
			                            						StatusText += string.Format("{0}: Successfully Changed Key {1} of AppID {2}\n", DateTime.Now, selectedDesfireAppKeyNumberTargetAsInt, AppNumberTargetAsInt);
			                            						TaskErr = ERROR.NoError;
			                            						return;
			                            					}
			                            					else
			                            					{
			                            						StatusText += string.Format("{0}: Unable to Change Key {1} of AppID {2}\n", DateTime.Now, selectedDesfireAppKeyNumberCurrentAsInt, AppNumberTargetAsInt);
			                            						TaskErr = ERROR.AuthenticationError;
			                            						return;
			                            					}
			                            				}
			                            				else
			                            				{
			                            					StatusText += "Unable to Auth";
			                            					TaskErr = ERROR.AuthenticationError;
			                            					return;
			                            				}
			                            			}

			                            			//RaisePropertyChanged("DataAsByteArray");

			                            			//Mouse.OverrideCursor = null;
			                            		}
			                            		else
			                            		{
			                            			TaskErr = ERROR.DeviceNotReadyError;
			                            			return;
			                            		}
			                            	}
			                            });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}

		/// <summary>
		///
		/// </summary>
		public ICommand DeleteSignleCardApplicationCommand { get { return new RelayCommand(OnNewDeleteSignleCardApplicationCommand); } }
		private void OnNewDeleteSignleCardApplicationCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask = new Task(() =>
			                            {
			                            	using (RFiDDevice device = RFiDDevice.Instance)
			                            	{
			                            		if (device != null)
			                            		{
			                            			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

			                            			//Mouse.OverrideCursor = Cursors.Wait;

			                            			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
			                            			{
			                            				if (IsValidAppNumberCurrent != false &&
			                            				    device.AuthToMifareDesfireApplication(
			                            				    	DesfireMasterKeyCurrent,
			                            				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
			                            				    	0) == ERROR.NoError)
			                            				{
			                            					StatusText += string.Format("{0}: Successfully Authenticated to PICC Master App 0\n", DateTime.Now);

			                            					if (device.DeleteMifareDesfireApplication(
			                            						DesfireMasterKeyCurrent,
			                            						SelectedDesfireMasterKeyEncryptionTypeCurrent,
			                            						AppNumberNewAsInt) == ERROR.NoError)
			                            					{
			                            						StatusText += string.Format("{0}: Successfully Deleted AppID {1}\n", DateTime.Now, AppNumberNewAsInt);
			                            						TaskErr = ERROR.NoError;
			                            						return;
			                            					}
			                            					else
			                            					{
			                            						StatusText += string.Format("{0}: Unable to Remove AppID {1}\n", DateTime.Now, AppNumberNewAsInt);
			                            						TaskErr = ERROR.AuthenticationError;
			                            						return;
			                            					}
			                            				}
			                            				else
			                            				{
			                            					StatusText += "Unable to Auth";
			                            					TaskErr = ERROR.AuthenticationError;
			                            				}
			                            			}

			                            			//RaisePropertyChanged("DataAsByteArray");

			                            			//Mouse.OverrideCursor = null;
			                            		}
			                            		else
			                            		{
			                            			TaskErr = ERROR.DeviceNotReadyError;
			                            			return;
			                            		}
			                            	}
			                            });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}
		
		/// <summary>
		/// 
		/// </summary>
		public ICommand DeleteFileCommand { get { return new RelayCommand(OnNewDeleteFileCommand); } }
		private void OnNewDeleteFileCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask = new Task(() =>
			                            {
			                            	using (RFiDDevice device = RFiDDevice.Instance)
			                            	{
			                            		if (device != null)
			                            		{
			                            			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

			                            			//Mouse.OverrideCursor = Cursors.Wait;

			                            			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
			                            			{
			                            				if (IsValidAppNumberCurrent != false &&
			                            				    device.AuthToMifareDesfireApplication(
			                            				    	DesfireMasterKeyCurrent,
			                            				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
			                            				    	selectedDesfireAppKeyNumberCurrentAsInt,AppNumberCurrentAsInt) == ERROR.NoError)
			                            				{
			                            					StatusText += string.Format("{0}: Successfully Authenticated to App {1}\n", DateTime.Now, AppNumberCurrentAsInt);

			                            					if (device.DeleteMifareDesfireFile(
			                            						DesfireAppKeyCurrent,
			                            						SelectedDesfireAppKeyEncryptionTypeCurrent,
			                            						AppNumberNewAsInt, FileNumberCurrentAsInt) == ERROR.NoError)
			                            					{
			                            						StatusText += string.Format("{0}: Successfully Deleted File {1}\n", DateTime.Now, FileNumberCurrentAsInt);
			                            						TaskErr = ERROR.NoError;
			                            						return;
			                            					}
			                            					else
			                            					{
			                            						StatusText += string.Format("{0}: Unable to Remove AppID {1}\n", DateTime.Now, AppNumberNewAsInt);
			                            						TaskErr = ERROR.AuthenticationError;
			                            						return;
			                            					}
			                            				}
			                            				else
			                            				{
			                            					StatusText += "Unable to Auth";
			                            					TaskErr = ERROR.AuthenticationError;
			                            				}
			                            			}

			                            			//RaisePropertyChanged("DataAsByteArray");

			                            			//Mouse.OverrideCursor = null;
			                            		}
			                            		else
			                            		{
			                            			TaskErr = ERROR.DeviceNotReadyError;
			                            			return;
			                            		}
			                            	}
			                            });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}

		/// <summary>
		/// public ICommand FormatDesfireCardCommand { get { return new RelayCommand<RFiDDevice>((_device) => OnNewFormatDesfireCardCommand(_device)); }}
		/// </summary>
		public ICommand FormatDesfireCardCommand { get { return new RelayCommand(OnNewFormatDesfireCardCommand); } }
		private void OnNewFormatDesfireCardCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask = new Task(() =>
			                            {
			                            	using (RFiDDevice device = RFiDDevice.Instance) //?? _device
			                            	{
			                            		if (device != null && device.GetMiFareDESFireChipAppIDs() == ERROR.NoError)
			                            		{
			                            			StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

			                            			//Mouse.OverrideCursor = Cursors.Wait;
			                            			//Thread.Sleep(10000);
			                            			if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
			                            			{
			                            				if (IsValidAppNumberCurrent != false &&
			                            				    device.AuthToMifareDesfireApplication(
			                            				    	DesfireMasterKeyCurrent,
			                            				    	SelectedDesfireMasterKeyEncryptionTypeCurrent,
			                            				    	0) == ERROR.NoError)
			                            				{
			                            					StatusText += string.Format("{0}: Successfully Authenticated to PICC Master App 0\n", DateTime.Now);

			                            					foreach (uint appID in device.AppIDList)
			                            					{
			                            						StatusText += string.Format("{0}: FoundAppID {1}\n", DateTime.Now, appID);

			                            						if (device.DeleteMifareDesfireApplication(
			                            							DesfireMasterKeyCurrent,
			                            							SelectedDesfireMasterKeyEncryptionTypeCurrent,
			                            							(int)appID) == ERROR.NoError)
			                            						{
			                            							StatusText += string.Format("{0}: Successfully Deleted AppID {1}\n", DateTime.Now, appID);
			                            						}
			                            						else
			                            						{
			                            							StatusText += string.Format("{0}: Unable to Remove AppID {1}\n", DateTime.Now, appID);
			                            							TaskErr = ERROR.AuthenticationError;
			                            							return;
			                            						}
			                            					}

			                            					TaskErr = device.FormatDesfireCard(DesfireMasterKeyCurrent, SelectedDesfireMasterKeyEncryptionTypeCurrent);
			                            				}
			                            				else
			                            				{
			                            					StatusText += "Unable to Auth";
			                            					TaskErr = ERROR.AuthenticationError;
			                            					return;
			                            				}
			                            			}

			                            			//RaisePropertyChanged("DataAsByteArray");

			                            			//Mouse.OverrideCursor = null;
			                            		}
			                            		else
			                            		{
			                            			TaskErr = ERROR.DeviceNotReadyError;
			                            			return;
			                            		}
			                            	}
			                            	return;
			                            });

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}

		/// <summary>
		///
		/// </summary>
		public ICommand AuthenticateToCardApplicationCommand { get { return new RelayCommand(OnNewAuthenticateToCardApplicationCommand); } }
		private void OnNewAuthenticateToCardApplicationCommand()
		{
			using (RFiDDevice device = RFiDDevice.Instance)
			{
				if (device != null)
				{
					StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

					Mouse.OverrideCursor = Cursors.Wait;

					if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireAppKeyCurrent) == KEY_ERROR.NO_ERROR)
					{
						if (IsValidAppNumberCurrent != false &&
						    device.AuthToMifareDesfireApplication(
						    	DesfireAppKeyCurrent,
						    	SelectedDesfireAppKeyEncryptionTypeCurrent,
						    	selectedDesfireAppKeyNumberCurrentAsInt,
						    	AppNumberCurrentAsInt) == ERROR.NoError)
						{
							StatusText += string.Format("{0}: Successfully Authenticated to App {1}\n", DateTime.Now, AppNumberCurrentAsInt);
						}
						else
							StatusText += "Unable to Auth";
					}

					//RaisePropertyChanged("DataAsByteArray");

					Mouse.OverrideCursor = null;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		public ICommand ChangeMasterCardKeyCommand { get { return new RelayCommand(OnNewChangeMasterCardKeyCommand); } }
		private void OnNewChangeMasterCardKeyCommand()
		{
			TaskErr = ERROR.Empty;

			Task desfireTask = new Task(
				() =>
				{
					using (RFiDDevice device = RFiDDevice.Instance)
					{
						if (device != null)
						{
							StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

							//Mouse.OverrideCursor = Cursors.Wait;

							if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireMasterKeyCurrent) == KEY_ERROR.NO_ERROR)
							{
								if (device.AuthToMifareDesfireApplication(
									CustomConverter.DesfireKeyToCheck,
									SelectedDesfireMasterKeyEncryptionTypeCurrent,
									0) == ERROR.NoError)
								{
									StatusText += string.Format("{0}: Successfully Authenticated to App 0\n", DateTime.Now);

									if (IsValidDesfireMasterKeyCurrent != false &&
									    IsValidDesfireMasterKeyTarget != false)
									{
										if (device.ChangeMifareDesfireApplicationKey(
											DesfireMasterKeyCurrent,
											0,
											SelectedDesfireMasterKeyEncryptionTypeCurrent,
											DesfireMasterKeyTarget,
											selectedDesfireAppKeyNumberTargetAsInt,
											SelectedDesfireMasterKeyEncryptionTypeTarget) == ERROR.NoError)
										{
											StatusText += string.Format("{0}: Keychange Successful\n", DateTime.Now);
											TaskErr = ERROR.NoError;
											return;
										}
										else
										{
											StatusText += string.Format("{0}: Unable to Change Key\n", DateTime.Now);
											TaskErr = ERROR.AuthenticationError;
											return;
										}
									}
									else
									{
										StatusText += string.Format("{0}: Key Error: Wrong Format\n", DateTime.Now);
									}
								}
								else
								{
									StatusText += "Unable to Auth";
									TaskErr = ERROR.AuthenticationError;
									return;
								}
							}
							//RaisePropertyChanged("DataAsByteArray");
							//Mouse.OverrideCursor = null;
						}
						else
						{
							TaskErr = ERROR.DeviceNotReadyError;
							return;
						}
					}
				});

			if (TaskErr == ERROR.Empty)
			{
				TaskErr = ERROR.DeviceNotReadyError;

				desfireTask.ContinueWith((x) =>
				                         {
				                         	if (TaskErr == ERROR.NoError)
				                         	{
				                         		IsTaskCompletedSuccessfully = true;
				                         	}
				                         	else
				                         	{
				                         		IsTaskCompletedSuccessfully = false;
				                         	}
				                         });

				desfireTask.Start();
			}

			return;
		}

		/// <summary>
		///
		/// </summary>
		public ICommand AuthenticateToCardMasterApplicationCommand { get { return new RelayCommand(OnNewAuthenticateToCardMasterApplicationCommand); } }
		private void OnNewAuthenticateToCardMasterApplicationCommand()
		{
			using (RFiDDevice device = RFiDDevice.Instance)
			{
				if (device != null)
				{
					StatusText = string.Format("{0}: Connection to Reader successfully established\n", DateTime.Now);

					Mouse.OverrideCursor = Cursors.Wait;

					if (CustomConverter.FormatMifareDesfireKeyStringWithSpacesEachByte(DesfireMasterKeyCurrent) == KEY_ERROR.NO_ERROR)
					{
						if (device.AuthToMifareDesfireApplication(
							DesfireMasterKeyCurrent,
							SelectedDesfireMasterKeyEncryptionTypeCurrent,
							0) == ERROR.NoError)
						{
							StatusText += string.Format("{0}: Successfully Authenticated to App 0\n", DateTime.Now);
						}
						else
							StatusText += "Unable to Auth";
					}

					//RaisePropertyChanged("DataAsByteArray");

					Mouse.OverrideCursor = null;
				}
			}
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
		private void Ok()
		{
			if (this.OnOk != null)
				this.OnOk(this);
			else
				Close();
		}

		public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }
		private void Cancel()
		{
			if (this.OnCancel != null)
				this.OnCancel(this);
			else
				Close();
		}

		[XmlIgnore]
		public Action<MifareDesfireSetupViewModel> OnOk { get; set; }

		[XmlIgnore]
		public Action<MifareDesfireSetupViewModel> OnCancel { get; set; }
		
		[XmlIgnore]
		public Action<MifareDesfireSetupViewModel> OnCloseRequest { get; set; }

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
	}
}