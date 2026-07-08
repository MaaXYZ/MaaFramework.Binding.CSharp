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
    /// <param name="configJson">
    ///     <para>JSON config for the control unit.</para>
    ///     <para>Required fields:
    ///     <br/>- library_path: path to the Android native control unit library
    ///     <br/>- screen_resolution.width / screen_resolution.height: raw screenshot and touch resolution</para>
    ///     <para>Optional fields:
    ///     <br/>- display_id: target display id, defaults to 0
    ///     <br/>- force_stop: whether to force stop before start_app, defaults to false</para>
    /// </param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAndroidNativeControllerCreate"/>.
    ///     <para>This controller is only available on Android.</para>
    ///     <para>The configured screen_resolution must match the control unit's raw screenshot/touch coordinate space.</para>
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
