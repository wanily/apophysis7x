using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Linear : Variation
	{
		public override void Calculate(IterationData data)
		{
			data.PostX += data.Weight * data.PreX;
			data.PostY += data.Weight * data.PreY;
			data.PostZ += data.Weight * data.PreZ;
		}
	}
}
