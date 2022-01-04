﻿namespace TomLonghurst.EnumerableAsyncProcessor.RunnableProcessors;

public class ParallelAsyncProcessor<TResult> : AbstractAsyncProcessor<TResult>
{
    private Task _totalProgressTask;

    public ParallelAsyncProcessor(List<Task<Task<TResult>>> initialTasks, CancellationToken cancellationToken) : base(initialTasks, cancellationToken)
    {
        cancellationToken.Register(() => initialTasks.ForEach(x => x.Dispose()));
    }

    internal override Task Process()
    {
        return _totalProgressTask = Parallel.ForEachAsync(InitialTasks,
            new ParallelOptions { MaxDegreeOfParallelism = -1, CancellationToken = CancellationToken },
            async (task, token) =>
            {
                task.Start();
                await task.Unwrap();
            });
    }

    public override Task ContinuationTask => _totalProgressTask;
}

public class ParallelAsyncProcessor : AbstractAsyncProcessor
{
    private Task _totalProgressTask;

    public ParallelAsyncProcessor(List<Task<Task>> initialTasks, CancellationToken cancellationToken) : base(initialTasks, cancellationToken)
    {
        cancellationToken.Register(() => initialTasks.ForEach(x => x.Dispose()));
    }

    internal override Task Process()
    {
        return _totalProgressTask = Parallel.ForEachAsync(InitialTasks,
            new ParallelOptions { MaxDegreeOfParallelism = -1, CancellationToken = CancellationToken },
            async (task, token) =>
            {
                task.Start();
                await task.Unwrap();
            });
    }

    public override Task Task => _totalProgressTask;
}