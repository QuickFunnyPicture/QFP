using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QFP.Core.Settings;
using QFP.Core.Tools;

namespace QFP.Core.Graphic;
public class Canvas
{
    public ITool Tool;

    public ImageSettings ImageSettings;
    public RenderTargetBitmap Bitmap { get; set; }

    public Canvas(ImageSettings settings)
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

    public void Draw(ITool tool, Point pointStart, Point pointEnd)
    {
        Bitmap = tool.Draw(pointStart, pointEnd, Bitmap);
    }
}
