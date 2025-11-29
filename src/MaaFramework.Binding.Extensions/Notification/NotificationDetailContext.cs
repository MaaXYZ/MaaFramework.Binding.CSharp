using System.Text.Json.Serialization;

namespace MaaFramework.Binding.Notification;

/// <inheritdoc cref="JsonSerializerContext"/>
[JsonSerializable(typeof(ResourceLoadingDetail))]
[JsonSerializable(typeof(ControllerActionDetail))]
[JsonSerializable(typeof(TaskerTaskDetail))]
[JsonSerializable(typeof(PipelineNodeDetail))]
[JsonSerializable(typeof(RecognitionNodeDetail))]
[JsonSerializable(typeof(ActionNodeDetail))]
[JsonSerializable(typeof(NodeAttr))]
[JsonSerializable(typeof(NodeNextListDetail))]
[JsonSerializable(typeof(NodeRecognitionDetail))]
[JsonSerializable(typeof(NodeActionDetail))]
public partial class NotificationDetailContext : JsonSerializerContext;
