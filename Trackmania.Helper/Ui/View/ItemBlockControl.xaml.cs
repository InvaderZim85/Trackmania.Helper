using System.Windows;
using System.Windows.Controls;
using Trackmania.Helper.DataObjects;

namespace Trackmania.Helper.Ui.View
{
    /// <summary>
    /// Interaction logic for ItemBlockControl.xaml
    /// </summary>
    public partial class ItemBlockControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ItemBlockControl"/>
        /// </summary>
        public ItemBlockControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The dependency property of <see cref="ItemBlockData"/>
        /// </summary>
        public static readonly DependencyProperty ItemBlockDataProperty = DependencyProperty.Register(
            nameof(ItemBlockData), typeof(ItemBlockModel), typeof(ItemBlockControl), new PropertyMetadata(default(ItemBlockModel)));

        /// <summary>
        /// Gets or sets the data of the item / block
        /// </summary>
        public ItemBlockModel ItemBlockData
        {
            get => (ItemBlockModel) GetValue(ItemBlockDataProperty);
            set => SetValue(ItemBlockDataProperty, value);
        }
    }
}
