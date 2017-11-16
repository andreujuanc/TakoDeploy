using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployLib.Model
{
    public static class ParallelExtensions
    {
        ///https://stackoverflow.com/questions/11564506/nesting-await-in-parallel-foreach
        public static Task ForEachAsync<T>(
          this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current).ContinueWith(t =>
                            {
                                //observe exceptions

                            });

                }));
        }
    }
}
