namespace MaaFramework.Binding;

/// <summary>
///     A readonly record struct providing properties of image information.
/// </summary>
/// <param name="Width">Gets the image width.</param>
/// <param name="Height">Gets the image height.</param>
/// <param name="Channels">Gets the image channels.</param>
/// <param name="Type">Gets the type of a image matrix element.
///     <para> This is an identifier compatible with the CvMat type system, like CV_16SC3 or 16-bit signed 3-channel array, and so on.</para>
/// </param>
public readonly record struct ImageInfo(
    int Width,
    int Height,
    int Channels,
    int Type
);
