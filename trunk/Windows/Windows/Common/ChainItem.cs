using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	abstract class ChainItem : ControlEventInterceptor
	{
		protected ChainItem([NotNull] Control control) : base(control)
		{
		}
	}
}