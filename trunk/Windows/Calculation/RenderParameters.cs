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

		public RenderParameters([NotNull] Flame flame, double density, Size size, int oversample, double filterRadius, bool withTransparency = true)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (density <= 0) throw new ArgumentOutOfRangeException(@"density");
			if (oversample <= 0) throw new ArgumentOutOfRangeException(@"oversample");
			if (filterRadius < 0) throw new ArgumentOutOfRangeException(@"filterRadius");
			if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException(@"size");

			Oversample = oversample;
			FilterRadius = filterRadius;
			WithTransparency = withTransparency;
			Flame = flame;
			Density = density;
			Size = size;

			mMessenger = mNullMessenger;
		}

		[NotNull]
		public Flame Flame { get; private set; }
		public Size Size { get; private set; }

		public double Density { get; private set; }
		public double FilterRadius { get; private set; }
		public int Oversample { get; private set; }
		public bool WithTransparency { get; private set; }

		[NotNull]
		public RenderMessengerBase Messenger
		{
			get { return mMessenger; }
			set
			{
				// ReSharper disable once ConstantNullCoalescingCondition
				mMessenger = value ?? mMessenger;
			}
		}
	}

	[PublicAPI]
	public class RenderData
	{
		private readonly RenderParameters mParameters;

		public RenderData(RenderParameters parameters)
		{
			mParameters = parameters;
			Calculate();
		}

		private void Calculate()
		{
			throw new NotImplementedException();
		}
	}
}