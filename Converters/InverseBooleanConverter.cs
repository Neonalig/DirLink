#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.Globalization;
using System.Windows.Data;

using MVVMUtils;

#endregion

namespace DirLink.Converters;

/// <summary>
/// Returns the inverse of the specified value.
/// </summary>
/// <seealso cref="ValueConverter{TFrom,TTo}"/>
[ValueConversion(typeof(bool?), typeof(bool?))]
[ValueConversion(typeof(bool), typeof(bool))]
public class InverseBooleanConverter : ValueConverter<bool?, bool?> {

    /// <inheritdoc />
    public override bool CanForwardWhenNull => true;

    /// <inheritdoc />
    public override bool CanReverse => true;

    /// <inheritdoc />
    public override bool CanReverseWhenNull => true;

    /// <inheritdoc />
    public override bool? Forward( bool? From, object? Parameter = null, CultureInfo? Culture = null ) => !From;

    /// <inheritdoc />
    public override bool? ForwardWhenNull( object? Parameter = null, CultureInfo? Culture = null ) => null;

    /// <inheritdoc />
    public override bool? Reverse( bool? To, object? Parameter = null, CultureInfo? Culture = null ) => !To;

    /// <inheritdoc />
    public override bool? ReverseWhenNull( object? Parameter = null, CultureInfo? Culture = null ) => null;

}
