using System.Globalization;
using System.IO;

using MVVMUtils;

namespace DirLink.Converters;

/// <summary>
/// Provides value conversions from <typeparamref name="T"/> to <see cref="string"/>.
/// </summary>
/// <typeparam name="T">The <see cref="FileSystemInfo"/> to convert from/to.</typeparam>
/// <seealso cref="ValueConverter{TFrom, TTo}"/>
public abstract class FileSystemInfoToStringConverterBase<T> : ValueConverter<T, string> where T : FileSystemInfo {
    /// <summary>
    /// Gets or sets a value indicating whether to return <see cref="FileSystemInfo.FullName"/> or <see cref="FileSystemInfo.Name"/>.
    /// </summary>
    /// <remarks>Does not affect reverse conversions (<see cref="string"/> -&gt; <see cref="FileSystemInfo"/>). Paths must be fully qualified for reverse conversions.</remarks>
    /// <value>
    /// <see langword="true" /> if <see cref="FileSystemInfo.FullName"/> is returned; otherwise, <see langword="false" /> if <see cref="FileSystemInfo.Name"/> is returned.
    /// </value>
    public bool UseFullName { get; set; }

    /// <inheritdoc />
    public override bool CanForwardWhenNull => true;

    /// <inheritdoc />
    public override bool CanReverse => true;

    /// <inheritdoc />
    public override bool CanReverseWhenNull => true;

    /// <inheritdoc />
    public override string Forward( T From, object? Parameter = null, CultureInfo? Culture = null ) => From.FullName;

    /// <inheritdoc />
    public override string ForwardWhenNull( object? Parameter = null, CultureInfo? Culture = null ) => string.Empty;

    // ReSharper disable MethodOverloadWithOptionalParameter //Required for inheritance
    /// <inheritdoc />
    public override T? Reverse( string To, object? Parameter = null, CultureInfo? Culture = null ) => Reverse(To);
    // ReSharper restore MethodOverloadWithOptionalParameter

    /// <inheritdoc />
    public override T? ReverseWhenNull( object? Parameter = null, CultureInfo? Culture = null ) => null;

    /// <summary>
    /// Converts the string into a <see cref="FileSystemInfo"/> instance, returning <see langword="null"/> if unsuccessful.
    /// </summary>
    /// <param name="To">The path to convert.</param>
    /// <returns>The converted instance, or <see langword="null"/>.</returns>
    public abstract T? Reverse( string To );
}