﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable CS1573 // 参数在 XML 注释中没有匹配的 param 标记
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

#nullable enable

using System.Text.Json;

namespace MaaFramework.Binding.Notification;

/// <summary>
///     A registry that manages and distributes MaaFramework callback notifications,
/// acting as a central processor to receive MaaCallback events and route them to
/// appropriate handlers.
/// </summary>
public sealed class NotificationHandlerRegistry
{
    public void OnCallback(object? sender, MaaCallbackEventArgs e)
    {
        switch (e.Message)
        {
            case MaaMsg.Resource.Loading.Starting:
                Resource.Loading.OnStarting(sender, e.Details); return;
            case MaaMsg.Resource.Loading.Succeeded:
                Resource.Loading.OnSucceeded(sender, e.Details); return;
            case MaaMsg.Resource.Loading.Failed:
                Resource.Loading.OnFailed(sender, e.Details); return;
            case MaaMsg.Controller.Action.Starting:
                Controller.Action.OnStarting(sender, e.Details); return;
            case MaaMsg.Controller.Action.Succeeded:
                Controller.Action.OnSucceeded(sender, e.Details); return;
            case MaaMsg.Controller.Action.Failed:
                Controller.Action.OnFailed(sender, e.Details); return;
            case MaaMsg.Tasker.Task.Starting:
                Tasker.Task.OnStarting(sender, e.Details); return;
            case MaaMsg.Tasker.Task.Succeeded:
                Tasker.Task.OnSucceeded(sender, e.Details); return;
            case MaaMsg.Tasker.Task.Failed:
                Tasker.Task.OnFailed(sender, e.Details); return;
            case MaaMsg.Node.NextList.Starting:
                Node.NextList.OnStarting(sender, e.Details); return;
            case MaaMsg.Node.NextList.Succeeded:
                Node.NextList.OnSucceeded(sender, e.Details); return;
            case MaaMsg.Node.NextList.Failed:
                Node.NextList.OnFailed(sender, e.Details); return;
            case MaaMsg.Node.Recognition.Starting:
                Node.Recognition.OnStarting(sender, e.Details); return;
            case MaaMsg.Node.Recognition.Succeeded:
                Node.Recognition.OnSucceeded(sender, e.Details); return;
            case MaaMsg.Node.Recognition.Failed:
                Node.Recognition.OnFailed(sender, e.Details); return;
            case MaaMsg.Node.Action.Starting:
                Node.Action.OnStarting(sender, e.Details); return;
            case MaaMsg.Node.Action.Succeeded:
                Node.Action.OnSucceeded(sender, e.Details); return;
            case MaaMsg.Node.Action.Failed:
                Node.Action.OnFailed(sender, e.Details); return;
            default:
                OnUnknown(sender, e); return;
        }
    }
    public event EventHandler<MaaCallbackEventArgs>? Unknown;
    internal void OnUnknown(object? sender, MaaCallbackEventArgs details) => Unknown?.Invoke(sender, details);

    public ResourceRegistry Resource { get; } = new();
    public sealed class ResourceRegistry
    {
        public LoadingRegistry Loading { get; } = new();
        public sealed class LoadingRegistry
        {
            public event EventHandler<ResourceLoadingDetail>? Starting;
            internal void OnStarting(object? sender, string details) => Starting?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.ResourceLoadingDetail) ?? throw new InvalidCastException());
            public event EventHandler<ResourceLoadingDetail>? Succeeded;
            internal void OnSucceeded(object? sender, string details) => Succeeded?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.ResourceLoadingDetail) ?? throw new InvalidCastException());
            public event EventHandler<ResourceLoadingDetail>? Failed;
            internal void OnFailed(object? sender, string details) => Failed?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.ResourceLoadingDetail) ?? throw new InvalidCastException());
        }

    }

    public ControllerRegistry Controller { get; } = new();
    public sealed class ControllerRegistry
    {
        public ActionRegistry Action { get; } = new();
        public sealed class ActionRegistry
        {
            public event EventHandler<ControllerActionDetail>? Starting;
            internal void OnStarting(object? sender, string details) => Starting?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.ControllerActionDetail) ?? throw new InvalidCastException());
            public event EventHandler<ControllerActionDetail>? Succeeded;
            internal void OnSucceeded(object? sender, string details) => Succeeded?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.ControllerActionDetail) ?? throw new InvalidCastException());
            public event EventHandler<ControllerActionDetail>? Failed;
            internal void OnFailed(object? sender, string details) => Failed?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.ControllerActionDetail) ?? throw new InvalidCastException());
        }

    }

    public TaskerRegistry Tasker { get; } = new();
    public sealed class TaskerRegistry
    {
        public TaskRegistry Task { get; } = new();
        public sealed class TaskRegistry
        {
            public event EventHandler<TaskerTaskDetail>? Starting;
            internal void OnStarting(object? sender, string details) => Starting?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.TaskerTaskDetail) ?? throw new InvalidCastException());
            public event EventHandler<TaskerTaskDetail>? Succeeded;
            internal void OnSucceeded(object? sender, string details) => Succeeded?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.TaskerTaskDetail) ?? throw new InvalidCastException());
            public event EventHandler<TaskerTaskDetail>? Failed;
            internal void OnFailed(object? sender, string details) => Failed?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.TaskerTaskDetail) ?? throw new InvalidCastException());
        }

    }

    public NodeRegistry Node { get; } = new();
    public sealed class NodeRegistry
    {
        public NextListRegistry NextList { get; } = new();
        public sealed class NextListRegistry
        {
            public event EventHandler<NodeNextListDetail>? Starting;
            internal void OnStarting(object? sender, string details) => Starting?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeNextListDetail) ?? throw new InvalidCastException());
            public event EventHandler<NodeNextListDetail>? Succeeded;
            internal void OnSucceeded(object? sender, string details) => Succeeded?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeNextListDetail) ?? throw new InvalidCastException());
            public event EventHandler<NodeNextListDetail>? Failed;
            internal void OnFailed(object? sender, string details) => Failed?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeNextListDetail) ?? throw new InvalidCastException());
        }

        public RecognitionRegistry Recognition { get; } = new();
        public sealed class RecognitionRegistry
        {
            public event EventHandler<NodeRecognitionDetail>? Starting;
            internal void OnStarting(object? sender, string details) => Starting?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeRecognitionDetail) ?? throw new InvalidCastException());
            public event EventHandler<NodeRecognitionDetail>? Succeeded;
            internal void OnSucceeded(object? sender, string details) => Succeeded?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeRecognitionDetail) ?? throw new InvalidCastException());
            public event EventHandler<NodeRecognitionDetail>? Failed;
            internal void OnFailed(object? sender, string details) => Failed?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeRecognitionDetail) ?? throw new InvalidCastException());
        }

        public ActionRegistry Action { get; } = new();
        public sealed class ActionRegistry
        {
            public event EventHandler<NodeActionDetail>? Starting;
            internal void OnStarting(object? sender, string details) => Starting?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeActionDetail) ?? throw new InvalidCastException());
            public event EventHandler<NodeActionDetail>? Succeeded;
            internal void OnSucceeded(object? sender, string details) => Succeeded?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeActionDetail) ?? throw new InvalidCastException());
            public event EventHandler<NodeActionDetail>? Failed;
            internal void OnFailed(object? sender, string details) => Failed?.Invoke(sender, JsonSerializer.Deserialize(details,
                    NotificationDetailContext.Default.NodeActionDetail) ?? throw new InvalidCastException());
        }

    }

}