namespace MaaFramework.Binding;

/// <summary>
///     A static class providing extension methods for the creation of MaaAdbController.
/// </summary>
public static class DeviceInfoExtension
{
    /// <summary>
    ///     Converts a <see cref="DeviceInfo"/> to a <see cref="MaaAdbController"/>.
    /// </summary>
    /// <param name="info">The DeviceInfo.</param>
    /// <param name="adbPath">The new adb path.</param>
    /// <param name="address">The new address.</param>
    /// <param name="types">The new AdbControllerTypes.</param>
    /// <param name="adbConfig">The new adb config.</param>
    /// <param name="agentPath">The new path of agent directory. Default is "./MaaAgentBinary" if use package "Maa.Framework" or "Maa.AgentBinary".</param>
    /// <param name="link">Executes <see cref="MaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSuccess"/>; otherwise, not check.</param>
    /// <returns>A MaaAdbController.</returns>
    public static MaaAdbController ToAdbController(this DeviceInfo info,
        string? adbPath = null,
        string? address = null,
        AdbControllerTypes? types = null,
        string? adbConfig = null,
        string agentPath = "./MaaAgentBinary",
        LinkOption link = LinkOption.Start,
        CheckStatusOption check = CheckStatusOption.ThrowIfNotSuccess)
    {
        ArgumentNullException.ThrowIfNull(info);

        return new(adbPath ?? info.AdbPath,
                   address ?? info.AdbSerial,
                   types ?? info.AdbTypes,
                   adbConfig ?? info.AdbConfig,
                   agentPath,
                   link,
                   check);
    }
}
