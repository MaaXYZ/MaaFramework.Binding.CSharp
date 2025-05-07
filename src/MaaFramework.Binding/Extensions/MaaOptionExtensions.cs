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
    public static bool SetOption_InferenceDevice(this IMaaOption<ResourceOption>? opt, InferenceDevice value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_InferenceDevice(this IMaaOption<ResourceOption>? opt, int value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOption_InferenceExecutionProvider(this IMaaOption<ResourceOption>? opt, InferenceExecutionProvider value)
        => opt?.SetOption(ResourceOption.InferenceExecutionProvider, value) ?? throw new ArgumentNullException(nameof(opt));
}
