using System;
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

using WPFUI.Controls;
using WPFUI.Controls.Interfaces;

using DependsOnAttribute = PropertyChanged.DependsOnAttribute;
using Icon = WPFUI.Common.Icon;

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
    /// Gets or sets the currently displayed page.
    /// </summary>
    /// <value>
    /// The current page.
    /// </value>
    public Page CurrentPage { get; set; }

    /// <summary>
    /// Gets the navigation items for pre-existing linkers.
    /// </summary>
    /// <value>
    /// The pre-existing linker navigation items.
    /// </value>
    [SuppressMessage("Style", "IDE0002", Justification = "Simplified member access collision with non-static reference name.")]
    public ObservableCollection<INavigationItem> LinkerPageNavItems { get; } = new ObservableCollection<INavigationItem> {
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

public static class CtoHelper {

    /// <summary>
    /// Constructs a <see cref="INavigationItem"/> with the specified <paramref name="Title"/>, <paramref name="Icon"/> and <paramref name="PageType"/>.
    /// </summary>
    /// <param name="Title">The title for the navigation item.</param>
    /// <param name="Icon">The icon that appears next to the title.</param>
    /// <param name="PageType">The type of the page that the frame should be navigated to when the navigation item is clicked.</param>
    /// <returns>A new <see cref="NavigationItem"/> instance.</returns>
    [SuppressMessage("Style", "IDE0071:Simplify interpolation", Justification = "ToString() call prevents boxing.")]
    public static INavigationItem GetNavigationItem( string Title, Icon Icon, Type PageType ) => new NavigationItem {
        Content = Title,
        Tag = $"linker{Title.GetHashCode().ToString()}",
        Icon = Icon,
        Type = PageType
    };

    /// <inheritdoc cref="GetNavigationItem(string, Icon, Type)"/>
    /// <typeparam name="T">The type of the page that the frame should be navigated to when the navigation item is clicked.</typeparam>
    public static INavigationItem GetNavigationItem<T>( string Title, Icon Icon ) where T : Page => GetNavigationItem(Title, Icon, typeof(T)); 
}