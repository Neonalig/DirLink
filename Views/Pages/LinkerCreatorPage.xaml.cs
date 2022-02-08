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
using System.Runtime.CompilerServices;
using System.Windows;

using DirLink.Views.Controls;

using MVVMUtils;

#endregion

namespace DirLink.Views.Pages;
/// <summary>
/// Interaction logic for LinkerPage.xaml
/// </summary>
public partial class LinkerCreatorPage : INotifyPropertyChanged {
    public LinkerCreatorPage() {
        InitializeComponent();
        DataContext = this;
    }

    /// <summary>
    /// Gets or sets the desired linker destination folder.
    /// </summary>
    /// <value>
    /// The desired linker destination folder.
    /// </value>
    public DirectoryInfo? Destination { get; set; }

    /// <summary>
    /// Gets the header for <see cref="DestCE"/>.
    /// </summary>
    /// <value>
    /// The header for <see cref="DestCE"/>.
    /// </value>
    public string? DestCEHeader { get; private set; } = "Pick a destination";

    /// <summary>
    /// Occurs when <see cref="FolderPicker.PathChanged"/> is raised.
    /// </summary>
    /// <param name="D">The <see cref="FolderPicker"/> dependency object.</param>
    /// <param name="E">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    /// <param name="OldValue">The old value.</param>
    /// <param name="NewValue">The new value.</param>
    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Member is on the event invocation list.")]
    //[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Member is an event handler")]
    void FolderPicker_PathChanged( FolderPicker D, DependencyPropertyChangedEventArgs E, DirectoryInfo OldValue, DirectoryInfo? NewValue ) => DestCEHeader = NewValue.With(Dir => $"Destination: {Dir.Name}", "Pick a destination");

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

    void FilePicker_PathChanged( FilePicker S, FileInfo NewValue ) {
        Debug.WriteLine($"PathChanged to {NewValue} on {S}/{S.GetParents().Log(5)}.");
    }
}