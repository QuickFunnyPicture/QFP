using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QFP.Core.Tools;

public interface ITool
{
    public void Draw(Brush brush, Point point1, Point point2, byte size, byte radius);

    public RenderTargetBitmap GetBitmap();
}
