using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.Lib.TaskLib;

namespace DotNetCore.MainConsole
{
    class Program
    {
        private static CancelLib lib = new CancelLib();

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //CancelTest();
            //TestParallel();
            TestBloking();
        }

        static void TestBloking()
        {
            var  tasklib=new TaskLib();
            //tasklib.BlockTest2();
            //Task.Run(async () => { await tasklib.BlockTest(); }).ConfigureAwait(false).GetAwaiter().GetResult();
            Task.Run(async () => { await tasklib.SemaphoreSlimTest(); }).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static void TestParallel()
        {
            var token = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            lib.IssueParallelCancel(Enumerable.Range(1, 100), token.Token);
        }

        /// <summary>
        /// 1s后会超时
        /// </summary>
        static void CancelTest()
        {
            var token = new CancellationTokenSource();
            Task.Run(async () =>
            {
                token.CancelAfter(TimeSpan.FromSeconds(1));
                await lib.IssueCancelRequestAsync(TimeSpan.FromSeconds(2), token.Token);
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}