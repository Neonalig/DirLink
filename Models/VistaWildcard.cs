#region Copyright (C) 2017-2022  Cody Bock
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.Runtime.InteropServices;

#endregion

namespace DirLink.Models;

/// <summary>
/// Describes a grouped wildcard. (i.e. 'Audio Files (*.mp3;*.wav)|*.mp3;*.wav')
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct VistaWildcard {

    /// <summary>
    /// The display name. (i.e. 'Audio Files')
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The wildcard patterns. (i.e. '*.mp3', '*.wav', '*.*')
    /// </summary>
    public readonly string[] Patterns;

    /// <summary>
    /// Initialises a new instance of the <see cref="VistaWildcard"/> struct.
    /// </summary>
    /// <param name="Name">The display name. (i.e. 'Audio Files')</param>
    /// <param name="Patterns">The wildcard patterns. (i.e. '*.mp3', '*.wav', '*.*')</param>
    public VistaWildcard( string Name, params string[] Patterns ) {
        this.Name = Name;
        this.Patterns = Patterns;

        string PatternString = string.Join(';', Patterns);
        _Str = $"{Name} ({PatternString})|{PatternString}";
    }

    /// <summary>
    /// The (cached) <see cref="ToString()"/> return value.
    /// </summary>
    readonly string _Str;

    /// <inheritdoc />
    public override string ToString() => _Str;

    /// <summary> Any File (*.*)|*.* </summary>
    public static readonly VistaWildcard Any = new VistaWildcard("Any File", "*.*");

    /// <summary> Audio File (*.mp3;*.wav;*.ogg;*.flac)|*.mp3;*.wav;*.ogg;*.flac </summary>
    public static readonly VistaWildcard Audio = new VistaWildcard("Audio File", "*.mp3", "*.wav", "*.ogg", "*.flac");

    /// <summary> Video File (*.mp4;*.m4v;*.mkv;*.mpg;*.mpeg;*.wmv;*.avi)|*.mp4;*.m4v;*.mkv;*.mpg;*.mpeg;*.wmv;*.avi </summary>
    public static readonly VistaWildcard Video = new VistaWildcard("Video File", "*.mp4", "*.m4v", "*.mkv", "*.mpg", "*.mpeg", "*.wmv", "*.avi");

    /// <summary> Image File (*.png;*.jpg;*.jpeg;*.jfif;*.bmp;*.tiff)|*.png;*.jpg;*.jpeg;*.jfif;*.bmp;*.tiff </summary>
    public static readonly VistaWildcard Image = new VistaWildcard("Image File", "*.png", "*.jpg", "*.jpeg", "*.jfif", "*.bmp", "*.tiff");

    /// <summary> Text File (*.txt;*.text)|*.txt;*.text </summary>
    public static readonly VistaWildcard Text = new VistaWildcard("Text File", "*.txt", "*.text");

    /// <summary>
    /// Performs an explicit conversion from <see cref="VistaWildcard"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="Wld">The wildcard.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static explicit operator string( VistaWildcard Wld ) => Wld.ToString();

}