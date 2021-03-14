using System.Windows.Media;
using ManiaPlanetSharp.GameBox.MetadataProviders;

namespace Trackmania.Helper.DataObjects
{
    /// <summary>
    /// Represents a map
    /// </summary>
    public sealed class MapModel
    {
        /// <summary>
        /// Gets the name of the map
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the unique id of the map
        /// </summary>
        public string UniqueId { get; }

        /// <summary>
        /// Gets the name of the author
        /// </summary>
        public string Author { get;  }

        /// <summary>
        /// Gets the author comment
        /// </summary>
        public string Comment { get;  }

        /// <summary>
        /// Gets the list with the times
        /// </summary>
        public string Times { get;  }

        /// <summary>
        /// Gets the mood of the track (e.G. Sunrise)
        /// </summary>
        public string Mood { get;  }

        /// <summary>
        /// Gets the value which indicates if the map is a multilap map
        /// </summary>
        public bool Multilap { get;  }

        /// <summary>
        /// Gets the display cost of the map
        /// </summary>
        public string DisplayCost { get;  }

        /// <summary>
        /// Gets the mod of the map
        /// </summary>
        public string Mod { get;  }

        /// <summary>
        /// Gets the custom music
        /// </summary>
        public bool CustomMusic { get;  }

        /// <summary>
        /// Gets the thumbnail
        /// </summary>
        public ImageSource Thumbnail { get;  }

        /// <summary>
        /// Creates a new instance of the <see cref="MapModel"/>
        /// </summary>
        /// <param name="provider">The map data provider</param>
        public MapModel(MapMetadataProvider provider)
        {
            Name = provider.Name ?? "/";
            UniqueId = provider.Uid;
            Author = string.IsNullOrEmpty(provider.AuthorNickname)
                ? provider.Author
                : $"{provider.Author} // {provider.AuthorNickname}";
            Comment = string.IsNullOrEmpty(provider.Comment) ? "/" : provider.Comment;
            Times =
                $"Author time {provider.AuthorTime} // Gold {provider.GoldTime} // Silver {provider.SilverTime} // Bronze {provider.BronzeTime}";
            Mood = provider.TimeOfDay;
            Multilap = provider.IsMultilap ?? false;
            DisplayCost = (provider.DisplayCost ?? 0).ToString("N0");
            Mod = string.IsNullOrEmpty(provider.Mod) ? "none" : provider.Mod;
            CustomMusic = provider.CustomMusic != null;
            Thumbnail = provider.Thumbnail.ToMapImage();
        }
    }
}
