using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ManiaPlanetSharp.GameBox;
using ManiaPlanetSharp.GameBox.MetadataProviders;
using Trackmania.Helper.Business;
using Trackmania.Helper.DataObjects;
using Trackmania.Helper.Global;
using ZimLabs.WpfBase;

namespace Trackmania.Helper.Ui.ViewModel
{
    /// <summary>
    /// Provides the logic for the <see cref="View.MainWindow"/>
    /// </summary>
    internal sealed class MainWindowViewModel : ViewModelBase
    {
        #region General properties
        /// <summary>
        /// Backing field for <see cref="BuildInformation"/>
        /// </summary>
        private string _buildInformation;

        /// <summary>
        /// Gets or sets the build information
        /// </summary>
        public string BuildInformation
        {
            get => _buildInformation;
            private set => SetField(ref _buildInformation, value);
        }

        /// <summary>
        /// Backing field for <see cref="Config"/>
        /// </summary>
        private ConfigModel _config;

        /// <summary>
        /// Gets or sets the config
        /// </summary>
        public ConfigModel Config
        {
            get => _config;
            set => SetField(ref _config, value);
        }

        /// <summary>
        /// Backing field for <see cref="ConfigOpen"/>
        /// </summary>
        private bool _configOpen;

        /// <summary>
        /// Gets or sets the value which indicates if the config flyout is open or closed
        /// </summary>
        public bool ConfigOpen
        {
            get => _configOpen;
            set => SetField(ref _configOpen, value);
        }
        #endregion

        #region Lists

        /// <summary>
        /// Backing field for <see cref="ItemList"/>
        /// </summary>
        private ObservableCollection<FileEntry> _itemList;

        /// <summary>
        /// Gets or sets the list with the items
        /// </summary>
        public ObservableCollection<FileEntry> ItemList
        {
            get => _itemList;
            set => SetField(ref _itemList, value);
        }

        /// <summary>
        /// Backing field for <see cref="SelectedItemEntry"/>
        /// </summary>
        private FileEntry _selectedItemEntry;

        /// <summary>
        /// Gets or sets the item entry
        /// </summary>
        public FileEntry SelectedItemEntry
        {
            get => _selectedItemEntry;
            set
            {
                SetField(ref _selectedItemEntry, value);
                if (value == null || value.IsDirectory)
                {
                    ItemData = null;
                    return;
                }

                var gbx = FileController.GetFileInformation(value);
                if (gbx.MainClass == ClassId.CGameItemModel)
                {
                    ItemData = new ItemBlockModel(new ItemMetadataProvider(gbx));
                }
            }
        }

        /// <summary>
        /// Backing field for <see cref="ItemData"/>
        /// </summary>
        private ItemBlockModel _itemData;

        /// <summary>
        /// Gets or sets the item data
        /// </summary>
        public ItemBlockModel ItemData
        {
            get => _itemData;
            set => SetField(ref _itemData, value);
        }

        /// <summary>
        /// Backing field for <see cref="BlockList"/>
        /// </summary>
        private ObservableCollection<FileEntry> _blockList;

        /// <summary>
        /// Gets or sets the list with the blocks
        /// </summary>
        public ObservableCollection<FileEntry> BlockList
        {
            get => _blockList;
            set => SetField(ref _blockList, value);
        }

        /// <summary>
        /// Backing field for <see cref="SelectedBlockEntry"/>
        /// </summary>
        private FileEntry _selectedBlockEntry;

        /// <summary>
        /// Gets or sets the item entry
        /// </summary>
        public FileEntry SelectedBlockEntry
        {
            get => _selectedBlockEntry;
            set
            {
                SetField(ref _selectedBlockEntry, value);
                if (value == null || value.IsDirectory)
                {
                    ItemData = null;
                    return;
                }

                var gbx = FileController.GetFileInformation(value);
                BlockData = gbx.MainClass switch
                {
                    ClassId.CGameItemModel => new ItemBlockModel(new ItemMetadataProvider(gbx)),
                    ClassId.CGameCtnMacroBlockInfo => new ItemBlockModel(new MacroblockMetadataProvider(gbx)),
                    _ => BlockData
                };
            }
        }

        /// <summary>
        /// Backing field for <see cref="BlockData"/>
        /// </summary>
        private ItemBlockModel _blockDate;

