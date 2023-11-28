using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding.Grpc.Interop;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning disable S1104 // Fields should not have public accessibility

/// <summary>
///     A struct provides the delegates of <see cref="MaaCustomActionApi" />.
/// </summary>
public struct MaaActionApi
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">The MaaSyncContextHandle.</param>
    /// <param name="task">The MaaStringView.</param>
    /// <param name="param">The MaaStringView.</param>
    /// <param name="box">The MaaRectHandle.</param>
    /// <param name="detail">The MaaStringView.</param>
    /// <param name="arg">The MaaTransparentArg.</param>
    /// <returns></returns>
    public delegate bool Run(
        string context,
        string task,
        string param,
        IMaaRectBuffer box,
        IMaaStringBuffer detail,
        nint arg);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="arg">The MaaTransparentArg.</param>
    public delegate void Stop(nint arg);
}

/// <summary>
///     MaaCustomActionApi
/// </summary>
public struct MaaCustomActionApi : IMaaDefStruct
{
    public required MaaActionApi.Run Run;

    public required MaaActionApi.Stop Stop;
}

