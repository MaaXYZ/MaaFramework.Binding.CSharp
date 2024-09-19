namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of image information.
/// </summary>
/// <param name="Width">Gets the image width.</param>
/// <param name="Height">Gets the image height.</param>
/// <param name="Channels">Gets the image channels.</param>
/// <param name="Type">Gets the image type.</param>
public sealed record ImageInfo(
    int Width,
    int Height,
    int Channels,
    int Type
);
