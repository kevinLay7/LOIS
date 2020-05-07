using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LOIS.ProcessingEngine
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fileChooser = new Microsoft.Win32.OpenFileDialog();
            fileChooser.Filter = "PDF |*.pdf";
            var file = fileChooser.ShowDialog();

            if (file.HasValue && file == true)
            {
                var converter = new LOIS.BLL.Imaging.PdfConverter();
                var images = converter.Test(fileChooser.FileName);

                if (images != null)
                {
                    var img = images;
                    using (var ms = new MemoryStream(img))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad; // here
                        image.StreamSource = ms;
                        image.EndInit();
                        PdfImageControl.Source = image;
                        this.Height = image.Height + 60;
                        this.Width = image.Width + 20;
                        PdfImageControl.Width = image.Width;
                        PdfImageControl.Height = image.Height;
                        this.ResizeMode = ResizeMode.NoResize;

                        imageWidth = image.Width;
                        imageHeight = image.Height;

                        rect = new Rectangle()
                        {
                            Height = Properties.Settings.Default.rectH,
                            Width = Properties.Settings.Default.rectW,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2
                        };
                        Canvas.SetLeft(rect, Properties.Settings.Default.startPointX);
                        Canvas.SetTop(rect, Properties.Settings.Default.startPointY);
                        OverlayCanvas.Children.Add(rect);
                        savedRect = rect;
                    }
                }
            }
        }

        private Point startPoint;
        private Rectangle rect;
        private Rectangle savedRect;
        private double imageWidth;
        private double imageHeight;

        private void OverlayCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (OverlayCanvas.Children.Count > 0)
            {
                for (int i = 0; i < OverlayCanvas.Children.Count; i++)
                {
                    OverlayCanvas.Children.Remove(OverlayCanvas.Children[i] as UIElement);
                }
            }

            startPoint = e.GetPosition(OverlayCanvas);

            rect = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            Canvas.SetLeft(rect, startPoint.X);
            Canvas.SetTop(rect, startPoint.Y);
            OverlayCanvas.Children.Add(rect);

        }

        private void OverlayCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || rect == null)
                return;

            var pos = e.GetPosition(OverlayCanvas);

            var x = Math.Min(pos.X, startPoint.X);
            var y = Math.Min(pos.Y, startPoint.Y);

            var w = Math.Max(pos.X, startPoint.X) - x;
            var h = Math.Max(pos.Y, startPoint.Y) - y;

            rect.Width = w;
            rect.Height = h;

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            savedRect = rect;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var spW = startPoint.X / imageWidth;
            var spH = startPoint.Y / imageHeight;
            var calcWidth = savedRect.Width / imageWidth;
            var calcHeight = savedRect.Height / imageHeight;

            Properties.Settings.Default.rectW = savedRect.Width;
            Properties.Settings.Default.rectH = savedRect.Height;
            Properties.Settings.Default.startPointX = startPoint.X;
            Properties.Settings.Default.startPointY = startPoint.Y;
            Properties.Settings.Default.refImageHeight = (int)imageHeight;
            Properties.Settings.Default.refImageWidth = (int)imageWidth;

            Properties.Settings.Default.Save();
        }


        private void OverlayCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
