namespace MaaFramework.Binding;

/// <summary>
///     A class providing properties of image information.
/// </summary>
public sealed class ImageInfo
{
    /// <summary>
    ///     Gets the image width.
    /// </summary>
    public required int Width { get; init; }

    /// <summary>
    ///     Gets the image height.
    /// </summary>
    public required int Height { get; init; }

    /// <summary>
    ///     Gets the image type.
    /// </summary>
    public required int Type { get; init; }
}
