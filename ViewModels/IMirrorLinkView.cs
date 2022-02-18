#region Copyright (C) 2017-2022  Cody Bock
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

namespace DirLink.ViewModels;

public interface IMirrorLinkView {
    /// <summary>
    /// Gets the text to display.
    /// </summary>
    /// <value>
    /// The text to display.
    /// </value>
    string Display { get; }
}