using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QFP.Core.Settings;

namespace QFP.Core.Graphic;

public class QFPImage
{
    public ImageSettings ImageSettings { get; set; }

    public RenderTargetBitmap Bitmap { get; private set; }

    public QFPImage(ImageSettings settings)
    {
        if (settings == null)
        {
            throw new ArgumentNullException();
        }

        if (settings.Width <= 0 || settings.Height <= 0)
        {
            throw new ArgumentException("Width or height is lower than one px");
        }

        if (settings.DpiX <= 0 || settings.DpiY <= 0)
        {
            throw new ArgumentException("DPI is lower than one");
        }

        ImageSettings = settings;

        Bitmap = new RenderTargetBitmap(settings.Width, settings.Height, settings.DpiX, settings.DpiY, PixelFormats.Pbgra32);

        var visual = new DrawingVisual();

        using (var r = visual.RenderOpen())
        {
            r.DrawEllipse(Brushes.White, null, new Point(0, 0), settings.Width * settings.Width, settings.Height * settings.Height);
        }

        Bitmap.Render(visual);
    }

    public RenderTargetBitmap DrawByBrush(Point position, Brush brush, byte brushSize)
    {
        if (brushSize <= 0)
        {
            throw new ArgumentException("Brush size is lower than one");
        }

        if (brush == null)
        {
            throw new ArgumentNullException();
        }

        var visual = new DrawingVisual();

        using (var r = visual.RenderOpen())
        {
            var radius = brushSize;
            r.DrawEllipse(brush, null, position, radius, radius);
        }

        Bitmap.Render(visual);

        return Bitmap;
    }
}
