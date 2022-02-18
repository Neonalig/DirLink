#region Copyright (C) 2017-2022  Cody Bock
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.Collections;
using System.Runtime.InteropServices;

#endregion

namespace DirLink.Models;

/// <summary>
/// A collection of <see cref="VistaWildcard"/>s.
/// </summary>
/// <seealso cref="IReadOnlyList{T}"/>
/// <seealso cref="VistaWildcard"/>
[StructLayout(LayoutKind.Sequential)]
public readonly struct VistaWildcardCollection : IReadOnlyList<VistaWildcard> {

    /// <summary>
    /// The wildcards to use.
    /// </summary>
    public readonly VistaWildcard[] Wildcards;

    /// <inheritdoc />
    public IEnumerator<VistaWildcard> GetEnumerator() {
        foreach ( VistaWildcard Wildcard in Wildcards ) {
            yield return Wildcard;
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public int Count => Wildcards.Length;


    /// <inheritdoc />
    public VistaWildcard this[ int Index ] => Wildcards[Index];

    /// <summary>
    /// Initialises a new instance of the <see cref="VistaWildcardCollection"/> struct.
    /// </summary>
    /// <param name="Wildcards">The wildcards to use.</param>
    public VistaWildcardCollection( params VistaWildcard[] Wildcards ) {
        this.Wildcards = Wildcards;
        _Str = string.Join('|', Wildcards);
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="VistaWildcardCollection"/> struct.
    /// </summary>
    /// <param name="Wildcards">The wildcards to construct and use.</param>
    public VistaWildcardCollection( params (string Name, string[] Patterns)[] Wildcards ) : this(Wildcards.Select(Tuple => new VistaWildcard(Tuple.Name, Tuple.Patterns))) { }

    /// <summary>
    /// The (cached) <see cref="ToString()"/> return value.
    /// </summary>
    readonly string _Str;

    /// <inheritdoc />
    public override string ToString() => _Str;

    /// <summary> Any File (*.*)|*.* </summary>
    public static readonly VistaWildcardCollection Any = new VistaWildcardCollection(VistaWildcard.Any);

    /// <summary>
    /// Performs an explicit conversion from <see cref="VistaWildcardCollection"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="Coll">The collection.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator string( VistaWildcardCollection Coll ) => Coll.ToString();

}