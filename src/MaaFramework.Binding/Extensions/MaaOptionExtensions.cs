using MaaFramework.Binding.Abstractions;

namespace MaaFramework.Binding;

#pragma warning disable S4136

/// <summary>
///     A static class providing extension methods for setting maa option.
/// </summary>
public static class MaaOptionExtensions
{
    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionScreenshotTargetLongSide(this IMaaOption<ControllerOption>? opt, int value)
        => opt?.SetOption(ControllerOption.ScreenshotTargetLongSide, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionScreenshotTargetShortSide(this IMaaOption<ControllerOption>? opt, int value)
        => opt?.SetOption(ControllerOption.ScreenshotTargetShortSide, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionRecording(this IMaaOption<ControllerOption>? opt, bool value)
        => opt?.SetOption(ControllerOption.Recording, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionLogDir(this IMaaOption<GlobalOption>? opt, string value)
        => opt?.SetOption(GlobalOption.LogDir, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionSaveDraw(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.SaveDraw, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionRecording(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.Recording, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionStdoutLevel(this IMaaOption<GlobalOption>? opt, LoggingLevel value)
        => opt?.SetOption(GlobalOption.StdoutLevel, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionShowHitDraw(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.ShowHitDraw, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionDebugMode(this IMaaOption<GlobalOption>? opt, bool value)
        => opt?.SetOption(GlobalOption.DebugMode, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionInferenceDevice(this IMaaOption<ResourceOption>? opt, InferenceDevice value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));

    /// <inheritdoc cref="IMaaOption{T}.SetOption{T}"/>
    public static bool SetOptionInferenceDevice(this IMaaOption<ResourceOption>? opt, int value)
        => opt?.SetOption(ResourceOption.InferenceDevice, value) ?? throw new ArgumentNullException(nameof(opt));
}
