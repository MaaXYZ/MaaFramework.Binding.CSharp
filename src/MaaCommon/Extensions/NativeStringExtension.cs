using MaaCommon.Common;

namespace MaaCommon.Extensions;

/// <summary>
///     Extension methods for <see cref="NativeString"/>
/// </summary>
internal static class NativeStringExtension
{
    /// <summary>
    ///     Convert <see cref="string"/> to <see cref="NativeString"/>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static NativeString ToNative(this string str)
    {
        return new NativeString(str);
    }
    
    /// <summary>
    ///     Convert <see cref="string"/> to <see cref="IntPtr"/>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static IntPtr ToNativePtr(this string str)
    {
        return str.ToNative().Value;
    }
}
