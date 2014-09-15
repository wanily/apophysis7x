using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	class MouseOverOperation : IteratorInputOperation
	{
		public MouseOverOperation([NotNull] Iterator iterator) : base(iterator)
		{
		}

		protected override string GetInfoString()
		{
			return string.Format(Iterator.GetVerboseDisplayName());
		}
	}
}