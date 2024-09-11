using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class BucketSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            var n = array.Length;
            var buckets = new List<int>[n];
            var max = array.Max();
            var min = array.Min();
            var bucketSize = (max - min) / n + 1;

            for (int i = 0; i < n; i++)
            {
                var bucketIndex = (array[i] - min) / bucketSize;
                if (buckets[bucketIndex] is null)
                {
                    buckets[bucketIndex] = new List<int>();
                }
                buckets[bucketIndex].Add(array[i]);
            }

            var index = 0;
            for (int i = 0; i < n; i++)
            {
                token.ThrowIfCancellationRequested();

                if (buckets[i] is null) continue;

                buckets[i].Sort();
                foreach (var item in buckets[i])
                {
                    token.ThrowIfCancellationRequested();

                    array[index++] = item;
                    await mainWindow.VisualizeArray(array);
                }
            }
        }
    }
}
