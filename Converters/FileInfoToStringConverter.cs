using System.IO;
using System.Windows.Data;

using MVVMUtils;

namespace DirLink.Converters;

/// <summary>
/// Provides value conversions from <see cref="FileInfo"/> to <see cref="string"/>.
/// </summary>
/// <seealso cref="FileSystemInfoToStringConverterBase{T}"/>
/// <seealso cref="ValueConverter{TFrom,TTo}"/>
[ValueConversion(typeof(FileInfo), typeof(string))]
public sealed class FileInfoToStringConverter : FileSystemInfoToStringConverterBase<FileInfo> {
    /// <inheritdoc />
    public override FileInfo? Reverse( string To ) {
        try {
            return new FileInfo(To);
        } catch {
            return null;
        }
    }
}