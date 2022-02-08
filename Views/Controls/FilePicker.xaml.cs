#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Ookii.Dialogs.Wpf;

using PropertyChanged;

#endregion

namespace DirLink.Views.Controls;

/// <summary>
/// Interaction logic for FileSystemInfoPicker.xaml
/// </summary>
public partial class FilePicker : INotifyPropertyChanged {
    public FilePicker() {
        InitializeComponent();
        DataContext = this;
        this.RegisterPropertyChangedCallback(( S, E ) => {
            switch ( E.PropertyName ) {
                case nameof(Path):
                    S.OnPathChanged(S, S.Path);
                    break;
            }
        });
    }

    /// <summary>
    /// Attempts to get a <see cref="FileInfo"/> instance pointing at the specified <paramref name="Path"/>, returning <see langword="true"/> if successful.
    /// </summary>
    /// <param name="Path">The path.</param>
    /// <param name="File">The f.</param>
    /// <returns><see langword="true"/> if successful; otherwise <see langword="false"/>.</returns>
    static bool TryGetFile( string Path, [NotNullWhen(true)] out FileInfo? File ) {
        try {
            File = new FileInfo(Path);
            return true;
        } catch {
            File = null;
            return false;
        }
    }

    /// <summary>
    /// Occurs when the <see cref="UserText"/> property is changed.
    /// </summary>
    public event CtoHelper.DependencyPropertyChangedCallback<FilePicker, string>? UserTextChanged;

    /// <summary>
    /// Gets or sets the user text.
    /// </summary>
    /// <value>
    /// The user text.
    /// </value>
    public string UserText {
        get => (string)GetValue(UserTextProperty);
        set => SetValue(UserTextProperty, value);
    }

