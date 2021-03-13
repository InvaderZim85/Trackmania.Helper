using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
        /// Backing field for <see cref="ListItem"/>
        /// </summary>
        private ObservableCollection<FileEntry> _listItem;

        /// <summary>
        /// Gets or sets the list with the items
        /// </summary>
        public ObservableCollection<FileEntry> ListItem
        {
            get => _listItem;
            set => SetField(ref _listItem, value);
        }

        /// <summary>
        /// Backing field for <see cref="ListBlock"/>
        /// </summary>
        private ObservableCollection<FileEntry> _listBlock;

        /// <summary>
        /// Gets or sets the list with the blocks
        /// </summary>
        public ObservableCollection<FileEntry> ListBlock
        {
            get => _listBlock;
            set => SetField(ref _listBlock, value);
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
                var itemList = await Task.Run(() =>
                    FileController.LoadDirectoryContent(Helper.Configuration.TrackmaniaDir,
                        Helper.FileType.Item));

                ListItem = new ObservableCollection<FileEntry>(itemList);
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
