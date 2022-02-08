#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.IO;

#endregion

namespace DirLink.Models;

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