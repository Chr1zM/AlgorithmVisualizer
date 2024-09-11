using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class SelectionSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            var n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                var minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    token.ThrowIfCancellationRequested();

                    if (array[j] >= array[minIndex]) continue;

                    minIndex = j;
                    await mainWindow.VisualizeArray(array);
                }

                MainWindow.Swap(array, minIndex, i);
                await mainWindow.VisualizeArray(array);
            }
        }
    }
}
