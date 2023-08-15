using MaaToolKit.Enums;
using MaaToolKit.Extensions.Interfaces;
using MaaToolKit.Interop;
using System.Runtime.InteropServices;
using static MaaToolKit.Interop.MaaApiWrapper;

namespace MaaToolKit.Extensions.ComponentModel;

/// <summary>
///     A class providing a reference implementation for Maa Resource section of <see cref="MaaApiWrapper"/>.
/// </summary>
public class MaaResource : IMaaNotify, IMaaPost, IDisposable
{
    internal IntPtr _handle;

    private static readonly HashSet<MaaResource> _resources = new HashSet<MaaResource>();
    internal static MaaResource GetMaaResource(IntPtr handle) => _resources.First(x => x._handle == handle);

    /// <inheritdoc/>
    public event MaaCallback? Notify;
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
    private readonly MaaApi.MaaCallback _callback;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

    /// <summary>
    ///     Creates a <see cref="MaaResource"/> instance.
    /// </summary>
    /// <remarks>
    ///     Wrapper of <see cref="CreateMaaResource"/>.
    /// </remarks>
    public MaaResource() : this("MaaResource")
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaResource"/> instance.
    /// </summary>
    /// <param name="identifier"></param>
    /// <remarks>
    ///     Wrapper of <see cref="CreateMaaResource"/>.
    /// </remarks>
    public MaaResource(string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);

        _callback = (msg, detail, arg) => Notify?.Invoke(
            Marshal.PtrToStringUTF8(msg) ?? string.Empty,
            Marshal.PtrToStringUTF8(detail) ?? "{}",
            arg);
        _handle = CreateMaaResource(_callback, identifier);
        _resources.Add(this);
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
    ///     Wrapper of <see cref="DisposeMaaResource"/>.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeMaaResource(_handle);
            _handle = IntPtr.Zero;
        }
    }

    /// <summary>
    ///     Appends a async job of loading resource from <paramref name="resourcePath"/> , could be called multiple times.
    /// </summary>
    /// <param name="resourcePath">The resource path.</param>
    /// <returns>A resource load job.</returns>
    /// <remarks>
    ///     Wrapper of <see cref="AppendAddResourceJob"/>.
    /// </remarks>
    public MaaJob Append(string resourcePath)
    {
        var id = AppendAddResourceJob(_handle, resourcePath);
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
    ///     Wrapper of <see cref="GetResourceStatus"/>.
    /// </remarks>
    public MaaJobStatus GetStatus(MaaJob job)
        => (MaaJobStatus)GetResourceStatus(_handle, job);

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="WaitResourceJob"/>.
    /// </remarks>
    public MaaJobStatus Wait(MaaJob job)
        => (MaaJobStatus)WaitResourceJob(_handle, job);

    /// <summary>
    ///     Gets whether the <see cref="MaaResource"/> is fully loaded.
    /// </summary>
    /// <value>
    ///     true if the <see cref="MaaResource"/> is fully loaded; otherwise, false.
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="IsResourceLoaded"/>.
    /// </remarks>
    public bool Loaded => IsResourceLoaded(_handle);

    /// <summary>
    ///     Sets the <see cref="MaaResource"/> option.
    /// </summary>
    /// <param name="option">The ResourceOption</param>
    /// <param name="value">The option value</param>
    /// <returns></returns>
    /// <remarks>
    ///     Wrapper of <see cref="SetResourceOption"/>.
    /// </remarks>
    public bool SetOption(ResourceOption option, string value)
        => SetResourceOption(_handle, option, value);

    /// <summary>
    ///     Gets the hash string of the <see cref="MaaResource"/>.
    /// </summary>
    /// <value>
    ///     Null if failed to get hash, or a UTF-8 string represent of hash
    /// </value>
    /// <remarks>
    ///     Wrapper of <see cref="GetResourceHash"/>.
    /// </remarks>
    public string? Hash => GetResourceHash(_handle);
}
