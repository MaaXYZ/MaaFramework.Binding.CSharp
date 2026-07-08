using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaReplayControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaReplayController : MaaController
{
    private readonly string _debugRecordingPath;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ RecordingPath = {_debugRecordingPath} }}";

    /// <summary>
    ///     Creates a <see cref="MaaReplayController"/> instance that replays recorded operations.
    /// </summary>
    /// <param name="recordingPath">
    ///     <para>Path to the recording JSONL file written by <see cref="MaaRecordController"/>.</para>
    ///     <para>Screenshot image paths in the file are resolved relative to this file's parent directory.</para>
    /// </param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaReplayControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaReplayController(string recordingPath, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(recordingPath);

        var handle = MaaReplayControllerCreate(recordingPath);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugRecordingPath = recordingPath;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, recordingPath);
    }
}
