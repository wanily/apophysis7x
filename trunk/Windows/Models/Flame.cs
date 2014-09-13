using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Flame
	{
		private AffineTransform mCamera;
		private IteratorCollection mIterators;
		private Palette mPalette;

		private static int mCounter;
		private int mIndex;
		private string mName;
		private Size mCanvasSize;
		private double mPixelsPerUnit;

		public Flame()
		{
			mIndex = ++mCounter;

			mIterators = new IteratorCollection(this);
			mPalette = PaletteCollection.GetRandomPalette(this);
			mCamera = new AffineTransform();

			mCanvasSize = new Size(1920, 1080);
			mPixelsPerUnit = 50.0 * mCanvasSize.Width / 100.0;
		}

		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		public string CalculatedName
		{
			get
			{
				if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Name.Trim()))
				{
					var today = DateTime.Today;
					return ApophysisSettings.NamePrefix + @"-" +
						today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
						today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
						today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + @"-" +
						mIndex.ToString(CultureInfo.InvariantCulture);
				}

				return Name;
			}
		}
		public Size CanvasSize
		{
			get { return mCanvasSize; }
			set
			{
				if (value.Width <= 0 || value.Height <= 0)
					throw new ArgumentOutOfRangeException("value");

				var old = mCanvasSize;

				mCanvasSize = value;
				PixelsPerUnit = PixelsPerUnit * value.Width / old.Width;
			}
		}
		public double PixelsPerUnit
		{
			get { return mPixelsPerUnit; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value");

				mPixelsPerUnit = value;
			}
		}

		[NotNull]
		public IteratorCollection Iterators
		{
			get { return mIterators; }
		}

		[NotNull]
		public Palette Palette
		{
			get { return mPalette; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mPalette = value;
			}
		}

		[NotNull]
		public AffineTransform Camera
		{
			get { return mCamera; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mCamera = value;
			}
		}

		public Flame Copy()
		{
			var copy = new Flame();
			ReduceCounter();

			copy.mIndex = mIndex;
			copy.Name = mName;
			copy.mIterators = mIterators.Copy(copy);
			copy.mPalette = mPalette.Copy();
			copy.mCamera.Origin.X = mCamera.Origin.X;
			copy.mCamera.Origin.Y = mCamera.Origin.Y;
			copy.mCamera.Matrix.X.X = mCamera.Matrix.X.X;
			copy.mCamera.Matrix.X.Y = mCamera.Matrix.X.Y;
			copy.mCamera.Matrix.Y.X = mCamera.Matrix.Y.X;
			copy.mCamera.Matrix.Y.Y = mCamera.Matrix.Y.Y;
			copy.mCanvasSize = mCanvasSize;
			copy.mPixelsPerUnit = mPixelsPerUnit;

			return copy;
		}
		public void ReadXml([NotNull] XElement element)
		{
			if (element == null) throw new ArgumentNullException("element");

			if ("flame" != element.Name.ToString().ToLower())
			{
				throw new ApophysisException("Expected XML node \"flame\" but received \"" + element.Name + "\"");
			}

			mCamera.Reset();

			var nameAttribute = element.Attribute(XName.Get("name"));
			Name = nameAttribute == null ? null : nameAttribute.Value;

			var sizeAttribute = element.Attribute(XName.Get("size"));
			if (sizeAttribute != null)
			{
				var size = sizeAttribute.ParseSize();
				if (size.Width <= 0 || size.Height <= 0)
				{
					throw new ApophysisException("Size must be greater than zero in both dimensions");
				}

				mCanvasSize = size;
			}
			else
			{
				mCanvasSize = new Size(1920, 1080);
			}

			var centerAttribute = element.Attribute(XName.Get("center"));
			if (centerAttribute != null)
			{
				var center = centerAttribute.ParseVector();
				mCamera.Move(center);
			}

			var angleAttribute = element.Attribute(XName.Get("angle"));
			if (angleAttribute != null)
			{
				var angle = angleAttribute.ParseFloat();
				mCamera.Rotate(angle);
			}

			var ppuAttribute = element.Attribute(XName.Get("scale"));
			if (ppuAttribute != null)
			{
				var pixelsPerUnit = ppuAttribute.ParseFloat(50.0 * mCanvasSize.Width / 100.0);
				if (pixelsPerUnit <= 0)
				{
					throw new ApophysisException("Scale must be greater than zero");
				}

				mPixelsPerUnit = pixelsPerUnit;
			}

			var iterators = element.Descendants(XName.Get("xform"));
			iterators = iterators.Concat(element.Descendants(XName.Get("finalxform")));
			Iterators.ReadXml(iterators);

			var palette = element.Descendants(XName.Get("palette")).FirstOrDefault();
			if (palette == null)
			{
				throw new ApophysisException("No descendant node \"palette\" found");
			}
			Palette.ReadCondensedHexData(palette.Value);
		}
		public bool IsEqual([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			if (!Equals(mName, flame.mName))
				return false;

			if (!mPalette.IsEqual(flame.mPalette))
				return false;

			if (!mIterators.IsEqual(flame.mIterators))
				return false;

			return true;
		}

		internal static void ReduceCounter()
		{
			mCounter--;
		}
	}
}