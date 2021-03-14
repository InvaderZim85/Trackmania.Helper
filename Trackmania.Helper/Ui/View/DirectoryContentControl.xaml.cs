using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Trackmania.Helper.DataObjects;

namespace Trackmania.Helper.Ui.View
{
    /// <summary>
    /// Interaction logic for DirectoryContentControl.xaml
    /// </summary>
    public partial class DirectoryContentControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DirectoryContentControl"/>
        /// </summary>
        public DirectoryContentControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The dependency property of <see cref="ContentList"/>
        /// </summary>
        public static readonly DependencyProperty ContentListProperty = DependencyProperty.Register(
            nameof(ContentList), typeof(ObservableCollection<FileEntry>), typeof(DirectoryContentControl), new PropertyMetadata(default(ObservableCollection<FileEntry>)));

        /// <summary>
        /// Gets or sets the list with the files
        /// </summary>
        public ObservableCollection<FileEntry> ContentList
        {
            get => (ObservableCollection<FileEntry>) GetValue(ContentListProperty);
            set => SetValue(ContentListProperty, value);
        }

        /// <summary>
        /// The dependency property of <see cref="SelectedEntry"/>
        /// </summary>
        public static readonly DependencyProperty SelectedEntryProperty = DependencyProperty.Register(
            nameof(SelectedEntry), typeof(FileEntry), typeof(DirectoryContentControl), new PropertyMetadata(default(FileEntry)));

        /// <summary>
        /// Gets or sets the selected file
        /// </summary>
        public FileEntry SelectedEntry
        {
            get => (FileEntry) GetValue(SelectedEntryProperty);
            set => SetValue(SelectedEntryProperty, value);
        }

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(ItemTreeView.SelectedItem is FileEntry entry))
                return;

            SetValue(SelectedEntryProperty, entry);
        }
    }
}
