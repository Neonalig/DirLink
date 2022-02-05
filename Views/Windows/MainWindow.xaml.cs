using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

using DirLink.Views.Pages;

using JetBrains.Annotations;

using WPFUI.Common;
using WPFUI.Controls;

namespace DirLink;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : INotifyPropertyChanged {
    public MainWindow() {
        InitializeComponent();
        CurrentPage = new LinkerPage();
        DataContext = this;

        Bd.Background = Background;
        Background = new SolidColorBrush(new Color { R = 0, G = 0, B = 0, A = 0 });

        Nav.Items.Add(new MainWindowNavItem());
    }

    /// <summary>
    /// Gets or sets the size and position of the window.
    /// </summary>
    /// <value>
    /// The size and position of the window.
    /// </value>
    [PropertyChanged.DependsOn(nameof(Left), nameof(Top), nameof(Width), nameof(Height))]
    public Rect PositionAndSize {
        get => new Rect(Left, Top, Width, Height);
        set {
            Left = value.Left;
            Top = value.Top;
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    /// Gets or sets the <see cref="Border"/>'s clip path in the window.
    /// </summary>
    /// <value>
    /// The <see cref="Border"/>'s clip path.
    /// </value>
    [PropertyChanged.DependsOn(nameof(Width), nameof(Height))]
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

    void Nav_MouseDown( object Sender, System.Windows.Input.MouseButtonEventArgs E ) {
        NavigationFluent NF = (NavigationFluent)Sender;
        Debug.WriteLine($"Page is {NF.PageNow}");
    }
}

public class MainWindowNavigationFluent : NavigationFluent, IDisposable {
    /// <summary>
    /// Initialises a new instance of the <see cref="MainWindowNavigationFluent"/> class.
    /// </summary>
    public MainWindowNavigationFluent() {
        IDisposable Subscription = this.Observe(ItemsProperty).Subscribe(Args => {
            Debug.WriteLine($"Items were changed ({Args} // {Args.GetType().Name})");
        });
        Subscriptions.Add(Subscription);
        Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
    }

    /// <summary>
    /// Occurs when the <see cref="System.Windows.Threading.Dispatcher.ShutdownStarted"/> <see langword="event"/> is raised.
    /// </summary>
    /// <param name="Sender">The source of the <see langword="event"/>.</param>
    /// <param name="E">The raised <see langword="event"/> arguments.</param>
    void Dispatcher_ShutdownStarted( object? Sender, EventArgs E ) => Dispose();

    /// <summary>
    /// Gets the subscriptions.
    /// </summary>
    /// <value>
    /// The subscriptions.
    /// </value>
    IList<IDisposable> Subscriptions { get; } = new List<IDisposable>();
    
    /// <inheritdoc />
    ~MainWindowNavigationFluent() => Dispose(false);

    /// <inheritdoc />
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "It does.")]
    public void Dispose() => Dispose(true);

    bool _IsDisposed = false;

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Look at it! It does!")]
    void Dispose(bool IsDisposing ) {
        if ( _IsDisposed ) { return; }
        foreach ( IDisposable Subscription in Subscriptions ) {
            Subscription.Dispose();
        }
        _IsDisposed = true;
        if ( IsDisposing ) {
            GC.SuppressFinalize(this); //What the hell is this then? Huh!?
        }

    }
}

[ContentProperty(nameof(Content))]
public class MainWindowNavItem : NavItem {

    public MainWindowNavItem() => Click += MainWindowNavItem_Click;

    /// <summary>
    /// Occurs when the <see cref="NavItem.Click"/> <see langword="event"/> is raised.
    /// </summary>
    /// <param name="Sender">The source of the <see langword="event"/>.</param>
    /// <param name="E">The raised <see langword="event"/> arguments.</param>
    static void MainWindowNavItem_Click( object Sender, RoutedEventArgs E ) {
        MainWindowNavItem Self = (MainWindowNavItem)Sender;
    }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    public Page? Content { get; set; } = null;

}

public static class Extensions {

    /// <summary>
    /// Observes the specified dependency property for changes.
    /// </summary>
    /// <typeparam name="T">The calling type.</typeparam>
    /// <param name="Component">The component.</param>
    /// <param name="DependencyProperty">The dependency property.</param>
    /// <returns>The <see cref="IObservable{T}"/> instance.</returns>
    public static IObservable<EventArgs> Observe<T>( this T Component, DependencyProperty DependencyProperty )
        where T : DependencyObject {
        return Observable.Create<EventArgs>(Observer => {
            void Update( object? Sender, EventArgs Args ) => Observer.OnNext(Args);
            DependencyPropertyDescriptor? Property = DependencyPropertyDescriptor.FromProperty(DependencyProperty, typeof(T));
            Property.AddValueChanged(Component, Update);
            // ReSharper disable once HeapView.CanAvoidClosure
            return Disposable.Create(() => Property.RemoveValueChanged(Component, Update));
        });
    }
}