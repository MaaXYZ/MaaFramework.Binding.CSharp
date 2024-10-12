using MaaFramework.Binding.Buffers;
using MaaFramework.Binding.Custom;

namespace MaaFramework.Binding.Interop.Native;

/// <summary>
///     A static class providing extension methods for converting <see cref="IMaaCustomResource"/> to custom callback delegate used in interop invoking.
/// </summary>
public static class IMaaCustomResourceExtension
{
    /// <summary>
    ///     Converts a <see cref="IMaaCustomAction"/> to a <see cref="MaaCustomActionCallback"/>.
    /// </summary>
    /// <param name="resource">The custom action.</param>
    /// <param name="callback">The callback.</param>
    /// <returns>The callback.</returns>
    /// <exception cref="InvalidOperationException">Failed to query detail.</exception>
    public static MaaCustomActionCallback Convert(this IMaaCustomAction resource, out MaaCustomActionCallback callback)
    {
        callback =
            (
                MaaContextHandle contextHandle,
                MaaTaskId taskId,
                string currentTaskName,
                string customActionName,
                string customActionParam,
                MaaRecoId recoId,
                MaaRectHandle boxHandle,
                nint transArg
            ) =>
            {
                var context = new Binding.MaaContext(contextHandle);
                var tasker = context.Tasker;
                var taskDetail = TaskDetail.Query(taskId, tasker) ?? throw new InvalidOperationException();
                var recognitionDetail = RecognitionDetail<MaaImageBuffer>.Query<MaaRectBuffer, MaaImageBuffer, MaaImageListBuffer>(recoId, tasker) ?? throw new InvalidOperationException();
                return resource.Run
                (
                    context,
                    new RunArgs<MaaImageBuffer>
                    (
                        TaskDetail: taskDetail,
                        TaskName: currentTaskName,
                        ActionName: customActionName,
                        ActionParam: customActionParam,
                        RecognitionDetail: recognitionDetail,
                        RecognitionBox: new MaaRectBuffer(boxHandle)
                    )
                );
            };
        return callback;
    }

    /// <summary>
    ///     Converts a <see cref="IMaaCustomRecognition"/> to a <see cref="MaaCustomRecognitionCallback"/>.
    /// </summary>
    /// <param name="resource">The custom recognition.</param>
    /// <param name="callback"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static MaaCustomRecognitionCallback Convert(this IMaaCustomRecognition resource, out MaaCustomRecognitionCallback callback)
    {
        callback =
            (
                MaaContextHandle contextHandle,
                MaaTaskId taskId,
                string currentTaskName,
                string customRecognitionName,
                string customRecognitionParam,
                MaaImageBufferHandle imageHandle,
                MaaRectHandle roiHandle,
                nint transArg,
                MaaRectHandle outBoxHandle,
                MaaStringBufferHandle outDetailHandle
            ) =>
            {
                var context = new Binding.MaaContext(contextHandle);
                var tasker = context.Tasker;
                var taskDetail = TaskDetail.Query(taskId, tasker) ?? throw new InvalidOperationException();
                return resource.Analyze
                (
                    context,
                    new AnalyzeArgs
                    (
                        TaskDetail: taskDetail,
                        TaskName: currentTaskName,
                        RecognitionName: customRecognitionName,
                        RecognitionParam: customRecognitionParam,
                        Image: new MaaImageBuffer(imageHandle),
                        Roi: new MaaRectBuffer(roiHandle)
                    ),
                    new AnalyzeResults
                    (
                        Box: new MaaRectBuffer(outBoxHandle),
                        Detail: new MaaStringBuffer(outDetailHandle)
                    )
                );
            };
        return callback;
    }
}
