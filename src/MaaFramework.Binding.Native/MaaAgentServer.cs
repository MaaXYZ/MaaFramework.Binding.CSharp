using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using static MaaFramework.Binding.Interop.Native.MaaAgentServer;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaAgentServer"/>.
/// </summary>
public class MaaAgentServer : IMaaAgentServer
{
    private readonly MaaMarshaledApis<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApis<MaaCustomRecognitionCallback> _recognitions = new();

    /// <inheritdoc/>
    public bool Register<T>(string name, T custom) where T : IMaaCustomResource
    {
        custom.Name = name;
        return Register(custom);
    }

    /// <inheritdoc/>
    /// <remarks>
    ///     Wrapper of <see cref="MaaAgentServerRegisterCustomAction"/> and <see cref="MaaAgentServerRegisterCustomRecognition"/>.
    /// </remarks>
    public bool Register<T>(T custom) where T : IMaaCustomResource => custom switch
    {
        IMaaCustomAction res
            => MaaAgentServerRegisterCustomAction(res.Name, res.Convert(out var callback), nint.Zero)
            && _actions.Set(res.Name, callback),
        IMaaCustomRecognition res
            => MaaAgentServerRegisterCustomRecognition(res.Name, res.Convert(out var callback), nint.Zero)
            && _recognitions.Set(res.Name, callback),
        _
            => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
    };

    /// <inheritdoc/>
    public bool StartUp(string identifier)
        => MaaAgentServerStartUp(identifier);

    /// <inheritdoc/>
    public void ShutDown()
        => MaaAgentServerShutDown();

    /// <inheritdoc/>
    public void Join()
        => MaaAgentServerJoin();

    /// <inheritdoc/>
    public void Detach()
        => MaaAgentServerDetach();
}
