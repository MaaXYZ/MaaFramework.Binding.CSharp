﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaaFramework.Binding.Notification;

/// <inheritdoc cref="MaaMsg.Resource.Loading.Prefix"/>
public record ResourceLoadingDetail(
    [property: JsonPropertyName("res_id")] int ResourceId,
    [property: JsonPropertyName("hash")] string Hash,
    [property: JsonPropertyName("path")] string Path
);

/// <inheritdoc cref="MaaMsg.Controller.Action.Prefix"/>
public record ControllerActionDetail(
    [property: JsonPropertyName("ctrl_id")] int ControllerId,
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("action")] string Action
);

/// <inheritdoc cref="MaaMsg.Tasker.Task.Prefix"/>
public record TaskerTaskDetail(
    [property: JsonPropertyName("task_id")] int TaskId,
    [property: JsonPropertyName("entry")] string Entry,
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("hash")] string Hash
);

/// <inheritdoc cref="MaaMsg.Node.NextList.Prefix"/>
public record NodeNextListDetail(
    [property: JsonPropertyName("task_id")] int TaskId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("list")] IReadOnlyList<string> NextList,
    [property: JsonPropertyName("focus")] JsonElement? Focus
);

/// <inheritdoc cref="MaaMsg.Node.Recognition.Prefix"/>
public record NodeRecognitionDetail(
    [property: JsonPropertyName("task_id")] int TaskId,
    [property: JsonPropertyName("reco_id")] int RecognitionId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement? Focus
);

/// <inheritdoc cref="MaaMsg.Node.Action.Prefix"/>
public record NodeActionDetail(
    [property: JsonPropertyName("task_id")] int TaskId,
    [property: JsonPropertyName("node_id")] int NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("focus")] JsonElement? Focus
);
