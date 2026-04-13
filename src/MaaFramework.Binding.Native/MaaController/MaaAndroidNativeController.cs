using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaAndroidNativeControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaAndroidNativeController : MaaController
{
    private readonly string _debugConfigJson;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ ConfigJson = {_debugConfigJson} }}";

    /// <summary>
    ///     Creates a <see cref="MaaAndroidNativeController"/> instance.
    /// </summary>
    /// <param name="configJson">JSON config for the control unit.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAndroidNativeControllerCreate"/>.
    ///     <para>This controller is only available on Android.</para>
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaAndroidNativeController([StringSyntax("Json")] string configJson, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(configJson);

        var handle = MaaAndroidNativeControllerCreate(configJson);
        _ = MaaControllerAddSink(handle, MaaEventCallback, (nint)MaaHandleType.Controller);
        SetHandle(handle, needReleased: true);

        _debugConfigJson = configJson;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, configJson);
    }
}
