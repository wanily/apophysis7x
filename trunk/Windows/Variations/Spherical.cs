using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Variations
{
	class Spherical : Variation
	{
		public override void Calculate(IterationData data)
		{
			var r = Weight / (data.PreX * data.PreX + data.PreY * data.PreY + double.Epsilon);

			data.PostX += data.PreX * r;
			data.PostY += data.PreY * r;
			data.PostZ += Weight * data.PreZ;
		}
	}
}