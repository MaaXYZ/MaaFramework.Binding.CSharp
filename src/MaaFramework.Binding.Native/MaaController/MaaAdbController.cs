using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaAdbControllerCreate"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaAdbController : MaaController
{
    private readonly AdbDeviceInfo _debugInfo;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => IsInvalid
        ? $"Invalid {GetType().Name}"
        : $"{GetType().Name} {{ {nameof(_debugInfo.Name)} = {_debugInfo.Name}, {nameof(_debugInfo.AdbSerial)} = {_debugInfo.AdbSerial}, {nameof(_debugInfo.ScreencapMethods)} = {_debugInfo.ScreencapMethods}, {nameof(_debugInfo.InputMethods)} = {_debugInfo.InputMethods}, AgentPath = {_debugInfo.AgentPath} }}";

    /// <summary>
    ///     Creates a <see cref="MaaAdbController"/> instance.
    /// </summary>
    /// <param name="info">The adb device info.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaAdbController(AdbDeviceInfo info, LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(info.AdbPath);
        ArgumentException.ThrowIfNullOrEmpty(info.AdbSerial);
        ArgumentException.ThrowIfNullOrEmpty(info.Config);
        ArgumentException.ThrowIfNullOrEmpty(info.AgentPath);

        var handle = MaaAdbControllerCreate(info.AdbPath, info.AdbSerial, (MaaAdbScreencapMethod)info.ScreencapMethods, (MaaAdbInputMethod)info.InputMethods, info.Config, info.AgentPath);
        _ = MaaControllerAddSink(handle, MaaEventCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        _debugInfo = info;

        if (link == LinkOption.Start)
            LinkStartOnConstructed(check, info);
    }

    /// <param name="adbPath">The path of adb executable file.</param>
    /// <param name="adbSerial">The adb serial.</param>
    /// <param name="screencapMethods">The screencap methods.</param>
    /// <param name="inputMethods">The input methods.</param>
    /// <param name="config">The adb config.</param>
    /// <param name="agentPath">The path of agent directory. Default is "./MaaAgentBinary" if package "Maa.Framework" or "Maa.AgentBinary" is used.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <inheritdoc cref="Binding.MaaAdbController(AdbDeviceInfo, LinkOption, CheckStatusOption)"/>
    public MaaAdbController(string adbPath, string adbSerial, AdbScreencapMethods screencapMethods, AdbInputMethods inputMethods, [StringSyntax("Json")] string config = "{}", string agentPath = "./MaaAgentBinary", LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
        : this(new AdbDeviceInfo(string.Empty, adbPath, adbSerial, screencapMethods, inputMethods, config, agentPath), link, check)
    {
    }
}
