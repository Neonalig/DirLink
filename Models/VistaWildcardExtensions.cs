#region Copyright (C) 2017-2022  Cody Bock
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

namespace DirLink.Models;

public static class VistaWildcardExtensions {

    /// <summary>
    /// Joins the item and array.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="A">The item.</param>
    /// <param name="B">The array.</param>
    /// <returns>The concatenation of <paramref name="A"/> and <paramref name="B"/>.</returns>
    internal static T[] Join<T>( T A, T[] B ) {
        T[] Res = new T[B.Length + 1];
        Res[0] = A;
        B.CopyTo(Res, 1);
        return Res;
    }

    /// <summary>
    /// Joins the array and item.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="A">The array.</param>
    /// <param name="B">The item.</param>
    /// <returns>The concatenation of <paramref name="A"/> and <paramref name="B"/>.</returns>
    internal static T[] Join<T>( T[] A, T B ) {
        T[] Res = new T[A.Length + 1];
        A.CopyTo(Res, 0);
        Res[^1] = B;
        return Res;
    }

    /// <summary>
    /// Joins the two arrays.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="A">The array.</param>
    /// <param name="B">The other array.</param>
    /// <returns>The concatenation of <paramref name="A"/> and <paramref name="B"/>.</returns>
    internal static T[] Join<T>( T[] A, T[] B ) {
        int L = A.Length;
        T[] Res = new T[L + B.Length];
        A.CopyTo(Res, 0);
        B.CopyTo(Res, L);
        return Res;
    }

    /// <summary>
    /// Constructs a new <see cref="VistaWildcardCollection"/> instance with the additional wildcard.
    /// </summary>
    /// <param name="A">The first wildcard.</param>
    /// <param name="B">The second wildcard.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection And(this VistaWildcard A, VistaWildcard B) => new VistaWildcardCollection(A, B);

    /// <summary>
    /// Constructs a new <see cref="VistaWildcardCollection"/> instance with the additional wildcard.
    /// </summary>
    /// <param name="A">The first wildcard.</param>
    /// <param name="B">The additional wildcards.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection And( this VistaWildcard A, params VistaWildcard[] B ) => new VistaWildcardCollection(Join(A, B));

    /// <summary>
    /// Constructs a new <see cref="VistaWildcardCollection"/> instance with the additional wildcard.
    /// </summary>
    /// <param name="A">The first wildcard.</param>
    /// <param name="B">The additional wildcard collection.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection And( this VistaWildcard A, VistaWildcardCollection B ) => new VistaWildcardCollection(Join(A, B.Wildcards));

    /// <summary>
    /// Constructs a new <see cref="VistaWildcardCollection"/> instance with the additional wildcard.
    /// </summary>
    /// <param name="A">The first wildcard.</param>
    /// <param name="B">The second wildcard.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection And( this VistaWildcardCollection A, VistaWildcard B ) => new VistaWildcardCollection(Join(A.Wildcards, B));

    /// <summary>
    /// Constructs a new <see cref="VistaWildcardCollection"/> instance with the additional wildcard.
    /// </summary>
    /// <param name="A">The first wildcard.</param>
    /// <param name="B">The additional wildcards.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection And( this VistaWildcardCollection A, params VistaWildcard[] B ) => new VistaWildcardCollection(Join(A.Wildcards, B));

    /// <summary>
    /// Constructs a new <see cref="VistaWildcardCollection"/> instance with the additional wildcard.
    /// </summary>
    /// <param name="A">The first wildcard.</param>
    /// <param name="B">The additional wildcard collection.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection And( this VistaWildcardCollection A, VistaWildcardCollection B ) => new VistaWildcardCollection(Join(A.Wildcards, B.Wildcards));

    /// <summary>
    /// Creates a new <see cref="VistaWildcardCollection"/> instance with the wildcard.
    /// </summary>
    /// <param name="Wildcard">The wildcard.</param>
    /// <param name="IncludeAllFiles">If <see langword="true" />, <see cref="VistaWildcard.Any"/> is also included; otherwise the collection will consist only of the specified wildcard.</param>
    /// <returns>A new <see cref="VistaWildcardCollection"/> instance.</returns>
    public static VistaWildcardCollection AsCollection( this VistaWildcard Wildcard, bool IncludeAllFiles = false ) => IncludeAllFiles
        ? new VistaWildcardCollection(Wildcard, VistaWildcard.Any)
        : new VistaWildcardCollection(Wildcard);

}