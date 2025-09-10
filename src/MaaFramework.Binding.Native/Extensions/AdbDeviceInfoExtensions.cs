namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the creation of MaaAdbController.
/// </summary>
public static class AdbDeviceInfoExtensions
{
    /// <summary>
    ///     Converts a <see cref="AdbDeviceInfo"/> to a <see cref="MaaAdbController"/>.
    /// </summary>
    /// <param name="info">The AdbDeviceInfo.</param>
    /// <param name="adbPath">The new adb path.</param>
    /// <param name="adbSerial">The new adb serial.</param>
    /// <param name="screencapMethods">The screencap methods.</param>
    /// <param name="inputMethods">The input methods.</param>
    /// <param name="config">The new adb config.</param>
    /// <param name="agentPath">The new path of agent directory. Default is "./MaaAgentBinary" if package "Maa.Framework" or "Maa.AgentBinary" is used.</param>
    /// <param name="link">Executes <see cref="MaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSucceeded"/>; otherwise, not check.</param>
    /// <returns>A MaaAdbController.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static MaaAdbController ToAdbController(this AdbDeviceInfo info,
        string? adbPath = null,
        string? adbSerial = null,
        AdbScreencapMethods? screencapMethods = null,
        AdbInputMethods? inputMethods = null,
        string? config = null,
        string agentPath = "./MaaAgentBinary",
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSucceeded)
    {
        ArgumentNullException.ThrowIfNull(info);

        if (adbPath is null
            && adbSerial is null
            && screencapMethods is null
            && inputMethods is null
            && config is null)
        {
            return new MaaAdbController(info, agentPath, link, check);
        }

        return new MaaAdbController(
            new AdbDeviceInfo(
                info.Name,
                adbPath ?? info.AdbPath,
                adbSerial ?? info.AdbSerial,
                screencapMethods ?? info.ScreencapMethods,
                inputMethods ?? info.InputMethods,
                config ?? info.Config
            ),
            agentPath,
            link,
            check
        );
    }
}
