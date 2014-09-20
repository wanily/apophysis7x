using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Swirl : Variation
	{
		private bool m15C;

		public override void Prepare(IterationData data)
		{
			m15C = ApophysisSettings.VariationsIn15CStyle;
		}

		public override void Calculate(IterationData data)
		{
			var theta = (data.PreX * data.PreX + data.PreY * data.PreY);

			var sin = System.Math.Sin(theta);
			var cos = System.Math.Cos(theta);

			data.PostX += Weight * (sin * data.PreX - cos * data.PreY);
			data.PostY += Weight * (cos * data.PreX + sin * data.PreY);

			if (m15C)
			{
				data.PostZ += Weight * data.PreZ;
			}
		}
	}
}