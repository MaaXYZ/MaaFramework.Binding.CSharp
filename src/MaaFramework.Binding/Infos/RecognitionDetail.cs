using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A class providing properties of recognition detail.
/// </summary>
/// <typeparam name="T">The implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
public sealed class RecognitionDetail<T> : IDisposable where T : IMaaImageBuffer, new()
{
    private bool _disposed;

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        HitBox?.Dispose();
        Raw.Dispose();
        Draws.Dispose();
    }

    /// <summary>
    ///     Gets or initializes the recognition id.
    /// </summary>
    /// <remarks>
    ///     From <see cref="NodeDetail.RecognitionId"/>.
    /// </remarks>
    public required MaaRecoId Id { get; init; }

    /// <summary>
    ///     Gets or initializes the recognition name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     Gets or initializes the hit box.
    /// </summary>
    /// <remarks>
    ///     Not hits if HitBox is <see langword="null"/>.
    /// </remarks>
    public required IMaaRectBuffer? HitBox { get; init; }

    /// <summary>
    ///     Gets or initializes the recognition detail.
    /// </summary>
    public required string Detail { get; init; }

    /// <summary>
    ///     Gets or initializes the raw image on the recognition completing.
    /// </summary>
    /// <remarks>
    ///     Sets <see cref="GlobalOption.DebugMessage"/> to true in <see cref="IMaaUtility"/> to get this.
    /// </remarks>
    public required T Raw { get; init; }

    /// <summary>
    ///     Gets or initializes the draw images on the recognition completed.
    /// </summary>
    /// <remarks>
    ///     Sets <see cref="GlobalOption.DebugMessage"/> to true in <see cref="IMaaUtility"/> to get this.
    /// </remarks>
    public required IMaaList<T> Draws { get; init; }

    /// <summary>
    ///     Queries the recognition detail.
    /// </summary>
    /// <typeparam name="TRect">The implemented type of <see cref="IMaaRectBuffer"/>.</typeparam>
    /// <typeparam name="TImage">The implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
    /// <typeparam name="TImageList">The implemented type of <see cref="IMaaList&lt;TImage&gt;"/>.</typeparam>
    /// <param name="recognitionId">The recognition id.</param>
    /// <param name="maa">The maa utility.</param>
    /// <returns>A <see cref="RecognitionDetail&lt;T&gt;"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    public static RecognitionDetail<TImage>? Query<TRect, TImage, TImageList>(MaaRecoId recognitionId, IMaaUtility maa)
        where TRect : IMaaRectBuffer, new()
        where TImage : IMaaImageBuffer, new()
        where TImageList : IMaaList<TImage>, new()
    {
        ArgumentNullException.ThrowIfNull(maa);

        var hitBox = new TRect();
        var raw = new TImage();
        var draws = new TImageList();
        if (!maa.QueryRecognitionDetail(recognitionId, out var name, out var hit, hitBox, out var detail, raw, draws))
            return null;

        return new RecognitionDetail<TImage>
        {
            Id = recognitionId,
            Name = name,
            HitBox = hit ? hitBox : null,
            Detail = detail,
            Raw = raw,
            Draws = draws,
        };
    }
}
