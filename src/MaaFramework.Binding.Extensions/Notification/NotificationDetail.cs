using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaaFramework.Binding.Notification;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

/// <inheritdoc cref="MaaMsg.Resource.Loading"/>
public enum ResourcePathType
{
    Bundle,
    OcrModel,
    Pipeline,
    Image,

    Unknown = -1
}

/// <inheritdoc cref="MaaMsg.Resource.Loading"/>
public record ResourceLoadingDetail(
    [property: JsonPropertyName("res_id")] long ResourceId,
    [property: JsonPropertyName("path")] string Path,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("hash")] string Hash
)
{
    // 1. .NET 7 没有支持 aot 的 JsonStringEnumConverter<TEnum>
    // 2. Unknown 向前兼容，使用 JsonStringEnumConverter 还需要设置 option 并且将 Unknown 设置为默认值 0
    [JsonIgnore] public ResourcePathType ParsedType => Enum.TryParse<ResourcePathType>(Type, true, out var result) ? result : ResourcePathType.Unknown;
};

/// <inheritdoc cref="MaaMsg.Controller.Action"/>
public record ControllerActionDetail(
    [property: JsonPropertyName("ctrl_id")] long ControllerId,
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("action")] string Action,
    [property: JsonPropertyName("param")] JsonElement Param,
    [property: JsonPropertyName("info")] JsonElement Info
);

/// <inheritdoc cref="MaaMsg.Tasker.Task"/>
public record TaskerTaskDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("entry")] string Entry,
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("hash")] string Hash
);

/// <inheritdoc cref="MaaMsg.Node.PipelineNode"/>
public record PipelineNodeDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("node_id")] long NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement Focus
);

/// <inheritdoc cref="MaaMsg.Node.RecognitionNode"/>
public record RecognitionNodeDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("node_id")] long NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement Focus
);

/// <inheritdoc cref="MaaMsg.Node.ActionNode"/>
public record ActionNodeDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("node_id")] long NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement Focus
);

/// <summary>
///     The node name with attributes.
/// </summary>
public record NodeAttr(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("jump_back")] bool JumpBack,
    [property: JsonPropertyName("anchor")] bool Anchor
);

/// <inheritdoc cref="MaaMsg.Node.NextList"/>
public record NodeNextListDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("list")] IReadOnlyList<NodeAttr> NextList,
    [property: JsonPropertyName("focus")] JsonElement Focus
);

/// <inheritdoc cref="MaaMsg.Node.Recognition"/>
public record NodeRecognitionDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("reco_id")] long RecognitionId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement Focus,
    [property: JsonPropertyName("anchor")] string? Anchor
);

/// <inheritdoc cref="MaaMsg.Node.Action"/>
public record NodeActionDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("action_id")] long ActionId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement Focus
);

public record WaitFreezesParam(
    [property: JsonPropertyName("time")] long MillisecondsTime,
    [property: JsonPropertyName("threshold")] double Threshold,
    [property: JsonPropertyName("method")] int Method,
    [property: JsonPropertyName("rate_limit")] long MillisecondsRateLimit,
    [property: JsonPropertyName("timeout")] long MillisecondsTimeout
)
{
    [JsonIgnore] public TimeSpan Time => TimeSpan.FromMilliseconds(MillisecondsTime);
    [JsonIgnore] public TimeSpan RateLimit => TimeSpan.FromMilliseconds(MillisecondsRateLimit);
    [JsonIgnore] public TimeSpan Timeout => TimeSpan.FromMilliseconds(MillisecondsTimeout);
}

/// <inheritdoc cref="MaaMsg.Node.WaitFreezes"/>
public record NodeWaitFreezesDetail(
    [property: JsonPropertyName("task_id")] long TaskId,
    [property: JsonPropertyName("wf_id")] long WaitFreezesId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("phase")] string Phase,
    [property: JsonPropertyName("roi")] IReadOnlyList<int> Roi,
    [property: JsonPropertyName("param")] WaitFreezesParam Param,
    [property: JsonPropertyName("reco_ids")] IReadOnlyList<long>? RecognitionIds,
    [property: JsonPropertyName("elapsed")] long? MillisecondsElapsed,
    [property: JsonPropertyName("focus")] JsonElement Focus
// https://github.com/MaaXYZ/MaaFramework/blob/v5.10.5/source/MaaFramework/Task/Component/ActionHelper.cpp#L46
)
{
    [JsonIgnore] public TimeSpan? Elapsed => MillisecondsElapsed.HasValue ? TimeSpan.FromMilliseconds(MillisecondsElapsed.Value) : null;
}
