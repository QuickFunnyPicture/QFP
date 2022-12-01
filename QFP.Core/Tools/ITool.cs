using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QFP.Core.Tools;

public interface ITool
{
    public RenderTargetBitmap Draw(Point pointStart, Point pointEnd, RenderTargetBitmap currentBitmap);
}
