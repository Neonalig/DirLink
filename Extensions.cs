#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

#endregion

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
    [ DebuggerHidden][ DebuggerStepThrough]
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
    [ DebuggerHidden][ DebuggerStepThrough]
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
    [ DebuggerHidden][ DebuggerStepThrough]
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

    /// <summary>
    /// Removes the keyboard focus from the given dependency object.
    /// </summary>
    /// <param name="Sender">The sender.</param>
    /// <param name="KillLogical">If <see langword="true" />, logical focus is revoked.</param>
    /// <param name="ClearKeyboard">If <see langword="true" />, keyboard focus is revoked.</param>
    public static void RemoveFocus( this DependencyObject Sender, bool KillLogical = true, bool ClearKeyboard = true ) {
        if ( KillLogical ) {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(Sender), null);
        }
        if ( ClearKeyboard ) {
            Keyboard.ClearFocus();
        }
    }

    /// <summary>
    /// Returns one of two options depending on whether the given value is <see langword="null"/> or not.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    /// <param name="Input">The input.</param>
    /// <param name="Default">The default method to invoke with <paramref name="Input"/> when <b>not</b> <see langword="null"/>.</param>
    /// <param name="Fallback">The fallback value to return when <paramref name="Input"/> is <see langword="null"/>.</param>
    /// <returns><paramref name="Default"/> when not <see langword="null"/>; otherwise <paramref name="Fallback"/>.</returns>
    public static TOut With<TIn, TOut>( this TIn? Input, Func<TIn, TOut> Default, TOut Fallback ) => Input is { } In ? Default(In) : Fallback;

    /// <summary>
    /// Returns one of two options depending on whether the given value is <see langword="null"/> or not.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    /// <param name="Input">The input.</param>
    /// <param name="Default">The default method to invoke with <paramref name="Input"/> when <b>not</b> <see langword="null"/>.</param>
    /// <param name="Fallback">The fallback value to return when <paramref name="Input"/> is <see langword="null"/>.</param>
    /// <returns><paramref name="Default"/> when not <see langword="null"/>; otherwise <paramref name="Fallback"/>.</returns>
    public static TOut With<TIn, TOut>( this TIn? Input, Func<TIn, TOut> Default, TOut Fallback ) where TIn : struct => Input is { } In ? Default(In) : Fallback;

    /// <summary>
    /// Attempts to grab the specified amount of items, returning early if the collection is too small.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="MaxCount">The maximum amount of items to return.</param>
    /// <returns>Up to 'n' amount of items from the given collections.</returns>
    public static IEnumerable<T> Grab<T>(this IEnumerable<T> Enum, int MaxCount ) {
        int I = 0;
        // ReSharper disable once LoopCanBePartlyConvertedToQuery
        foreach ( T Item in Enum ) {
            if (I >= MaxCount ) { yield break; }
            yield return Item;
            I++;
        }
    }

    /// <summary>
    /// Selects all items in the collection, converting them where necessary.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    /// <param name="Enum">The enum.</param>
    /// <param name="Conversion">The conversion.</param>
    /// <returns>The collection with the <paramref name="Conversion"/> applied to each item.</returns>
    public static IEnumerable<TOut> Select<TIn, TOut>(this IEnumerable<TIn> Enum, Func<TIn, TOut> Conversion ) {
        foreach ( TIn Item in Enum ) {
            yield return Conversion.Invoke(Item);
        }
    }

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="Count">The maximum amount of items to iterate. If the collection is smaller, then only those items will be logged.</param>
    /// <param name="Separator">The separator text between items.</param>
    /// <param name="ToString">The method to invoke to convert the object to a <see cref="string"/> representation.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, int Count, string Separator, Func<T, string> ToString ) => Log(Enum.Grab(Count), Separator, ToString);

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="Count">The maximum amount of items to iterate. If the collection is smaller, then only those items will be logged.</param>
    /// <param name="ToString">The method to invoke to convert the object to a <see cref="string"/> representation.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, int Count, Func<T, string> ToString ) => $"'{Log(Enum, Count, "', '", ToString)}'";

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="Separator">The separator text between items.</param>
    /// <param name="ToString">The method to invoke to convert the object to a <see cref="string"/> representation.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, string Separator, Func<T, string> ToString ) => string.Join(Separator, Enum.Select(ToString));

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="ToString">The method to invoke to convert the object to a <see cref="string"/> representation.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, Func<T, string> ToString ) => $"'{Log(Enum, "', '", ToString)}'";

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="Count">The maximum amount of items to iterate. If the collection is smaller, then only those items will be logged.</param>
    /// <param name="Separator">The separator text between items.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, int Count, string Separator ) => Log(Enum.Grab(Count), Separator);

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="Count">The maximum amount of items to iterate. If the collection is smaller, then only those items will be logged.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, int Count ) => $"'{Log(Enum, Count, "', '")}'";

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <param name="Separator">The separator text between items.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum, string Separator) => string.Join(Separator, Enum.Select(Item => Item!.ToString()));

    /// <summary>
    /// Logs at most the specified count of items from the collection.
    /// </summary>
    /// <typeparam name="T">The collection containing type.</typeparam>
    /// <param name="Enum">The collection to iterate.</param>
    /// <returns>A <see cref="string"/> representation of the collection.</returns>
    public static string Log<T>( [ItemNotNull] this IEnumerable<T> Enum) => $"'{Log(Enum, "', '")}'";

    /// <inheritdoc cref="string.TrimEnd(char)"/>
    /// <param name="String">The string to trim.</param>
    /// <param name="Trim">The text to remove from the end of the string.</param>
    public static string TrimEnd( this string String, string Trim ) {
        int L = Trim.Length;
        while ( String.EndsWith(Trim) ) {
            String = String[..^L];
        }
        return String;
    }

    /// <summary>
    /// Performs the modification on all items in the collection, returning a new collection of equal length.
    /// </summary>
    /// <typeparam name="TIn">The type of the input.</typeparam>
    /// <typeparam name="TOut">The type of the output.</typeparam>
    /// <param name="List">The input array.</param>
    /// <param name="Func">The function.</param>
    /// <returns>The modified values.</returns>
    public static TOut[] Select<TIn, TOut>( this IList<TIn> List, Func<TIn, TOut> Func ) {
        int L = List.Count;
        TOut[] Result = new TOut[L];
        for ( int I = 0; I < L; I++ ) {
            Result[I] = Func(List[I]);
        }
        return Result;
    }

}