using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Variations;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class Flame
	{
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
		private Vector2 mOrigin;
		private double mAngle;

		private Vector2 mHalfSize;
		private double mScaleFactor;
		private Vector2 mScaleVector;
		private Vector2 mScaleVectorInv;
		private double mSinAngle;
		private double mCosAngle;
		private double mSinAngleInv;
		private double mCosAngleInv;

		private static readonly Vector2 mFlipY = new Vector2(1, -1).Freeze();

		public Flame()
		{
			mIndex = ++mCounter;

			mIterators = new IteratorCollection(this);
			mPalette = PaletteCollection.GetRandomPalette(this);

			mOrigin = new Vector2();
			mAngle = 0;
			mCanvasSize = new Size(1920, 1080);
			mPixelsPerUnit = 25.0 * mCanvasSize.Width / 100.0;
			mBrightness = 4;
			mGamma = 4;
			mGammaThreshold = 0.001;
			mVibrancy = 1;
			mBackground = Color.Black;

			UpdateCalculatedValues();
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

		[NotNull]
		public Vector2 Origin
		{
			get { return mOrigin; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mOrigin = value;
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
				mPixelsPerUnit = mPixelsPerUnit * value.Width / old.Width;

				UpdateCalculatedValues();
			}
		}
		public double Angle
		{
			get { return mAngle; }
			set
			{
				mAngle = value;
				UpdateCalculatedValues();
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

				UpdateCalculatedValues();
			}
		}
		public double Zoom
		{
			get { return mZoom; }
			set
			{
				mZoom = value;
				UpdateCalculatedValues();
			}
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
		public Flame Copy()
		{
			var copy = new Flame();
			ReduceCounter();

			copy.mIndex = mIndex;
			copy.Name = mName;
			copy.mIterators = mIterators.Copy(copy);
			copy.mPalette = mPalette.Copy();
			copy.mOrigin = mOrigin.Copy();
			copy.mAngle = mAngle;
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

			copy.UpdateCalculatedValues();

			return copy;
		}

		[NotNull]
		public Vector2 CanvasToWorld(Vector2 canvas, Vector2 center = null, Vector2 scale = null)
		{
			var c = center ?? mHalfSize;
			var s = scale ?? mScaleVectorInv;

			var vector = new Vector2(
				(canvas.X - c.X) * s.X - Origin.X,
				(canvas.Y - c.Y) * s.Y + Origin.Y);

			return RotateVector(vector, mCosAngleInv, mSinAngleInv);
		}

		[NotNull]
		public Vector2 WorldToCanvas(Vector2 world, Vector2 center = null, Vector2 scale = null)
		{
			var c = center ?? mHalfSize;
			var s = scale ?? mScaleVector;

			var vector = new Vector2(
				(world.X + Origin.X) * s.X + c.X,
				(world.Y - Origin.Y) * s.Y + c.Y);

			return RotateVector(vector, mCosAngle, mSinAngle);
		}

		public void WriteXml([NotNull] out XElement element)
		{
			var plugins = Iterators
				.SelectMany(x => x.Variations)
				.Where(x => System.Math.Abs(x.Weight) > double.Epsilon && x is ExternalVariation)
				.Select(x => x.Name)
				.ToArray();

			element = new XElement(XName.Get("flame"));

			var iteratorElements = new Collection<XElement>();
			XElement paletteElement;
			
			element.Add(new XAttribute(XName.Get("name"), CalculatedName));
			element.Add(new XAttribute(XName.Get("version"), ApophysisSettings.FlameExportVersionString));
			element.Add(new XAttribute(XName.Get("size"), CanvasSize.Serialize()));
			element.Add(new XAttribute(XName.Get("center"), Origin.Serialize()));
			element.Add(new XAttribute(XName.Get("scale"), PixelsPerUnit.Serialize()));
			element.Add(new XAttribute(XName.Get("angle"), Angle.Serialize()));
			element.Add(new XAttribute(XName.Get("rotate"), ((int)System.Math.Round(Angle * -180.0 / System.Math.PI)).Serialize()));
			element.Add(new XAttribute(XName.Get("zoom"), Zoom.Serialize()));
			element.Add(new XAttribute(XName.Get("cam_pitch"), Pitch.Serialize()));
			element.Add(new XAttribute(XName.Get("cam_yaw"), Yaw.Serialize()));
			element.Add(new XAttribute(XName.Get("cam_perspective"), Perspective.Serialize()));
			element.Add(new XAttribute(XName.Get("cam_zpos"), Height.Serialize()));
			element.Add(new XAttribute(XName.Get("cam_dof"), DepthOfField.Serialize()));
			element.Add(new XAttribute(XName.Get("background"), Background.Serialize()));
			element.Add(new XAttribute(XName.Get("brightness"), Brightness.Serialize()));
			element.Add(new XAttribute(XName.Get("gamma"), Gamma.Serialize()));
			element.Add(new XAttribute(XName.Get("vibrancy"), Vibrancy.Serialize()));
			element.Add(new XAttribute(XName.Get("gamma_threshold"), GammaThreshold.Serialize()));
			element.Add(new XAttribute(XName.Get("plugins"), string.Join(@" ", plugins)));
			element.Add(new XAttribute(XName.Get("new_linear"), "1"));

			Iterators.WriteXml(iteratorElements);
			Palette.WriteXml(out paletteElement);

			foreach (var iteratorElement in iteratorElements)
			{
				element.Add(iteratorElement);
			}

			element.Add(paletteElement);
		}
		public void ReadXml([NotNull] XElement element)
		{
			ReadXml(element, false);
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

			if (!Equals(mOrigin.X, flame.mOrigin.X))
				return false;

			if (!Equals(mOrigin.Y, flame.mOrigin.Y))
				return false;

			if (!Equals(mAngle, flame.mAngle))
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

		public void Randomize()
		{
			const int minIterators = 2;
			const int maxIterators = 5;

			var random = new Random((int)DateTime.Now.Ticks % mCounter);
			var numIterators = random.Next(minIterators, maxIterators + 1);

			Iterators.Reset();

			for (int i = 0; i < numIterators; i++)
			{
				if (i > 0)
				{
					Iterators.Add();
				}

				var iterator = Iterators[i];

				if (random.Next()%10 < 9)
				{
					iterator.PreAffine.Origin.X = -1;
				}

				iterator.Color = random.NextDouble();
				iterator.ColorSpeed = random.NextDouble()*2 - 1;
				
				iterator.PreAffine.Move(new Vector2(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1));
				iterator.PreAffine.Rotate(random.NextDouble() * System.Math.PI * 2 - System.Math.PI);
				iterator.PreAffine.Scale(i > 0 ? (random.NextDouble() * 0.8 + 0.2) : (random.NextDouble() * 0.4 + 0.6));

				if (random.Next()%2 > 0)
				{
					iterator.PreAffine.MatrixTransform(new Matrix2X2 { X = new Vector2(1, random.NextDouble() - 0.5), Y = new Vector2(random.NextDouble() - 0.5, 1)});
				}

				iterator.Variations.SetWeight(VariationRegistry.GetName<Linear>(), random.NextDouble()*0.5 + 0.5);
			}

			if (random.Next()%2 > 0)
			{
				var totalArea = 0.0;
				for (int i = 0; i < numIterators; i++)
				{
					var matrix = Iterators[i].PreAffine;
					var tri = new[] {matrix.Matrix.X, matrix.Matrix.Y};
					var angle = System.Math.Atan2(tri[1].Y, tri[1].X) - System.Math.Atan2(tri[0].Y, tri[0].X);
					var area = tri[1].Length*tri[0].Length*System.Math.Sin(angle)*0.5;

					Iterators[i].Weight = System.Math.Abs(area);
					totalArea += Iterators[i].Weight;
				}

				for (int i = 0; i < numIterators; i++)
				{
					Iterators[i].Weight /= totalArea;
				}

				Iterators.NormalizeWeights();
			}
			else
			{
				for (int i = 0; i < numIterators; i++)
				{
					Iterators[i].Weight = 1.0 / numIterators;
				}
			}

			Brightness = 4;
			Gamma = 4;
			GammaThreshold = 0.001;
			Vibrancy = 1;
			Background = Color.Black;
			Zoom = 0;
			PixelsPerUnit = 25.0 * mCanvasSize.Width / 100.0;

			Angle = random.NextDouble()*System.Math.PI*2 - System.Math.PI;
			Origin = new Vector2(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1);
			Palette = PaletteCollection.GetRandomPalette(this);
		}

		[NotNull]
		public static Flame Random()
		{
			var flame = new Flame();
			flame.Randomize();
			return flame;
		}

		private void UpdateCalculatedValues()
		{
			mHalfSize = new Vector2(CanvasSize.Width / 2.0, CanvasSize.Height / 2.0).Freeze();
			mScaleFactor = (System.Math.Pow(2, mZoom) * mPixelsPerUnit);
			mScaleVector = mScaleFactor * mFlipY;
			mScaleVectorInv = (1.0 / mScaleFactor) * mFlipY;
			mSinAngle = System.Math.Sin(mAngle);
			mCosAngle = System.Math.Cos(mAngle);
			mSinAngleInv = System.Math.Sin(-mAngle);
			mCosAngleInv = System.Math.Cos(-mAngle);
		}
		private static Vector2 RotateVector(Vector2 vector, double cos, double sin)
		{
			return new Vector2(
				vector.X * cos - vector.Y * sin,
				vector.X * sin + vector.Y * cos);
		}

		internal static void ReduceCounter()
		{
			mCounter--;
		}

		internal void ReadXml([NotNull] XElement element, bool isReadingBatch)
		{
			if (element == null) throw new ArgumentNullException("element");

			if ("flame" != element.Name.ToString().ToLower())
			{
				throw new ApophysisException("Expected XML node \"flame\" but received \"" + element.Name + "\"");
			}

			if (!isReadingBatch)
			{
				MessageCenter.FlameParsing.Enter();
			}

			try
			{
				var nameAttribute = element.Attribute(XName.Get("name"));
				Name = nameAttribute == null ? null : nameAttribute.Value;

				var builder = new StringBuilder();
				var header = string.Format("Flame \"{0}\"", CalculatedName);

				builder.AppendLine();
				builder.AppendLine(header);
				builder.Append(new string('=', header.Length + 3));

				MessageCenter.SendMessage(builder.ToString());

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
					mOrigin = center;
				}

				var angleAttribute = element.Attribute(XName.Get("angle"));
				if (angleAttribute != null)
				{
					var angle = angleAttribute.ParseFloat();
					mAngle = angle;
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
					mPitch = pitch;
				}

				var yawAttribute = element.Attribute(XName.Get("cam_yaw"));
				if (yawAttribute != null)
				{
					var yaw = yawAttribute.ParseFloat();
					mYaw = yaw;
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
				UpdateCalculatedValues();

				MessageCenter.SendMessage("Done!");
			}
			catch (ApophysisException exception)
			{
				Trace.TraceError(exception.Message);
				MessageCenter.SendMessage(string.Format("Error: {0}", exception.Message));

				throw;
			}
			finally
			{
				if (!isReadingBatch)
				{
					MessageCenter.FlameParsing.Dispose();
				}
			}
		}
	}
}