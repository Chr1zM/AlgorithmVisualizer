using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AlgorithmVisualizer.Algorithms.Sorts;
using Path = System.IO.Path;
using Brushes = System.Windows.Media.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Drawing;
using Accord.Video.FFMPEG;

namespace AlgorithmVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cancellationTokenSource;

        private string _imageDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "CanvasFrames");

        private bool _isFinished = false;

        private bool _isRecording = true; // TODO Checkbox if recording is enabled
        private int _frameIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _isFinished = false;
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

            if (_isRecording)
            {
                Directory.CreateDirectory(_imageDirectory);
            }

            try
            {
                await sortStrategy.Sort(array, this, _cancellationTokenSource.Token);
                _isFinished = true;
                await VisualizeArray(array);
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

            if (_isRecording)
            {
                CreateVideoFromImages(_imageDirectory, "output.mp4");

                try
                {
                    Directory.Delete(_imageDirectory, true);
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        "Der Image Ordner im Dokumente-Verzeichnis konnte nicht gelöscht werden.",
                        "Fehler",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }

                _frameIndex = 0;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();

            if (_isRecording)
            {
                Directory.Delete(_imageDirectory, true);
                _frameIndex = 0;
            }
        }

        private static ISortStrategy GetSortStrategy(string selectedAlgorithm)
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
                    Fill = _isFinished ? Brushes.Green : Brushes.Blue
                };
                Canvas.SetLeft(rect, i * barWidth);
                Canvas.SetBottom(rect, 0);
                VisualizationCanvas.Children.Add(rect);
            }

            if (_isRecording)
            {
                var imagePath = Path.Combine(_imageDirectory, $"frame{_frameIndex:D4}.png");
                SaveCanvasAsImage(VisualizationCanvas, imagePath);
                _frameIndex++;
            }

            await Task.Delay(100 / (int)SpeedSlider.Value); // Pause nach jedem Schritt
        }

        private void SaveCanvasAsImage(Canvas canvas, string filePath)
        {
            canvas.UpdateLayout();
            var width = (int)canvas.ActualWidth;
            var height = (int)canvas.ActualHeight;
            var renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);

            renderTargetBitmap.Render(canvas);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using var fileStream = new FileStream(filePath, FileMode.Create);
            encoder.Save(fileStream);
        }

        private void CreateVideoFromImages(string imageDirectory, string outputFileName)
        {
            var imageFiles = Directory.GetFiles(imageDirectory, "*.png");

            if (imageFiles.Length == 0)
            {
                MessageBox.Show("Keine Bilder zum Erstellen eines Videos gefunden.");
                return;
            }

            using var firstImage = new Bitmap(imageFiles[0]);
            var width = firstImage.Width % 2 == 0 ? firstImage.Width : firstImage.Width + 1;
            var height = firstImage.Height % 2 == 0 ? firstImage.Height : firstImage.Height + 1;

            using var videoWriter = new VideoFileWriter();
            videoWriter.Open(outputFileName, width, height, 20, VideoCodec.MPEG4);

            foreach (var imageFile in imageFiles)
            {
                using var bitmap = new Bitmap(imageFile);
                if (bitmap.Width != width || bitmap.Height != height)
                {
                    using var resizedBitmap = new Bitmap(width, height);
                    using var graphics = Graphics.FromImage(resizedBitmap);

                    graphics.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, width, height));
                    videoWriter.WriteVideoFrame(resizedBitmap);
                }
                else
                {
                    videoWriter.WriteVideoFrame(bitmap);
                }
            }

            videoWriter.Close();
        }
    }
}