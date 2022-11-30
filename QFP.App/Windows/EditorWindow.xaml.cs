using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QFP.App.Windows;
using QFP.Core.Graphic;
using QFP.Core.Settings;
using QFP.Core.Tools;

namespace QFP.App;

public partial class EditorWindow : Window
{
    private ITool Tool;

    private bool _isDrawMode = true;

    private SolidColorBrush _currentBrush = Brushes.Black;

    private ImageSettings Settings;

    public EditorWindow()
    {
        InitializeComponent();
        Settings = new ImageSettings
        {
            Width = 512,
            Height = 512,
            DpiX = 72,
            DpiY = 72
        };
        Tool = new BrushTool(Settings);

        Image_Canvas.Source = Tool.GetBitmap();

        UpdatePickedColor();
    }
    private void Grid_Canvas_MouseEnter(object sender, MouseEventArgs e)
    {
        var cursor = _isDrawMode ? Cursors.Pen : Cursors.Cross;
        Cursor = cursor;
    }

    private void Grid_Canvas_MouseLeave(object sender, MouseEventArgs e)
    {
        Cursor = Cursors.Arrow;
    }

    private void Image_Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var controlPosition = e.GetPosition(Image_Canvas);
            var imageControl = Image_Canvas;
            var x = Math.Floor(controlPosition.X * imageControl.Source.Width / imageControl.ActualWidth);
            var y = Math.Floor(controlPosition.Y * imageControl.Source.Height / imageControl.ActualHeight);
            var point = new Point(x, y);
            var point2 = point;
            var brush = _isDrawMode ? _currentBrush : Brushes.White;
            Tool.Draw(brush, point, point2, size: (byte)(_isDrawMode ? 2 : 10), radius:(byte)(_isDrawMode ? 2 : 10));

            Image_Canvas.Source = Tool.GetBitmap();
        }
    }

    private void Button_BrushMode_Click(object sender, RoutedEventArgs e)
    {
        _isDrawMode = true;
    }

    private void Button_EraseMode_Click(object sender, RoutedEventArgs e)
    {
        _isDrawMode = false;
    }

    private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        UpdatePickedColor();
    }

    private void MenuItem_NewProject_Click(object sender, RoutedEventArgs e)
    {
        var cp = new CreateProjectWindow();
        cp.ShowDialog();

        if (!cp.IsCancelled)
        {
            Settings = new ImageSettings
            {
                Width = cp.ImageWidth,
                Height = cp.ImageHeight,
                DpiX = cp.Dpi,
                DpiY = cp.Dpi
            };
            Tool = new BrushTool(Settings);

            Image_Canvas.Source = Tool.GetBitmap();
        }
    }

    private void UpdatePickedColor()
    {
        var colorBitmap = new RenderTargetBitmap(100, 100, 1, 1, PixelFormats.Pbgra32);

        var colorVisual = new DrawingVisual();

        using (var r = colorVisual.RenderOpen())
        {
            _currentBrush = new SolidColorBrush(Color.FromRgb((byte)Slider_Red.Value, (byte)Slider_Green.Value, (byte)Slider_Blue.Value));

            r.DrawEllipse(_currentBrush, null, new Point(0, 0), 1000, 1000);
        }

        Image_PickedColor.Source = colorBitmap;

        colorBitmap.Render(colorVisual);
    }
}
