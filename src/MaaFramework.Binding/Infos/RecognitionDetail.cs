using MaaFramework.Binding.Abstractions;
using MaaFramework.Binding.Buffers;

namespace MaaFramework.Binding;

/// <summary>
///     A sealed record providing properties of recognition detail.
/// </summary>
/// <param name="Id">Gets the recognition id.</param>
/// <param name="NodeName">Gets the node name.</param>
/// <param name="Algorithm">Gets the algorithm name of the recognition.</param>
/// <param name="Hit">Gets a value indicates whether the recognition is hit.</param>
/// <param name="HitBox">Gets the hit box if hit; otherwise <see langword="null"/>.</param>
/// <param name="Detail">Gets the recognition detail.</param>
/// <param name="Raw">Gets the raw image on the recognition completing if in debug mode; otherwise <see langword="null"/>.</param>
/// <param name="Draws">Gets the draw images on the recognition completed if in debug mode; otherwise <see langword="null"/>.</param>
public sealed record RecognitionDetail(
    MaaRecoId Id,
    string NodeName,
    string Algorithm,
    bool Hit,
    IMaaRectBuffer? HitBox,
    string Detail,
    MaaImage? Raw,
    IList<MaaImage>? Draws
) : IDisposable
{
    private bool _disposed;
    private IMaaDisposable? _draws;

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;

        HitBox?.Dispose();
        Raw?.Dispose();
        _draws?.Dispose();
    }

    /// <summary>
    ///     Queries the recognition detail.
    /// </summary>
    /// <typeparam name="TRect">The implemented type of <see cref="IMaaRectBuffer"/>.</typeparam>
    /// <typeparam name="TImage">The implemented type of <see cref="IMaaImageBuffer"/>.</typeparam>
    /// <typeparam name="TImageList">The implemented type of <see cref="IMaaListBuffer{T}"/>.</typeparam>
    /// <param name="recognitionId">The recognition id from <see cref="NodeDetail.RecognitionId"/>..</param>
    /// <param name="tasker">The maa tasker.</param>
    /// <returns>A <see cref="RecognitionDetail"/> if query was successful; otherwise, <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"/>
    public static RecognitionDetail? Query<TRect, TImage, TImageList>(MaaRecoId recognitionId, IMaaTasker tasker)
        where TRect : IMaaRectBuffer, new()
        where TImage : IMaaImageBuffer, new()
        where TImageList : IMaaListBuffer<TImage>, new()
    {
        ArgumentNullException.ThrowIfNull(tasker);

        var hitBox = new TRect();
        var raw = new TImage();
        var draws = new TImageList();
        if (!tasker.GetRecognitionDetail(recognitionId, out var nodeName, out var algorithm, out var hit, hitBox, out var detail, raw, draws))
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

#pragma warning disable CA2000
        return new RecognitionDetail(
            Id: recognitionId,
            NodeName: nodeName,
            Algorithm: algorithm,
            Hit: hit,
            HitBox: hitBox,
            Detail: detail,
            Raw: raw is null ? null : MaaImage.Load(raw),
            Draws: draws?.Select(static x => MaaImage.Load(x)).ToList()
        )
        {
            _draws = draws,
        };
#pragma warning restore CA2000
    }
}
