using System.IO;
using System.Windows.Data;

using MVVMUtils;

namespace DirLink.Converters;

/// <summary>
/// Provides value conversions from <see cref="DirectoryInfo"/> to <see cref="string"/>.
/// </summary>
/// <seealso cref="FileSystemInfoToStringConverterBase{T}"/>
/// <seealso cref="ValueConverter{TFrom,TTo}"/>
[ValueConversion(typeof(DirectoryInfo), typeof(string))]
public sealed class DirectoryInfoToStringConverter : FileSystemInfoToStringConverterBase<DirectoryInfo> {
    /// <inheritdoc />
    public override DirectoryInfo? Reverse( string To ) {
        try {
            return new DirectoryInfo(To);
        } catch {
            return null;
        }
    }
}