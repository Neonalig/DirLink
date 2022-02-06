using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DirLink.Views.Controls;
/// <summary>
/// Interaction logic for FolderSystemInfoPicker.xaml
/// </summary>
public partial class FolderPicker {
    public FolderPicker() {
        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Attempts to get a <see cref="DirectoryInfo"/> instance pointing at the specified <paramref name="Path"/>, returning <see langword="true"/> if successful.
    /// </summary>
    /// <param name="Path">The path.</param>
    /// <param name="Folder">The folder.</param>
    /// <returns><see langword="true"/> if successful; otherwise <see langword="false"/>.</returns>
    static bool TryGetFolder( string Path, [NotNullWhen(true)] out DirectoryInfo? Folder ) {
        try {
            Folder = new DirectoryInfo(Path);
            return true;
        } catch {
            Folder = null;
            return false;
        }
    }

    /// <summary>
    /// Occurs when the <see cref="UserText"/> property is changed.
    /// </summary>
    public event CtoHelper.DependencyPropertyChangedCallback<FolderPicker, string>? UserTextChanged;

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
    public static readonly DependencyProperty UserTextProperty = CtoHelper.RegisterDependencyProperty<FolderPicker, string>(nameof(UserText), string.Empty, UserTextPropertyChanged);
    //DependencyProperty.Register(nameof(UserText), typeof(string), typeof(FolderPicker), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Whether to ignore the next PropertyChanged callback as it occurred from an internal value synchronisation.
    /// </summary>
    bool _SetValueInternal = false;

    /// <summary>
    /// Raised when the <see cref="UserTextProperty"/> value is changed.
    /// </summary>
    /// <param name="S">The <see cref="FolderPicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    static void UserTextPropertyChanged( FolderPicker S, DependencyPropertyChangedEventArgs E, string OldValue, string NewValue ) {
        if ( !S._SetValueInternal ) {
            S._SetValueInternal = true;
            S.Path = TryGetFolder(NewValue, out DirectoryInfo? DI).Return(DI);
            S._SetValueInternal = false;
        }
        S.OnUserTextChanged(S, E, OldValue, NewValue);
    }

    /// <summary>
    /// Called when the <see cref="UserText"/> property is changed.
    /// </summary>
    /// <param name="S">The <see cref="FolderPicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    protected virtual void OnUserTextChanged( FolderPicker S, DependencyPropertyChangedEventArgs E, string OldValue, string NewValue ) => UserTextChanged?.Invoke(S, E, OldValue, NewValue);

    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    /// <value>
    /// The path.
    /// </value>
    public DirectoryInfo? Path {
        get => (DirectoryInfo?)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    /// <summary>
    /// Occurs when the <see cref="Path"/> property is changed.
    /// </summary>
    public event CtoHelper.DependencyPropertyChangedCallback<FolderPicker, DirectoryInfo?>? PathChanged;

    /// <summary>
    /// Dependency property for <see cref="Path"/>.
    /// </summary>
    public static readonly DependencyProperty PathProperty = CtoHelper.RegisterDependencyProperty<FolderPicker, DirectoryInfo?>(nameof(Path), null, PathPropertyChanged);
    //DependencyProperty.Register(nameof(Path), typeof(DirectoryInfo), typeof(FolderPicker), new PropertyMetadata(null));

    /// <summary>
    /// Raised when the <see cref="PathProperty"/> value is changed.
    /// </summary>
    /// <param name="S">The <see cref="FolderPicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    static void PathPropertyChanged( FolderPicker S, DependencyPropertyChangedEventArgs E, DirectoryInfo? OldValue, DirectoryInfo? NewValue ) {
        if ( !S._SetValueInternal ) {
            S._SetValueInternal = true;
            S.UserText = NewValue?.FullName ?? string.Empty;
            S._SetValueInternal = false;
        }
        S.OnPathChanged(S, E, OldValue, NewValue);
    }

    /// <summary>
    /// Called when the <see cref="Path"/> property is changed.
    /// </summary>
    /// <param name="S">The <see cref="FolderPicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    protected virtual void OnPathChanged( FolderPicker S, DependencyPropertyChangedEventArgs E, DirectoryInfo? OldValue, DirectoryInfo? NewValue ) => PathChanged?.Invoke(S, E, OldValue, NewValue);

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
}
