using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaAdbControllerCreate"/>.
/// </summary>
public class MaaAdbController : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaAdbController"/> instance.
    /// </summary>
    /// <param name="adbPath">The path of adb executable file.</param>
    /// <param name="adbSerial">The adb serial.</param>
    /// <param name="screencapMethods">The screencap methods.</param>
    /// <param name="inputMethods">The input methods.</param>
    /// <param name="config">The path of adb config file.</param>
    /// <param name="agentPath">The path of agent directory. Default is "./MaaAgentBinary" if package "Maa.Framework" or "Maa.AgentBinary" is used.</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreate"/>.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="MaaJobStatusException"/>
    public MaaAdbController(string adbPath, string adbSerial, AdbScreencapMethods screencapMethods, AdbInputMethods inputMethods, string config, string agentPath = "./MaaAgentBinary", LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentException.ThrowIfNullOrEmpty(adbPath);
        ArgumentException.ThrowIfNullOrEmpty(adbSerial);
        if (screencapMethods == AdbScreencapMethods.None) throw new ArgumentException($"Value cannot be {AdbScreencapMethods.None}.", nameof(screencapMethods));
        if (inputMethods == AdbInputMethods.None) throw new ArgumentException($"Value cannot be {AdbInputMethods.None}.", nameof(inputMethods));
        ArgumentException.ThrowIfNullOrEmpty(config);
        ArgumentException.ThrowIfNullOrEmpty(agentPath);

        var handle = MaaAdbControllerCreate(adbPath, adbSerial, (MaaAdbScreencapMethod)screencapMethods, (MaaAdbInputMethod)inputMethods, config, agentPath, MaaNotificationCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        if (link != LinkOption.Start)
            return;

        var status = LinkStart().Wait();
        if (check == CheckStatusOption.ThrowIfNotSucceeded)
            status.ThrowIfNot(MaaJobStatus.Succeeded, MaaJobStatusException.MaaControllerMessage, adbPath, adbSerial);
    }
}
