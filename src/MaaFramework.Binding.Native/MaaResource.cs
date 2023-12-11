using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Native.Abstractions;
using MaaFramework.Binding.Native.Interop;
using static MaaFramework.Binding.Native.Interop.MaaResource;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Native.Interop.MaaResource"/>.
/// </summary>
public class MaaResource : MaaCommon<ResourceOption>, IMaaResource<nint>
{
    /// <summary>
    ///     Converts a <see cref="IMaaResource{nint}"/> instance to a <see cref="MaaResource"/>.
    /// </summary>
    /// <param name="maaResource">The <see cref="IMaaResource{nint}"/> instance.</param>
    public MaaResource(IMaaResource<nint> maaResource)
    {
        SetHandle(maaResource.Handle, needReleased: true);
    }

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
        var handle = MaaResourceCreate(maaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle, needReleased: true);
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
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaResourceMessage);
            }
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle()
        => MaaResourceDestroy(Handle);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourcePostPath"/>.
    /// </remarks>
    public IMaaJob AppendPath(string resourcePath)
    {
        var id = MaaResourcePostPath(Handle, resourcePath);
        return new MaaJob(id, this);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Always return false.
    /// </remarks>
    public bool SetParam(IMaaJob job, string param)
        => false;

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(IMaaJob job)
        => (MaaJobStatus)MaaResourceStatus(Handle, job.Id);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceWait"/>.
    /// </remarks>
    public MaaJobStatus Wait(IMaaJob job)
        => (MaaJobStatus)MaaResourceWait(Handle, job.Id);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceLoaded"/>.
    /// </remarks>
    public bool Loaded => MaaResourceLoaded(Handle).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceSetOption"/>.
    /// </remarks>
    sealed protected override bool SetOption(ResourceOption option, MaaOptionValue[] value)
        => MaaResourceSetOption(Handle, (MaaResOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetHash"/>.
    /// </remarks>
    public string? Hash
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            var ret = MaaResourceGetHash(Handle, buffer.Handle).ToBoolean();
            return ret ? buffer.ToString() : null;
        }
    }
}
