using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.Lib.TaskLib
{
    public class ParallelLib
    {
        public ConcurrentDictionary<int, string> ParallelSetDict(int max)
        {
            ConcurrentDictionary<int, string> dict = new ConcurrentDictionary<int, string>();
            CancellationTokenSource token = new CancellationTokenSource();
            Parallel.ForEach(Enumerable.Range(0, max), new ParallelOptions()
            {
                CancellationToken = token.Token
            }, item =>
            {
                if (!dict.TryAdd(item, item.ToString()))
                {
                    token.Cancel();
                }
            });
            return dict;
        }
    }
}