using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Trackmania.Helper.Ui.ViewModel;

namespace Trackmania.Helper.Ui.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MainWindow"/>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the main window was loaded
        /// </summary>
        /// <param name="sender">The <see cref="MainWindow"/></param>
        /// <param name="e">The event arguments</param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
                viewModel.InitViewModel();
        }
    }
}
