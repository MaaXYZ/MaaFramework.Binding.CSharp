using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding.Buffers;

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer with generic handle.
/// </summary>
public interface IMaaStringBuffer<out T> : IMaaStringBuffer, IMaaDisposableHandle<T>
{
}

/// <summary>
///     An interface defining wrapped members for MaaStringBuffer.
/// </summary>
public interface IMaaStringBuffer : IDisposable
{
    /// <summary>
    ///     Gets a value indicates whether the string of the MaaStringBuffer is empty.
    /// </summary>
    /// <returns>true if the string is empty; otherwise, false.</returns>
    bool IsEmpty { get; }

    /// <summary>
    ///     Clears the string of the MaaStringBuffer.
    /// </summary>
    /// <returns>true if the string was cleared successfully; otherwise, false.</returns>
    bool Clear();

    /// <summary>
    ///     Gets the string from the MaaStringBuffer.
    /// </summary>
    /// <returns>The string.</returns>
    string GetValue();

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
    bool SetValue(string str, bool useEx = true);
}
