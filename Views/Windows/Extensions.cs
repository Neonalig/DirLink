#region Copyright (C) 2017-2022  Starflash Studios

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html

#endregion

using System.Collections.Generic;

namespace DirLink;

/// <summary>
/// Extension methods and shorthand.
/// </summary>
public static class Extensions {

    /// <summary>
    /// Returns only elements of the specified type.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The desired output type.</typeparam>
    /// <param name="Enum">The enum.</param>
    /// <returns>The elements with the type <typeparamref name="TOut"/>.</returns>
    public static IEnumerable<TOut> Select<TIn, TOut>( this IEnumerable<TIn> Enum ) {
        foreach ( TIn Item in Enum ) {
            if ( Item is TOut Out ) {
                yield return Out;
            }
        }
    }
}