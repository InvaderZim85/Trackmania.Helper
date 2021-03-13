using Trackmania.Helper.DataObjects;

namespace Trackmania.Helper.Global
{
    /// <summary>
    /// Provides the different url types
    /// </summary>
    public enum UrlType
    {
        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlTrackmaniaExchange"/>
        /// </summary>
        TrackmaniaExchange,

        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlItemExchange"/>
        /// </summary>
        ItemExchange,

        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlTrackmaniaNews"/>
        /// </summary>
        TrackmaniaNews,

        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlManiaPortal"/>
        /// </summary>
        ManiaPortal,

        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlAccountPortal"/>
        /// </summary>
        AccountPortal,

        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlOfficialPage"/>
        /// </summary>
        OfficialPage,

        /// <summary>
        /// Opens the path which is stored at <see cref="ConfigModel.UrlOfficialForum"/>
        /// </summary>
        OfficialForum
    }
}
