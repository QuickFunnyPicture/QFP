using QFP.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QFP.Core.Tools;
public class BrushTool : ITool
{
    private ImageSettings ImageSettings { get; set; }

    private RenderTargetBitmap Bitmap { get; set; }

    public BrushTool(ImageSettings settings)
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

    public void Draw(Brush brush, Point pointStart, Point pointEnd, byte size, byte radius)
    {
        if (brush == null)
        {
            throw new ArgumentNullException();
        }

        if (size <= 1)
        {
            throw new ArgumentException("Brush size is lower than one");
        }

        var visual = new DrawingVisual();

        using (var r = visual.RenderOpen())
        {
            r.DrawEllipse(brush, null, pointStart, radius, radius);
        }

        Bitmap.Render(visual);
    }

    public RenderTargetBitmap GetBitmap()
    {
        return Bitmap;
    }
}
