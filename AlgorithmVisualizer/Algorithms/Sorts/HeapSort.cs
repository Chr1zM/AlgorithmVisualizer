using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class HeapSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            var n = array.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
            {
                await Heapify(array, n, i, mainWindow, token);
            }

            for (int i = n - 1; i > 0; i--)
            {
                MainWindow.Swap(array, 0, i);
                await mainWindow.VisualizeArray(array);
                await Heapify(array, i, 0, mainWindow, token);
            }
        }

        private async Task Heapify(int[] array, int n, int i, MainWindow mainWindow, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var largest = i;
            var left = 2 * i + 1;
            var right = 2 * i + 2;

            if (left < n && array[left] > array[largest])
            {
                largest = left;
            }

            if (right < n && array[right] > array[largest])
            {
                largest = right;
            }

            if (largest == i) return;

            MainWindow.Swap(array, i, largest);
            await mainWindow.VisualizeArray(array);
            await Heapify(array, n, largest, mainWindow, token);
        }

    }
}
