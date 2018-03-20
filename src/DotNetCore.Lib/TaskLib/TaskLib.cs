using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.Lib.TaskLib
{
    public class TaskLib
    {
        public async Task BlockTest()
        {
            BlockingCollection<string> block = new BlockingCollection<string>();
            var maxnumbers = new int[] {100, 300, 500, 1000};
            var tasks = maxnumbers.Select(i =>
            {
                return Task.Factory.StartNew(() => AddTestItemToBlock(i, i.ToString(), block));
            });
            var s = tasks.ToList();
            s.Add(Task.Factory.StartNew(() =>
            {
                foreach (var item in block.GetConsumingEnumerable())
                {
                    Console.WriteLine(item);
                }
            }));
            await Task.WhenAll(s);
        }

        private void AddTestItemToBlock(int max, string prefix, BlockingCollection<string> block)
        {
            for (int i = 0; i < max; i++)
            {
                block.Add($"{prefix}-{i}");
            }

            //block.CompleteAdding();
        }


        public void BlockTest2()
        {
            BlockingCollection<string> block = new BlockingCollection<string>();
            ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
            var t = new Task[50];
            for (int i = 0; i <= 49; i++)
            {
                t[i] = Task.Factory.StartNew((obj) =>
                    {
                        Thread.Sleep(200);
                        block.Add(obj.ToString());
                        queue.Enqueue(obj.ToString());
                        Console.WriteLine("Task中的数据: {0}", obj.ToString());
                    }, i + 1);
            }

            Thread.Sleep(5000);
            while (!block.IsCompleted)
            {
                foreach (var b in block.GetConsumingEnumerable())
                {
                    Console.WriteLine("开始输出阻塞队列: {0}", b);
                    Console.WriteLine(block.IsCompleted);
                    Console.WriteLine("并发队列的数量: {0}", queue.Count);
                    if (queue.Count == 50)
                    {
                        block.CompleteAdding();
                    }
                }

                Console.WriteLine("调用GetConsumingEnumerable方法遍历完之后阻塞队列的数量: {0}", block.Count);
            }

            Console.WriteLine("********");

            Console.WriteLine("是否完成添加: {0}", block.IsAddingCompleted);
        }


        public async Task<List<int>> LockTest()
        {
            ManualResetEventSlim slim = new ManualResetEventSlim();
            List<int> list = new List<int>();
            var tasks = Enumerable.Range(0, 10000).Select(i =>
            {
                return Task.Factory.StartNew(() =>
                {
                    slim.Wait();
                    list.Add(i);
                    slim.Set();
                });
            });
            await Task.WhenAll(tasks);
            return list;
        }

        public async Task SemaphoreSlimTest()
        {
            var numbers = Enumerable.Range(0, 1000);
            SemaphoreSlim semaphoreSlim = new SemaphoreSlim(10);
            var tasks = numbers.Select(async x =>
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    var id = Thread.CurrentThread.ManagedThreadId;
                    Console.WriteLine($"item:{x},threadid:{id}");
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}