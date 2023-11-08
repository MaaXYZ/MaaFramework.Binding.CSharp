using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Exceptions;
using MaaFramework.Binding.Interfaces;
using MaaFramework.Binding.Interop;
using System.Runtime.InteropServices;
using static MaaFramework.Binding.Interop.MaaApi;

namespace MaaFramework.Binding;

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables

/// <summary>
///     A class providing a reference implementation for Maa Resource section of <see cref="MaaApi"/>.
/// </summary>
public class MaaResource : IMaaNotify, IMaaPost, IDisposable
{
    internal MaaResourceHandle _handle;
    private bool disposed;

    /// <inheritdoc cref="MaaResource(MaaCallbackTransparentArg)"/>
    public MaaResource()
        : this(MaaCallbackTransparentArg.Zero)

    /// <summary>
    ///     Creates a <see cref="MaaResource"/> instance.
    /// </summary>
    /// <param name="maaCallbackTransparentArg">The maaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceCreate"/>.
    /// </remarks>
    public MaaResource(MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        _callback = (msg, detail, arg) => Callback?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
        _handle = MaaResourceCreate(_callback, maaCallbackTransparentArg);
    }

    /// <inheritdoc cref="MaaResource(MaaCallbackTransparentArg, string[])"/>
    public MaaResource(params string[] paths)
        : this(MaaCallbackTransparentArg.Zero, paths)
    {
    }

    /// <inheritdoc cref="MaaResource(MaaCallbackTransparentArg)"/>
    /// <param name="maaCallbackTransparentArg">The maaCallbackTransparentArg.</param>
    /// <param name="paths">The paths of maa resource.</param>
    /// <exception cref="MaaJobStatusException" />
    public MaaResource(MaaCallbackTransparentArg maaCallbackTransparentArg, params string[] paths)
        : this(maaCallbackTransparentArg)
    {
        foreach (var path in paths)
        {
            AppendPath(path)
                .Wait()
                .ThrowIfNot(MaaJobStatus.Success);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Disposes the <see cref="MaaResource"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            // if (disposing) Dispose managed resources.
            MaaResourceDestroy(_handle);
            _handle = MaaResourceHandle.Zero;
            disposed = true;
        }
    }

    /// <summary>
    ///     Appends a async job of loading resource from <paramref name="resourcePath"/> , could be called multiple times.
    /// </summary>
    /// <param name="resourcePath">The resource path.</param>
    /// <returns>A resource load job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourcePostPath"/>.
    /// </remarks>
    public MaaJob AppendPath(string resourcePath)
    {
        var id = MaaResourcePostPath(_handle, resourcePath);
        return new(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Always return false.
    /// </remarks>
    public bool SetParam(MaaJob job, string param)
        => false;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)MaaResourceStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
        => (MaaJobStatus)MaaResourceWait(_handle, job);

    /// <summary>
    ///     Gets whether the <see cref="MaaResource"/> is fully loaded.
    /// </summary>
    /// <value>
    ///     true if the <see cref="MaaResource"/> is fully loaded; otherwise, false.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceLoaded"/>.
    /// </remarks>
    public bool Loaded => MaaResourceLoaded(_handle).ToBoolean();

#pragma warning disable // TODO
    /// <summary>
    ///     Sets <paramref name="value"/> to a option of the <see cref="MaaResource"/>.
    /// </summary>
    /// <param name="option">The option.</param>
    /// <param name="value">The value.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceSetOption"/>.
    /// </remarks>
    /// <returns>true if the option was setted successfully; otherwise, false.</returns>
    private bool SetOption(ResourceOption option, MaaOptionValue[] value)
        => MaaResourceSetOption(_handle, (MaaResOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();
#pragma warning restore

    /// <summary>
    ///     Gets the hash string of the <see cref="MaaResource"/>.
    /// </summary>
    /// <value>
    ///     A string if the hash was successfully got; otherwise, null.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetHash"/>.
    /// </remarks>
    public string? Hash
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            var ret = MaaResourceGetHash(_handle, buffer._handle).ToBoolean();
            return ret ? buffer.ToString() : null;
        }
    }
}
