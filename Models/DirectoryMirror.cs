using System.IO;

namespace DirLink.Views.Pages;

public class DirectoryMirror : IMirrorLink {

    /// <summary>
    /// Initialises a new instance of the <see cref="DirectoryMirror"/> class.
    /// </summary>
    /// <param name="Folder">The folder.</param>
    /// <param name="OnlyChildren">If <see langword="true" />, only the children are mirrored; otherwise the entire directory is mirrored. See <see cref="OnlyChildren"/> for more information.</param>
    public DirectoryMirror(DirectoryInfo Folder, bool OnlyChildren = false ) {
        this.Folder = Folder;
        this.OnlyChildren = OnlyChildren;
    }

    /// <summary>
    /// Gets or sets the folder to mirror into the destination directory.
    /// </summary>
    /// <value>
    /// The folder to mirror.
    /// </value>
    public DirectoryInfo Folder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only the children in the directory should be mirrored, or the entire directory.
    /// </summary>
    /// <remarks>
    /// <b>Example:</b><br/>
    /// <example>
    /// <br/>Given a directory with the following children:<br/>
    /// <![CDATA[
    /// CopyDir/
    ///     SomeFolder/
    ///         SubFileC.pdf
    ///     FileA.txt
    ///     FileB.mp3
    /// ]]>
    /// <para/>With <see cref="OnlyChildren"/> <see langword="true"/>, the destination will look like:<br/>
    /// <![CDATA[
    /// BaseDir/
    ///     SomeFolder/
    ///         SubFileC.pdf
    ///     FileA.txt
    ///     FileB.mp3
    /// ]]>
    /// <para/>With <see cref="OnlyChildren"/> <see langword="false"/>, the destination will look like:<br/>
    /// <![CDATA[
    /// BaseDir/
    ///     CopyDir/
    ///         SomeFolder/
    ///             SubFileC.pdf
    ///         FileA.txt
    ///         FileB.mp3
    /// ]]>
    /// </example>
    /// </remarks>
    /// <value>
    /// <see langword="true" /> if the children are to be mirrored; otherwise, <see langword="false" /> if the entire directory is to be mirrored.
    /// </value>
    public bool OnlyChildren { get; set; } = false;

}