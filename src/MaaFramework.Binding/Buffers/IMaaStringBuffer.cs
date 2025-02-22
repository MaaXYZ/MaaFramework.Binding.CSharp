using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer with generic handle.
/// </summary>
/// <typeparam name="THandle">The type of handle.</typeparam>
public interface IMaaStringBuffer<THandle> : IMaaStringBuffer, IMaaBuffer<THandle, IMaaStringBuffer<THandle>>, IMaaDisposableHandle<THandle>
{
}

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer.
/// </summary>
public interface IMaaStringBuffer : IMaaBuffer<IMaaStringBuffer>
{
    /// <summary>
    ///     Gets a value indicates whether the string of the <see cref="IMaaStringBuffer"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the string is empty; otherwise, <see langword="false"/>.</returns>
    bool IsEmpty { get; }

    /// <summary>
    ///     Clears the string of the <see cref="IMaaStringBuffer"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the string was cleared successfully; otherwise, <see langword="false"/>.</returns>
    bool Clear();

    /// <summary>
    ///     Gets the string from the <see cref="IMaaStringBuffer"/>.
    /// </summary>
    /// <returns>The <see cref="string"/>.</returns>
    string GetValue();

    /// <summary>
    ///     Gets the size of the string.
    /// </summary>
    /// <returns>The size.</returns>
    MaaSize Size { get; }

    /// <summary>
    ///     Sets a string into the MaaStringBuffer.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="useEx">Uses MaaSetStringEx if <see langword="true"/>; otherwise, Uses MaaSetString.</param>
    /// <returns><see langword="true"/> if the string was set successfully; otherwise, <see langword="false"/>.</returns>
    bool SetValue(string str, bool useEx = true);
}
