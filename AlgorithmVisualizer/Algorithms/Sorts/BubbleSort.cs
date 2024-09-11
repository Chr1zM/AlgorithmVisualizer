using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class BubbleSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            var n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    token.ThrowIfCancellationRequested();

                    if (array[j] <= array[j + 1]) continue;

                    MainWindow.Swap(array, j, j + 1);
                    await mainWindow.VisualizeArray(array);
                }
            }
        }
    }
}
