using System.Text.Json.Serialization;

namespace MaaFramework.Binding.Notification;

/// <inheritdoc cref="JsonSerializerContext"/>
[JsonSerializable(typeof(ResourceLoadingDetail))]
[JsonSerializable(typeof(ControllerActionDetail))]
[JsonSerializable(typeof(TaskerTaskDetail))]
[JsonSerializable(typeof(NodeNextListDetail))]
[JsonSerializable(typeof(NodeRecognitionDetail))]
[JsonSerializable(typeof(TaskActionDetail))]
public partial class NotificationDetailContext : JsonSerializerContext { }
