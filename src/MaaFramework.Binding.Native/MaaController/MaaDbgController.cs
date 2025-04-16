using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaDbgControllerCreate"/>.
/// </summary>
public class MaaDbgController : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaDbgController"/> instance.
    /// </summary>
    /// <param name="readPath">The read path.</param>
    /// <param name="writePath">The write path.</param>
    /// <param name="type">The DebugControllerType.</param>
    /// <param name="config">The config.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaDbgControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaDbgController(string readPath, string writePath, DbgControllerType type, [StringSyntax("Json")] string config = "{}", LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(readPath);
        ArgumentException.ThrowIfNullOrEmpty(writePath);
        if (type == DbgControllerType.None) throw new ArgumentException($"Value cannot be {DbgControllerType.None}.", nameof(type));
        ArgumentException.ThrowIfNullOrEmpty(config);

        var handle = MaaDbgControllerCreate(readPath, writePath, (MaaDbgControllerType)type, config, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, readPath, writePath, type, config);
    }
}
