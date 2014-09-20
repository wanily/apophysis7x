using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Linear3D : Variation
	{
		public override void Calculate(IterationData data)
		{
			data.PostX += Weight * data.PreX;
			data.PostY += Weight * data.PreY;
			data.PostZ += Weight * data.PreZ;
		}

		public override string Name
		{
			get { return "linear3D"; }
		}
	}
}