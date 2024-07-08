using System.Runtime.InteropServices;
using System.Text;

namespace MaaFramework.Binding.Interop.Native;

/// <summary>
///     A class that implements the extension methods for converting between MaaDef types and C# types.
/// </summary>
public static class MaaDefConverter
{
    /// <summary>
    ///     Converts a MaaStringView (<see cref="MaaStringView"/>) to a <see cref="string"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToStringUTF8(this MaaStringView value, MaaSize size = MaaSize.MinValue)
        => size == MaaSize.MinValue
        ? Marshal.PtrToStringUTF8(value) ?? throw new ArgumentNullException(nameof(value))
        : Marshal.PtrToStringUTF8(value, (int)size);

    /// <summary>
    ///     Converts a MaaBool (<see cref="MaaBool"/>) to a <see cref="bool"/>.
    /// </summary>
    public static bool ToBoolean(this MaaBool value)
        => 0 != value;

    /// <summary>
    ///     Converts a <see cref="bool"/> to a MaaBool (<see cref="MaaBool"/>).
    /// </summary>
    public static MaaBool ToMaaBool(this bool value)
        => (MaaBool)(value ? 1 : 0);

    /// <summary>
    ///     Converts a <see cref="int"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ToMaaOptionValue(this int value)
        => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="bool"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ToMaaOptionValue(this bool value)
        => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="string"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ToMaaOptionValue(this string value)
        => Encoding.UTF8.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="string"/> to a <see cref="byte"/>[].
    /// </summary>
    /// <remarks>
    ///     Passes byte[] to avoid garbage collection.
    /// </remarks>
    public static byte[] ToBytes(this string value)
        => Encoding.UTF8.GetBytes(value);
}
