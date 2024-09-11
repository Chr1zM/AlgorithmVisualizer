using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class MergeSort : ISortStrategy
    {
        public Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            return Sort(array, 0, array.Length - 1, mainWindow, token);
        }

        private async Task Sort(int[] array, int left, int right, MainWindow mainWindow, CancellationToken token)
        {
            if (left < right)
            {
                var middle = (left + right) / 2;
                await Sort(array, left, middle, mainWindow, token);
                await Sort(array, middle + 1, right, mainWindow, token);
                await Merge(array, left, middle, right, mainWindow, token);
            }
        }

        private async Task Merge(int[] array, int left, int middle, int right, MainWindow mainWindow, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var n1 = middle - left + 1;
            var n2 = right - middle;

            var leftArray = new int[n1];
            var rightArray = new int[n2];

            for (int i = 0; i < n1; i++)
            {
                leftArray[i] = array[left + i];
            }

            for (int i = 0; i < n2; i++)
            {
                rightArray[i] = array[middle + 1 + i];
            }

            int j = 0, k = 0;
            int l = left;

            while (j < n1 && k < n2)
            {
                token.ThrowIfCancellationRequested();

                if (leftArray[j] <= rightArray[k])
                {
                    array[l] = leftArray[j];
                    j++;
                }
                else
                {
                    array[l] = rightArray[k];
                    k++;
                }
                l++;
                await mainWindow.VisualizeArray(array);
            }

            while (j < n1)
            {
                token.ThrowIfCancellationRequested();

                array[l] = leftArray[j];
                j++;
                l++;
                await mainWindow.VisualizeArray(array);
            }

            while (k < n2)
            {
                token.ThrowIfCancellationRequested();

                array[l] = rightArray[k];
                k++;
                l++;
                await mainWindow.VisualizeArray(array);
            }
        }
    }
}
