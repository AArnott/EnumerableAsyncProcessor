using TomLonghurst.EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors.Abstract;

namespace TomLonghurst.EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors;

public class ResultOneAtATimeAsyncProcessor<TResult> : ResultAbstractAsyncProcessor<TResult>
{
    public ResultOneAtATimeAsyncProcessor(int count, Func<Task<TResult>> taskSelector, CancellationTokenSource cancellationTokenSource) : base(count, taskSelector, cancellationTokenSource)
    {
    }

    internal override async Task Process()
    {
        foreach (var taskCompletionSource in EnumerableTaskCompletionSources)
        {
            await ProcessItem(taskCompletionSource);
        }
    }
}