using System.IO;

namespace DirLink.Views.Pages;

public class FileMirror : IMirrorLink {

    /// <summary>
    /// Gets or sets the file to mirror into the destination directory.
    /// </summary>
    /// <value>
    /// The file to mirror.
    /// </value>
    public FileInfo File { get; set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="FileMirror"/> struct.
    /// </summary>
    /// <param name="File">The file to mirror.</param>
    public FileMirror( FileInfo File ) => this.File = File;

}