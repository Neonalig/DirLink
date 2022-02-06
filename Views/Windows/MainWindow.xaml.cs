using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using DirLink.Views.Pages;

using JetBrains.Annotations;

using WPFUI.Controls.Interfaces;

using DependsOnAttribute = PropertyChanged.DependsOnAttribute;

namespace DirLink;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged {
    public MainWindow() {
        InitializeComponent();
        DataContext = this;

        Bd.Background = Background;
        Background = new SolidColorBrush(new Color { R = 0, G = 0, B = 0, A = 0 });

        //Nav.Items = LinkerPageNavItems;
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
    /// Gets the navigation items for pre-existing linkers.
    /// </summary>
    /// <value>
    /// The pre-existing linker navigation items.
    /// </value>
    [SuppressMessage("Style", "IDE0002", Justification = "Simplified member access collision with non-static reference name.")]
    public ObservableCollection<INavigationItem> LinkerPageNavItems { get; } = new ObservableCollection<INavigationItem> {
        CtoHelper.GetNavigationItem<LinkerCreatorPage>("Create New Linker", WPFUI.Common.Icon.New24).Modify(NI => NI.Margin = new Thickness(0, 0, 0, 2)),
        CtoHelper.GetNavigationItem<LinkerCreatorPage>("Games", WPFUI.Common.Icon.Games48)
    };

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