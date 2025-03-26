using MaaFramework.Binding.Custom;
using MaaFramework.Binding.Interop.Native;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using static MaaFramework.Binding.Interop.Native.MaaAgentServer;

namespace MaaFramework.Binding;

/// <summary>
///     A wrapper class providing a reference implementation for <see cref="MaaFramework.Binding.Interop.Native.MaaAgentServer"/>.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class MaaAgentServer : IMaaAgentServer
{
    private readonly MaaMarshaledApiRegistry<MaaCustomActionCallback> _actions = new();
    private readonly MaaMarshaledApiRegistry<MaaCustomRecognitionCallback> _recognitions = new();
    private string? _debugSocketId;

    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => $"{GetType().Name} {{ SocketId = {_debugSocketId}, CustomActions = [{string.Join(", ", _actions.Names)}] , CustomRecognitions = [{string.Join(" & ", _recognitions.Names)}] }}";

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
            && _actions.Register(res.Name, callback),
        IMaaCustomRecognition res
            => MaaAgentServerRegisterCustomRecognition(res.Name, res.Convert(out var callback), nint.Zero)
            && _recognitions.Register(res.Name, callback),
        _
            => throw new NotImplementedException($"Type '{typeof(T)}' is not implemented."),
    };

    /// <inheritdoc/>
    public bool StartUp(string identifier)
    {
        _debugSocketId = identifier;
        return MaaAgentServerStartUp(_debugSocketId);
    }

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
