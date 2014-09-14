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
		private double mZoom;
		private double mVibrancy;
		private double mGammaThreshold;
		private double mGamma;
		private double mBrightness;
		private double mDepthOfField;
		private double mHeight;
		private double mPerspective;
		private double mYaw;
		private double mPitch;
		private Color mBackground;

		public Flame()
		{
			mIndex = ++mCounter;

			mIterators = new IteratorCollection(this);
			mPalette = PaletteCollection.GetRandomPalette(this);
			mCamera = new AffineTransform();

			mCanvasSize = new Size(1920, 1080);
			mPixelsPerUnit = 25.0 * mCanvasSize.Width / 100.0;
			mBrightness = 4;
			mGamma = 4;
			mGammaThreshold = 0.001;
			mVibrancy = 1;
			mBackground = Color.Black;
		}

		[CanBeNull]
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}

		[NotNull]
		public string CalculatedName
		{
			get
			{
				if (string.IsNullOrEmpty(mName) || string.IsNullOrEmpty(mName.Trim()))
				{
					var today = DateTime.Today;
					return ApophysisSettings.NamePrefix + @"-" +
						today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
						today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
						today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + @"-" +
						mIndex.ToString(CultureInfo.InvariantCulture);
				}

				return mName;
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
		public double Zoom
		{
			get { return mZoom; }
			set { mZoom = value; }
		}
		public double Pitch
		{
			get { return mPitch; }
			set { mPitch = value; }
		}
		public double Yaw
		{
			get { return mYaw; }
			set { mYaw = value; }
		}
		public double Perspective
		{
			get { return mPerspective; }
			set { mPerspective = value; }
		}
		public double Height
		{
			get { return mHeight; }
			set { mHeight = value; }
		}
		public double DepthOfField
		{
			get { return mDepthOfField; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value"); mDepthOfField = value;
			}
		}
		public double Brightness
		{
			get { return mBrightness; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("value");
				mBrightness = value;
			}
		}
		public double Gamma
		{
			get { return mGamma; }
			set
			{
				if (value < 1)
					throw new ArgumentOutOfRangeException("value");
				mGamma = value;
			}
		}
		public double GammaThreshold
		{
			get { return mGammaThreshold; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");
				mGammaThreshold = value;
			}
		}
		public double Vibrancy
		{
			get { return mVibrancy; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("value");
				mVibrancy = value;
			}
		}
		public Color Background
		{
			get { return mBackground; }
			set
			{
				mBackground = Color.FromArgb(value.R, value.G, value.B);
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

		[NotNull]
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
			copy.mZoom = mZoom;
			copy.mPitch = mPitch;
			copy.mYaw = mYaw;
			copy.mPerspective = mPerspective;
			copy.mHeight = mHeight;
			copy.mDepthOfField = mDepthOfField;
			copy.mBrightness = mBrightness;
			copy.mGamma = mGamma;
			copy.mGammaThreshold = mGammaThreshold;
			copy.mVibrancy = mVibrancy;
			copy.mBackground = mBackground;

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

			var zoomAttribute = element.Attribute(XName.Get("zoom"));
			if (zoomAttribute != null)
			{
				var zoom = zoomAttribute.ParseFloat();
				mZoom = zoom;
			}

			var pitchAttribute = element.Attribute(XName.Get("cam_pitch"));
			if (pitchAttribute != null)
			{
				var pitch = pitchAttribute.ParseFloat();
				mPitch = pitch * System.Math.PI / 180.0;
			}

			var yawAttribute = element.Attribute(XName.Get("cam_yaw"));
			if (yawAttribute != null)
			{
				var yaw = yawAttribute.ParseFloat();
				mYaw = yaw * System.Math.PI / 180.0;
			}

			var heightAttribute = element.Attribute(XName.Get("cam_zpos"));
			if (heightAttribute != null)
			{
				var height = heightAttribute.ParseFloat();
				mHeight = height;
			}

			var dofAttribute = element.Attribute(XName.Get("cam_dof"));
			if (dofAttribute != null)
			{
				var dof = dofAttribute.ParseFloat();
				if (dof < 0)
				{
					throw new ApophysisException("Depth of field must be greater than or equal to zero");
				}
				mDepthOfField = dof;
			}

			var perspectiveAttribute = element.Attribute(XName.Get("cam_perspective"));
			if (perspectiveAttribute != null)
			{
				var perspective = perspectiveAttribute.ParseFloat();
				mPerspective = perspective;
			}

			var brightnessAttribute = element.Attribute(XName.Get("brightness"));
			if (brightnessAttribute != null)
			{
				var brightness = brightnessAttribute.ParseFloat();
				if (brightness <= 0)
				{
					throw new ApophysisException("Brightness must be greater than zero");
				}
				mBrightness = brightness;
			}

			var gammaAttribute = element.Attribute(XName.Get("gamma"));
			if (gammaAttribute != null)
			{
				var gamma = gammaAttribute.ParseFloat();
				if (gamma < 0)
				{
					throw new ApophysisException("Gamma must be greater than or equal to one");
				}
				mGamma = gamma;
			}

			var gammaThresholdAttribute = element.Attribute(XName.Get("gamma_threshold"));
			if (gammaThresholdAttribute != null)
			{
				var gammaThreshold = gammaThresholdAttribute.ParseFloat();
				if (gammaThreshold < 0)
				{
					throw new ApophysisException("Gamma threshold must be greater than or equal to zero");
				}
				mGammaThreshold = gammaThreshold;
			}

			var vibrancyAttribute = element.Attribute(XName.Get("vibrancy"));
			if (vibrancyAttribute != null)
			{
				var vibrancy = vibrancyAttribute.ParseFloat();
				if (vibrancy < 0)
				{
					throw new ApophysisException("Vibrancy must be greater than or equal to zero");
				}
				mVibrancy = vibrancy;
			}

			var colorAttribute = element.Attribute(XName.Get("background"));
			if (colorAttribute != null)
			{
				var background = colorAttribute.ParseColor();
				mBackground = background;
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

			if (!Equals(mCanvasSize, flame.mCanvasSize))
				return false;

			if (!Equals(mPixelsPerUnit, flame.mPixelsPerUnit))
				return false;

			if (!Equals(mZoom, flame.mZoom))
				return false;

			if (!Equals(mPitch, flame.mPitch))
				return false;

			if (!Equals(mYaw, flame.mYaw))
				return false;

			if (!Equals(mHeight, flame.mHeight))
				return false;

			if (!Equals(mPerspective, flame.mPerspective))
				return false;

			if (!Equals(mDepthOfField, flame.mDepthOfField))
				return false;

			if (!Equals(mBrightness, flame.mBrightness))
				return false;

			if (!Equals(mGamma, flame.mGamma))
				return false;

			if (!Equals(mGammaThreshold, flame.mGammaThreshold))
				return false;

			if (!Equals(mVibrancy, flame.mVibrancy))
				return false;

			if (!Equals(mBackground, flame.mBackground))
				return false;

			if (!mPalette.IsEqual(flame.mPalette))
				return false;

			if (!mIterators.IsEqual(flame.mIterators))
				return false;

			return true;
		}

		[NotNull]
		public static Flame Random()
		{
			//todo add random flame feature
			return new Flame();
		}

		internal static void ReduceCounter()
		{
			mCounter--;
		}
	}
}