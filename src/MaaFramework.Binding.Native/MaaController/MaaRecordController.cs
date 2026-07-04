using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaRecordControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaRecordController : MaaController
{
#pragma warning disable // 删除未读的私有成员
    private readonly MaaController _debugInner;
    private readonly string _debugRecordingPath;
#pragma warning restore // 删除未读的私有成员

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ RecordingPath = {_debugRecordingPath} }}";

    /// <summary>
    ///     Creates a <see cref="MaaRecordController"/> instance.
    /// </summary>
    /// <param name="inner">
    ///     <para>The inner controller to forward all operations to. Must not be null.</para>
    ///     <para>The record controller does NOT take ownership of the inner controller.</para>
    /// </param>
    /// <param name="recordingPath">
    ///     <para>Path to the recording JSONL file to write.</para>
    ///     <para>Screenshot images will be saved to a "{stem}-Screenshot" folder in the same directory as this file.</para>
    ///     <para>The recorded file can be replayed using <see cref="MaaReplayController"/>.</para>
    /// </param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaRecordControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaRecordController(MaaController inner, string recordingPath, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentException.ThrowIfNullOrEmpty(recordingPath);

        var handle = MaaRecordControllerCreate(inner.Handle, recordingPath);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugInner = inner;
        _debugRecordingPath = recordingPath;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, inner, recordingPath);
    }
}
