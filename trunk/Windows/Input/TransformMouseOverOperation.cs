using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformMouseOverOperation : TransformInputOperation
	{
		public TransformMouseOverOperation([NotNull] Transform transform) : base(transform)
		{
		}

		protected override string GetInfoString()
		{
			return string.Format("Transform #{0}", Transform.Index + 1);
		}
	}
}