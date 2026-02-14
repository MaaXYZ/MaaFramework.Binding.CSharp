using MaaFramework.Binding.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace MaaFramework.Binding;

/// <inheritdoc/>
public class MaaTaskJob(MaaId id, IMaaTasker tasker, IMaaPost maa) : MaaJob(id, maa)
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Debugger display.")]
    public override string ToString()
    {
        if (Status == MaaJobStatus.Invalid)
            return base.ToString();

        var detail = this.QueryTaskDetail();
        return $"{GetType().Name} {{ {nameof(detail.Status)} = {detail?.Status}, {nameof(detail.Entry)} = {detail?.Entry}, {nameof(Id)} = {Id} }}";
    }

    /// <summary>
    ///     Gets the maa tasker.
    /// </summary>
    /// <remarks>
    ///     A property used to simplify design of <see cref="TaskDetail.Query"/>.
    /// </remarks>
    public IMaaTasker Tasker => tasker;

    /// <summary>
    ///     Waits for a <see cref="MaaTaskJob"/> to complete with an expected <paramref name="status"/>.
    /// </summary>
    /// <param name="status">The expected status.</param>
    /// <returns><see langword="this"/> if it completes with the <paramref name="status"/>; otherwise, <see langword="null"/>.</returns>
    public MaaTaskJob? WaitFor(MaaJobStatus status)
        => Wait() == status ? this : null;

    /// <summary>
    ///     Overrides pipeline for this task, dynamically modifies pipeline configuration during task execution.
    /// </summary>
    /// <param name="pipelineOverride">The json used to override the pipeline.</param>
    /// <returns></returns>
    public bool OverridePipeline([StringSyntax("Json")] string pipelineOverride)
        => tasker.OverridePipeline(this, pipelineOverride);
}
