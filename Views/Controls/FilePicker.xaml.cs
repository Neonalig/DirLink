#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
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