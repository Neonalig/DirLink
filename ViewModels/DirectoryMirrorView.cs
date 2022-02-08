#region Copyright (C) 2017-2022  Starflash Studios
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License (Version 3.0)
// as published by the Free Software Foundation.
// 
// More information can be found here: https://www.gnu.org/licenses/gpl-3.0.en.html
#endregion

#region Using Directives

using System.ComponentModel;
using System.IO;

using DirLink.Models;

#endregion

namespace DirLink.ViewModels;

public class DirectoryMirrorView : MirrorLinkView<DirectoryMirror> {

    public DirectoryMirrorView() : base(null) => this.RegisterPropertyChangedCallback(DirectoryMirrorView_PropertyChanged);

    /// <summary>
    /// Raised when a property value is changed.
    /// </summary>
    /// <param name="E">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    void DirectoryMirrorView_PropertyChanged( PropertyChangedEventArgs E ) {
        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        switch ( E.PropertyName ) {
            case nameof(Path):
                if ( Path is null ) {
                    Model = null;
                } else {
                    if ( Model is null ) {
                        Model = new DirectoryMirror(Path) {
                            OnlyChildren = OnlyChildren
                        };
                    } else {
                        Model.Folder = Path;
                    }
                }
                break;
            case nameof(OnlyChildren) when Model is not null:
                Model.OnlyChildren = OnlyChildren;
                break;
        }
    }

    /// <inheritdoc />
    public override string Display => Model.With(FM => $"Directory: {FM.Folder.Name}", "Please enter a folder path.");

    /// <inheritdoc cref="DirectoryMirror.Folder"/>
    public DirectoryInfo? Path { get; set; }

    /// <inheritdoc cref="DirectoryMirror.OnlyChildren"/>
    public bool OnlyChildren { get; set; }

}