using System;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Models
{
	[PublicAPI]
	public class Iterator
	{
		private readonly Flame mFlame;

		private AffineTransform mPreAffine;
		private AffineTransform mPostAffine;

		public Iterator([NotNull] Flame hostingFlame)
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;
			mPreAffine = new AffineTransform();
			mPostAffine = new AffineTransform();
		}

		public int Index
		{
			get { return mFlame.Iterators.IndexOf(this); }
		}

		public AffineTransform PreAffine
		{
			get { return mPreAffine; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mPreAffine = value;
			}
		}
		public AffineTransform PostAffine
		{
			get { return mPostAffine; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mPostAffine = value;
			}
		}
	}
}