using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace QFP.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RenderTargetBitmap _bitmap;

        private bool _isDrawMode = true;

        private SolidColorBrush _currentBrush = Brushes.Black;

        public MainWindow()
        {
            InitializeComponent();

            int width = 128;
            int height = width;

            _bitmap = new RenderTargetBitmap(512, 512, 72, 72, PixelFormats.Pbgra32);

            var visual = new DrawingVisual();

            using (var r = visual.RenderOpen())
            {
                r.DrawEllipse(Brushes.White, null, new Point(0, 0), width * width, height * height);
            }

            _bitmap.Render(visual);

            CanvasImage.Source = _bitmap;

            UpdatePickedColor();
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            var cursor = _isDrawMode ? Cursors.Pen : Cursors.Hand;
            Cursor = cursor;
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CanvasImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CanvasImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var controlPosition = e.GetPosition(CanvasImage);
                var imageControl = CanvasImage;
                var x = Math.Floor(controlPosition.X * imageControl.Source.Width / imageControl.ActualWidth);
                var y = Math.Floor(controlPosition.Y * imageControl.Source.Height / imageControl.ActualHeight);
                var point = new Point(x, y);

                var visual = new DrawingVisual();

                using (var r = visual.RenderOpen())
                {
                    var brush = _isDrawMode ? _currentBrush : Brushes.White;
                    var radius = _isDrawMode ? 2 : 10;
                    r.DrawEllipse(brush, null, point, radius, radius);
                }

                _bitmap.Render(visual);

                CanvasImage.Source = _bitmap;
            }
        }

        private void btn_BrushMode_Click(object sender, RoutedEventArgs e)
        {
            _isDrawMode = true;
        }

        private void btn_EraseMode_Click(object sender, RoutedEventArgs e)
        {
            _isDrawMode = false;
        }

        private void UpdatePickedColor()
        {
            var colorBitmap = new RenderTargetBitmap(128, 128, 72, 72, PixelFormats.Pbgra32);

            var colorVisual = new DrawingVisual();

            using (var r = colorVisual.RenderOpen())
            {
                _currentBrush = new SolidColorBrush(Color.FromRgb((byte)slider_Red.Value, (byte)slider_Green.Value, (byte)slider_Blue.Value));

                r.DrawEllipse(_currentBrush, null, new Point(0, 0), 1000, 1000);
            }

            image_PickerColor.Source = colorBitmap;

            colorBitmap.Render(colorVisual);
        }

        private void colorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdatePickedColor();
        }
    }
}
