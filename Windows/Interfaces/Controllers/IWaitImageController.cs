using System.Drawing;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IWaitImageController
	{
		Bitmap DrawWaitImage(Size size, Color backgroundColor, Color glyphColor, float glyphFontSize = 50.0f);
	}
}