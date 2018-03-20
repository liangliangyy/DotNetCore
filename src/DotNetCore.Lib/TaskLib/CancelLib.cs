using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.Lib.TaskLib
{
    public class CancelLib
    {
        public async Task IssueCancelRequestAsync(TimeSpan delay, CancellationToken token)
        {
            await Task.Delay(delay, token);
        }

        public void IssueParallelCancel(IEnumerable<int> numbers, CancellationToken token)
        {
            Parallel.ForEach(numbers,
                new ParallelOptions() {CancellationToken = token, TaskScheduler = TaskScheduler.Default},
                item =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine(item);
                });
        }
    }
}