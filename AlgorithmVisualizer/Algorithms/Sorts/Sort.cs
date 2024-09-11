using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmVisualizer.Algorithms.Sorts
{
    internal interface ISortStrategy
    {
        Task Sort(int[] array, MainWindow mainWindow, CancellationToken token);
    }
}
