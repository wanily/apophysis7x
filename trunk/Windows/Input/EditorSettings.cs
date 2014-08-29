using System.ComponentModel;

namespace Xyrus.Apophysis.Windows.Input
{
	public class EditorSettings : Component
	{
		public double MoveAmount { get; set; }
		public double AngleSnap { get; set; }
		public double ScaleSnap { get; set; }
	}
}