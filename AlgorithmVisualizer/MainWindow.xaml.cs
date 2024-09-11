using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlgorithmVisualizer.Algorithms.Sorts;

namespace AlgorithmVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var array = GenerateRandomArray(100);

            // Ausgewählter Algorithmus aus der ComboBox
            var selectedAlgorithm = (AlgorithmSelection.SelectedItem as ComboBoxItem)?.Content.ToString();

            var sortStrategy = GetSortStrategy(selectedAlgorithm);

            if (sortStrategy is null)
            {
                MessageBox.Show(
                    "Bitte wählen Sie einen Algorithmus aus.",
                    "Fehler",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await sortStrategy.Sort(array, this, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(
                    "Der Algorithmus wurde abgebrochen.",
                    "Abgebrochen",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }

        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }


        private static ISortStrategy? GetSortStrategy(string? selectedAlgorithm)
        {
            return selectedAlgorithm switch
            {
                "QuickSort" => new QuickSort(),
                "MergeSort" => new MergeSort(),
                "BubbleSort" => new BubbleSort(),
                "SelectionSort" => new SelectionSort(),
                "InsertionSort" => new InsertionSort(),
                "HeapSort" => new HeapSort(),
                "BucketSort" => new BucketSort(),
                "BogoSort" => new BogoSort(),
                _ => null
            };
        }

        private int[] GenerateRandomArray(int size)
        {
            var rand = new Random();
            var array = new int[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = rand.Next(1, 170); // Werte zwischen 1 und 100
            }
            return array;
        }

        public static void Swap(IList<int> array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }

        public async Task VisualizeArray(IReadOnlyList<int> array)
        {
            // Hier wird die Visualisierung aktualisiert
            VisualizationCanvas.Children.Clear();

            var barWidth = (int)VisualizationCanvas.ActualWidth / array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                var rect = new Rectangle
                {
                    Height = array[i] * 2, // Annahme: Die Höhe wird als 2 * Wert des Elements gesetzt
                    Width = barWidth - 2, // Abstand zwischen den Balken
                    Fill = Brushes.Blue
                };
                Canvas.SetLeft(rect, i * barWidth);
                Canvas.SetBottom(rect, 0);
                VisualizationCanvas.Children.Add(rect);
            }
            await Task.Delay(50); // 200ms Pause nach jedem Schritt
        }
    }
}