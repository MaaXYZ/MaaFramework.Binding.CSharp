using MaaFramework.Binding.Custom;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace MaaFramework.Binding.Interop.Native;

/// <summary>
///     A class that implements the extension methods for converting between MaaDef types and C# types.
/// </summary>
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(MaaStringViewMarshaller))]
[CustomMarshaller(typeof(IMaaCustomController), MarshalMode.ManagedToUnmanagedIn, typeof(MaaCustomControllerMarshaller.ManagedToUnmanagedIn))]
public static class MaaMarshaller
{
    /// <summary>
    ///     Converts a MaaStringView (<see cref="nint"/>) to a <see cref="string"/>.
    /// </summary>
    /// <exception cref="MaaInteroperationException"/>
    public static string ConvertToString(nint value)
        => Marshal.PtrToStringUTF8(value).ThrowIfNull();

    /// <inheritdoc cref="ConvertToString(nint)"/>
    public static string ConvertToString(nint value, int size)
        => Marshal.PtrToStringUTF8(value, size).ThrowIfNull();

    /// <summary>
    ///     Converts a <see cref="int"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ConvertToMaaOptionValue(int value)
        => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="nuint"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ConvertToMaaOptionValue(nuint value)
        => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="bool"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ConvertToMaaOptionValue(bool value)
        => BitConverter.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="string"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ConvertToMaaOptionValue(string value)
        => Encoding.UTF8.GetBytes(value);

    /// <summary>
    ///     Converts a <see cref="IEnumerable{T}"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ConvertToMaaOptionValue(IEnumerable<int> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value is int[] array)
        {
            var result = new byte[array.Length * 4];
            Span<byte> byteSpan = result;
            MemoryMarshal.Cast<int, byte>(array).CopyTo(byteSpan);
            return result;
        }

        if (value is List<int> list)
        {
            var result = new byte[list.Count * 4];
            Span<byte> byteSpan = result;
            MemoryMarshal.Cast<int, byte>(CollectionsMarshal.AsSpan(list)).CopyTo(byteSpan);
            return result;
        }

        if (value.TryGetNonEnumeratedCount(out var count))
        {
            var result = new byte[count * 4];
            var i = 0;
            foreach (var item in value)
            {
                // 边界检查 MemoryMarshal.Cast<int, byte>(new ReadOnlySpan<int>(in item)).CopyTo(new Span<byte>(result, i++ * 4, 4));
                Unsafe.WriteUnaligned(ref result[i++ * 4], item);
            }
            return result;
        }

        return [.. value.SelectMany(BitConverter.GetBytes)];
    }
}

/// <inheritdoc cref="Utf8StringMarshaller"/>
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(MaaStringViewMarshaller))]
public static unsafe class MaaStringViewMarshaller
{
    /// <inheritdoc cref="Utf8StringMarshaller.ConvertToManaged"/>
    public static string ConvertToManaged(byte* unmanaged)
        => Marshal.PtrToStringUTF8((nint)unmanaged).ThrowIfNull();
}
