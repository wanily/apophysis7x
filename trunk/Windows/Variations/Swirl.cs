using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Swirl : Variation
	{
		public override void Calculate(IterationData data)
		{
			var theta = (data.PreX * data.PreX + data.PreY * data.PreY);

			var sin = System.Math.Sin(theta);
			var cos = System.Math.Cos(theta);

			data.PostX += data.Weight * (sin * data.PreX - cos * data.PreY);
			data.PostY += data.Weight * (cos * data.PreX + sin * data.PreY);
			data.PostZ += data.Weight * data.PreZ;
		}
	}
}