using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZimLabs.WpfBase;

namespace Trackmania.Helper.DataObjects
{
    /// <summary>
    /// Represents a file entry
    /// </summary>
    public sealed class FileEntry : ObservableObject
    {
        /// <summary>
        /// Contains the file size
        /// </summary>
        private readonly long _fileSize;

        /// <summary>
        /// Backing field for <see cref="Name"/>
        /// </summary>
        private string _name;

        /// <summary>
        /// Gets or sets the value which indicates if the node is the root node
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry
        /// </summary>
        public string Name
        {
            get => _name;
            private set => SetField(ref _name, value);
        }

        /// <summary>
        /// Gets the path of the file / directory
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets or sets the info
        /// </summary>
        public string Info => IsDirectory ? $"({SubNodes.Count})" : _fileSize.ToFormattedFileSize();

        /// <summary>
        /// Gets or sets the value which indicates if the item is a directory
        /// </summary>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Gets or sets the value which indicates if the item is selected or not
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the value which indicates if the item is expanded
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets the icon file kind
        /// </summary>
        public string Kind => IsDirectory ? "Folder" : "File";

        /// <summary>
        /// Gets the file info (only available when <see cref="IsDirectory"/> = <see langword="true"/>)
        /// </summary>
        public FileInfo FileInfo { get; }

        /// <summary>
        /// Gets the count of files
        /// </summary>
        public int FileCount => SubNodes == null
            ? 0
            : SubNodes.Count(c => !c.IsDirectory) + SubNodes.Sum(s => s.FileCount);

        /// <summary>
        /// Backing field for <see cref="ExistsAlready"/>
        /// </summary>
        private bool _existsAlready;

        /// <summary>
        /// Gets or sets the value which indicates if the file already exists (only available when <see cref="IsDirectory"/> = <see langword="false"/>)
        /// </summary>
        public bool ExistsAlready
        {
            get => _existsAlready;
            set
            {
                _existsAlready = value;
                Import = !value;
            }
        }

        /// <summary>
        /// Gets or sets the value which indicates if the file should be imported (only available when <see cref="IsDirectory"/> = <see langword="false"/>)
        /// </summary>
        public bool Import { get; set; }

        /// <summary>
        /// Gets or sets the list with the files. Only available when <see cref="IsRoot"/> is <see langword="true"/>
        /// </summary>
        public List<FileInfo> Files { get; set; }

        /// <summary>
        /// Gets or sets the sub nodes of the entry
        /// </summary>
        public List<FileEntry> SubNodes { get; set; } = new();

        /// <summary>
        /// Creates a new instance of the <see cref="FileEntry"/>
        /// </summary>
        /// <param name="file">The file info object</param>
        public FileEntry(FileInfo file)
        {
            Name = file.Name;
            _fileSize = file.Length;
            FileInfo = file;
            Path = file.FullName;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="FileEntry"/>
        /// </summary>
        /// <param name="directory">The dir info object"/></param>
        public FileEntry(FileSystemInfo directory)
        {
            Name = directory.Name;
            IsDirectory = true;
            Path = directory.FullName;
        }
    }
}
