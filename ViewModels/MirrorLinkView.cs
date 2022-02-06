using System.ComponentModel;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace DirLink.Views.Pages;

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