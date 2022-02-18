#region Copyright (C) 2017-2022  Cody Bock
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.IO;
using System.Windows.Data;

using MVVMUtils;

#endregion

namespace DirLink.Converters;

/// <summary>
/// Provides value conversions from <see cref="FileInfo"/> to <see cref="string"/>.
/// </summary>
/// <seealso cref="FileSystemInfoToStringConverterBase{T}"/>
/// <seealso cref="ValueConverter{TFrom,TTo}"/>
[ValueConversion(typeof(FileInfo), typeof(string))]
public sealed class FileInfoToStringConverter : FileSystemInfoToStringConverterBase<FileInfo> {
    /// <inheritdoc />
    public override FileInfo? Reverse( string To ) {
        try {
            return new FileInfo(To);
        } catch {
            return null;
        }
    }
}