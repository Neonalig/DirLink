#region Copyright (C) 2017-2022  Starflash Studios

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html

#endregion

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using WPFUI.Controls;
using WPFUI.Controls.Interfaces;

using Icon = WPFUI.Common.Icon;

namespace DirLink;

public static class CtoHelper {

    /// <summary>
    /// Constructs a <see cref="INavigationItem"/> with the specified <paramref name="Title"/>, <paramref name="Icon"/> and <paramref name="PageType"/>.
    /// </summary>
    /// <param name="Title">The title for the navigation item.</param>
    /// <param name="Icon">The icon that appears next to the title.</param>
    /// <param name="PageType">The type of the page that the frame should be navigated to when the navigation item is clicked.</param>
    /// <returns>A new <see cref="NavigationItem"/> instance.</returns>
    [SuppressMessage("Style", "IDE0071:Simplify interpolation", Justification = "ToString() call prevents boxing.")]
    public static NavigationItem GetNavigationItem( string Title, Icon Icon, Type PageType ) => new NavigationItem {
        Content = Title,
        Tag = $"linker{Title.GetHashCode().ToString()}",
        Icon = Icon,
        Type = PageType
    };

    /// <inheritdoc cref="GetNavigationItem(string, Icon, Type)"/>
    /// <typeparam name="T">The type of the page that the frame should be navigated to when the navigation item is clicked.</typeparam>
    public static NavigationItem GetNavigationItem<T>( string Title, Icon Icon ) where T : Page => GetNavigationItem(Title, Icon, typeof(T));

    /// <summary>
    /// Invokes the modification method with the specified <paramref name="Item"/>, then returns the modified <paramref name="Item"/>.
    /// </summary>
    /// <typeparam name="T">The type of item to modify.</typeparam>
    /// <param name="Item">The item to modify.</param>
    /// <param name="Modification">The modification to apply to the item.</param>
    /// <returns>The <paramref name="Item"/>.</returns>
    public static T Modify<T>(this T Item, Action<T> Modification) {
        Modification(Item);
        return Item;
    }

    /// <inheritdoc cref="Modify{T}(T, Action{T})"/>
    /// <typeparam name="TIn">The type of item to modify.</typeparam>
    /// <typeparam name="TOut">The new type after the modification.</typeparam>
    public static TOut ChainModify<TIn, TOut>( this TIn Item, Func<TIn, TOut> Modification ) => Modification(Item);

    /// <summary>
    /// Registers the dependency property on the object.
    /// </summary>
    /// <typeparam name="TBase">The type of the base object.</typeparam>
    /// <typeparam name="TVal">The type of the dependency property value.</typeparam>
    /// <param name="ValueName">The name of the value.</param>
    /// <param name="DefaultValue">The default value.</param>
    /// <param name="Callback">The callback.</param>
    /// <returns>A new <see cref="DependencyProperty"/>.</returns>
    public static DependencyProperty RegisterDependencyProperty<TBase, TVal>( string ValueName, TVal DefaultValue, DependencyPropertyChangedCallback<TBase, TVal>? Callback = null ) where TBase : DependencyObject =>
        DependencyProperty.Register(
            ValueName.CatchNull(),
            typeof(TVal),
            typeof(TBase),
            Callback switch { { } CB => new PropertyMetadata(DefaultValue, CB.GetPropertyChangedCallback()),
                _                    => new PropertyMetadata(DefaultValue)
            });

    /// <summary>
    /// Registers the dependency property on the object.
    /// </summary>
    /// <typeparam name="TBase">The type of the base object.</typeparam>
    /// <typeparam name="TVal">The type of the dependency property value.</typeparam>
    /// <param name="Base">The base object.</param>
    /// <param name="ValueReference">The value reference.</param>
    /// <param name="DefaultValue">The default value.</param>
    /// <param name="ValueName">The name of the value.</param>
    /// <param name="Callback">The callback.</param>
    /// <returns>A new <see cref="DependencyProperty"/>.</returns>
    public static DependencyProperty RegisterDependencyProperty<TBase, TVal>( this TBase Base, TVal ValueReference, TVal DefaultValue, DependencyPropertyChangedCallback<TBase, TVal>? Callback = null, [CallerArgumentExpression("ValueReference")] string? ValueName = null ) where TBase : DependencyObject => RegisterDependencyProperty(ValueName.CatchNull(), DefaultValue, Callback);

    /// <summary>
    /// Gets a <see cref="PropertyChangedCallback"/> with a known value type.
    /// </summary>
    /// <typeparam name="TBase">The base <see cref="DependencyObject"/> type.</typeparam>
    /// <typeparam name="TVal">The value type.</typeparam>
    /// <param name="TypedCallback">The typed callback.</param>
    /// <returns>A new <see cref="PropertyChangedCallback"/> instance.</returns>
    public static PropertyChangedCallback GetPropertyChangedCallback<TBase, TVal>( this DependencyPropertyChangedCallback<TBase, TVal> TypedCallback ) where TBase : DependencyObject {
        void PropertyChangedCallback( DependencyObject D, DependencyPropertyChangedEventArgs E ) => TypedCallback((TBase)D, E, (TVal)E.OldValue, (TVal)E.NewValue);
        return PropertyChangedCallback;
    }

    /// <summary>
    /// <see cref="PropertyChangedCallback"/> with a known value type.
    /// </summary>
    /// <typeparam name="TBase">The base <see cref="DependencyObject"/> type.</typeparam>
    /// <typeparam name="TVal">The value type.</typeparam>
    /// <param name="D">The dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    public delegate void DependencyPropertyChangedCallback<in TBase, in TVal>( TBase D, DependencyPropertyChangedEventArgs E, TVal OldValue, TVal NewValue );

    /// <summary>
    /// Registers a <see cref="INotifyPropertyChanged.PropertyChanged"/> listener with a known type.
    /// </summary>
    /// <typeparam name="T">The base type.</typeparam>
    /// <param name="Base">The base.</param>
    /// <param name="TypedCallback">The typed callback.</param>
    public static void RegisterPropertyChangedCallback<T>( this T Base, PropertyChanged<T> TypedCallback ) where T : INotifyPropertyChanged {
        void PropertyChanged( object? S, PropertyChangedEventArgs E ) => TypedCallback(S.AsNotNull<T>(), E);
        Base.PropertyChanged += PropertyChanged;
    }

    /// <summary>
    /// <see cref="PropertyChanged"/> event listener with a known value type.
    /// </summary>
    /// <typeparam name="T">The type of the sender.</typeparam>
    /// <param name="Sender">The sender.</param>
    /// <param name="E">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    public delegate void PropertyChanged<in T>( T Sender, PropertyChangedEventArgs E ) where T : INotifyPropertyChanged;

    /// <summary>
    /// Registers a <see cref="INotifyPropertyChanged.PropertyChanged"/> listener with a known type.
    /// </summary>
    /// <typeparam name="T">The base type.</typeparam>
    /// <param name="Base">The base.</param>
    /// <param name="Callback">The callback.</param>
    public static void RegisterPropertyChangedCallback<T>( this T Base, SimplePropertyChanged Callback ) where T : INotifyPropertyChanged {
        void PropertyChanged( object? S, PropertyChangedEventArgs E ) => Callback(E);
        Base.PropertyChanged += PropertyChanged;
    }

    /// <summary>
    /// <see cref="PropertyChanged"/> event listener with a known value type.
    /// </summary>
    /// <param name="E">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    public delegate void SimplePropertyChanged( PropertyChangedEventArgs E );
}