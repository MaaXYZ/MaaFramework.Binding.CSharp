﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaController"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaController : MaaCommon, IMaaController<MaaControllerHandle>, IMaaPost
{
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ }}";

    [ExcludeFromCodeCoverage(Justification = "Test for stateful mode.")]
    internal MaaController(MaaControllerHandle handle)
    {
        SetHandle(handle, needReleased: false);
    }

    /// <summary>
    ///     Creates a <see cref="MaaController"/> instance.
    /// </summary>
    protected MaaController()
    {
    }

    /// <summary>
    ///     Connects the address specified by the constructor on constructed.
    /// </summary>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <param name="args">The key arguments.</param>
    /// <exception cref="MaaJobStatusException"/>
    protected void LinkStartOnConstructed(CheckStatusOption check, params object?[] args)
    {
        var status = LinkStart().Wait();
        if (check == CheckStatusOption.ThrowIfNotSucceeded)
            _ = status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaControllerMessage, args);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerDestroy"/>.
    /// </remarks>
    protected override void ReleaseHandle(MaaControllerHandle handle)
    {
        try
        {
            if (LastJob != null)
                _ = MaaControllerWait(handle, LastJob.Id);
        }
        finally
        {
            MaaControllerDestroy(handle);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerSetOption"/>.
    /// </remarks>
    public bool SetOption<T>(ControllerOption opt, T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var optValue = (value, opt) switch
        {
            (int vvvv, ControllerOption.ScreenshotTargetLongSide
                    or ControllerOption.ScreenshotTargetShortSide) => vvvv.ToMaaOptionValue(),
#pragma warning disable CS0618 // 类型或成员已过时
            (bool vvv, ControllerOption.Recording
                    or ControllerOption.ScreenshotUseRawSize) => vvv.ToMaaOptionValue(),
#pragma warning restore CS0618 // 类型或成员已过时

            _ => throw new NotSupportedException($"'{nameof(ControllerOption)}.{opt}' or type '{typeof(T)}' is not supported."),
        };

        return MaaControllerSetOption(Handle, (MaaCtrlOption)opt, optValue, (MaaOptionValueSize)optValue.Length);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostConnection"/>.
    /// </remarks>
    public MaaJob LinkStart()
        => CreateJob(MaaControllerPostConnection(Handle));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostClick"/>.
    /// </remarks>
    public MaaJob Click(int x, int y)
        => CreateJob(MaaControllerPostClick(Handle, x, y));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostSwipe"/>.
    /// </remarks>
    public MaaJob Swipe(int x1, int y1, int x2, int y2, int duration)
        => CreateJob(MaaControllerPostSwipe(Handle, x1, y1, x2, y2, duration));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostPressKey"/>.
    /// </remarks>
    [Obsolete("Use ClickKey() instead.")]
    public MaaJob PressKey(int keyCode)
        => CreateJob(MaaControllerPostPressKey(Handle, keyCode));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostClickKey"/>.
    /// </remarks>
    public MaaJob ClickKey(int keyCode)
        => CreateJob(MaaControllerPostClickKey(Handle, keyCode));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostInputText"/>.
    /// </remarks>
    public MaaJob InputText(string text)
        => CreateJob(MaaControllerPostInputText(Handle, text));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostStartApp"/>.
    /// </remarks>
    public MaaJob StartApp(string intent)
        => CreateJob(MaaControllerPostStartApp(Handle, intent));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostStopApp"/>.
    /// </remarks>
    public MaaJob StopApp(string intent)
        => CreateJob(MaaControllerPostStopApp(Handle, intent));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchDown"/>.
    /// </remarks>
    public MaaJob TouchDown(int contact, int x, int y, int pressure)
        => CreateJob(MaaControllerPostTouchDown(Handle, contact, x, y, pressure));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchMove"/>.
    /// </remarks>
    public MaaJob TouchMove(int contact, int x, int y, int pressure)
        => CreateJob(MaaControllerPostTouchMove(Handle, contact, x, y, pressure));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostTouchUp"/>.
    /// </remarks>
    public MaaJob TouchUp(int contact)
        => CreateJob(MaaControllerPostTouchUp(Handle, contact));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostKeyDown"/>.
    /// </remarks>
    public MaaJob KeyDown(int keyCode)
        => CreateJob(MaaControllerPostKeyDown(Handle, keyCode));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostKeyUp"/>.
    /// </remarks>
    public MaaJob KeyUp(int keyCode)
        => CreateJob(MaaControllerPostKeyUp(Handle, keyCode));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerPostScreencap"/>.
    /// </remarks>
    public MaaJob Screencap()
        => CreateJob(MaaControllerPostScreencap(Handle));

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerStatus"/>.
    /// </remarks>
    [Obsolete("Deprecated from v4.5.0.")]
    public MaaJobStatus GetStatus(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var id = job.Id;
        var handle = Handle;
        return IsInvalid ? MaaJobStatus.Invalid : (MaaJobStatus)MaaControllerStatus(handle, id);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerWait"/>.
    /// </remarks>
    [Obsolete("Deprecated from v4.5.0.")]
    public MaaJobStatus Wait(MaaJob job)
    {
        ArgumentNullException.ThrowIfNull(job);

        var id = job.Id;
        var handle = Handle;
        return IsInvalid ? MaaJobStatus.Invalid : (MaaJobStatus)MaaControllerWait(handle, id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MaaJob CreateJob(MaaResId id)
    {
        var job = new MaaJob(id, this);
        if (id != MaaDef.MaaInvalidId)
            LastJob = job;
        return job;
    }

    /// <inheritdoc/>
    public MaaJob? LastJob { get; private set; }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerConnected"/>.
    /// </remarks>
    public bool IsConnected
        => MaaControllerConnected(Handle);

    /// <inheritdoc/>
    public bool GetCachedImage(IMaaImageBuffer maaImage)
        => GetCachedImage((MaaImageBuffer)maaImage);

    /// <inheritdoc cref="IMaaController.GetCachedImage"/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerCachedImage"/>.
    /// </remarks>
    public bool GetCachedImage(MaaImageBuffer maaImage)
    {
        ArgumentNullException.ThrowIfNull(maaImage);

        return MaaControllerCachedImage(Handle, maaImage.Handle);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaControllerGetUuid"/>.
    /// </remarks>
    public string? Uuid
    {
        get
        {
            using var buffer = new MaaStringBuffer();
            return MaaControllerGetUuid(Handle, buffer.Handle)
                ? buffer.ToString()
                : null;
        }
    }
}
