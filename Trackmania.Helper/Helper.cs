using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using Trackmania.Helper.DataObjects;
using Trackmania.Helper.Global;

namespace Trackmania.Helper
{
    /// <summary>
    /// Provides several helper functions
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// The type of the files
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// The item entries. Extension with is stored at <see cref="ConfigModel.ExtensionItem"/>
            /// </summary>
            Item,

            /// <summary>
            /// The block entries. Extension with is stored at <see cref="ConfigModel.ExtensionBlock"/>
            /// </summary>
            Block,

            /// <summary>
            /// The map entries. Extension with is stored at <see cref="ConfigModel.ExtensionMap"/>
            /// </summary>
            Map,

            /// <summary>
            /// The replay entries. Extension with is stored at <see cref="ConfigModel.ExtensionReplay"/>
            /// </summary>
            Replay,

            /// <summary>
            /// The screenshot entries. Extension is unknown
            /// </summary>
            Screenshot
        }

        /// <summary>
        /// The path of the configuration file
        /// </summary>
        private static readonly string ConfigFile = Path.Combine(GetBaseFolder(), "Trackmania.Helper.Config.json");

        /// <summary>
        /// Backing field for <see cref="Configuration"/>
        /// </summary>
        private static ConfigModel _configuration;

        /// <summary>
        /// Gets or sets the configuration
        /// </summary>
        public static ConfigModel Configuration
        {
            get => _configuration ??= LoadConfiguration();
            set => _configuration = value;
        }

        #region Config
        /// <summary>
        /// Loads the configuration
        /// </summary>
        /// <returns>The configuration</returns>
        private static ConfigModel LoadConfiguration()
        {
            if (!File.Exists(ConfigFile))
                return new ConfigModel();

            var content = File.ReadAllText(ConfigFile, Encoding.UTF8);

            return JsonConvert.DeserializeObject<ConfigModel>(content);
        }

        /// <summary>
        /// Saves the current configuration
        /// </summary>
        public static void SaveConfiguration()
        {
            var content = JsonConvert.SerializeObject(Configuration, Formatting.Indented);

            File.WriteAllText(ConfigFile, content, Encoding.UTF8);
        }
        #endregion

        #region Logger
        /// <summary>
        /// Init the logger
        /// </summary>
        public static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log\\log-.txt", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} | {Level:u3} | {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }

        /// <summary>
        /// Closes the logger
        /// </summary>
        public static void CloseLogger()
        {
            Log.CloseAndFlush();
        }
        #endregion

        #region General helper functions
        /// <summary>
        /// Gets the path of the base folder
        /// </summary>
        /// <returns>The path of the base folder</returns>
        public static string GetBaseFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Opens the desired url
        /// </summary>
        /// <param name="urlType">The type of the url</param>
        public static void OpenUrl(UrlType urlType)
        {
            var url = urlType switch
            {
                UrlType.AccountPortal => Configuration.UrlAccountPortal,
                UrlType.ItemExchange => Configuration.UrlItemExchange,
                UrlType.ManiaPortal => Configuration.UrlManiaPortal,
                UrlType.OfficialForum => Configuration.UrlOfficialForum,
                UrlType.OfficialPage => Configuration.UrlOfficialPage,
                UrlType.TrackmaniaExchange => Configuration.UrlTrackmaniaExchange,
                UrlType.TrackmaniaNews => Configuration.UrlTrackmaniaNews,
                _ => ""
            };

            if (string.IsNullOrEmpty(url))
                return;

            OpenPath(url);
        }

        /// <summary>
        /// Opens the desired path
        /// </summary>
        /// <param name="path">The path</param>
        public static void OpenPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            Process.Start(path);
        }

        /// <summary>
        /// Gets the build information
        /// </summary>
        /// <returns>The build information</returns>
        public static string GetBuildInformation()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version == null)
                return "/";

            var year = version.Major + 2000; // Add 2000 to get the current year
            var days = version.Minor;
            var minutes = version.Revision;

            var date = new DateTime(year, 1, 1).GetStartOfDay().AddDays(days - 1).AddMinutes(minutes);

            return $"v{version} - {date:yyyy.MM.dd HH:mm} {version.Build + 1}";
        }
        #endregion

        #region Extensions
        /// <summary>
        /// Checks if the string value contains the given substring
        /// </summary>
        /// <param name="value">The string value</param>
        /// <param name="substring">The sub string which should be found</param>
        /// <returns>true when the string value contains the substring, otherwise false</returns>
        public static bool ContainsIgnoreCase(this string value, string substring)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(substring))
                return false;

            return value.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Check two strings for equality and ignores the casing
        /// </summary>
        /// <param name="value">The value which should be checked</param>
        /// <param name="match">The comparative value</param>
        /// <returns>true when the strings are equal, otherwise false</returns>
        public static bool EqualsIgnoreCase(this string value, string match)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(match))
                return false;

            return string.Equals(value, match, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Sets the time of the current date
        /// </summary>
        /// <param name="value">The current date</param>
        /// <param name="hour">The desired hour (0 through 23)</param>
        /// <returns>The given date with the given time</returns>
        public static DateTime SetTime(this DateTime value, int hour)
        {
            return value.SetTime(hour, 0, 0);
        }

        /// <summary>
        /// Sets the time of the current date
        /// </summary>
        /// <param name="value">The current date</param>
        /// <param name="hour">The desired hour (0 through 23)</param>
        /// <param name="minute">The desired minute (0 through 59)</param>
        /// <returns>The given date with the given time</returns>
        public static DateTime SetTime(this DateTime value, int hour, int minute)
        {
            return value.SetTime(hour, minute, 0);
        }

        /// <summary>
        /// Sets the time of the current date
        /// </summary>
        /// <param name="value">The current date</param>
        /// <param name="hour">The desired hour (0 through 23)</param>
        /// <param name="minute">The desired minute (0 through 59)</param>
        /// <param name="second">The desired second (0 through 59)</param>
        /// <returns>The given date with the given time</returns>
        public static DateTime SetTime(this DateTime value, int hour, int minute, int second)
        {
            if (hour > 23)
                hour = 0;

            if (minute > 59)
                minute = 0;

            if (second > 59)
                second = 0;

            return new DateTime(value.Year, value.Month, value.Day, hour, minute, second);
        }

        /// <summary>
        /// Set the time of the given date to 00:00:00
        /// </summary>
        /// <param name="value">The current date</param>
        /// <returns>The given date which time was set to 00:00:00</returns>
        public static DateTime GetStartOfDay(this DateTime value)
        {
            return value.SetTime(0, 0, 0);
        }

        /// <summary>
        /// Sets the time of the given date to 23:59:59
        /// </summary>
        /// <param name="value">The current date</param>
        /// <returns>The given date which time was set to 23:59:59</returns>
        public static DateTime GetEndOfDay(this DateTime value)
        {
            return value.SetTime(23, 59, 59);
        }

        /// <summary>
        /// Converts the file length into a readable size
        /// </summary>
        /// <param name="length">The file length</param>
        /// <returns>The readable size</returns>
        public static string ToFormattedFileSize(this long length)
        {
            switch (length)
            {
                case var _ when length < 1024:
                    return "1 KB";
                case var _ when length >= 1024 && length < Math.Pow(1024, 2):
                    return $"{length / 1024d:N2} KB";
                case var _ when length >= Math.Pow(1024, 2) && length < Math.Pow(1024, 3):
                    return $"{length / Math.Pow(1024, 2):N2} MB";
                case var _ when length >= Math.Pow(1024, 3):
                    return $"{length / Math.Pow(1024, 2):N2} GB";
            }

            return "";
        }
        #endregion
    }
}
