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

    public Brush Brush { get; set; }
    public byte Size { get; set; }
    public byte Radius { get; set; }

    public BrushTool(Brush brush, byte size, byte radius)
    {
        if (brush == null)
        {
            throw new ArgumentNullException();
        }

        if (size <= 1)
        {
            throw new ArgumentException("Brush size is lower than one");
        }
        if (radius <= 1)
        {
            throw new ArgumentException("Radius size is lower than one");
        }
        Brush = brush;
        Size = size;
        Radius = radius;
    }

    public RenderTargetBitmap Draw(Point pointStart, Point pointEnd, RenderTargetBitmap currentBitmap)
    {
        var visual = new DrawingVisual();

        using (var r = visual.RenderOpen())
        {
            r.DrawEllipse(Brush, null, pointStart, Size, Radius);
        }
        currentBitmap.Render(visual);

        return currentBitmap;
    }

}
