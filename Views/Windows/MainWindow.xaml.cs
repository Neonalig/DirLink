using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using DirLink.Views.Pages;

using JetBrains.Annotations;

using DependsOnAttribute = PropertyChanged.DependsOnAttribute;

namespace DirLink;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged {
    public MainWindow() {
        InitializeComponent();
        CurrentPage = new LinkerCreatorPage();
        DataContext = this;

        Bd.Background = Background;
        Background = new SolidColorBrush(new Color { R = 0, G = 0, B = 0, A = 0 });
    }

    /// <summary>
    /// Gets or sets the <see cref="Border"/>'s clip path in the window.
    /// </summary>
    /// <value>
    /// The <see cref="Border"/>'s clip path.
    /// </value>
    [DependsOn(nameof(Width), nameof(Height))]
    public Rect BorderClip {
        get => new Rect(0, 0, Width, Height);
        set {
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    /// Gets or sets the currently displayed page.
    /// </summary>
    /// <value>
    /// The current page.
    /// </value>
    public Page CurrentPage { get; set; }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Called when a property is changed.
    /// </summary>
    /// <param name="PropertyName">The name of the property.</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged( [CallerMemberName] string? PropertyName = null ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));

    void Nav_Navigated( object Sender, RoutedEventArgs E ) {
        Debug.WriteLine($"Navigated on {Sender} w/ args {E}.");
    }
}