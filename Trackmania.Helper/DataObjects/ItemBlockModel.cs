using System.Windows.Media;
using ManiaPlanetSharp.GameBox.MetadataProviders;

namespace Trackmania.Helper.DataObjects
{
    /// <summary>
    /// Represents an item / block (also macro blocks)
    /// </summary>
    public sealed class ItemBlockModel
    {
        /// <summary>
        /// Gets the name of the item / block
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the item / block
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the id of the author
        /// </summary>
        public string AuthorId { get; }

        /// <summary>
        /// Gets the type of the item / block
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the path of the item / block
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the placement parameters of the item / block
        /// </summary>
        public string PlacementParameters { get; }

        /// <summary>
        /// Gets the thumbnail
        /// </summary>
        public ImageSource Thumbnail { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="ItemBlockModel"/>
        /// </summary>
        /// <param name="provider">The item / block data provider</param>
        public ItemBlockModel(ItemMetadataProvider provider)
        {
            Name = provider.Name;
            Description = provider.Description;
            AuthorId = provider.Author;
            Type = provider.Type.ToString();
            Path = provider.Path;
            PlacementParameters = provider.GroundPoint?.ToString() ?? "/";
            Thumbnail = provider.ToItemBlockImage();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ItemBlockModel"/>
        /// </summary>
        /// <param name="provider">The item / block data provider</param>
        public ItemBlockModel(MacroblockMetadataProvider provider)
        {
            Name = provider.Name;
            Description = provider.Description;
            AuthorId = provider.Author;
            Type = "/";
            Path = provider.Path;
            PlacementParameters = "/";
            Thumbnail = provider.ToItemBlockImage();
        }
    }
}
