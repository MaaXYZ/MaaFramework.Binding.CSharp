# MaaFramework.Binding.Extensions

Document Language: [简体中文](README.zh_cn.md) / [English](README.md)

## Library Development Goals

MaaFramework.Binding(.Native) is a class library designed for interoperability with MaaFramework APIs, adhering to these core principles:

1. **Performance & Stability First** - Prioritize execution efficiency and runtime stability over code elegance
2. **Zero Third-Party Dependencies** - No external library dependencies introduced
3. **No Logging System Integration** - Fully relies on MaaFramework's built-in logging system

Any Binding implementations violating these principles will be placed in MaaFramework.Binding.Extensions.

## Usage Guide

- **[MaaFramework.Binding.Notification](#MaaFrameworkBindingNotification)** - Manage and dispatch MaaFramework callback notifications
- **TODO** - Parse JSON data structures returned by MaaFramework

### MaaFramework.Binding.Notification

Following the [Standardized Interface Design](https://github.com/MaaXYZ/MaaFramework/blob/main/docs/en_us/4.2-StandardizedInterfaceDesign.md), uses System.Text.Json for callback parsing and dispatching.

Base event definition:
```csharp
public event EventHandler<MaaCallbackEventArgs>? Callback;
```

While MaaFramework.Binding natively handles callbacks through the Callback event, it lacks structured parsing. The Extensions library enhances functionality through:

#### 1. Handler Registry Mechanism

Type-safe event registration via `NotificationHandlerRegistry`:

```csharp
// Example: Resource loading event handling
var reg = new NotificationHandlerRegistry();
reg.Resource.Loading.Starting += (sender, detail) => 
{
    Console.WriteLine($"Resource loading started: {detail}");
};
maa.Callback += reg.OnCallback;
```

#### 2. Extension Method Conversion

Convert handlers to standard event handlers using `ToCallback` extension:

```csharp
public static EventHandler<MaaCallbackEventArgs> ToCallback<TDetail>(
    this NotificationHandler<TDetail> notify, 
    string prefixOfMaaMsg)
```

Supported TDetail types:
- ResourceLoadingDetail
- ControllerActionDetail
- TaskerTaskDetail
- NodeNextListDetail
- NodeRecognitionDetail
- NodeActionDetail
- string

Usage example:
```csharp
NotificationHandler<ResourceLoadingDetail> OnResourceLoading = (type, detail) =>
{
    switch (type)
    {
        case NotificationType.Starting:
            Console.WriteLine($"Resource Loading started: {detail}");
            break;
        case NotificationType.Succeeded:
            Console.WriteLine($"Resource Loading succeeded: {detail}");
            break;
        case NotificationType.Failed:
            Console.WriteLine($"Resource Loading failed: {detail}");
            break;
        default:
            Console.WriteLine($"Resource Unknown Callback: {detail}");
            break;
    }
};
maa.Callback += OnResourceLoading.ToCallback();
```