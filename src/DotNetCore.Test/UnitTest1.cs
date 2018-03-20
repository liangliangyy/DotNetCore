using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DotNetCore.Lib.TaskLib;

namespace DotNetCore.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ParallelLib lib = new ParallelLib();
            var result = lib.ParallelSetDict(20000);
            Assert.Equal(result.Count, 20000);
        }

        [Fact]
        public void Test2()
        {
            TaskLib lib = new TaskLib();
            Task.Factory.StartNew(async () =>
            {
                var result = await lib.LockTest();
                Assert.Equal(result.Count, 20000);
                Assert.Equal(result.Distinct().Count(), 20000);
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}