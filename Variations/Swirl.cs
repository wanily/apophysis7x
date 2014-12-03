using System.Numerics;
using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Swirl : Variation
	{
		private bool m15C;

		public override void Prepare(Matrix3x2? affineMatrix = null)
		{
			m15C = VariationsIn15CStyle;
		}

		public override void Calculate(IterationData data)
		{
			var theta = (data.PreX * data.PreX + data.PreY * data.PreY);

			var sin = Float.Sin(theta);
			var cos = Float.Cos(theta);

			data.PostX += Weight * (sin * data.PreX - cos * data.PreY);
			data.PostY += Weight * (cos * data.PreX + sin * data.PreY);

			if (m15C)
			{
				data.PostZ += Weight * data.PreZ;
			}
		}
	}
}