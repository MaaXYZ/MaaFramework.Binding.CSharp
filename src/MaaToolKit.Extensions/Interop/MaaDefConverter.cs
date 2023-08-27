using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MaaToolKit.Extensions.Interop;

/// <summary>
///     A class that implements the extension methods for converting between MaaDef types and C# types.
/// </summary>
public static class MaaDefConverter
{
    /// <summary>
    ///     Converts a MaaString (<see cref="MaaString"/>) to a <see cref="string"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToStringUTF8(this MaaString value)
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

    internal static byte[]? GetBytesFromFuncWithBuffer(this nint handle, Func<nint, nint, MaaSize, MaaSize> func, MaaSize bufferSize)
    {
        MaaSize size;
        var buffer = Marshal.AllocHGlobal((int)bufferSize);
        try
        {
            size = func(handle, buffer, bufferSize);
        }
        catch (SEHException seh)
        {
            Debug.WriteLine(seh);
            throw;
        }

        if (size == MaaDef.MaaNullSize)
        {
            Marshal.FreeHGlobal(buffer);
            return null;
        }

        var result = new byte[size];
        Marshal.Copy(buffer, result, 0, (int)size);
        Marshal.FreeHGlobal(buffer);
        return result;
    }

    internal static string? GetStringFromFuncWithBuffer(this nint handle, Func<nint, nint, MaaSize, MaaSize> func, MaaSize bufferSize)
    {
        var buffer = Marshal.AllocHGlobal((int)bufferSize);
        var size = func(handle, buffer, bufferSize);
        if (size == MaaDef.MaaNullSize)
        {
            Marshal.FreeHGlobal(buffer);
            return null;
        }

        var result = Marshal.PtrToStringUTF8(buffer, (int)size);
        Marshal.FreeHGlobal(buffer);
        return result;
    }
}
