﻿
using EnumerableAsyncProcessor.Extensions;
using EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors.Abstract;

namespace EnumerableAsyncProcessor.RunnableProcessors.ResultProcessors;

public class ResultRateLimitedParallelAsyncProcessor<TOutput> : ResultAbstractAsyncProcessor<TOutput>
{
    private readonly int _levelsOfParallelism;

    internal ResultRateLimitedParallelAsyncProcessor(int count, Func<Task<TOutput>> taskSelector, int levelsOfParallelism, CancellationTokenSource cancellationTokenSource) : base(count, taskSelector, cancellationTokenSource)
    {
        _levelsOfParallelism = levelsOfParallelism;
    }

    internal override Task Process()
    {
        return EnumerableTaskCompletionSources.InParallelAsync(_levelsOfParallelism, 
            async taskCompletionSource =>
            {
                await Task.Run(() => ProcessItem(taskCompletionSource));
            });
    }
}