        /// <summary>
        /// Gets or sets the item data
        /// </summary>
        public ItemBlockModel BlockData
        {
            get => _blockDate;
            set => SetField(ref _blockDate, value);
        }

        /// <summary>
        /// Backing field for <see cref="MapList"/>
        /// </summary>
        private ObservableCollection<FileEntry> _mapList;

        /// <summary>
        /// Gets or sets the list with the maps
        /// </summary>
        public ObservableCollection<FileEntry> MapList
        {
            get => _mapList;
            set => SetField(ref _mapList, value);
        }

        /// <summary>
        /// Backing field for <see cref="SelectedMap"/>
        /// </summary>
        private FileEntry _selectedMap;

        /// <summary>
        /// Gets or sets the selected map
        /// </summary>
        public FileEntry SelectedMap
        {
            get => _selectedMap;
            set
            {
                SetField(ref _selectedMap, value);
                if (value == null || value.IsDirectory)
                {
                    MapData = null;
                    return;
                }

                var gbx = FileController.GetFileInformation(value);
                if (gbx.MainClass == ClassId.CGameCtnChallenge)
                {
                    MapData = new MapModel(new MapMetadataProvider(gbx));
                }
            }
        }

        /// <summary>
        /// Backing field for <see cref="MapData"/>
        /// </summary>
        private MapModel _mapData;

        /// <summary>
        /// Gets or sets the data of the map
        /// </summary>
        public MapModel MapData
        {
            get => _mapData;
            set => SetField(ref _mapData, value);
        }
        #endregion
        /// <summary>
        /// The command to open the desired page
        /// </summary>
        public ICommand OpenPageCommand => new RelayCommand<UrlType>(OpenPage);

        /// <summary>
        /// The command to open the config
        /// </summary>
        public ICommand ConfigCommand => new DelegateCommand(() =>
        {
            ConfigOpen = !ConfigOpen;
        });

        /// <summary>
        /// The command to save the configuration
        /// </summary>
        public ICommand SaveConfigCommand => new DelegateCommand(SaveConfig);

        /// <summary>
        /// The command to browse for the trackmania directory
        /// </summary>
        public ICommand BrowseTrackmaniaDirCommand => new DelegateCommand(BrowseTrackmaniaDir);

        /// <summary>
        /// Init the view model
        /// </summary>
        public async void InitViewModel()
        {
            BuildInformation = Helper.GetBuildInformation();
            Config = Helper.Configuration;

            await LoadContent();
        }

        /// <summary>
        /// Loads the content of the directories
        /// </summary>
        /// <returns>The awaitable task</returns>
        private async Task LoadContent()
        {
            var controller = await ShowProgress("Please wait", "Please wait while loading the content...");

            try
            {
                // Items
                controller.SetMessage("Load items...");
                var itemList = await Task.Run(() =>
                    FileController.LoadDirectoryContent(Helper.Configuration.TrackmaniaDir,
                        Helper.FileType.Item));

                ItemList = new ObservableCollection<FileEntry>(itemList);

                // Blocks
                controller.SetMessage("Load blocks...");
                var blockList = await Task.Run(() =>
                    FileController.LoadDirectoryContent(Helper.Configuration.TrackmaniaDir, Helper.FileType.Block));

                BlockList = new ObservableCollection<FileEntry>(blockList);

                // Maps
                controller.SetMessage("Load maps..");
                var maps = await Task.Run(() =>
                    FileController.LoadDirectoryContent(Helper.Configuration.TrackmaniaDir, Helper.FileType.Map));

                MapList = new ObservableCollection<FileEntry>(maps);
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
            finally
            {
                await controller.CloseAsync();
            }
        }

        /// <summary>
        /// Opens the page specified by the url type
        /// </summary>
        /// <param name="urlType">The desired url</param>
        private void OpenPage(UrlType urlType)
        {
            Helper.OpenUrl(urlType);
        }

        /// <summary>
        /// Saves the configuration
        /// </summary>
        private void SaveConfig()
        {
            Helper.SaveConfiguration();
        }

        /// <summary>
        /// Browse for the trackmania directory
        /// </summary>
        private void BrowseTrackmaniaDir()
        {
            Config.TrackmaniaDir = BrowseDirectoryDialog("Select the directory which contains Trackmania",
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }
    }
}
