#region Copyright (C) 2017-2022  Starflash Studios

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

    /// <summary>
    /// Throws an <see cref="Exception"/> if true.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Exception"/> to throw.</typeparam>
    /// <param name="Throw">If <see langword="true" />, the exception is thrown; otherwise code execution continues as expected.</param>
    /// <param name="Exc">The exception to throw if <see langword="true"/>.</param>
    static void ThrowIfTrue<T>( [DoesNotReturnIf(true)] bool Throw, Func<T> Exc ) where T : Exception {
        if ( Throw ) {
            throw Exc();
        }
    }


    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified <paramref name="Item"/> is <see langword="null"/>, and returns it if not.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="Item">The item.</param>
    /// <param name="ItemName">The name of the item.</param>
    /// <exception cref="ArgumentNullException">The specified argument was null.</exception>
    [DebuggerHidden, DebuggerStepThrough]
    public static void ThrowIfNull<T>( [NotNull] this T? Item, [CallerArgumentExpression("Item")] string? ItemName = null ) {
        if ( Item is null ) {
            ThrowIfTrue(true, () => new ArgumentNullException(ItemName, ItemName is { } IN ? $"The specified argument ({IN}) was null." : "The specified argument was null."));
        }
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified <paramref name="Item"/> is <see langword="null"/>, and returns it if not.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    /// <param name="Item">The item.</param>
    /// <param name="ItemName">The name of the item.</param>
    /// <returns>The same <paramref name="Item"/> if not <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">The specified argument was null.</exception>
    [DebuggerHidden, DebuggerStepThrough]
    [return: NotNull]
    public static T CatchNull<T>( [NotNull] this T? Item, [CallerArgumentExpression("Item")] string? ItemName = null ) {
        if ( Item is null ) {
            throw new ArgumentNullException(ItemName, ItemName is { } IN ? $"The specified argument ({IN}) was null." : "The specified argument was null.");
        }
        return Item;
    }
    
    /// <summary>
    /// Casts the specified input to the desired output type.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type to output.</typeparam>
    /// <param name="Item">The item.</param>
    /// <returns>The cast item, or <see langword="null"/> if <paramref name="Item"/> is also <see langword="null"/>.</returns>
    [return: NotNullIfNotNull("Item")]
    public static TOut? As<TIn, TOut>( this TIn? Item ) => Item is not null && Item is TOut Out ? Out : default;
    //public static TOut? As<TIn, TOut>( this TIn? Item ) where TOut : class => Item is null ? null : Item as TOut;

    /// <summary>
    /// Casts the specified input to the desired output type.
    /// </summary>
    /// <typeparam name="TOut">The type to output.</typeparam>
    /// <param name="Item">The item.</param>
    /// <returns>The cast item, or <see langword="null"/> if unable to be cast, or <paramref name="Item"/> is also <see langword="null"/>.</returns>
    [return: NotNullIfNotNull("Item")]
    public static TOut? As<TOut>( this object? Item ) => Item is TOut Out ? Out : default;

    /// <summary>
    /// Casts the specified input to the desired output type.
    /// </summary>
    /// <typeparam name="TOut">The type to output.</typeparam>
    /// <param name="Item">The item.</param>
    /// <param name="ItemName">The name of the item.</param>
    /// <returns>The cast item.</returns>
    /// <exception cref="ArgumentNullException">The specified argument was null, or could not be cast.</exception>
    [DebuggerHidden, DebuggerStepThrough]
    [return: NotNull]
    public static TOut AsNotNull<TOut>( [NotNull] this object? Item, [CallerArgumentExpression("Item")] string? ItemName = null ) {
        if ( Item is not TOut Out ) {
            throw new ArgumentNullException(ItemName, ItemName is { } IN ? $"The specified argument ({IN}) was null, or could not be cast to type {typeof(TOut)}." : $"The specified argument was null, or could not be cast to type {typeof(TOut)}.");
        }
        return Out;
    }

    /// <summary>
    /// Returns the attempt value if <see langword="false"/>; otherwise <see langword="null"/>/<see langword="default"/> if not.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="Attempt">If <see langword="true" />, returns <paramref name="Val"/>; otherwise <see langword="null"/>/<see langword="default"/> is returned instead.</param>
    /// <param name="Val">The value.</param>
    /// <returns><paramref name="Val"/> if <see langword="true"/>; otherwise <see langword="null"/>/<see langword="default"/> if not.</returns>
    [return: NotNullIfNotNull("Val")]
    public static T? Return<T>( this bool Attempt, T? Val ) => Attempt ? Val : default;
}