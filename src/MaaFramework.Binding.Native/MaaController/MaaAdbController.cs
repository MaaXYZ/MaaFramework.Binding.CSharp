using static MaaFramework.Binding.Interop.Native.MaaController;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaAdbControllerCreateV2"/>.
/// </summary>
public class MaaAdbController : MaaController
{
    /// <summary>
    ///     Creates a <see cref="MaaAdbController"/> instance.
    /// </summary>
    /// <param name="adbPath">The path of adb executable file.</param>
    /// <param name="address">The device address.</param>
    /// <param name="type">The AdbControllerTypes including touch type, key type and screencap type.</param>
    /// <param name="adbConfig">The path of adb config file.</param>
    /// <param name="agentPath">The path of agent directory. Default is "./MaaAgentBinary" if use package "Maa.Framework" or "Maa.AgentBinary".</param>
    /// <param name="link">Executes <see cref="IMaaController.LinkStart"/> if <see cref="LinkOption.Start"/>; otherwise, not link.</param>
    /// <param name="check">Checks LinkStart().Wait() status if <see cref="CheckStatusOption.ThrowIfNotSuccess"/>; otherwise, not check.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreateV2"/>.
    /// </remarks>
    /// <exception cref="ArgumentException" />
    /// <exception cref="MaaJobStatusException" />
    public MaaAdbController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath = "./MaaAgentBinary", LinkOption link = LinkOption.Start, CheckStatusOption check = CheckStatusOption.ThrowIfNotSuccess)
    {
        ArgumentException.ThrowIfNullOrEmpty(adbPath);
        ArgumentException.ThrowIfNullOrEmpty(address);
        type.Check();
        ArgumentException.ThrowIfNullOrEmpty(adbConfig);
        ArgumentException.ThrowIfNullOrEmpty(agentPath);

        var handle = MaaAdbControllerCreateV2(adbPath, address, (MaaAdbControllerType)type, adbConfig, agentPath, MaaApiCallback, nint.Zero);
        SetHandle(handle, needReleased: true);

        if (link == LinkOption.Start)
        {
            var status = LinkStart().Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage, adbPath, address);
            }
        }
    }
}
