using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped static abstract members for MaaImageListBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaImageListBufferStatic<THandle>
{
    /// <summary>
    ///     Gets the image encoded data list from a MaaImageListBuffer.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    /// <param name="dataList">The list of image data (PNG).</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the MaaImageListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetEncodedDataList(THandle handle, [MaybeNullWhen(false)] out IList<byte[]> dataList);

    /// <summary>
    ///     Gets the image encoded data list from a function using MaaImageListBuffer.
    /// </summary>
    /// <param name="dataList">The list of image data (PNG).</param>
    /// <param name="writeBuffer">The function used to write the data list to the list buffer.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the MaaImageListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetEncodedDataList([MaybeNullWhen(false)] out IList<byte[]> dataList, Func<THandle, bool> writeBuffer);

    /// <summary>
    ///     Sets the image encoded data list to a MaaImageListBuffer.
    /// </summary>
    /// <param name="handle">The MaaImageListBufferHandle.</param>
    /// <param name="dataList">The list of image data (PNG).</param>
    /// <returns><see langword="true"/> if all element were set successfully to the MaaImageListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetEncodedDataList(THandle handle, IEnumerable<byte[]> dataList);

    /// <inheritdoc cref="TrySetEncodedDataList(THandle, IEnumerable{byte[]})"/>
    static abstract bool TrySetEncodedDataList(THandle handle, IEnumerable<Stream> dataList);

    /// <summary>
    ///     Sets the image encoded data list to a function using MaaImageListBuffer.
    /// </summary>
    /// <param name="dataList">The list of image data (PNG).</param>
    /// <param name="readBuffer">The function used to read the data list to the list buffer.</param>
    /// <returns><see langword="true"/> if all element were set successfully to the MaaImageListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetEncodedDataList(IEnumerable<byte[]> dataList, Func<THandle, bool> readBuffer);

    /// <inheritdoc cref="TrySetEncodedDataList(IEnumerable{byte[]}, Func{THandle, bool})"/>
    static abstract bool TrySetEncodedDataList(IEnumerable<Stream> dataList, Func<THandle, bool> readBuffer);
}

/// <summary>
///     An interface defining wrapped static abstract members for MaaStringListBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaStringListBufferStatic<THandle>
{
    /// <summary>
    ///     Gets the <see langword="string"/> list from a MaaStringListBuffer.
    /// </summary>
    /// <param name="handle">The MaaStringListBufferHandle.</param>
    /// <param name="stringList">The list of strings.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the MaaStringListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetList(THandle handle, [MaybeNullWhen(false)] out IList<string> stringList);

    /// <summary>
    ///     Gets the <see langword="string"/> list from a function using MaaStringListBuffer.
    /// </summary>
    /// <param name="stringList">The list of strings.</param>
    /// <param name="writeBuffer">The function used to write the string list to the list buffer.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the MaaStringListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetList([MaybeNullWhen(false)] out IList<string> stringList, Func<THandle, bool> writeBuffer);

    /// <summary>
    ///     Sets the <see langword="string"/> list to a MaaStringListBuffer.
    /// </summary>
    /// <param name="handle">The MaaStringListBufferHandle.</param>
    /// <param name="stringList">The list of strings.</param>
    /// <returns><see langword="true"/> if all element were set successfully to the MaaStringListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetList(THandle handle, IEnumerable<string> stringList);

    /// <summary>
    ///     Sets the <see langword="string"/> list to a function using MaaStringListBuffer.
    /// </summary>
    /// <param name="stringList">The list of strings.</param>
    /// <param name="readBuffer">The function used to read the string list to the list buffer.</param>
    /// <returns><see langword="true"/> if all element were set successfully to the MaaStringListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetList(IEnumerable<string> stringList, Func<THandle, bool> readBuffer);
}

/// <summary>
///     An interface defining wrapped static abstract members for DesktopWindowListBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IDesktopWindowListBufferStatic<THandle>
{
    /// <summary>
    ///     Gets the <see cref="DesktopWindowInfo"/> list from a DesktopWindowListBuffer.
    /// </summary>
    /// <param name="handle">The DesktopWindowListBufferHandle.</param>
    /// <param name="windowList">The list of windows.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the DesktopWindowListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetList(THandle handle, [MaybeNullWhen(false)] out IList<DesktopWindowInfo> windowList);

    /// <summary>
    ///     Gets the <see cref="DesktopWindowInfo"/> list from a function using DesktopWindowListBuffer.
    /// </summary>
    /// <param name="windowList">The list of windows.</param>
    /// <param name="writeBuffer">The function used to write the window list to the list buffer.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the DesktopWindowListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetList([MaybeNullWhen(false)] out IList<DesktopWindowInfo> windowList, Func<THandle, bool> writeBuffer);
}

/// <summary>
///     An interface defining wrapped static abstract members for AdbDeviceListBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IAdbDeviceListBufferStatic<THandle>
{
    /// <summary>
    ///     Gets the <see cref="AdbDeviceInfo"/> list from a AdbDeviceListBuffer.
    /// </summary>
    /// <param name="handle">The AdbDeviceListBufferHandle.</param>
    /// <param name="deviceList">The list of devices.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the AdbDeviceListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetList(THandle handle, [MaybeNullWhen(false)] out IList<AdbDeviceInfo> deviceList);

    /// <summary>
    ///     Gets the <see cref="AdbDeviceInfo"/> list from a function using AdbDeviceListBuffer.
    /// </summary>
    /// <param name="deviceList">The list of devices.</param>
    /// <param name="writeBuffer">The function used to write the device list to the list buffer.</param>
    /// <returns><see langword="true"/> if an empty list or each element was got successfully from the AdbDeviceListBuffer; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetList([MaybeNullWhen(false)] out IList<AdbDeviceInfo> deviceList, Func<THandle, bool> writeBuffer);
}
