using System.Text.Json.Serialization;

namespace MaaFramework.Binding.Extensions.Notification;

/// <inheritdoc cref="JsonSerializerContext"/>
[JsonSerializable(typeof(ResourceLoadingDetail))]
[JsonSerializable(typeof(ControllerActionDetail))]
[JsonSerializable(typeof(TaskerTaskDetail))]
[JsonSerializable(typeof(TaskNextListDetail))]
[JsonSerializable(typeof(TaskRecognitionDetail))]
[JsonSerializable(typeof(TaskActionDetail))]
public partial class NotificationDetailContext : JsonSerializerContext { }
