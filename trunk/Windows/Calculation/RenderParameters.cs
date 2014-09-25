using System;
using System.Drawing;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class RenderParameters
	{
		private readonly RenderMessengerBase mNullMessenger = new RenderMessengerBase();
		private RenderMessengerBase mMessenger;

		public RenderParameters(Flame flame, double density, Size size, int oversample, double filterRadius, bool withTransparency = true)
		{
			if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException(@"size");
			if (density <= 0) throw new ArgumentOutOfRangeException(@"density");
			if (oversample <= 0) throw new ArgumentOutOfRangeException(@"oversample");
			if (filterRadius < 0) throw new ArgumentOutOfRangeException(@"filterRadius");

			Oversample = oversample;
			FilterRadius = filterRadius;
			WithTransparency = withTransparency;
			Flame = flame;
			Density = density;
			Size = size;

			mMessenger = mNullMessenger;
		}

		public Flame Flame { get; private set; }
		public Size Size { get; private set; }

		public double Density { get; private set; }
		public double FilterRadius { get; private set; }
		public int Oversample { get; private set; }

		public bool WithTransparency { get; private set; }

		public RenderMessengerBase Messenger
		{
			get { return mMessenger; }
			set
			{
				mMessenger = value ?? mMessenger;
			}
		}
	}
}