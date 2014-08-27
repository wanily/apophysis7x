namespace Xyrus.Apophysis.Windows.Input
{
	public class EditorSettings
	{
		public EditorSettings()
		{
			AngleSnap = 15;
			ScaleSnap = 125;
		}

		public double AngleSnap { get; set; }
		public double ScaleSnap { get; set; }
	}
}