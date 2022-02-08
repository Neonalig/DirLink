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

public class FileMirrorView : MirrorLinkView<FileMirror> {

    public FileMirrorView() : base(null) => this.RegisterPropertyChangedCallback(FileMirrorView_PropertyChanged);

    /// <summary>
    /// Raised when a property value is changed.
    /// </summary>
    /// <param name="E">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    void FileMirrorView_PropertyChanged( PropertyChangedEventArgs E ) {
        Debug.WriteLine("Prop changed.");
        // ReSharper disable once ConvertSwitchStatementToSwitchExpression
        switch ( E.PropertyName ) {
            case nameof(Path):
                if ( Path == null ) {
                    Model = null;
                } else {
                    if ( Model is null ) {
                        Model = new FileMirror(Path);
                    } else {
                        Model.File = Path;
                    }
                }
                break;
        }
    }

    string Int_Display { get; set; } = "Please enter a file path.";

    /// <inheritdoc />
    [DependsOn(nameof(Int_Display))]
    public override string Display => Int_Display;

    FileInfo? Int_Path { get; set; }

    /// <inheritdoc cref="FileMirror.File"/>
    [DependsOn(nameof(Int_Path))]
    public FileInfo? Path {
        get => Int_Path;
        set {
            Int_Path = value;
            Int_Display = Model.With(FM => $"File: {FM.File.Name}", "Please enter a file path.");
        }
    }

}