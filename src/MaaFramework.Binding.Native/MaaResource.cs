using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Enums;
using MaaFramework.Binding.Exceptions;
using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.Framework.MaaResource;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.Framework.MaaResource"/>.
/// </summary>
public class MaaResource : MaaCommon<ResourceOption>
{
    internal MaaResourceHandle _handle;
    private bool disposed;

    /// <inheritdoc cref="MaaResource(MaaCallbackTransparentArg)"/>
    public MaaResource()
        : this(MaaCallbackTransparentArg.Zero)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaResource"/> instance.
    /// </summary>
    /// <param name="maaCallbackTransparentArg">The maaCallbackTransparentArg.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceCreate"/>.
    /// </remarks>
    public MaaResource(MaaCallbackTransparentArg maaCallbackTransparentArg)
    {
        _handle = MaaResourceCreate(MaaApiCallback, maaCallbackTransparentArg);
    }

    /// <inheritdoc cref="MaaResource(CheckStatusOption, string[])"/>
    public MaaResource(params string[] paths)
        : this(CheckStatusOption.ThrowIfNotSuccess, paths)
    {
    }

    /// <inheritdoc cref="MaaResource(MaaCallbackTransparentArg, CheckStatusOption, string[])"/>
    public MaaResource(CheckStatusOption check, params string[] paths)
    : this(MaaCallbackTransparentArg.Zero, check, paths)
    {
    }

    /// <param name="maaCallbackTransparentArg">The maaCallbackTransparentArg.</param>
    /// <param name="check">Checks AppendPath(path).Wait() status if true; otherwise, not checks.</param>
    /// <param name="paths">The paths of maa resource.</param>
    /// <exception cref="MaaJobStatusException" />
    /// <inheritdoc cref="MaaResource(MaaCallbackTransparentArg)"/>
    public MaaResource(MaaCallbackTransparentArg maaCallbackTransparentArg, CheckStatusOption check, params string[] paths)
        : this(maaCallbackTransparentArg)
    {
        foreach (var path in paths)
        {
            var status = AppendPath(path).Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfMaaResourceNotSuccess();
            }
        }
    }

    /// <summary>
    ///     Disposes the <see cref="MaaResource"/> instance.
    /// </summary>
    /// <param name="disposing"></param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected override void Dispose(bool disposing)
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
    public override bool SetParam(MaaJob job, string param)
        => false;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceStatus"/>.
    /// </remarks>
    public override MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)MaaResourceStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceWait"/>.
    /// </remarks>
    public override MaaJobStatus Wait(MaaJob job)
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

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceSetOption"/>.
    /// </remarks>
    internal override bool SetOption(ResourceOption option, MaaOptionValue[] value)
        => MaaResourceSetOption(_handle, (MaaResOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <summary>
    ///     Gets the hash string of the <see cref="MaaResource"/>.
    /// </summary>
    /// <value>
    ///     A string if the hash was got successfully; otherwise, null.
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
