using MaaToolKit.Extensions.Enums;
using MaaToolKit.Extensions.Exceptions;
using MaaToolKit.Extensions.Interfaces;
using MaaToolKit.Extensions.Interop;
using System.Runtime.InteropServices;
using static MaaToolKit.Extensions.Interop.MaaApi;

namespace MaaToolKit.Extensions.ComponentModel;

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables

/// <summary>
///     A class providing a reference implementation for Maa Resource section of <see cref="MaaApi"/>.
/// </summary>
public class MaaResource : IMaaNotify, IMaaPost, IDisposable
{
    internal MaaResourceHandle _handle;

    private static readonly HashSet<MaaResource> _resources = new();
    internal static MaaResource Get(IntPtr handle)
    {
        var ret = _resources.FirstOrDefault(x => x._handle == handle);
        if (ret == null)
        {
            ret = new(); // { _handle = handle };

            // TODOO: MaaResource() 应该为 internal, 但 MaaCallbackTransparentArg 还没有更好的替代
            // 不过只从该程序集调用 FW, ret 不可能为 null
            ret.Dispose(true);
            ret._handle = handle;
            _resources.Add(ret);
        }

        return ret;
    }

    /// <inheritdoc/>
    public event IMaaNotify.MaaCallback? Callback;
    private readonly MaaResourceCallback _callback;

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
        _callback = (msg, detail, arg) => Callback?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
        _handle = MaaResourceCreate(_callback, maaCallbackTransparentArg);
        _resources.Add(this);
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
            this.Append(path)
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
        if (disposing && _handle != MaaResourceHandle.Zero)
        {
            MaaResourceDestroy(_handle);
            _handle = MaaResourceHandle.Zero;
        }
    }

    /// <summary>
    ///     Appends a async job of loading resource from <paramref name="resourcePath"/> , could be called multiple times.
    /// </summary>
    /// <param name="resourcePath">The resource path.</param>
    /// <returns>A resource load job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourcePostResource"/>.
    /// </remarks>
    public MaaJob Append(string resourcePath)
    {
        var id = MaaResourcePostResource(_handle, resourcePath);
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
    /// <returns>true if the option was successfully setted; otherwise, false.</returns>
    private bool SetOption(ResourceOption option, MaaOptionValue[] value)
        => MaaResourceSetOption(_handle, (MaaResOption)option, ref value[0], (MaaOptionValueSize)value.Length).ToBoolean();
#pragma warning restore

    /// <summary>
    ///     Gets the hash string of the <see cref="MaaResource"/>.
    /// </summary>
    /// <value>
    ///     Null if failed to get hash, or a UTF-8 string represent of hash
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="MaaResourceGetHash"/>.
    /// </remarks>
    public string? Hash => _handle.GetStringFromFuncWithBuffer(
        MaaResourceGetHash,
        bufferSize: 1 << 10);
}
