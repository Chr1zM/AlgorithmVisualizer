using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class InsertionSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            var n = array.Length;
            for (int i = 1; i < n; i++)
            {
                var key = array[i];
                var j = i - 1;

                while (j >= 0 && array[j] > key)
                {
                    token.ThrowIfCancellationRequested();

                    array[j + 1] = array[j];
                    j--;
                    await mainWindow.VisualizeArray(array);
                }

                array[j + 1] = key;
                await mainWindow.VisualizeArray(array);
            }
        }
    }
}
