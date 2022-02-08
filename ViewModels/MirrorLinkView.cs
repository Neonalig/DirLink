#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;

using DirLink.Models;

#endregion

namespace DirLink.ViewModels;

public abstract class MirrorLinkView<T> : IMirrorLinkView, INotifyPropertyChanged where T : class, IMirrorLink {
    /// <inheritdoc />
    public abstract string Display { get; }

    /// <summary>
    /// Gets or sets the model.
    /// </summary>
    /// <value>
    /// The model.
    /// </value>
    protected T? Model { get; set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="MirrorLinkView{T}"/> class.
    /// </summary>
    /// <param name="Model">The model.</param>
    protected MirrorLinkView( T? Model = null ) => this.Model = Model;

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