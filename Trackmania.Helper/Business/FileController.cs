using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManiaPlanetSharp.GameBox;
using Serilog;
using Trackmania.Helper.DataObjects;

namespace Trackmania.Helper.Business
{
    /// <summary>
    /// Provides the functions for the interaction with the files
    /// </summary>
    internal static class FileController
    {
        /// <summary>
        /// Loads the content of the specified directory
        /// </summary>
        /// <param name="directoryPath">The directory which should be loaded</param>
        /// <param name="type">The type of the files</param>
        /// <returns>The content of the directory</returns>
        /// <exception cref="ArgumentNullException">Will be thrown when the <paramref name="directoryPath"/> is null or empty</exception>
        /// <exception cref="FileNotFoundException">Will be thrown when the specified directory doesn't exist</exception>
        public static List<FileEntry> LoadDirectoryContent(string directoryPath, Helper.FileType type)
        {
            var path = GetCompletePath(type, directoryPath);

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
                throw new FileNotFoundException("The specified directory doesn't exist.", path);

            var searchPatterns = GetSearchPatterns(type);
            var result = new List<FileEntry>();
            void AddSubDirs(FileEntry node, DirectoryInfo dir)
            {
                foreach (var subDir in dir.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    var subNode = new FileEntry(subDir);
                    AddSubDirs(subNode, subDir);

                    node.SubNodes.Add(subNode);
                }

                // Add the files
                foreach (var file in dir.GetFiles(searchPatterns, SearchOption.TopDirectoryOnly))
                {
                    var fileNode = new FileEntry(file);

                    node.SubNodes.Add(fileNode);
                }
            }

            var rootDir = new DirectoryInfo(path);

            // Get all directories
            foreach (var dir in rootDir.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                var dirNode = new FileEntry(dir);

                AddSubDirs(dirNode, dir);

                result.Add(dirNode);
            }

            // Get all files
            result.AddRange(rootDir.GetFiles(searchPatterns, SearchOption.TopDirectoryOnly).Select(file => new FileEntry(file)));

            return result;
        }

        /// <summary>
        /// Gets the search patterns based on the specified <see cref="Helper.FileType"/>
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The search patterns</returns>
        private static IReadOnlyCollection<string> GetSearchPatterns(Helper.FileType type)
        {
            var result = new List<string>();

            switch (type)
            {
                case Helper.FileType.Item:
                    result.Add(Helper.Configuration.ExtensionItem);
                    break;
                case Helper.FileType.Block:
                    result.Add(Helper.Configuration.ExtensionBlock);
                    result.Add(Helper.Configuration.ExtensionMacroBlock);
                    break;
                case Helper.FileType.Map:
                    result.Add(Helper.Configuration.ExtensionMap);
                    break;
                case Helper.FileType.Replay:
                    result.Add(Helper.Configuration.ExtensionReplay);
                    break;
                case Helper.FileType.Screenshot:
                    result.Add(Helper.Configuration.ExtensionScreenshot);
                    break;
                default:
                    result.Add("*.*");
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the complete path based on the specified type
        /// </summary>
        /// <param name="type">The desired type</param>
        /// <param name="mainPath">The main path</param>
        /// <returns>The complete path</returns>
        private static string GetCompletePath(Helper.FileType type, string mainPath)
        {
            return type switch
            {
                Helper.FileType.Item => Path.Combine(mainPath, "Items"),
                Helper.FileType.Block => Path.Combine(mainPath, "Blocks"),
                Helper.FileType.Map => Path.Combine(mainPath, "Maps"),
                Helper.FileType.Replay => Path.Combine(mainPath, "Replays"),
                Helper.FileType.Screenshot => Path.Combine(mainPath, "ScreenShots"),
                _ => ""
            };
        }

        /// <summary>
        /// Gets the file information from a GBX file
        /// </summary>
        /// <param name="file">The file from which the information is to be determined</param>
        /// <returns>The file information</returns>
        public static GameBoxFile GetFileInformation(FileEntry file)
        {
            // Check if the file is null / a directory / not a gbx file
            if (file == null || file.IsDirectory || !file.FileInfo.Extension.ContainsIgnoreCase("gbx"))
                return null;

            try
            {
                return GameBoxFile.Parse(file.FileInfo.FullName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error has occurred while parsing the GBX file. File: '{file.FileInfo.FullName}'");
                return null;
            }
        }
    }
}
