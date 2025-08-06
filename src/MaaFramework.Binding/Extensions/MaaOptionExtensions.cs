using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

#pragma warning disable S4136

/// <summary>
///     A static class providing extension methods for setting maa option.
/// </summary>
public static class MaaOptionExtensions
{
    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_ScreenshotTargetLongSide(this IMaaOption<ControllerOption>? opt, int value)
        => opt?.SetOption(ControllerOption.ScreenshotTargetLongSide, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_ScreenshotTargetShortSide(this IMaaOption<ControllerOption>? opt, int value)
        => opt?.SetOption(ControllerOption.ScreenshotTargetShortSide, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_ScreenshotUseRawSize(this IMaaOption<ControllerOption>? opt, bool value)
        => opt?.SetOption(ControllerOption.ScreenshotUseRawSize, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_Recording(this IMaaOption<ControllerOption>? opt, bool value)
        => opt?.SetOption(ControllerOption.Recording, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_LogDir(this IMaaOption<GlobalOption>? opt, string value)
        => opt?.SetOption(GlobalOption.LogDir, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_SaveDraw(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.SaveDraw, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_Recording(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.Recording, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_StdoutLevel(this IMaaOption<GlobalOption>? opt, LoggingLevel value)
        => opt?.SetOption(GlobalOption.StdoutLevel, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_ShowHitDraw(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.ShowHitDraw, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_DebugMode(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.DebugMode, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_InferenceDevice(this IMaaOption<ResourceOption>? opt, int value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_InferenceDevice(this IMaaOption<ResourceOption>? opt, InferenceDevice value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_InferenceDevice(this IMaaOption<ResourceOption>? opt, InferenceCoreMLFlags value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_InferenceExecutionProvider(this IMaaOption<ResourceOption>? opt, InferenceExecutionProvider value)
        => opt?.SetOption(ResourceOption.InferenceExecutionProvider, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    private static bool SetOption_Inference<T>(this IMaaOption<ResourceOption>? opt, InferenceExecutionProvider executionProvider, T value)
    {
        ArgumentNullException.ThrowIfNull(opt);
        return opt.SetOption(ResourceOption.InferenceExecutionProvider, executionProvider)
            && opt.SetOption(ResourceOption.InferenceDevice, value);
    }

    /// <summary>
    ///     Sets inference execution provider and device.
    /// </summary>
    /// <param name="opt">The option.</param>
    /// <returns><see langword="true"/> if the option was set successfully; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static bool SetInference_UseAutoExecutionProvider(this IMaaOption<ResourceOption>? opt)
        => opt.SetOption_Inference(InferenceExecutionProvider.Auto, InferenceDevice.Auto);

    /// <inheritdoc cref="SetInference_UseAutoExecutionProvider"/>
    public static bool SetInference_UseCpu(this IMaaOption<ResourceOption>? opt)
        => opt.SetOption_Inference(InferenceExecutionProvider.CPU, InferenceDevice.CPU);

    /// <param name="opt">The option.</param>
    /// <param name="deviceId">The adapter id.</param>
    /// <inheritdoc cref="SetInference_UseAutoExecutionProvider"/>
    public static bool SetInference_UseDirectML(this IMaaOption<ResourceOption>? opt, int deviceId = (int)InferenceDevice.Auto)
        => opt.SetOption_Inference(InferenceExecutionProvider.DirectML, deviceId);

    /// <inheritdoc cref="SetInference_UseDirectML(IMaaOption{ResourceOption}?, int)"/>
    public static bool SetInference_UseDirectML(this IMaaOption<ResourceOption>? opt, InferenceDevice deviceId = InferenceDevice.Auto)
        => opt.SetOption_Inference(InferenceExecutionProvider.DirectML, deviceId);

    /// <param name="opt">The option.</param>
    /// <param name="coreMLFlags">The CoreML flags.</param>
    /// <remarks>
    ///     <para>Reference to <a href="https://github.com/microsoft/onnxruntime/blob/main/include/onnxruntime/core/providers/coreml/coreml_provider_factory.h">COREMLFlags</a>.</para>
    ///     <para>But you need to pay attention to the onnxruntime version we use, the latest flag may not be supported.</para>
    /// </remarks>
    /// <inheritdoc cref="SetInference_UseAutoExecutionProvider"/>
    public static bool SetInference_UseCoreML(this IMaaOption<ResourceOption>? opt, int coreMLFlags = (int)InferenceDevice.Auto)
        => opt.SetOption_Inference(InferenceExecutionProvider.CoreML, coreMLFlags);

    /// <inheritdoc cref="SetInference_UseCoreML(IMaaOption{ResourceOption}?, int)"/>
    public static bool SetInference_UseCoreML(this IMaaOption<ResourceOption>? opt, InferenceCoreMLFlags coreMLFlags = InferenceCoreMLFlags.Auto)
        => opt.SetOption_Inference(InferenceExecutionProvider.CoreML, coreMLFlags);

    // not implemented
    // public static bool SetInference_UseCuda(this IMaaOption<ResourceOption>? opt, InferenceDevice device)
    //     => opt.SetOption_Inference(InferenceExecutionProvider.CUDA, InferenceDevice.Auto);
    // public static bool SetInference_UseCuda(this IMaaOption<ResourceOption>? opt, int nvidiaGpuId)
    //     => opt.SetOption_Inference(InferenceExecutionProvider.CUDA, nvidiaGpuId);
}
