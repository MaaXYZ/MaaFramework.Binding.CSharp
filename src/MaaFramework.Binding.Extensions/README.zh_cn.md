# MaaFramework.Binding.Extensions

文档语言: [简体中文](README.zh_cn.md) / [English](README.md)

## 类库编写目标

MaaFramework.Binding(.Native) 是以 MaaFramework API 互操作为目标编写的类库，其核心设计原则如下：

1. **性能与稳定性优先** - 优先保证接口执行效率与运行稳定性，优雅性作为次要考量
2. **零第三方依赖** - 不引入任何第三方库依赖
3. **无日志系统集成** - 完全依托 MaaFramework 的日志系统进行输出

任何不符合上述原则的 Binding 功能实现都将放在 MaaFramework.Binding.Extensions 库中。

## 使用指南

- **[MaaFramework.Binding.Notification](#MaaFrameworkBindingNotification)** - 管理和分发 MaaFramework 回调通知的处理程序
- **TODO** - 解析 MaaFramework 返回的 Json 数据结构

### MaaFramework.Binding.Notification

根据[标准化接口设计](https://github.com/MaaXYZ/MaaFramework/blob/main/docs/zh_cn/4.2-标准化接口设计.md)，使用 System.Text.Json 进行回调解析与派发。

基础事件定义：
```csharp
public event EventHandler<MaaCallbackEventArgs>? Callback;
```

MaaFramework.Binding 原生通过 Callback 事件处理回调，但未提供结构化解析。Extensions 库通过以下方式增强功能：

#### 1. 处理程序注册表机制

通过 `NotificationHandlerRegistry` 实现类型安全的事件注册：

```csharp
// 示例：资源加载事件处理
var reg = new NotificationHandlerRegistry();
reg.Resource.Loading.Starting += (sender, detail) => 
{
    Console.WriteLine($"Resource loading started: {detail}");
};
maa.Callback += reg.OnCallback;
```

#### 2. 扩展方法转换

通过 `ToCallback` 扩展方法将处理程序转换为标准事件处理器：

```csharp
public static EventHandler<MaaCallbackEventArgs> ToCallback<TDetail>(
    this NotificationHandler<TDetail> notify, 
    string prefixOfMaaMsg)
```

TDetail 为以下类型：
- ResourceLoadingDetail
- ControllerActionDetail
- TaskerTaskDetail
- NodeNextListDetail
- NodeRecognitionDetail
- NodeActionDetail
- string

使用示例：
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
