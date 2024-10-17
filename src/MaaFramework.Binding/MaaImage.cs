using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A non-generic sealed class used to wrap <see cref="IMaaImageBuffer"/>.
/// </summary>
/// <param name="Buffer">The IMaaImageBuffer.</param>
public sealed record MaaImage(IMaaImageBuffer Buffer) : IMaaDisposable
{
    /// <inheritdoc/>
    public override string ToString()
    {
        var info = Info;
        return $"{GetType().Name}: {info.Width}x{info.Height} {{ {nameof(info.Channels)} = {info.Channels}, {nameof(info.Type)} = {info.Type} }}";
    }

    /// <inheritdoc cref="IMaaImageBuffer.Info"/>
    public ImageInfo Info => Buffer.Info;

    /// <inheritdoc/>
    public bool IsInvalid => Buffer.IsInvalid;

    /// <inheritdoc/>
    public void Dispose()
    {
        Buffer.Dispose();
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="MaaImage"/> class from the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The stream containing image information.</param>
    /// <returns>A <see cref="MaaImage"/>.</returns>
    public static MaaImage Load<T>(Stream stream) where T : IMaaImageBuffer, new()
        => new(new T
        {
            EncodedDataStream = stream
        });

    /// <summary>
    ///     Creates a new instance of the <see cref="MaaImage"/> class from the given file.
    /// </summary>
    /// <param name="filename">The filename of an image.</param>
    /// <returns>A <see cref="MaaImage"/>.</returns>
    public static MaaImage Load<T>(string filename) where T : IMaaImageBuffer, new()
    {
        using var stream = File.OpenRead(filename);
        return Load<T>(stream);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="MaaImage"/> class from the given <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The buffer containing image information.</param>
    /// <returns>A <see cref="MaaImage"/>.</returns>
    public static MaaImage Load(IMaaImageBuffer buffer)
        => new(buffer);

    /// <summary>
    ///  Saves this <see cref="MaaImage"/> to the specified <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The stream to save the image to.</param>
    public void Save(Stream stream)
    {
        if (Buffer.IsInvalid) throw new InvalidOperationException();

        Buffer.EncodedDataStream.CopyTo(stream);
    }

    /// <summary>
    ///  Saves this <see cref="MaaImage"/> to the specified file.
    /// </summary>
    /// <param name="filename">The filename to save the image to.</param>
    public void Save(string filename)
    {
        using var stream = File.OpenWrite(filename);
        Save(stream);
    }

    /// <summary>
    ///  Saves this <see cref="MaaImage"/> to the specified <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The buffer to save the image to.</param>
    public void Save(IMaaImageBuffer buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        if (Buffer.IsInvalid) throw new InvalidOperationException();

        Buffer.CopyTo(buffer);
    }
}
