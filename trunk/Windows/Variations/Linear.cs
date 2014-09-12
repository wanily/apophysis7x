using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Linear : Variation
	{
		public override void Calculate(IterationData data)
		{
			data.PostX += Weight * data.PreX;
			data.PostY += Weight * data.PreY;
			data.PostZ += Weight * data.PreZ;
		}
	}
}
