using TomLonghurst.EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors.Abstract;

namespace TomLonghurst.EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors;

public class ResultRateLimitedParallelAsyncProcessor<TInput, TOutput> : ResultAbstractAsyncProcessor<TInput, TOutput>
{
    private readonly int _levelsOfParallelism;
    
    public ResultRateLimitedParallelAsyncProcessor(IReadOnlyCollection<TInput> items, Func<TInput, Task<TOutput>> taskSelector, int levelsOfParallelism, CancellationTokenSource cancellationTokenSource) : base(items, taskSelector, cancellationTokenSource)
    {
        _levelsOfParallelism = levelsOfParallelism;
    }

    internal override Task Process()
    {
        return Parallel.ForEachAsync(ItemisedTaskCompletionSourceContainers,
            new ParallelOptions { MaxDegreeOfParallelism = _levelsOfParallelism, CancellationToken = CancellationToken},
            async (itemisedTaskCompletionSourceContainer, _) =>
            {
                await ProcessItem(itemisedTaskCompletionSourceContainer);
            });
    }
}