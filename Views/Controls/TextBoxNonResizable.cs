#region Copyright (C) 2017-2022  Cody Bock
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.Windows;
using System.Windows.Controls;

#endregion

namespace DirLink.Views.Controls;

/// <summary>
/// Implementation of <see cref="TextBox"/> with <see cref="MeasureOverride(Size)"/> overwritten to disable dynamic width expansion.
/// </summary>
/// <seealso cref="TextBox" />
public class TextBoxNonResizable : TextBox {
    protected override Size MeasureOverride( Size Constraint ) => _Zero;

    static readonly Size _Zero = new Size(0, 0);
}