using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using MaaFramework.Binding.Custom;

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
    /// <exception cref="ArgumentNullException"></exception>
    public static string ConvertToString(nint value, MaaSize size = MaaSize.MinValue)
        => size == MaaSize.MinValue
        ? Marshal.PtrToStringUTF8(value) ?? throw new ArgumentNullException(nameof(value))
        : Marshal.PtrToStringUTF8(value, (int)size);

    /// <summary>
    ///     Converts a <see cref="int"/> to a MaaOptionValue (<see cref="byte"/>[]).
    /// </summary>
    public static byte[] ConvertToMaaOptionValue(int value)
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
}

/// <inheritdoc cref="Utf8StringMarshaller"/>
[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(MaaStringViewMarshaller))]
public static unsafe class MaaStringViewMarshaller
{
    /// <inheritdoc cref="Utf8StringMarshaller.ConvertToManaged"/>
    public static string ConvertToManaged(byte* unmanaged)
        => Marshal.PtrToStringUTF8((nint)unmanaged) ?? throw new ArgumentNullException(nameof(unmanaged));
}
