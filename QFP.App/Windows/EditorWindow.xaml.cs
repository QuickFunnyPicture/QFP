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
    private Canvas Canvas { get; set; }

    private ITool CurrentTool { get; set; }

    private ImageSettings Settings { get; set; }

    private Cursor ToolCursor { get; set; }

    private BrushTool BrushTool { get; set; }
    private BrushTool EraseTool { get; set; }

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

        Canvas = new Canvas(Settings);

        BrushTool = new BrushTool(Brushes.Black, 2, 2);
        EraseTool = new BrushTool(Brushes.White, 2, 10);

        CurrentTool = BrushTool;
        ToolCursor = Cursors.Pen;

        Image_Canvas.Source = Canvas.Bitmap;

        UpdatePickedColor();
    }
    private void Grid_Canvas_MouseEnter(object sender, MouseEventArgs e)
    {
        Cursor = ToolCursor;
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
            Canvas.Draw(CurrentTool, point, point2);

            Image_Canvas.Source = Canvas.Bitmap;
        }
    }

    private void Button_BrushMode_Click(object sender, RoutedEventArgs e)
    {
        CurrentTool = BrushTool;
        ToolCursor = Cursors.Pen;
    }

    private void Button_EraseMode_Click(object sender, RoutedEventArgs e)
    {
        CurrentTool = EraseTool;
        ToolCursor = Cursors.Cross;
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
            Canvas = new Canvas(Settings);

            Image_Canvas.Source = Canvas.Bitmap;
        }
    }

    private void UpdatePickedColor()
    {
        var colorBitmap = new RenderTargetBitmap(100, 100, 1, 1, PixelFormats.Pbgra32);

        var colorVisual = new DrawingVisual();

        using (var r = colorVisual.RenderOpen())
        {
            BrushTool.Brush = new SolidColorBrush(Color.FromRgb((byte)Slider_Red.Value, (byte)Slider_Green.Value, (byte)Slider_Blue.Value));

            r.DrawEllipse(BrushTool.Brush, null, new Point(0, 0), 1000, 1000);
        }

        Image_PickedColor.Source = colorBitmap;

        colorBitmap.Render(colorVisual);
    }
}
