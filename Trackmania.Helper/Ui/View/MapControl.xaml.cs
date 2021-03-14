using System.Windows;
using System.Windows.Controls;
using ManiaPlanetSharp.GameBox.MetadataProviders;
using Trackmania.Helper.DataObjects;

namespace Trackmania.Helper.Ui.View
{
    /// <summary>
    /// Interaction logic for MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MapControl"/>
        /// </summary>
        public MapControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The dependency property of <see cref="MapData"/>
        /// </summary>
        public static readonly DependencyProperty MapDataProperty = DependencyProperty.Register(
            nameof(MapData), typeof(MapModel), typeof(MapControl), new PropertyMetadata(default(MapModel)));

        /// <summary>
        /// Gets or sets the data of the map
        /// </summary>
        public MapModel MapData
        {
            get => (MapModel) GetValue(MapDataProperty);
            set => SetValue(MapDataProperty, value);
        }
    }
}
