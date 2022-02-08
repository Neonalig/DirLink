#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using DirLink.Views.Pages;

using WPFUI.Controls.Interfaces;

#endregion

namespace DirLink.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged {
    public MainWindow() {
        InitializeComponent();
        DataContext = this;

        Bd.Background = Background;
        Background = new SolidColorBrush(new Color { R = 0, G = 0, B = 0, A = 0 });

        SizeChanged += MainWindow_SizeChanged;
        RemakeBorderClip();
    }

    /// <summary>
    /// Occurs when the <see cref="FrameworkElement.SizeChanged"/> <see langword="event"/> is raised.
    /// </summary>
    /// <param name="Sender">The source of the <see langword="event"/>.</param>
    /// <param name="E">The raised <see langword="event"/> arguments.</param>
    void MainWindow_SizeChanged( object Sender, SizeChangedEventArgs E ) {
        if (E.WidthChanged || E.HeightChanged) {
            RemakeBorderClip();
        }
    }

    /// <summary>
    /// Remakes the <see cref="BorderClip"/> property with the new window size.
    /// </summary>
    void RemakeBorderClip() => BorderClip = new Rect(0, 0, Width, Height);

    /// <summary>
    /// Gets or sets the <see cref="Border"/>'s clip path in the window.
    /// </summary>
    /// <value>
    /// The <see cref="Border"/>'s clip path.
    /// </value>
    public Rect BorderClip { get; set; }

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