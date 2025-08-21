using System.Diagnostics.CodeAnalysis;
using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A non-generic sealed class used to wrap <see cref="IMaaImageBuffer"/>.
/// </summary>
/// <param name="buffer">The IMaaImageBuffer.</param>
public sealed class MaaImage(IMaaImageBuffer buffer) : IMaaDisposable
{
    private bool _isCached;
    private readonly MemoryStream _cacheStream = new(); // MemoryStream implements IDisposable, but does not actually have any resources to dispose.
    private ImageInfo _cacheInfo = new(-1, -1, -1, -1);

    /// <summary>
    ///     Gets the IMaaImageBuffer.
    /// </summary>
    public IMaaImageBuffer Buffer => buffer;

    /// <inheritdoc/>
    public bool ThrowOnInvalid
    {
        get => buffer.ThrowOnInvalid;
        set => buffer.ThrowOnInvalid = value;
    }

    /// <inheritdoc/>
    public bool IsStateless => buffer.IsStateless;

    /// <inheritdoc/>
    event System.ComponentModel.CancelEventHandler? IMaaDisposable.Releasing
    {
        add => buffer.Releasing += value;
        remove => buffer.Releasing -= value;
    }

    /// <inheritdoc/>
    event EventHandler? IMaaDisposable.Released
    {
        add => buffer.Released += value;
        remove => buffer.Released -= value;
    }

    /// <summary>
    ///     Caches the image data from the <see cref="IMaaImageBuffer"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the image was cached successfully; otherwise, <see langword="false"/>.</returns>
    public bool TryCache()
    {
        if (_isCached && Buffer.IsInvalid)
            return true;

        var succeeded = Buffer.TryGetEncodedData(out Stream? stream);
        using (stream)
        {
            if (!succeeded || !stream!.CanRead)
                return false;

            _cacheInfo = Buffer.GetInfo();
            _cacheStream.SetLength(0);
            stream.CopyTo(_cacheStream);
            return _isCached = true;
        }
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    public override string ToString()
    {
        if (IsInvalid)
            return $"Invalid {GetType().Name}";

        var info = GetInfo();
        return $"{GetType().Name}: {info.Width}x{info.Height} {{ {nameof(info.Channels)} = {info.Channels}, {nameof(info.Type)} = {info.Type} }}";
    }

    /// <inheritdoc cref="IMaaImageBuffer.GetInfo"/>
    public ImageInfo GetInfo()
        => _isCached ? _cacheInfo : Buffer.GetInfo();

    /// <inheritdoc/>
    public bool IsInvalid => !_isCached && Buffer.IsInvalid;

    /// <inheritdoc/>
    public void Dispose()
        => Buffer.Dispose();

    #region Load & Save

    /// <summary>
    ///     Creates a new instance of the <see cref="MaaImage"/> class from the given <paramref name="stream"/>.
    /// </summary>
    /// <param name="stream">The stream containing image information.</param>
    /// <returns>A <see cref="MaaImage"/>.</returns>
    public static MaaImage Load<T>(Stream stream) where T : IMaaImageBuffer, new()
    {
        var buffer = new T();
        MaaInteroperationException.ThrowIfNot(
            buffer.TrySetEncodedData(stream),
            $"Failed to load png image from '{nameof(stream)}'.");
        return new(buffer);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="MaaImage"/> class from the given file.
    /// </summary>
    /// <param name="filePath">The file path of an image.</param>
    /// <returns>A <see cref="MaaImage"/>.</returns>
    public static MaaImage Load<T>(string filePath) where T : IMaaImageBuffer, new()
    {
        using var stream = File.OpenRead(filePath);
        return Load<T>(stream);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="MaaImage"/> class from the given <paramref name="span"/>.
    /// </summary>
    /// <param name="span">The readonly span containing image data.</param>
    /// <returns>A <see cref="MaaImage"/>.</returns>
    public static MaaImage Load<T>(ReadOnlySpan<byte> span) where T : IMaaImageBuffer, new()
    {
        var buffer = new T();
        MaaInteroperationException.ThrowIfNot(
            buffer.TrySetEncodedData(span),
            $"Failed to load png image from '{nameof(span)}'.");
        return new(buffer);
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
    /// <exception cref="MaaInteroperationException">The <see cref="Buffer"/> is disposed.</exception>
    public void Save(Stream stream)
    {
        MaaInteroperationException.ThrowIf(IsInvalid, $"The '{nameof(Buffer)}' is disposed.");
        if (_isCached)
        {
            _cacheStream.CopyTo(stream);
            return;
        }

        var succeeded = Buffer.TryGetEncodedData(out Stream? dataStream);
        using (dataStream)
        {
            MaaInteroperationException.ThrowIfNot(succeeded, $"Failed to get encoded data from '{nameof(Buffer)}'.");
            dataStream!.CopyTo(stream);
            dataStream.Close();
        }
    }

    /// <summary>
    ///  Saves this <see cref="MaaImage"/> to the specified file.
    /// </summary>
    /// <param name="filename">The filePath to save the image to.</param>
    public void Save(string filename)
    {
        using var stream = File.OpenWrite(filename);
        Save(stream);
    }

    /// <summary>
    ///  Saves this <see cref="MaaImage"/> to the specified <paramref name="buffer"/>.
    /// </summary>
    /// <param name="buffer">The buffer to save the image to.</param>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="MaaInteroperationException">The <paramref name="buffer"/> is disposed.</exception>
    public void Save(IMaaImageBuffer buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        MaaInteroperationException.ThrowIf(IsInvalid, $"The '{nameof(Buffer)}' is disposed.");

        var isSuccessful = _isCached
            ? buffer.TrySetEncodedData(_cacheStream)
            : Buffer.TryCopyTo(buffer);

        MaaInteroperationException.ThrowIfNot(isSuccessful, $"Failed to set encoded data to '{nameof(buffer)}'.");
    }

    #endregion
}
