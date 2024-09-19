using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of recognition detail.
/// </summary>
/// <typeparam name="T">The implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
/// <param name="Id">Gets the recognition id.</param>
/// <param name="Name">Gets the recognition name.</param>
/// <param name="Algorithm">Gets the algorithm name of the recognition.</param>
/// <param name="HitBox">Gets the hit box if hit; otherwise <see langword="null"/>.</param>
/// <param name="Detail">Gets the recognition detail.</param>
/// <param name="Raw">Gets the raw image on the recognition completing if in debug mode; otherwise <see langword="null"/>.</param>
/// <param name="Draws">Gets the draw images on the recognition completed if in debug mode; otherwise <see langword="null"/>.</param>
public sealed record RecognitionDetail<T>(
    MaaRecoId Id,
    string Name,
    string Algorithm,
    IMaaRectBuffer? HitBox,
    string Detail,
    T? Raw,
    IMaaListBuffer<T>? Draws
) : IDisposable where T : IMaaImageBuffer
{
    private bool _disposed;

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        HitBox?.Dispose();
        Raw?.Dispose();
        Draws?.Dispose();
    }

    /// <summary>
    ///     Queries the recognition detail.
    /// </summary>
    /// <typeparam name="TRect">The implemented type of <see cref="IMaaRectBuffer"/>.</typeparam>
    /// <typeparam name="TImage">The implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
    /// <typeparam name="TImageList">The implemented type of <see cref="IMaaListBuffer{T}"/>.</typeparam>
    /// <param name="recognitionId">The recognition id from <see cref="NodeDetail.RecognitionId"/>..</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <returns>A <see cref="RecognitionDetail{T}"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    public static RecognitionDetail<TImage>? Query<TRect, TImage, TImageList>(MaaRecoId recognitionId, IMaaTasker tasker)
        where TRect : IMaaRectBuffer, new()
        where TImage : IMaaImageBuffer, new()
        where TImageList : IMaaListBuffer<TImage>, new()
    {
        ArgumentNullException.ThrowIfNull(tasker);

        var hitBox = new TRect();
        var raw = new TImage();
        var draws = new TImageList();
        if (!tasker.GetRecognitionDetail(recognitionId, out var name, out var algorithm, out var hit, hitBox, out var detail, raw, draws))
        {
            hitBox.Dispose();
            raw.Dispose();
            draws.Dispose();
            return null;
        }

        if (!hit)
        {
            hitBox.Dispose();
            hitBox = default;
        }
        if (raw.IsEmpty)
        {
            raw.Dispose();
            raw = default;
        }
        if (draws.IsEmpty)
        {
            draws.Dispose();
            draws = default;
        }

        return new RecognitionDetail<TImage>(
            Id: recognitionId,
            Name: name,
            Algorithm: algorithm,
            HitBox: hitBox,
            Detail: detail,
            Raw: raw,
            Draws: draws
        );
    }
}
