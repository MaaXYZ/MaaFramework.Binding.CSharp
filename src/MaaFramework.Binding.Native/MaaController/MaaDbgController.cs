using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaDbgControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaDbgController : MaaController
{
    private readonly string _debugReadPath;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ ReadFrom = {_debugReadPath} }}";

    /// <summary>
    ///     Creates a <see cref="MaaDbgController"/> instance that serves images from a directory.
    /// </summary>
    /// <param name="readPath">
    ///     <para>Path to a directory of images (or a single image file).</para>
    ///     <para>Images are loaded on connect and cycled through on each screencap request.</para>
    ///     <para>All input operations (click, swipe, etc.) are no-ops that return success.</para>
    /// </param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDbgControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaDbgController(string readPath, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(readPath);

        var handle = MaaDbgControllerCreate(readPath);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugReadPath = readPath;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, readPath);
    }
}
