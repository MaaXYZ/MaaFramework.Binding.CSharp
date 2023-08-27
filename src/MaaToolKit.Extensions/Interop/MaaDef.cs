// Default value: https://github.com/MaaAssistantArknights/MaaFramework/blob/main/source/MaaFramework/API/MaaTypes.h
// MaaFramework/MaaDef.h

global using MaaResourceHandle = nint;
global using MaaControllerHandle = nint;
global using MaaInstanceHandle = nint;

global using MaaBool = System.Byte;
global using MaaSize = System.UInt64;
// const MaaNullSize

global using MaaString = nint;
global using MaaJsonString = nint;

global using MaaStatus = System.Int32;
// enum MaaJobStatus

global using MaaId = System.Int64;
global using MaaCtrlId = System.Int64;
global using MaaResId = System.Int64;
global using MaaTaskId = System.Int64;
// const MaaInvalidId

global using MaaOption = System.Int32;
global using MaaOptionValue = System.Byte; // ref MaaOptionValue
global using MaaOptionValueSize = System.UInt64; // MaaOptionValue.Length

global using MaaGlobalOption = System.Int32;
// enum GlobalOption
global using MaaResOption = System.Int32;
// enum ResourceOption
global using MaaCtrlOption = System.Int32;
// enum ResourceOption
global using MaaInstOption = System.Int32;
// enum InstanceOption
// const MaaTaskParam_Empty
global using MaaAdbControllerType = System.Int32;
// enum AdbControllerType

global using MaaCallbackTransparentArg = nint;

global using MaaResourceCallback = MaaToolKit.Extensions.Interop.MaaDef.MaaApiCallback;
global using MaaControllerCallback = MaaToolKit.Extensions.Interop.MaaDef.MaaApiCallback;
global using MaaInstanceCallback = MaaToolKit.Extensions.Interop.MaaDef.MaaApiCallback;

global using MaaCustomControllerHandle = nint;
global using MaaCustomRecognizerHandle = nint;
global using MaaCustomActionHandle = nint;
global using MaaSyncContextHandle = nint;

global using int32_t = System.Int32;
global using uint8_t = System.Byte;

namespace MaaToolKit.Extensions.Interop;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public static class MaaDef
{
    // MaaFramework/MaaDef.h
    internal const MaaSize MaaNullSize = MaaSize.MaxValue;
    internal const MaaId MaaInvalidId = 0;
    internal const string EmptyMaaTaskParam = "{}";
    public delegate void MaaApiCallback(MaaString msg, MaaJsonString details_json, MaaCallbackTransparentArg callback_arg);
}

public interface IMaaDefStruct
{

}
