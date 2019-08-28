using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;

namespace CSScratchpad.Script {
    public class DisectCakeBuild : Common, IRunnable {
        public void Run() {
            Console.WriteLine("Yuhuuu");
            Console.WriteLine("Yuhuuu");
        }
    }

    #region : yuhuu :

    public sealed class AssemblyInfoSettings {
        /// <summary>
        /// Gets or sets whether or not the assembly is CLS compliant.
        /// </summary>
        /// <value>Whether or not the assembly is CLS compliant.</value>
        public Boolean? CLSCompliant {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public String Company {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not the assembly is COM visible.
        /// </summary>
        /// <value>Whether or not the assembly is COM visible.</value>
        public Boolean? ComVisible {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration of the assembly.
        /// </summary>
        /// <value>The configuration.</value>
        public String Configuration {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        public String Copyright {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the custom attribute(s) that should be added to the assembly info file.
        /// </summary>
        /// <value>The namespace(s).</value>
        public ICollection<AssemblyInfoCustomAttribute> CustomAttributes {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The assembly description.</value>
        public String Description {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file version.
        /// </summary>
        /// <value>The file version.</value>
        public String FileVersion {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public String Guid {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the informational version.
        /// </summary>
        /// <value>The informational version.</value>
        public String InformationalVersion {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name(s) of the assembly(s) that internals should be visible to.
        /// </summary>
        /// <value>The name(s) of the assembly(s).</value>
        public ICollection<String> InternalsVisibleTo {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>The assembly product.</value>
        public String Product {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The assembly title.</value>
        public String Title {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the trademark.
        /// </summary>
        /// <value>The trademark.</value>
        public String Trademark {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public String Version {
            get;
            set;
        }

        public AssemblyInfoSettings() {
        }
    }

    public sealed class AssemblyInfoCustomAttribute {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The attribute name.</value>
        public String Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace for the attribute.</value>
        public String NameSpace {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value for the attribute.</value>
        public String Value {
            get;
            set;
        }

        public AssemblyInfoCustomAttribute() {
        }
    }

    public sealed class AssemblyInfoParser {
        private const String CSharpNonQuotedPattern = "^\\s*\\[assembly: {0} ?\\((?<attributeValue>.*)\\)";

        private const String CSharpQuotedPattern = "^\\s*\\[assembly: {0} ?\\(\"(?<attributeValue>.*)\"\\)";

        private const String VBNonQuotedPattern = "^\\s*\\<Assembly: {0} ?\\((?<attributeValue>.*)\\)";

        private const String VBQuotedPattern = "^\\s*\\<Assembly: {0} ?\\(\"(?<attributeValue>.*)\"\\)";

        private const String DefaultVersion = "1.0.0.0";

        private readonly IFileSystem _fileSystem = new FileSystem();

        public AssemblyInfoParseResult Parse(String assemblyInfoPath) {
            AssemblyInfoParseResult assemblyInfoParseResult;

            String str = "^\\s*\\[assembly: {0} ?\\((?<attributeValue>.*)\\)";
            String str1 = "^\\s*\\[assembly: {0} ?\\(\"(?<attributeValue>.*)\"\\)";
            IFile file = _fileSystem.GetFile(assemblyInfoPath);
            if (!file.Exists) {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Assembly info file '{0}' does not exist.", assemblyInfoPath));
            }
            if (file.Path.GetExtension() == ".vb") {
                str = "^\\s*\\<Assembly: {0} ?\\((?<attributeValue>.*)\\)";
                str1 = "^\\s*\\<Assembly: {0} ?\\(\"(?<attributeValue>.*)\"\\)";
            }
            using (var streamReader = new StreamReader(file.OpenRead())) {
                String end = streamReader.ReadToEnd();
                assemblyInfoParseResult = new AssemblyInfoParseResult(AssemblyInfoParser.ParseSingle(str, "CLSCompliant", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyCompany", end), AssemblyInfoParser.ParseSingle(str, "ComVisible", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyConfiguration", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyCopyright", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyDescription", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyFileVersion", end) ?? "1.0.0.0", AssemblyInfoParser.ParseSingle(str1, "Guid", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyInformationalVersion", end) ?? "1.0.0.0", AssemblyInfoParser.ParseSingle(str1, "AssemblyProduct", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyTitle", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyTrademark", end), AssemblyInfoParser.ParseSingle(str1, "AssemblyVersion", end) ?? "1.0.0.0", AssemblyInfoParser.ParseMultiple(str1, "InternalsVisibleTo", end));
            }
            return assemblyInfoParseResult;
        }

        private static IEnumerable<String> ParseMultiple(String pattern, String attributeName, String content) {
            var regex = new Regex(String.Format(CultureInfo.InvariantCulture, pattern, attributeName), RegexOptions.Multiline);
            foreach (Match match in regex.Matches(content)) {
                if (match.Groups.Count <= 0) {
                    continue;
                }
                String value = match.Groups["attributeValue"].Value;
                if (String.IsNullOrWhiteSpace(value)) {
                    continue;
                }
                yield return value;
            }
        }

        private static String ParseSingle(String pattern, String attributeName, String content) => AssemblyInfoParser.ParseMultiple(pattern, attributeName, content).SingleOrDefault<String>();
    }

    public sealed class AssemblyInfoParseResult {
        private readonly List<String> _internalsVisibleTo;

        /// <summary>
        /// Gets the assembly file version.
        /// </summary>
        /// <value>The assembly file version.</value>
        public String AssemblyFileVersion {
            get;
        }

        /// <summary>
        /// Gets the assembly informational version.
        /// </summary>
        /// <value>The assembly informational version.</value>
        public String AssemblyInformationalVersion {
            get;
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public String AssemblyVersion {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the assembly is CLS compliant.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assembly is CLS compliant; otherwise, <c>false</c>.
        /// </value>
        public Boolean ClsCompliant {
            get;
        }

        /// <summary>
        /// Gets the assembly company attribute.
        /// </summary>
        /// <value>The assembly company attribute.</value>
        public String Company {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the assembly is accessible from COM.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assembly is accessible from COM; otherwise, <c>false</c>.
        /// </value>
        public Boolean ComVisible {
            get;
        }

        /// <summary>
        /// Gets the assembly configuration attribute.
        /// </summary>
        /// <value>The assembly Configuration attribute.</value>
        public String Configuration {
            get;
        }

        /// <summary>
        /// Gets the assembly copyright attribute.
        /// </summary>
        /// <value>The assembly copyright attribute.</value>
        public String Copyright {
            get;
        }

        /// <summary>
        /// Gets the assembly's description attribute.
        /// </summary>
        /// <value>The assembly's Description attribute.</value>
        public String Description {
            get;
        }

        /// <summary>
        /// Gets the assembly GUID attribute.
        /// </summary>
        /// <value>The assembly GUID attribute.</value>
        public String Guid {
            get;
        }

        /// <summary>
        /// Gets the assemblies that internals are visible to.
        /// </summary>
        /// <value>The assemblies that internals are visible to.</value>
        public ICollection<String> InternalsVisibleTo => _internalsVisibleTo;

        /// <summary>
        /// Gets the assembly product Attribute.
        /// </summary>
        /// <value>The assembly product attribute.</value>
        public String Product {
            get;
        }

        /// <summary>
        /// Gets the assembly title Attribute.
        /// </summary>
        /// <value>The assembly Title attribute.</value>
        public String Title {
            get;
        }

        /// <summary>
        /// Gets the assembly trademark Attribute.
        /// </summary>
        /// <value>The assembly Trademark attribute.</value>
        public String Trademark {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Common.Solution.Project.Properties.AssemblyInfoParseResult" /> class.
        /// </summary>
        /// <param name="clsCompliant">Whether the assembly is CLS compliant.</param>
        /// <param name="company">The assembly company attribute.</param>
        /// <param name="comVisible">Whether the assembly is accessible from COM.</param>
        /// <param name="configuration">The assembly configuration attribute.</param>
        /// <param name="copyright">The assembly copyright attribute.</param>
        /// <param name="description">The assembly description attribute.</param>
        /// <param name="assemblyFileVersion">The assembly file version.</param>
        /// <param name="guid">The assembly GUID attribute.</param>
        /// <param name="assemblyInformationalVersion">The assembly informational version.</param>
        /// <param name="product">The assembly product attribute.</param>
        /// <param name="title">The assembly title attribute.</param>
        /// <param name="trademark">The assembly trademark attribute.</param>
        /// <param name="assemblyVersion">The assembly version.</param>
        /// <param name="internalsVisibleTo">The assemblies that internals are visible to.</param>
        public AssemblyInfoParseResult(String clsCompliant, String company, String comVisible, String configuration, String copyright, String description, String assemblyFileVersion, String guid, String assemblyInformationalVersion, String product, String title, String trademark, String assemblyVersion, IEnumerable<String> internalsVisibleTo) {
            ClsCompliant = String.IsNullOrWhiteSpace(clsCompliant) ? false : Boolean.Parse(clsCompliant);
            Company = company ?? String.Empty;
            ComVisible = String.IsNullOrWhiteSpace(comVisible) ? false : Boolean.Parse(comVisible);
            Configuration = configuration ?? String.Empty;
            Copyright = copyright ?? String.Empty;
            Description = description ?? String.Empty;
            AssemblyFileVersion = assemblyFileVersion ?? String.Empty;
            Guid = guid ?? String.Empty;
            AssemblyInformationalVersion = assemblyInformationalVersion ?? String.Empty;
            Product = product ?? String.Empty;
            Title = title ?? String.Empty;
            Trademark = trademark ?? String.Empty;
            AssemblyVersion = assemblyVersion ?? String.Empty;
            _internalsVisibleTo = new List<String>(internalsVisibleTo ?? Enumerable.Empty<String>());
        }
    }

    /// <summary>
    /// Contains extension methods for <see cref="T:Cake.Core.IO.IFile" />.
    /// </summary>
    public static class FileExtensions {
        /// <summary>Opens the file using the specified options.</summary>
        /// <param name="file">The file.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> to the file.</returns>
        public static Stream Open(this IFile file, FileMode mode) {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            return file.Open(mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None);
        }

        /// <summary>Opens the file using the specified options.</summary>
        /// <param name="file">The file.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> to the file.</returns>
        public static Stream Open(this IFile file, FileMode mode, FileAccess access) {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            return file.Open(mode, access, FileShare.None);
        }

        /// <summary>Opens the file for reading.</summary>
        /// <param name="file">The file.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> to the file.</returns>
        public static Stream OpenRead(this IFile file) {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            return file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens the file for writing.
        /// If the file already exists, it will be overwritten.
        /// </summary>
        /// <param name="file">The file to be opened.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> to the file.</returns>
        public static Stream OpenWrite(this IFile file) {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            return file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        }

        /// <summary>Enumerates line in file</summary>
        /// <param name="file">The file to be read from.</param>
        /// <param name="encoding">The encoding that is applied to the content of the file.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerable`1" /> of file line content</returns>
        public static IEnumerable<String> ReadLines(this IFile file, Encoding encoding) {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            using (Stream stream = file.OpenRead()) {
                using (var streamReader = new StreamReader(stream, encoding)) {
                    var stringList = new List<String>();
                    String str;
                    while ((str = streamReader.ReadLine()) != null)
                        stringList.Add(str);
                    return (IEnumerable<String>) stringList;
                }
            }
        }
    }

    public interface IDirectory : IFileSystemInfo {
        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        DirectoryPath Path {
            get;
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        void Create();

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        void Delete(Boolean recursive);

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        IEnumerable<IDirectory> GetDirectories(String filter, SearchScope scope);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(String filter, SearchScope scope);

        /// <summary>
        /// Moves the directory to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        void Move(DirectoryPath destination);
    }

    public enum SearchScope {
        /// <summary>
        /// The current directory.
        /// </summary>
        Current,
        /// <summary>
        /// The current directory and child directories.
        /// </summary>
        Recursive
    }

    public interface IFileSystem {
        /// <summary>
        /// Gets a <see cref="T:Cake.Core.IO.IDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.IDirectory" /> instance representing the specified path.</returns>
        IDirectory GetDirectory(DirectoryPath path);

        /// <summary>
        /// Gets a <see cref="T:Cake.Core.IO.IFile" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.IFile" /> instance representing the specified path.</returns>
        IFile GetFile(FilePath path);
    }

    public sealed class FileSystem : IFileSystem {
        public FileSystem() {
        }

        /// <summary>
        /// Gets a <see cref="T:Cake.Core.IO.IDirectory" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.IDirectory" /> instance representing the specified path.</returns>
        public IDirectory GetDirectory(DirectoryPath path) => new CakeContextDirectory(path);

        /// <summary>
        /// Gets a <see cref="T:Cake.Core.IO.IFile" /> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.IFile" /> instance representing the specified path.</returns>
        public IFile GetFile(FilePath path) => new CakeContextFile(path);
    }

    internal sealed class CakeContextFile : IFile, IFileSystemInfo {
        private readonly FileInfo _file;

        public FileAttributes Attributes {
            get {
                return _file.Attributes;
            }
            set {
                _file.Attributes = value;
            }
        }

        Path IFileSystemInfo.Path => Path;

        public Boolean Exists => _file.Exists;

        public Boolean Hidden => (_file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        public Int64 Length => _file.Length;

        public FilePath Path {
            get;
        }

        public CakeContextFile(FilePath path) {
            Path = path;
            _file = new FileInfo(path.FullPath);
        }

        public void Copy(FilePath destination, Boolean overwrite) {
            if (destination == null) {
                throw new ArgumentNullException("destination");
            }
            _file.CopyTo(destination.FullPath, overwrite);
        }

        public void Delete() => _file.Delete();

        public void Move(FilePath destination) {
            if (destination == null) {
                throw new ArgumentNullException("destination");
            }
            _file.MoveTo(destination.FullPath);
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare) => _file.Open(fileMode, fileAccess, fileShare);
    }

    internal sealed class CakeContextDirectory : IDirectory, IFileSystemInfo {
        private readonly DirectoryInfo _directory;

        Path IFileSystemInfo.Path => Path;

        public Boolean Exists => _directory.Exists;

        public Boolean Hidden => (_directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        public DirectoryPath Path {
            get;
        }

        public CakeContextDirectory(DirectoryPath path) {
            Path = path;
            _directory = new DirectoryInfo(Path.FullPath);
        }

        public void Create() => _directory.Create();

        public void Delete(Boolean recursive) => _directory.Delete(recursive);

        public IEnumerable<IDirectory> GetDirectories(String filter, SearchScope scope) => from directory in (IEnumerable<DirectoryInfo>) _directory.GetDirectories(filter, scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories)
                                                                                           select new CakeContextDirectory(directory.FullName);

        public IEnumerable<IFile> GetFiles(String filter, SearchScope scope) => from file in (IEnumerable<FileInfo>) _directory.GetFiles(filter, scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories)
                                                                                select new CakeContextFile(file.FullName);

        public void Move(DirectoryPath destination) {
            if (destination == null) {
                throw new ArgumentNullException("destination");
            }
            _directory.MoveTo(destination.FullPath);
        }
    }

    public interface IFile : IFileSystemInfo {
        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <value>The file attributes.</value>
        FileAttributes Attributes {
            get;
            set;
        }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        Int64 Length {
            get;
        }

        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        /// <value>The path.</value>
        FilePath Path {
            get;
        }

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        void Copy(FilePath destination, Boolean overwrite);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        void Delete();

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        void Move(FilePath destination);

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> to the file.</returns>
        Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);
    }

    public interface IFileSystemInfo {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Cake.Core.IO.IFileSystemInfo" /> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry exists; otherwise, <c>false</c>.
        /// </value>
        Boolean Exists {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Cake.Core.IO.IFileSystemInfo" /> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry is hidden; otherwise, <c>false</c>.
        /// </value>
        Boolean Hidden {
            get;
        }

        /// <summary>
        /// Gets the path to the entry.
        /// </summary>
        /// <value>The path.</value>
        Path Path {
            get;
        }
    }

    public sealed class FilePath : Path {
        /// <summary>
        /// Gets a value indicating whether this path has a file extension.
        /// </summary>
        /// <value>
        /// <c>true</c> if this file path has a file extension; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasExtension => System.IO.Path.HasExtension(base.FullPath);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Core.IO.FilePath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public FilePath(String path) : base(path) {
        }

        /// <summary>
        /// Appends a file extension to the path.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>A new <see cref="T:Cake.Core.IO.FilePath" /> with an appended extension.</returns>
        public FilePath AppendExtension(String extension) {
            if (extension == null) {
                throw new ArgumentNullException("extension");
            }
            if (!extension.StartsWith(".", StringComparison.OrdinalIgnoreCase)) {
                extension = String.Concat(".", extension);
            }
            return new FilePath(String.Concat(base.FullPath, extension));
        }

        /// <summary>
        /// Changes the file extension of the path.
        /// </summary>
        /// <param name="extension">The new extension.</param>
        /// <returns>A new <see cref="T:Cake.Core.IO.FilePath" /> with a new extension.</returns>
        public FilePath ChangeExtension(String extension) => new FilePath(System.IO.Path.ChangeExtension(base.FullPath, extension));

        /// <summary>
        /// Collapses a <see cref="T:Cake.Core.IO.FilePath" /> containing ellipses.
        /// </summary>
        /// <returns>A collapsed <see cref="T:Cake.Core.IO.FilePath" />.</returns>
        public FilePath Collapse() => new FilePath(PathCollapser.Collapse(this));

        /// <summary>
        /// Performs a conversion from <see cref="T:System.String" /> to <see cref="T:Cake.Core.IO.FilePath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.FilePath" />.</returns>
        public static FilePath FromString(String path) => new FilePath(path);

        /// <summary>
        /// Gets the directory part of the path.
        /// </summary>
        /// <returns>The directory part of the path.</returns>
        public DirectoryPath GetDirectory() {
            String directoryName = System.IO.Path.GetDirectoryName(base.FullPath);
            if (String.IsNullOrWhiteSpace(directoryName)) {
                directoryName = "./";
            }
            return new DirectoryPath(directoryName);
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <returns>The file extension.</returns>
        public String GetExtension() {
            String extension = System.IO.Path.GetExtension(base.FullPath);
            if (!String.IsNullOrWhiteSpace(extension)) {
                return extension;
            }
            return null;
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <returns>The filename.</returns>
        public FilePath GetFilename() => new FilePath(System.IO.Path.GetFileName(base.FullPath));

        /// <summary>
        /// Gets the filename without its extension.
        /// </summary>
        /// <returns>The filename without its extension.</returns>
        public FilePath GetFilenameWithoutExtension() => new FilePath(System.IO.Path.GetFileNameWithoutExtension(base.FullPath));

        /// <summary>
        /// Get the relative path to another directory.
        /// </summary>
        /// <param name="to">The target directory path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.DirectoryPath" />.</returns>
        public DirectoryPath GetRelativePath(DirectoryPath to) => GetDirectory().GetRelativePath(to);

        /// <summary>
        /// Get the relative path to another file.
        /// </summary>
        /// <param name="to">The target file path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.FilePath" />.</returns>
        public FilePath GetRelativePath(FilePath to) => GetDirectory().GetRelativePath(to);

        /// <summary>
        /// Makes the path absolute (if relative) using the current working directory.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An absolute path.</returns>
        public FilePath MakeAbsolute(ICakeEnvironment environment) {
            if (environment == null) {
                throw new ArgumentNullException("environment");
            }
            if (!base.IsRelative) {
                return new FilePath(base.FullPath);
            }
            return environment.WorkingDirectory.CombineWithFilePath(this).Collapse();
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the specified directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An absolute path.</returns>
        public FilePath MakeAbsolute(DirectoryPath path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (path.IsRelative) {
                throw new InvalidOperationException("Cannot make a file path absolute with a relative directory path.");
            }
            if (!base.IsRelative) {
                return new FilePath(base.FullPath);
            }
            return path.CombineWithFilePath(this).Collapse();
        }

        public static implicit operator FilePath(String path) {
            return FilePath.FromString(path);
        }
    }

    public abstract class Path {
        private static readonly Char[] _invalidPathCharacters;

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public String FullPath {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this path is relative.
        /// </summary>
        /// <value>
        /// <c>true</c> if this path is relative; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsRelative {
            get;
        }

        /// <summary>
        /// Gets the segments making up the path.
        /// </summary>
        /// <value>The segments making up the path.</value>
        public String[] Segments {
            get;
        }

        static Path() {
            Char[] invalidPathChars = System.IO.Path.GetInvalidPathChars();
            Char[] chrArray = new Char[] { '*', '?' };
            Path._invalidPathCharacters = ((IEnumerable<Char>) invalidPathChars).Concat<Char>((IEnumerable<Char>) chrArray).ToArray<Char>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Core.IO.Path" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        protected Path(String path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (String.IsNullOrWhiteSpace(path)) {
                throw new ArgumentException("Path cannot be empty.", "path");
            }
            String str = path;
            for (Int32 i = 0; i < str.Length; i++) {
                Char chr = str[i];
                if (Path._invalidPathCharacters.Contains<Char>(chr)) {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Illegal characters in path ({0}).", chr), "path");
                }
            }
            FullPath = path.Replace('\\', '/').Trim();
            FullPath = FullPath == "./" ? String.Empty : FullPath;
            if (FullPath.StartsWith("./", StringComparison.Ordinal)) {
                FullPath = FullPath.Substring(2);
            }
            FullPath = FullPath.TrimEnd(new Char[] { '/', '\\' });
            if (FullPath.EndsWith(":", StringComparison.OrdinalIgnoreCase)) {
                FullPath = String.Concat(FullPath, "/");
            }
            IsRelative = !System.IO.Path.IsPathRooted(FullPath);
            Segments = FullPath.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (FullPath.StartsWith("/") && Segments.Length != 0) {
                Segments[0] = String.Concat("/", Segments[0]);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this path.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override String ToString() => FullPath;
    }

    public interface ICakeEnvironment {
        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <value>The application root path.</value>
        DirectoryPath ApplicationRoot {
            get;
        }

        /// <summary>
        /// Gets the platform Cake is running on.
        /// </summary>
        /// <value>The platform Cake is running on.</value>
        ICakePlatform Platform {
            get;
        }

        /// <summary>
        /// Gets the runtime Cake is running in.
        /// </summary>
        /// <value>The runtime Cake is running in.</value>
        ICakeRuntime Runtime {
            get;
        }

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>The working directory.</value>
        DirectoryPath WorkingDirectory {
            get;
            set;
        }

        /// <summary>
        /// Gets the application root path.
        /// </summary>
        /// <returns>The application root path.</returns>
        [Obsolete("Please use ICakeEnvironment.ApplicationRoot instead.")]
        DirectoryPath GetApplicationRoot();

        /// <summary>
        /// Gets an environment variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <returns>The value of the environment variable.</returns>
        String GetEnvironmentVariable(String variable);

        /// <summary>
        /// Gets all environment variables.
        /// </summary>
        /// <returns>The environment variables as IDictionary&lt;string, string&gt; </returns>
        IDictionary<String, String> GetEnvironmentVariables();

        /// <summary>
        /// Gets a special path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.DirectoryPath" /> to the special path.</returns>
        DirectoryPath GetSpecialPath(SpecialPath path);

        /// <summary>
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        [Obsolete("Please use ICakeEnvironment.Runtime.TargetFramework instead.")]
        FrameworkName GetTargetFramework();

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        [Obsolete("Please use ICakeEnvironment.Platform.Is64Bit instead.")]
        Boolean Is64BitOperativeSystem();

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        [Obsolete("Please use ICakeEnvironment.Platform.IsUnix instead.")]
        Boolean IsUnix();
    }

    public enum SpecialPath {
        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data for the current roaming user.
        /// </summary>
        ApplicationData,
        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data that is used by all users.
        /// </summary>
        CommonApplicationData,
        /// <summary>
        /// The directory that serves as a common repository for application-specific
        /// data that is used by the current, non-roaming user.
        /// </summary>
        LocalApplicationData,
        /// <summary>
        /// The Program Files folder.
        /// </summary>
        ProgramFiles,
        /// <summary>
        /// The Program Files (X86) folder.
        /// </summary>
        ProgramFilesX86,
        /// <summary>
        /// The Windows folder.
        /// </summary>
        Windows,
        /// <summary>
        /// The current user's temporary folder.
        /// </summary>
        LocalTemp
    }

    public interface ICakeRuntime {
        /// <summary>
        /// Gets the version of Cake executing the script.
        /// </summary>
        /// <returns>The Cake.exe version.</returns>
        Version CakeVersion {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether we're running on CoreClr.
        /// </summary>
        /// <value>
        /// <c>true</c> if we're runnning on CoreClr; otherwise, <c>false</c>.
        /// </value>
        Boolean IsCoreClr {
            get;
        }

        /// <summary>
        /// Gets the target .NET framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        FrameworkName TargetFramework {
            get;
        }
    }

    public interface ICakePlatform {
        /// <summary>
        /// Gets the platform family.
        /// </summary>
        /// <value>The platform family.</value>
        PlatformFamily Family {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether or not the current operative system is 64 bit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if current operative system is 64 bit; otherwise, <c>false</c>.
        /// </value>
        Boolean Is64Bit {
            get;
        }
    }

    public enum PlatformFamily {
        /// <summary>
        /// The platform family is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// Represents the Windows platform family.
        /// </summary>
        Windows,
        /// <summary>
        /// Represents the Linux platform family.
        /// </summary>
        Linux,
        /// <summary>
        /// Represents the OSX platform family.
        /// </summary>
        OSX
    }

    public sealed class DirectoryPath : Path {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Core.IO.DirectoryPath" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public DirectoryPath(String path) : base(path) {
        }

        /// <summary>
        /// Collapses a <see cref="T:Cake.Core.IO.DirectoryPath" /> containing ellipses.
        /// </summary>
        /// <returns>A collapsed <see cref="T:Cake.Core.IO.DirectoryPath" />.</returns>
        public DirectoryPath Collapse() => new DirectoryPath(PathCollapser.Collapse(this));

        /// <summary>
        /// Combines the current path with another <see cref="T:Cake.Core.IO.DirectoryPath" />.
        /// The provided <see cref="T:Cake.Core.IO.DirectoryPath" /> must be relative.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A combination of the current path and the provided <see cref="T:Cake.Core.IO.DirectoryPath" />.</returns>
        public DirectoryPath Combine(DirectoryPath path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative) {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute directory path.");
            }
            return new DirectoryPath(System.IO.Path.Combine(base.FullPath, path.FullPath));
        }

        /// <summary>
        /// Combines the current path with a <see cref="T:Cake.Core.IO.FilePath" />.
        /// The provided <see cref="T:Cake.Core.IO.FilePath" /> must be relative.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A combination of the current path and the provided <see cref="T:Cake.Core.IO.FilePath" />.</returns>
        public FilePath CombineWithFilePath(FilePath path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (!path.IsRelative) {
                throw new InvalidOperationException("Cannot combine a directory path with an absolute file path.");
            }
            return new FilePath(System.IO.Path.Combine(base.FullPath, path.FullPath));
        }

        /// <summary>
        /// Performs a conversion from <see cref="T:System.String" /> to <see cref="T:Cake.Core.IO.DirectoryPath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.DirectoryPath" />.</returns>
        public static DirectoryPath FromString(String path) => new DirectoryPath(path);

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        /// <returns>The directory name.</returns>
        /// <remarks>
        ///    If this is passed a file path, it will return the file name.
        ///    This is by-and-large equivalent to how DirectoryInfo handles this scenario.
        ///    If we wanted to return the *actual* directory name, we'd need to pull in IFileSystem,
        ///    and do various checks to make sure things exists.
        /// </remarks>
        public String GetDirectoryName() => base.Segments.Last<String>();

        /// <summary>
        /// Combines the current path with the file name of a <see cref="T:Cake.Core.IO.FilePath" />.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A combination of the current path and the file name of the provided <see cref="T:Cake.Core.IO.FilePath" />.</returns>
        public FilePath GetFilePath(FilePath path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            return new FilePath(System.IO.Path.Combine(base.FullPath, path.GetFilename().FullPath));
        }

        /// <summary>
        /// Get the relative path to another directory.
        /// </summary>
        /// <param name="to">The target directory path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.DirectoryPath" />.</returns>
        public DirectoryPath GetRelativePath(DirectoryPath to) => RelativePathResolver.Resolve(this, to);

        /// <summary>
        /// Get the relative path to another file.
        /// </summary>
        /// <param name="to">The target file path.</param>
        /// <returns>A <see cref="T:Cake.Core.IO.FilePath" />.</returns>
        public FilePath GetRelativePath(FilePath to) {
            if (to == null) {
                throw new ArgumentNullException("to");
            }
            return GetRelativePath(to.GetDirectory()).GetFilePath(to.GetFilename());
        }

        /// <summary>
        /// Makes the path absolute to another (absolute) path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An absolute path.</returns>
        public DirectoryPath MakeAbsolute(DirectoryPath path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (path.IsRelative) {
                throw new InvalidOperationException("The provided path cannot be relative.");
            }
            if (!base.IsRelative) {
                return new DirectoryPath(base.FullPath);
            }
            return path.Combine(this).Collapse();
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the current working directory.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An absolute path.</returns>
        public DirectoryPath MakeAbsolute(ICakeEnvironment environment) {
            if (environment == null) {
                throw new ArgumentNullException("environment");
            }
            if (!base.IsRelative) {
                return new DirectoryPath(base.FullPath);
            }
            return environment.WorkingDirectory.Combine(this).Collapse();
        }

        public static implicit operator DirectoryPath(String path) {
            return DirectoryPath.FromString(path);
        }
    }

    internal static class PathCollapser {
        public static String Collapse(Path path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            var strs = new Stack<String>();
            String[] strArrays = path.FullPath.Split(new Char[] { '/', '\\' });
            for (Int32 i = 0; i <  strArrays.Length; i++) {
                String str = strArrays[i];
                if (str != ".") {
                    if (str != "..") {
                        strs.Push(str);
                    }
                    else if (strs.Count > 1) {
                        strs.Pop();
                    }
                }
            }
            String str1 = String.Join("/", strs.Reverse<String>());
            if (str1 != String.Empty) {
                return str1;
            }
            return ".";
        }
    }

    internal static class RelativePathResolver {
        public static DirectoryPath Resolve(DirectoryPath from, DirectoryPath to) {
            if (from == null) {
                throw new ArgumentNullException("from");
            }
            if (to == null) {
                throw new ArgumentNullException("to");
            }
            if (to.IsRelative) {
                throw new InvalidOperationException("Target path must be an absolute path.");
            }
            if (from.IsRelative) {
                throw new InvalidOperationException("Source path must be an absolute path.");
            }
            if (from.Segments[0] != to.Segments[0]) {
                throw new InvalidOperationException("Paths must share a common prefix.");
            }
            if (String.CompareOrdinal(from.FullPath, to.FullPath) == 0) {
                return new DirectoryPath(".");
            }
            Int32 num = Math.Min(from.Segments.Length, to.Segments.Length);
            Int32 num1 = 1;
            for (Int32 i = 1; i < num && String.CompareOrdinal(from.Segments[i], to.Segments[i]) == 0; i++) {
                num1++;
            }
            IEnumerable<String> strs = Enumerable.Repeat<String>("..", from.Segments.Length - num1);
            IEnumerable<String> strs1 = to.Segments.Skip<String>(num1);
            return new DirectoryPath(System.IO.Path.Combine(strs.Concat<String>(strs1).ToArray<String>()));
        }
    }

    #endregion

    #region : yuhuu 2 :

    /// <summary>Represents the environment Cake operates in.</summary>
    public sealed class CakeEnvironment : ICakeEnvironment {
        private readonly ICakeLog _log;

        /// <summary>Gets or sets the working directory.</summary>
        /// <value>The working directory.</value>
        public DirectoryPath WorkingDirectory {
            get {
                return System.IO.Directory.GetCurrentDirectory();
            }
            set {
                CakeEnvironment.SetWorkingDirectory(value);
            }
        }

        /// <summary>Gets the application root path.</summary>
        /// <value>The application root path.</value>
        public DirectoryPath ApplicationRoot { get; }

        /// <summary>Gets the platform Cake is running on.</summary>
        /// <value>The platform Cake is running on.</value>
        public ICakePlatform Platform { get; }

        /// <summary>Gets the runtime Cake is running in.</summary>
        /// <value>The runtime Cake is running in.</value>
        public ICakeRuntime Runtime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Core.CakeEnvironment" /> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <param name="runtime">The runtime.</param>
        /// <param name="log">The log.</param>
        public CakeEnvironment(ICakePlatform platform, ICakeRuntime runtime, ICakeLog log) {
            Platform = platform;
            Runtime = runtime;
            _log = log;
            ApplicationRoot = new DirectoryPath(System.IO.Path.GetDirectoryName(AssemblyHelper.GetExecutingAssembly().Location));
            WorkingDirectory = new DirectoryPath(System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>Gets a special path.</summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="T:Cake.Core.IO.DirectoryPath" /> to the special path.
        /// </returns>
        public DirectoryPath GetSpecialPath(SpecialPath path) => SpecialPathHelper.GetFolderPath(Platform, path);

        /// <summary>Gets an environment variable.</summary>
        /// <param name="variable">The variable.</param>
        /// <returns>The value of the environment variable.</returns>
        public String GetEnvironmentVariable(String variable) => Environment.GetEnvironmentVariable(variable);

        /// <summary>Gets all environment variables.</summary>
        /// <returns>The environment variables as IDictionary&lt;string, string&gt; </returns>
        public IDictionary<String, String> GetEnvironmentVariables() => (IDictionary<String, String>) Environment.GetEnvironmentVariables().Cast<DictionaryEntry>().Aggregate<DictionaryEntry, Dictionary<String, String>, Dictionary<String, String>>(new Dictionary<String, String>((IEqualityComparer<String>) StringComparer.OrdinalIgnoreCase), (dictionary, entry) => {
            String key = (String) entry.Key;
            String x = entry.Value as String;
            if (dictionary.TryGetValue(key, out String y)) {
                if (!StringComparer.OrdinalIgnoreCase.Equals(x, y))
                    Console.WriteLine("GetEnvironmentVariables() encountered duplicate for key: {0}, value: {1} (existing value: {2})", key, x, y);
            }
            else
                dictionary.Add(key, x);
            return dictionary;
        }, dictionary => dictionary);

        /// <summary>
        /// Gets whether or not the current operative system is 64 bit.
        /// </summary>
        /// <returns>
        /// Whether or not the current operative system is 64 bit.
        /// </returns>
        [Obsolete("Please use CakeEnvironment.Platform.Is64Bit instead.")]
        public Boolean Is64BitOperativeSystem() => Platform.Is64Bit;

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        [Obsolete("Please use CakeEnvironment.Platform.IsUnix instead.")]
        public Boolean IsUnix() => Platform.IsUnix();

        /// <summary>Gets the application root path.</summary>
        /// <returns>The application root path.</returns>
        [Obsolete("Please use CakeEnvironment.ApplicationRoot instead.")]
        public DirectoryPath GetApplicationRoot() => ApplicationRoot;

        /// <summary>
        /// Gets the target .Net framework version that the current AppDomain is targeting.
        /// </summary>
        /// <returns>The target framework.</returns>
        [Obsolete("Please use CakeEnvironment.Runtime.TargetFramework instead.")]
        public FrameworkName GetTargetFramework() => Runtime.TargetFramework;

        private static void SetWorkingDirectory(DirectoryPath path) {
            if (path.IsRelative)
                throw new InvalidOperationException("Working directory can not be set to a relative path.");
            System.IO.Directory.SetCurrentDirectory(path.FullPath);
        }
    }

    internal static class SpecialPathHelper {
        public static DirectoryPath GetFolderPath(ICakePlatform platform, SpecialPath path) {
            switch (path) {
                case SpecialPath.LocalTemp:
                    return new DirectoryPath(System.IO.Path.GetTempPath());
                case SpecialPath.ApplicationData:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                case SpecialPath.CommonApplicationData:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                case SpecialPath.LocalApplicationData:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                case SpecialPath.ProgramFiles:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                case SpecialPath.ProgramFilesX86:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
                case SpecialPath.Windows:
                    return new DirectoryPath(Environment.GetFolderPath(Environment.SpecialFolder.Windows));
                default:
                    throw new NotSupportedException(String.Format((IFormatProvider) CultureInfo.InvariantCulture, "The special path '{0}' is not supported.", path));
            }
        }
    }

    /// <summary>Represents a project in a MSBuild solution.</summary>
    public class SolutionProject {
        /// <summary>Gets the project identity.</summary>
        public String Id { get; }

        /// <summary>Gets the project name.</summary>
        public String Name { get; }

        /// <summary>Gets the project path.</summary>
        public FilePath Path { get; }

        /// <summary>Gets the project type identity.</summary>
        public String Type { get; }

        /// <summary>Gets the parent project if any, otherwise null.</summary>
        public SolutionProject Parent { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Common.Solution.SolutionProject" /> class.
        /// </summary>
        /// <param name="id">The project identity.</param>
        /// <param name="name">The project name.</param>
        /// <param name="path">The project path.</param>
        /// <param name="type">The project type identity.</param>
        public SolutionProject(String id, String name, FilePath path, String type) {
            Id = id;
            Name = name;
            Path = path;
            Type = type;
        }
    }

    /// <summary>The MSBuild solution file parser.</summary>
    public sealed class SolutionParser {
        private readonly IFileSystem _fileSystem = new FileSystem();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Common.Solution.SolutionParser" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public SolutionParser() { }

        /// <summary>Parses a MSBuild solution.</summary>
        /// <param name="solutionPath">The solution path.</param>
        /// <returns>A parsed solution.</returns>
        public SolutionParserResult Parse(String solutionPath) {
            IFile file = _fileSystem.GetFile(solutionPath);
            if (!file.Exists)
                throw new InvalidOperationException(String.Format((IFormatProvider) CultureInfo.InvariantCulture, "Solution file '{0}' does not exist.", solutionPath));
            String version = null;
            String visualStudioVersion = null;
            String minimumVisualStudioVersion = null;
            var projects = new List<SolutionProject>();
            Boolean flag = false;
            foreach (String readLine in file.ReadLines(Encoding.UTF8)) {
                String line = readLine.Trim();
                if (readLine.StartsWith("Project(\"{")) {
                    SolutionProject solutionProjectLine = SolutionParser.ParseSolutionProjectLine(file, readLine);
                    if (StringComparer.OrdinalIgnoreCase.Equals(solutionProjectLine.Type, "{2150E333-8FDC-42A3-9474-1A3956D46DE8}"))
                        projects.Add(new SolutionFolder(solutionProjectLine.Id, solutionProjectLine.Name, solutionProjectLine.Path));
                    else
                        projects.Add(solutionProjectLine);
                }
                else if (readLine.StartsWith("Microsoft Visual Studio Solution File, "))
                    version = String.Concat<Char>(readLine.Skip<Char>(39));
                else if (readLine.StartsWith("VisualStudioVersion = "))
                    visualStudioVersion = String.Concat<Char>(readLine.Skip<Char>(22));
                else if (readLine.StartsWith("MinimumVisualStudioVersion = "))
                    minimumVisualStudioVersion = String.Concat<Char>(readLine.Skip<Char>(29));
                else if (line.StartsWith("GlobalSection(NestedProjects)"))
                    flag = true;
                else if (flag && line.StartsWith("EndGlobalSection"))
                    flag = false;
                else if (flag)
                    SolutionParser.ParseNestedProjectLine(projects, line);
            }
            return new SolutionParserResult(version, visualStudioVersion, minimumVisualStudioVersion, (IReadOnlyCollection<SolutionProject>) projects.AsReadOnly());
        }

        private static SolutionProject ParseSolutionProjectLine(IFile file, String line) {
            Boolean flag = false;
            var stringBuilder1 = new StringBuilder();
            var stringBuilder2 = new StringBuilder();
            var stringBuilder3 = new StringBuilder();
            var stringBuilder4 = new StringBuilder();
            var stringBuilderArray = new StringBuilder[4]
            {
            stringBuilder1,
            stringBuilder2,
            stringBuilder3,
            stringBuilder4
            };
            Int32 index = 0;
            foreach (Char ch in line.Skip<Char>(8)) {
                if (ch == 34) {
                    flag = !flag;
                    if (!flag) {
                        if (index++ >= stringBuilderArray.Length)
                            break;
                    }
                }
                else if (flag)
                    stringBuilderArray[index].Append(ch);
            }
            return new SolutionProject(stringBuilder4.ToString(), stringBuilder2.ToString(), file.Path.GetDirectory().CombineWithFilePath((FilePath) stringBuilder3.ToString()), stringBuilder1.ToString());
        }

        private static void ParseNestedProjectLine(List<SolutionProject> projects, String line) {
            String[] projectIds = line.Split(new String[1]
            {
            " = "
            }, StringSplitOptions.RemoveEmptyEntries);
            SolutionProject solutionProject = projects.FirstOrDefault<SolutionProject>(x => StringComparer.OrdinalIgnoreCase.Equals(x.Id, projectIds[0].Trim()));
            if (solutionProject == null)
                return;
            var solutionFolder = projects.FirstOrDefault<SolutionProject>(x => StringComparer.OrdinalIgnoreCase.Equals(x.Id, projectIds[1].Trim())) as SolutionFolder;
            if (solutionFolder == null)
                return;
            solutionFolder.Items.Add(solutionProject);
            solutionProject.Parent = solutionFolder;
        }
    }

    /// <summary>Represents a folder in a MSBuild solution.</summary>
    public sealed class SolutionFolder : SolutionProject {
        /// <summary>Visual Studio project type guid for solution folder</summary>
        /// <remarks>
        /// More information can be found http://www.codeproject.com/Reference/720512/List-of-Visual-Studio-Project-Type-GUIDs
        /// </remarks>
        public const String TypeIdentifier = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

        /// <summary>Gets Child items of this folder</summary>
        public List<SolutionProject> Items { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Common.Solution.SolutionFolder" /> class.
        /// </summary>
        /// <param name="id">The folder project identity.</param>
        /// <param name="name">The folder name.</param>
        /// <param name="path">The folder path.</param>
        public SolutionFolder(String id, String name, FilePath path)
          : base(id, name, path, "{2150E333-8FDC-42A3-9474-1A3956D46DE8}") {
            Items = new List<SolutionProject>();
        }
    }

    /// <summary>Represents the content in an MSBuild solution file.</summary>
    public sealed class SolutionParserResult {
        /// <summary>Gets the file format version.</summary>
        public String Version { get; }

        /// <summary>
        /// Gets the version of Visual Studio that created the file.
        /// </summary>
        public String VisualStudioVersion { get; }

        /// <summary>Gets the minimum supported version of Visual Studio.</summary>
        public String MinimumVisualStudioVersion { get; }

        /// <summary>Gets all solution projects.</summary>
        public IReadOnlyCollection<SolutionProject> Projects { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Cake.Common.Solution.SolutionParserResult" /> class.
        /// </summary>
        /// <param name="version">The file format version.</param>
        /// <param name="visualStudioVersion">The version of Visual Studio that created the file.</param>
        /// <param name="minimumVisualStudioVersion">The minimum supported version of Visual Studio.</param>
        /// <param name="projects">The solution projects.</param>
        public SolutionParserResult(String version, String visualStudioVersion, String minimumVisualStudioVersion, IReadOnlyCollection<SolutionProject> projects) {
            Version = version;
            VisualStudioVersion = visualStudioVersion;
            MinimumVisualStudioVersion = minimumVisualStudioVersion;
            Projects = projects;
        }
    }

    /// <summary>
    /// Contains extension methods for <see cref="T:Cake.Core.ICakePlatform" />.
    /// </summary>
    public static class CakePlatformExtensions {
        /// <summary>
        /// Determines whether the specified platform is a Unix platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns><c>true</c> if the platform is a Unix platform; otherwise <c>false</c>.</returns>
        public static Boolean IsUnix(this ICakePlatform platform) {
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));
            return EnvironmentHelper.IsUnix(platform.Family);
        }
    }

    internal class EnvironmentHelper {
        private static Boolean? _isRunningOnMac;

        public static Boolean Is64BitOperativeSystem() => Environment.Is64BitOperatingSystem;

        public static PlatformFamily GetPlatformFamily() {
            Int32 platform = (Int32) Environment.OSVersion.Platform;
            if (platform <= 3 || platform == 5)
                return PlatformFamily.Windows;
            if (!EnvironmentHelper._isRunningOnMac.HasValue)
                EnvironmentHelper._isRunningOnMac = new global::System.Boolean?(Native.MacOSX.IsRunningOnMac());
            Boolean? isRunningOnMac = EnvironmentHelper._isRunningOnMac;
            if ((isRunningOnMac.HasValue ? (isRunningOnMac.GetValueOrDefault() ? 1 : 0) : (platform == 6 ? 1 : 0)) != 0)
                return PlatformFamily.OSX;
            return platform == 4 || platform == 6 || platform == 128 ? PlatformFamily.Linux : PlatformFamily.Unknown;
        }

        public static Boolean IsCoreClr() => false;

        public static Boolean IsUnix() => EnvironmentHelper.IsUnix(EnvironmentHelper.GetPlatformFamily());

        public static Boolean IsUnix(PlatformFamily family) {
            if (family != PlatformFamily.Linux)
                return family == PlatformFamily.OSX;
            return true;
        }

        public static FrameworkName GetFramework() => new FrameworkName(".NETFramework,Version=v4.6.1");
    }

    internal static class Native {
        public static class MacOSX {
            [DllImport("libc")]
            internal static extern Int32 uname(IntPtr buf);

            public static Boolean IsRunningOnMac() {
                try {
                    IntPtr num = IntPtr.Zero;
                    try {
                        num = Marshal.AllocHGlobal(8192);
                        if (Native.MacOSX.uname(num) == 0) {
                            if (Marshal.PtrToStringAnsi(num) == "Darwin")
                                return true;
                        }
                    }
                    finally {
                        if (num != IntPtr.Zero)
                            Marshal.FreeHGlobal(num);
                    }
                }
                catch {
                }
                return false;
            }
        }
    }

    internal static class AssemblyHelper {
        public static Assembly GetExecutingAssembly() => Assembly.GetExecutingAssembly();

        public static Assembly LoadAssembly(AssemblyName assemblyName) {
            if (assemblyName == null)
                throw new ArgumentNullException(nameof(assemblyName));
            return Assembly.Load(assemblyName);
        }

        public static Assembly LoadAssembly(ICakeEnvironment environment, IFileSystem fileSystem, FilePath path) => Assembly.LoadFrom(path.FullPath);
    }

    /// <summary>Represents a log.</summary>
    public interface ICakeLog {
        /// <summary>Gets or sets the verbosity.</summary>
        /// <value>The verbosity.</value>
        Verbosity Verbosity { get; set; }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void Write(Verbosity verbosity, LogLevel level, String format, params Object[] args);
    }

    /// <summary>Represents verbosity.</summary>
    public enum Verbosity {
        Quiet,
        Minimal,
        Normal,
        Verbose,
        Diagnostic,
    }

    /// <summary>Represents a log level.</summary>
    public enum LogLevel {
        Fatal,
        Error,
        Warning,
        Information,
        Verbose,
        Debug,
    }

    #endregion
}
