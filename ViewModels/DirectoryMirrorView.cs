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

using PropertyChanged;

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

    string Int_Display { get; set; } = "Please enter a folder path.";

    /// <inheritdoc />
    [DependsOn(nameof(Int_Display))]
    public override string Display => Int_Display;

    DirectoryInfo? Int_Path { get; set; }

    /// <inheritdoc cref="DirectoryMirror.Folder"/>
    [DependsOn(nameof(Int_Path))]
    public DirectoryInfo? Path {
        get => Int_Path;
        set {
            Int_Path = value;
            Int_Display = Model.With(FM => $"Directory: {FM.Folder.Name}", "Please enter a folder path.");
        }
    }

    /// <inheritdoc cref="DirectoryMirror.OnlyChildren"/>
    public bool OnlyChildren { get; set; }

}