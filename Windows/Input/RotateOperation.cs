using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;
using Iterator = Xyrus.Apophysis.Models.Iterator;

namespace Xyrus.Apophysis.Windows.Input
{
	class RotateOperation : IteratorInputOperation
	{
		public RotateOperation([NotNull] Iterator iterator, float rotationAngle, RotationAxis axis)
			: base(iterator)
		{
			RotationAngle = rotationAngle;
			Axis = axis;
		}

		public float RotationAngle
		{
			get;
			private set;
		}
		public RotationAxis Axis
		{
			get; 
			private set;
		}

		protected override string GetInfoString()
		{
			var angle = RotationAngle * 180.0f / Float.Pi;
			if (angle < 0)
			{
				angle = 360 + angle;
			}

			return string.Format("Rotate:\t {0}°", (360 - angle).ToString("0", InputController.Culture).PadLeft(5));
		}
	}
}