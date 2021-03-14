using ZimLabs.WpfBase;

namespace Trackmania.Helper.DataObjects
{
    /// <summary>
    /// Provides the different configurations
    /// </summary>
    internal sealed class ConfigModel : ObservableObject
    {
        #region URLs

        /// <summary>
        /// Gets or sets the url of trackmania exchange
        /// </summary>
        public string UrlTrackmaniaExchange { get; set; } = "https://trackmania.exchange/";

        /// <summary>
        /// Gets or sets the url of item exchange
        /// </summary>
        public string UrlItemExchange { get; set; } = "https://item.mania.exchange/";

        /// <summary>
        /// Gets or sets the url of the news page
        /// </summary>
        public string UrlTrackmaniaNews { get; set; } = "https://trackmania.news/";

        /// <summary>
        /// Gets or sets the url of the mania portal (the main page) 
        /// </summary>
        public string UrlManiaPortal { get; set; } = "https://mania-exchange.com/";

        /// <summary>
        /// Gets or sets the url of the account portal
        /// </summary>
        public string UrlAccountPortal { get; set; } = "https://accounts.mania-exchange.com";

        /// <summary>
        /// Gets or sets the url of the official page
        /// </summary>
        public string UrlOfficialPage { get; set; } = "https://www.trackmania.com/";

        /// <summary>
        /// Gets or sets the url of the official forum
        /// </summary>
        public string UrlOfficialForum { get; set; } = "https://forums.ubisoft.com/forumdisplay.php/2228-Trackmania";

        #endregion

        #region LocalPaths
        /// <summary>
        /// Backing field for <see cref="TrackmaniaDir"/>
        /// </summary>
        private string _trackmaniaDir = "";

        /// <summary>
        /// Gets or sets the path of the trackmania directory
        /// </summary>
        public string TrackmaniaDir
        {
            get => _trackmaniaDir;
            set => SetField(ref _trackmaniaDir, value);
        }
        #endregion

        #region Extensions
        /// <summary>
        /// Gets or sets the extension of an item
        /// </summary>
        public string ExtensionItem { get; set; } = "*.Item.Gbx";

        /// <summary>
        /// Gets or sets the extension of a block
        /// </summary>
        public string ExtensionBlock { get; set; } = "*.Block.Gbx";

        /// <summary>
        /// Gets or sets the extension of a macroblock
        /// </summary>
        public string ExtensionMacroBlock { get; set; } = "*.Macroblock.Gbx";

        /// <summary>
        /// Gets or sets the extension of a map
        /// </summary>
        public string ExtensionMap { get; set; } = "*.Map.Gbx";

        /// <summary>
        /// Gets or sets the extension of a screenshot
        /// </summary>
        public string ExtensionScreenshot { get; set; } = "*.*";

        /// <summary>
        /// Gets or sets the extension of a replay
        /// </summary>
        public string ExtensionReplay { get; set; } = "*.Replay.Gbx";
        #endregion
    }
}