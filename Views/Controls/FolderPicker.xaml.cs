using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using PropertyChanged;

namespace DirLink.Views.Controls;
/// <summary>
/// Interaction logic for FolderSystemInfoPicker.xaml
/// </summary>
public partial class FolderPicker : INotifyPropertyChanged {
    public FolderPicker() {
        InitializeComponent();
        DataContext = this;
        PropertyChanged += ( S, E ) => {
            switch ( E.PropertyName ) {
                case nameof(UserText):
                    FolderPicker FP = S.AsNotNull<FolderPicker>();
                    FP.Pth = TryGetFolder(FP.UserText, out DirectoryInfo? Dl).Return(Dl);
                    break;
            }
        };
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
    /// Gets or sets the user text.
    /// </summary>
    /// <value>
    /// The user text.
    /// </value>
    public string UserText { get; set; } = string.Empty;

    DirectoryInfo? Pth { get; set; }

    /// <summary>
    /// Gets or sets the user-selected path.
    /// </summary>
    /// <value>
    /// The path.
    /// </value>
    [DependsOn(nameof(Pth))]
    public DirectoryInfo? Path {
        get => Pth;
        set {
            Pth = value;
            UserText = value?.FullName ?? string.Empty;
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
}
