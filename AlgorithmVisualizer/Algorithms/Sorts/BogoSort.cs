using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal class BogoSort : ISortStrategy
    {
        public async Task Sort(int[] array, MainWindow mainWindow, CancellationToken token)
        {
            while (!IsSorted(array))
            {
                token.ThrowIfCancellationRequested();

                Shuffle(array);
                await mainWindow.VisualizeArray(array);
            }
        }

        private static void Shuffle(int[] array)
        {
            var random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                var randomIndex = random.Next(0, array.Length);
                MainWindow.Swap(array, i, randomIndex);
            }
        }

        private static bool IsSorted(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
