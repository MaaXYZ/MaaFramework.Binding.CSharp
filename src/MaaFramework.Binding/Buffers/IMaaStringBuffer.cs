using MaaFramework.Binding.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaStringBuffer<THandle> : IMaaStringBuffer, IMaaBuffer<THandle, IMaaStringBuffer>, IMaaDisposableHandle<THandle>
{
    // Implement IMaaStringBufferStatic<THandle> at the same time if this interface is implemented.
}

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer.
/// </summary>
public interface IMaaStringBuffer : IMaaBuffer<IMaaStringBuffer>, IMaaDisposable
{
    /// <summary>
    ///     Gets a value indicates whether the string of the <see cref="IMaaStringBuffer"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the string is empty; otherwise, <see langword="false"/>.</returns>
    bool IsEmpty { get; }

    /// <summary>
    ///     Gets the size of the string.
    /// </summary>
    /// <returns>The size.</returns>
    MaaSize Size { get; }

    /// <summary>
    ///     Clears the string of the <see cref="IMaaStringBuffer"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the string was cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool TryClear();

    /// <inheritdoc cref="IMaaStringBufferStatic{THandle}.TryGetValue(THandle, out string)"/>
    bool TryGetValue([MaybeNullWhen(false)] out string str);

    /// <inheritdoc cref="IMaaStringBufferStatic{THandle}.TrySetValue(THandle, string, bool)"/>
    bool TrySetValue(string str, bool useEx = true);
}

/// <summary>
///     An interface defining wrapped static abstract members for MaaStringBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaStringBufferStatic<THandle>
{
    /// <summary>
    ///     Gets the string from a MaaStringBuffer.
    /// </summary>
    /// <param name="handle">The MaaStringBufferHandle.</param>
    /// <param name="str">The string.</param>
    /// <returns>The <see cref="string"/>.</returns>
    /// <returns><see langword="true"/> if the string was got successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetValue(THandle handle, [MaybeNullWhen(false)] out string str);

    /// <summary>
    ///     Gets the string from a function using MaaRectBuffer.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="writeBuffer">The function used to write the string to the buffer.</param>
    /// <returns>The <see cref="string"/>.</returns>
    /// <returns><see langword="true"/> if the string was got successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TryGetValue([MaybeNullWhen(false)] out string str, Func<THandle, bool> writeBuffer);

    /// <summary>
    ///     Sets the string to a MaaStringBuffer.
    /// </summary>
    /// <param name="handle">The MaaStringBufferHandle.</param>
    /// <param name="str">The string.</param>
    /// <param name="useEx">Uses MaaSetStringEx() if <see langword="true"/>; otherwise, Uses MaaSetString().</param>
    /// <returns><see langword="true"/> if the <see cref="string"/> was set successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetValue(THandle handle, string str, bool useEx = true);

    /// <summary>
    ///     Sets the string to a function using MaaRectBuffer.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="useEx">Uses MaaSetStringEx() if <see langword="true"/>; otherwise, Uses MaaSetString().</param>
    /// <param name="readBuffer">The function used to read the string from the buffer.</param>
    /// <returns>The <see cref="string"/>.</returns>
    /// <returns><see langword="true"/> if the string was got successfully; otherwise, <see langword="false"/>.</returns>
    static abstract bool TrySetValue(string str, bool useEx, Func<THandle, bool> readBuffer);
}
