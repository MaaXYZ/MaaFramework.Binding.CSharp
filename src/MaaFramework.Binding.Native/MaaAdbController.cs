using static MaaFramework.Binding.Native.Interop.MaaController;

namespace MaaFramework.Binding.Native;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaAdbControllerCreateV2"/>.
/// </summary>
public class MaaAdbController : MaaController
{
    /// <inheritdoc cref="MaaAdbController(string, string, AdbControllerTypes, string, string, LinkOption)"/>
    public MaaAdbController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath)
        : this(adbPath, address, type, adbConfig, agentPath, LinkOption.Start)
    {
    }

    /// <inheritdoc cref="MaaAdbController(string, string, AdbControllerTypes, string, string, CheckStatusOption, LinkOption)"/>
    public MaaAdbController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, LinkOption link)
        : this(adbPath, address, type, adbConfig, agentPath, CheckStatusOption.ThrowIfNotSuccess, link)
    {
    }

    /// <inheritdoc cref="MaaAdbController(string, string, AdbControllerTypes, string, string, MaaCallbackTransparentArg, CheckStatusOption, LinkOption)"/>
    public MaaAdbController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, CheckStatusOption check, LinkOption link)
        : this(adbPath, address, type, adbConfig, agentPath, MaaCallbackTransparentArg.Zero, check, link)
    {
    }

    /// <summary>
    ///     Creates a <see cref="MaaAdbController"/> instance.
    /// </summary>
    /// <param name="adbPath">The path of adb executable file.</param>
    /// <param name="address">The device address.</param>
    /// <param name="type">The AdbControllerTypes including touch type, key type and screencap type.</param>
    /// <param name="adbConfig">The path of adb config file.</param>
    /// <param name="agentPath">The path of agent directory.</param>
    /// <param name="maaCallbackTransparentArg">The maaCallbackTransparentArg.</param>
    /// <param name="check">Checks LinkStart().Wait() status if true; otherwise, not check.</param>
    /// <param name="link">Executes <see cref="MaaController.LinkStart"/> if true; otherwise, not link.</param>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAdbControllerCreateV2"/>.
    /// </remarks>
    /// <exception cref="ArgumentException" />
    /// <exception cref="MaaJobStatusException" />
    public MaaAdbController(string adbPath, string address, AdbControllerTypes type, string adbConfig, string agentPath, MaaCallbackTransparentArg maaCallbackTransparentArg, CheckStatusOption check, LinkOption link)
    {
        ArgumentException.ThrowIfNullOrEmpty(adbPath);
        ArgumentException.ThrowIfNullOrEmpty(address);
        type.Check();
        ArgumentException.ThrowIfNullOrEmpty(adbConfig);
        ArgumentException.ThrowIfNullOrEmpty(agentPath);

        var handle = MaaAdbControllerCreateV2(adbPath, address, (int)type, adbConfig, agentPath, maaApiCallback, maaCallbackTransparentArg);
        SetHandle(handle);

        if (link == LinkOption.Start)
        {
            var status = LinkStart().Wait();
            if (check == CheckStatusOption.ThrowIfNotSuccess)
            {
                status.ThrowIfNot(MaaJobStatus.Success, MaaJobStatusException.MaaControllerMessage);
            }
        }
    }
}
