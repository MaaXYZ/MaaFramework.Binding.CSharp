using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.Interop;

/// <summary>
///     A class that implements the extension methods for converting between MaaDef types and C# types.
/// </summary>
public static class MaaDefConverter
{
    /// <summary>
    ///     Converts a MaaStringView (<see cref="MaaStringView"/>) to a <see cref="string"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToStringUTF8(this MaaStringView value)
        => Marshal.PtrToStringUTF8(value)
        ?? throw new ArgumentNullException(nameof(value));

    /// <summary>
    ///     Converts a MaaBool (<see cref="MaaBool"/>) to a <see cref="bool"/>.
    /// </summary>
    public static bool ToBoolean(this MaaBool value)
        => 0 != value;

    /// <summary>
    ///     Converts a <see cref="string"/> to a MaaOptionValue[] (<see cref="MaaOptionValue"/>[]).
    /// </summary>
    public static MaaOptionValue[] ToMaaOptionValues(this string value)
        => Encoding.UTF8.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="bool"/> to a MaaOptionValue[] (<see cref="MaaOptionValue"/>[]).
    /// </summary>
    public static MaaOptionValue[] ToMaaOptionValues(this bool value)
        => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="int"/> to a MaaOptionValue[] (<see cref="MaaOptionValue"/>[]).
    /// </summary>
    public static MaaOptionValue[] ToMaaOptionValues(this int value)
        => BitConverter.GetBytes(value);
}
