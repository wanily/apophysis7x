using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Variations
{
	class Spherical : Variation
	{
		private bool m15C;

		public override void Prepare(AffineTransform affineMatrix = null)
		{
			m15C = VariationsIn15CStyle;
		}

		public override void Calculate(IterationData data)
		{
			var r = Weight / (data.PreX * data.PreX + data.PreY * data.PreY + double.Epsilon);

			data.PostX += data.PreX * r;
			data.PostY += data.PreY * r;

			if (m15C)
			{
				data.PostZ += Weight * data.PreZ;
			}
		}
	}
}