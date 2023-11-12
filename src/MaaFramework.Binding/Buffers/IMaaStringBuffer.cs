using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer.
/// </summary>
public interface IMaaStringBuffer : IMaaDisposableHandle
{
    /// <summary>
    ///     Indicates whether the string of the MaaStringBuffer is empty.
    /// </summary>
    /// <returns>true if the string is empty; otherwise, false.</returns>
    bool IsEmpty();

    /// <summary>
    ///     Clears the string of the MaaStringBuffer.
    /// </summary>
    /// <returns>true if the string was cleared successfully; otherwise, false.</returns>
    bool Clear();

    /// <summary>
    ///     Gets the string from the MaaStringBuffer.
    /// </summary>
    /// <returns>The string.</returns>
    string Get();

    /// <summary>
    ///     Gets the size of the string.
    /// </summary>
    /// <returns>The size.</returns>
    ulong Size { get; }

    /// <summary>
    ///     Sets a string into the MaaStringBuffer.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="useEx">Uses MaaSetStringEx if true; otherwise, Uses MaaSetString.</param>
    /// <returns>true if the string was setted successfully; otherwise, false.</returns>
    bool Set(string str, bool useEx = true);

    /// <summary>
    ///     Gets or Sets the string of the MaaStringBuffer.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    string String { get; set; }
}
