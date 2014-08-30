using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class MouseOverOperation : InputOperation
	{
		public MouseOverOperation([NotNull] Iterator iterator) : base(iterator)
		{
		}

		protected override string GetInfoString()
		{
			return string.Format("Transform #{0}", Iterator.Index + 1);
		}
	}
}