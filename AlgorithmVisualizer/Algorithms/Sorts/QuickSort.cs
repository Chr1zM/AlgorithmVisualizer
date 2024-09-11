using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class QuickSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            await Sort(array, 0, array.Length - 1, mainWindow, token);
        }

        public async Task Sort(int[] array, int left, int right, MainWindow mainWindow, CancellationToken token)
        {
            if (left < right)
            {
                var pivotIndex = await Partition(array, left, right, mainWindow, token);
                await Sort(array, left, pivotIndex - 1, mainWindow, token);
                await Sort(array, pivotIndex + 1, right, mainWindow, token);
            }
        }

        private async Task<int> Partition(int[] array, int left, int right, MainWindow mainWindow, CancellationToken token)
        {
            var pivot = array[right];
            var i = left - 1;

            for (int j = left; j < right; j++)
            {
                token.ThrowIfCancellationRequested();

                if (array[j] >= pivot) continue;

                i++;
                MainWindow.Swap(array, i, j);
                await mainWindow.VisualizeArray(array); // Zeige den aktuellen Stand nach jedem Schritt
            }

            MainWindow.Swap(array, i + 1, right);
            await mainWindow.VisualizeArray(array); // Zeige den Stand nach dem Pivot-Wechsel
            return i + 1;
        }
    }
}
