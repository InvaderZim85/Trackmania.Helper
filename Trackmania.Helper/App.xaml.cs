using System;
using System.Windows;
using Serilog;
using Trackmania.Helper.Ui.View;

namespace Trackmania.Helper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Occurs when the application starts
        /// </summary>
        /// <param name="sender">The <see cref="App"/></param>
        /// <param name="e">The event arguments</param>
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Helper.InitLogger();
            Log.Information($"Started application. Build {Helper.GetBuildInformation()}");
            try
            {
                // Start the main window
                new MainWindow().Show();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "A fatal error has occurred.");
            }
            finally
            {
                Helper.CloseLogger();
            }
        }
    }
}
