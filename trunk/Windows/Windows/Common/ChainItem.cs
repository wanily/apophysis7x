using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	abstract class ChainItem<T> : ControlEventInterceptor<T> where T: Control
	{
		protected ChainItem([NotNull] T control) : base(control)
		{
		}
	}

	abstract class ChainItem : ChainItem<Control>
	{
		protected ChainItem([NotNull] Control control)
			: base(control)
		{
		}
	}
}