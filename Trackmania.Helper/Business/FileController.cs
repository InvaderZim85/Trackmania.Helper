using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var extension = GetExtension(type);
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
                foreach (var file in dir.GetFiles(extension, SearchOption.TopDirectoryOnly))
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
            result.AddRange(rootDir.GetFiles(extension, SearchOption.TopDirectoryOnly).Select(file => new FileEntry(file)));

            return result;
        }

        /// <summary>
        /// Gets the extension based on the specified <see cref="Helper.FileType"/>
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The extension</returns>
        private static string GetExtension(Helper.FileType type)
        {
            return type switch
            {
                Helper.FileType.Item => Helper.Configuration.ExtensionItem,
                Helper.FileType.Block => Helper.Configuration.ExtensionBlock,
                Helper.FileType.Map => Helper.Configuration.ExtensionMap,
                Helper.FileType.Replay => Helper.Configuration.ExtensionReplay,
                Helper.FileType.Screenshot => Helper.Configuration.ExtensionScreenshot,
                _ => "*.*"
            };
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
    }
}