    /// <summary>
    /// Dependency property for <see cref="UserText"/>.
    /// </summary>
    public static readonly DependencyProperty UserTextProperty = CtoHelper.RegisterDependencyProperty<FilePicker, string>(nameof(UserText), string.Empty, UserTextPropertyChanged);
    //DependencyProperty.Register(nameof(UserText), typeof(string), typeof(FilePicker), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Raised when the <see cref="UserTextProperty"/> value is changed.
    /// </summary>
    /// <param name="S">The <see cref="FilePicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    static void UserTextPropertyChanged( FilePicker S, DependencyPropertyChangedEventArgs E, string OldValue, string NewValue ) {
        Debug.WriteLine("UserText was changed.");
        S.Path = TryGetFile(NewValue, out FileInfo? DI).Return(DI);
        S.OnUserTextChanged(S, E, OldValue, NewValue);
    }

    /// <summary>
    /// Called when the <see cref="UserText"/> property is changed.
    /// </summary>
    /// <param name="S">The <see cref="FilePicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    [SuppressPropertyChangedWarnings] protected virtual void OnUserTextChanged( FilePicker S, DependencyPropertyChangedEventArgs E, string OldValue, string NewValue ) => UserTextChanged?.Invoke(S, E, OldValue, NewValue);

    public delegate void PathChangedEvent( FilePicker S, FileInfo? NewValue );
    public event PathChangedEvent? PathChanged;

    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    /// <value>
    /// The path.
    /// </value>
    public FileInfo? Path { get; private set; }

    /// <summary>
    /// Called when the <see cref="Path"/> property is changed.
    /// </summary>
    /// <param name="S">The <see cref="FilePicker"/> dependency object.</param>
    /// <param name="NewValue">The new value.</param>
    [SuppressPropertyChangedWarnings] protected virtual void OnPathChanged( FilePicker S, FileInfo? NewValue ) => PathChanged?.Invoke(S, NewValue);

    /// <summary>
    /// Occurs when the <see cref="UIElement.KeyDown"/> <see langword="event"/> is raised.
    /// </summary>
    /// <param name="Sender">The source of the <see langword="event"/>.</param>
    /// <param name="E">The raised <see langword="event"/> arguments.</param>
    void TextBox_KeyDown( object Sender, KeyEventArgs E ) {
        TextBox Snd = (TextBox)Sender;
        switch ( E.Key ) {
            //Remove keyboard focus if enter is pressed and the textbox didn't already consume the event.
            case Key.Enter:
                Snd.RemoveFocus();
                E.Handled = true;
                break;
        }
    }

    /// <summary>
    /// Occurs when the <see cref="UIElement.PreviewKeyDown"/> <see langword="event"/> is raised.
    /// </summary>
    /// <param name="Sender">The source of the <see langword="event"/>.</param>
    /// <param name="E">The raised <see langword="event"/> arguments.</param>
    void TextBox_PreviewKeyDown( object Sender, KeyEventArgs E ) {
        TextBox Snd = (TextBox)Sender;
        switch ( E.Key ) {
            //Remove keyboard focus if backspace is pressed when the textfield is empty.
            case Key.Back when string.IsNullOrEmpty(Snd.Text):
                Snd.RemoveFocus();
                E.Handled = true;
                break;
        }

    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Called when a property value is changed.
    /// </summary>
    /// <param name="PropertyName">The name of the property.</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged( [CallerMemberName] string? PropertyName = null ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));

    /// <summary>
    /// Occurs when the <see cref="Button.Click"/> <see langword="event"/> is raised.
    /// <br/> Opens a file browser dialog, allowing the user to quickly select a file via the windows default file picker UI.
    /// </summary>
    /// <param name="Sender">The source of the <see langword="event"/>.</param>
    /// <param name="E">The raised <see langword="event"/> arguments.</param>
    void Button_Click( object Sender, RoutedEventArgs E ) {
        if ( BrowseDialogOpen ) { return; }
        BrowseDialogOpen = true;

        VistaOpenFileDialog Diag = new VistaOpenFileDialog {
            Title = "Pick a file...",
            CheckFileExists = true,
            CheckPathExists = true,
            ValidateNames = true,
            ShowReadOnly = AllowReadOnly,
            RestoreDirectory = true,
            InitialDirectory = (InitialDirectory ?? Desktop).FullName,
            FileName = UserText,
            Filter = Wildcards.ToString()
        };

        if ( Diag.ShowDialog() == true ) {
            UserText = Diag.FileName;
        }

        BrowseDialogOpen = false;
    }

    /// <summary>
    /// Gets a value indicating whether the file browser dialog is currently open. If <see langword="true"/>, user interaction with other components is temporarily disabled.
    /// </summary>
    /// <value>
    ///  see langword="true" /> if a <see cref="VistaOpenFileDialog"/> is currently shown; otherwise, <see langword="false" />.
    /// </value>
    public bool BrowseDialogOpen { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow read-only files to be selected.
    /// </summary>
    /// <value>
    /// <see langword="true" /> if read-only files are allowed; otherwise, <see langword="false" />.
    /// </value>
    public bool AllowReadOnly { get; set; } = false;

    /// <summary>
    /// Gets or sets the wildcards.
    /// </summary>
    /// <value>
    /// The wildcards.
    /// </value>
    public VistaWildcardCollection Wildcards { get; set; } = VistaWildcardCollection.Any;

    /// <summary>
    /// Gets or sets the index of the initial <see cref="VistaWildcard"/> to default to from the <see cref="Wildcards"/> collection.
    /// </summary>
    /// <value>
    /// The initial wildcard index.
    /// </value>
    public int InitialWildcard { get; set; } = 0;

    /// <summary>
    /// Gets or sets the initial directory when restoration is unavailable (the dialog is opened for the first time).
    /// </summary>
    /// <value>
    /// The initial directory, or <see langword="null"/> for the user's desktop directory.
    /// </value>
    public DirectoryInfo? InitialDirectory { get; set; } = null;

    /// <summary> The desktop directory. </summary>
    public static readonly DirectoryInfo Desktop = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
}

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