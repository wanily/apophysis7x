using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Xml.Linq;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Strings;
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
		private float mPixelsPerUnit;
		private float mZoom;
		private float mVibrancy;
		private float mGammaThreshold;
		private float mGamma;
		private float mBrightness;
		private float mDepthOfField;
		private float mHeight;
		private float mPerspective;
		private float mYaw;
		private float mPitch;
		private Color mBackground;
		private Vector2 mOrigin;
		private float mAngle;

		private Vector2 mHalfSize;
		private float mScaleFactor;
		private Vector2 mScaleVector;
		private Vector2 mScaleVectorInv;
		private float mSinAngle;
		private float mCosAngle;
		private float mSinAngleInv;
		private float mCosAngleInv;

		private static readonly Vector2 mFlipY = new Vector2(1, -1);

		public Flame() : this(true)
		{
		}
		private Flame(bool init)
		{
			mIndex = ++mCounter;
			if (!init)
				return;

			mIterators = new IteratorCollection(this);
			mPalette = PaletteCollection.GetRandomPalette(this);

			mOrigin = new Vector2();
			mAngle = 0;
			mCanvasSize = new Size(1920, 1080);
			mPixelsPerUnit = 25.0f * mCanvasSize.Width / 100.0f;
			mBrightness = 4;
			mGamma = 4;
			mGammaThreshold = 0.001f;
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
				var name = mName;

				if (string.IsNullOrEmpty(mName) || string.IsNullOrEmpty(mName.Trim()))
				{
					var today = DateTime.Today;
					name = ApophysisSettings.Common.NamePrefix + @"-" +
						today.Year.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0') +
						today.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') +
						today.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + @"-" +
						mIndex.ToString(CultureInfo.InvariantCulture);
				}

				return name;
			}
		}

		public Vector2 Origin
		{
			get { return mOrigin; }
			set
			{
				if (value == null) throw new ArgumentNullException(@"value");
				mOrigin = value;
			}
		}

		public Size CanvasSize
		{
			get { return mCanvasSize; }
			set
			{
				if (value.Width <= 0 || value.Height <= 0)
					throw new ArgumentOutOfRangeException(@"value");

				var old = mCanvasSize;

				mCanvasSize = value;
				mPixelsPerUnit = mPixelsPerUnit * value.Width / old.Width;

				UpdateCalculatedValues();
			}
		}
		public float Angle
		{
			get { return mAngle; }
			set
			{
				mAngle = value;
				UpdateCalculatedValues();
			}
		}
		public float PixelsPerUnit
		{
			get { return mPixelsPerUnit; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(@"value");

				mPixelsPerUnit = value;

				UpdateCalculatedValues();
			}
		}
		public float Zoom
		{
			get { return mZoom; }
			set
			{
				mZoom = value;
				UpdateCalculatedValues();
			}
		}
		public float Pitch
		{
			get { return mPitch; }
			set { mPitch = value; }
		}
		public float Yaw
		{
			get { return mYaw; }
			set { mYaw = value; }
		}
		public float Perspective
		{
			get { return mPerspective; }
			set { mPerspective = value; }
		}
		public float Height
		{
			get { return mHeight; }
			set { mHeight = value; }
		}
		public float DepthOfField
		{
			get { return mDepthOfField; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(@"value"); 
				mDepthOfField = value;
			}
		}
		public float Brightness
		{
			get { return mBrightness; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(@"value");
				mBrightness = value;
			}
		}
		public float Gamma
		{
			get { return mGamma; }
			set
			{
				if (value < 1)
					throw new ArgumentOutOfRangeException(@"value");
				mGamma = value;
			}
		}
		public float GammaThreshold
		{
			get { return mGammaThreshold; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(@"value");
				mGammaThreshold = value;
			}
		}
		public float Vibrancy
		{
			get { return mVibrancy; }
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException(@"value");
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
		}

		[NotNull]
		public Flame Copy()
		{
			var copy = new Flame(false);
			ReduceCounter();

			copy.mIndex = mIndex;
			copy.Name = mName;
			copy.mIterators = mIterators.Copy(copy);
			copy.mPalette = mPalette.Copy();
			copy.mOrigin = mOrigin;
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

		public Vector2 CanvasToWorld(Vector2 canvas, Vector2? center = null, Vector2? scale = null)
		{
			var c = center ?? mHalfSize;
			var s = scale ?? mScaleVectorInv;

			var vector = new Vector2(
				(canvas.X - c.X) * s.X - Origin.X,
				(canvas.Y - c.Y) * s.Y + Origin.Y);

			return RotateVector(vector, mCosAngleInv, mSinAngleInv);
		}

		public Vector2 WorldToCanvas(Vector2 world, Vector2? center = null, Vector2? scale = null)
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
				.Where(x => System.Math.Abs(x.Weight) > float.Epsilon && x is ExternalVariation)
				.Select(x => x.Name)
				.ToArray();

			element = new XElement(XName.Get(@"flame"));

			var iteratorElements = new Collection<XElement>();
			XElement paletteElement;

			element.Add(new XAttribute(XName.Get(@"name"), CalculatedName));
			element.Add(new XAttribute(XName.Get(@"version"), ApophysisSettings.Common.FlameExportVersionString));
			element.Add(new XAttribute(XName.Get(@"size"), CanvasSize.Serialize()));
			element.Add(new XAttribute(XName.Get(@"center"), Origin.Serialize()));
			element.Add(new XAttribute(XName.Get(@"scale"), PixelsPerUnit.Serialize()));
			element.Add(new XAttribute(XName.Get(@"angle"), Angle.Serialize()));
			element.Add(new XAttribute(XName.Get(@"rotate"), ((int)System.Math.Round(Angle * -180.0 / System.Math.PI)).Serialize()));
			element.Add(new XAttribute(XName.Get(@"zoom"), Zoom.Serialize()));
			element.Add(new XAttribute(XName.Get(@"cam_pitch"), Pitch.Serialize()));
			element.Add(new XAttribute(XName.Get(@"cam_yaw"), Yaw.Serialize()));
			element.Add(new XAttribute(XName.Get(@"cam_perspective"), Perspective.Serialize()));
			element.Add(new XAttribute(XName.Get(@"cam_zpos"), Height.Serialize()));
			element.Add(new XAttribute(XName.Get(@"cam_dof"), DepthOfField.Serialize()));
			element.Add(new XAttribute(XName.Get(@"background"), Background.Serialize()));
			element.Add(new XAttribute(XName.Get(@"brightness"), Brightness.Serialize()));
			element.Add(new XAttribute(XName.Get(@"gamma"), Gamma.Serialize()));
			element.Add(new XAttribute(XName.Get(@"vibrancy"), Vibrancy.Serialize()));
			element.Add(new XAttribute(XName.Get(@"gamma_threshold"), GammaThreshold.Serialize()));
			element.Add(new XAttribute(XName.Get(@"plugins"), string.Join(@" ", plugins)));

			if (Variation.VariationsIn15CStyle)
				element.Add(new XAttribute(XName.Get(@"new_linear"), @"1"));

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
			if (flame == null) throw new ArgumentNullException(@"flame");

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
					iterator.PreAffine = iterator.PreAffine.Alter(m31: -1);
				}

				iterator.Color = random.NextFloat();
				iterator.ColorSpeed = random.NextFloat() * 2 - 1;

				iterator.PreAffine = iterator.PreAffine.Move(new Vector2(random.NextFloat() * 2 - 1, random.NextFloat() * 2 - 1));
				iterator.PreAffine = iterator.PreAffine.Rotate(random.NextFloat() * Float.Pi * 2 - Float.Pi);
				iterator.PreAffine = iterator.PreAffine.Scale(i > 0 ? (random.NextFloat() * 0.8f + 0.2f) : (random.NextFloat() * 0.4f + 0.6f));

				if (random.Next()%2 > 0)
				{
					iterator.PreAffine = iterator.PreAffine.Transform(new Matrix3x2(1, random.NextFloat() - 0.5f, random.NextFloat() - 0.5f, 1, 0, 0));
				}

				iterator.Variations.SetWeight(VariationRegistry.GetName<Linear>(), random.NextFloat()*0.5f + 0.5f);
			}

			if (random.Next()%2 > 0)
			{
				var totalArea = 0.0f;
				for (int i = 0; i < numIterators; i++)
				{
					var matrix = Iterators[i].PreAffine;
					var tri = new[] {new Vector2(matrix.M11, matrix.M12), new Vector2(matrix.M21, matrix.M22) };
					var angle = Float.Atan2(tri[1].Y, tri[1].X) - Float.Atan2(tri[0].Y, tri[0].X);
					var area = tri[1].Length() * tri[0].Length() * Float.Sin(angle) * 0.5f;

					Iterators[i].Weight = Float.Abs(area);
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
					Iterators[i].Weight = 1.0f / numIterators;
				}
			}

			Brightness = 4;
			Gamma = 4;
			GammaThreshold = 0.001f;
			Vibrancy = 1;
			Background = Color.Black;
			Zoom = 0;
			PixelsPerUnit = 25.0f * mCanvasSize.Width / 100.0f;

			Angle = random.NextFloat() * Float.Pi * 2 - Float.Pi;
			Origin = new Vector2(random.NextFloat() * 2 - 1, random.NextFloat() * 2 - 1);
			Palette.Overwrite(PaletteCollection.GetRandomPalette(this));
		}

		[NotNull]
		public static Flame Random()
		{
			var flame = new Flame();
			flame.Randomize();
			return flame;
		}

		public float GetChaosCoefficient(int fromIteratorIndex, int toIteratorIndex)
		{
			var count = Iterators.Count(x => x.GroupIndex == 0);
			if (fromIteratorIndex < 0 || fromIteratorIndex >= count)
				throw new ArgumentOutOfRangeException(@"fromIteratorIndex");
			if (toIteratorIndex < 0 || toIteratorIndex >= count)
				throw new ArgumentOutOfRangeException(@"toIteratorIndex");

			//todo xaos
			return 1;
		}

		private void UpdateCalculatedValues()
		{
			mHalfSize = new Vector2(CanvasSize.Width / 2.0f, CanvasSize.Height / 2.0f);
			mScaleFactor = (Float.Power(2, mZoom) * mPixelsPerUnit);
			mScaleVector = mScaleFactor * mFlipY;
			mScaleVectorInv = (1.0f / mScaleFactor) * mFlipY;
			mSinAngle = Float.Sin(mAngle);
			mCosAngle = Float.Cos(mAngle);
			mSinAngleInv = Float.Sin(-mAngle);
			mCosAngleInv = Float.Cos(-mAngle);
		}
		private static Vector2 RotateVector(Vector2 vector, float cos, float sin)
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
			if (element == null) throw new ArgumentNullException(@"element");

			if (@"flame" != element.Name.ToString().ToLower())
			{
				throw new ApophysisException(string.Format(Messages.UnexpectedXmlTagError, @"flame", element.Name));
			}

			if (!isReadingBatch)
			{
				MessageCenter.FlameParsing.Enter();
			}

			try
			{
				var nameAttribute = element.Attribute(XName.Get(@"name"));
				Name = nameAttribute == null ? null : nameAttribute.Value;

				var builder = new StringBuilder();
				var header = string.Format(Messages.FlameNameMessageHeader, CalculatedName);

				builder.AppendLine();
				builder.AppendLine(header);
				builder.Append(new string('=', header.Length + 3));

				MessageCenter.SendMessage(builder.ToString());

				bool usesVariationsIn15CStyle = false;
				var newLinearAttribute = element.Attribute(XName.Get(@"new_linear"));
				if (newLinearAttribute != null)
				{
					usesVariationsIn15CStyle = System.Math.Abs(newLinearAttribute.ParseFloat()) > float.Epsilon;
				}

				if (Variation.VariationsIn15CStyle != usesVariationsIn15CStyle)
				{
					MessageCenter.SendMessage(Messages.FlameVersionMismatch);
				}

				var sizeAttribute = element.Attribute(XName.Get(@"size"));
				if (sizeAttribute != null)
				{
					var size = sizeAttribute.ParseSize();
					if (size.Width <= 0 || size.Height <= 0)
					{
						throw new ApophysisException(Messages.FlameSizeRangeError);
					}

					mCanvasSize = size;
				}
				else
				{
					mCanvasSize = new Size(1920, 1080);
				}

				var centerAttribute = element.Attribute(XName.Get(@"center"));
				if (centerAttribute != null)
				{
					var center = centerAttribute.ParseVector();
					mOrigin = center;
				}

				var angleAttribute = element.Attribute(XName.Get(@"angle"));
				if (angleAttribute != null)
				{
					var angle = angleAttribute.ParseFloat();
					mAngle = angle;
				}

				var ppuAttribute = element.Attribute(XName.Get(@"scale"));
				if (ppuAttribute != null)
				{
					var pixelsPerUnit = ppuAttribute.ParseFloat(50.0f * mCanvasSize.Width / 100.0f);
					if (pixelsPerUnit <= 0)
					{
						throw new ApophysisException(Messages.FlamePixelsPerUnitRangeError);
					}

					mPixelsPerUnit = pixelsPerUnit;
				}

				var zoomAttribute = element.Attribute(XName.Get(@"zoom"));
				if (zoomAttribute != null)
				{
					var zoom = zoomAttribute.ParseFloat();
					mZoom = zoom;
				}

				var pitchAttribute = element.Attribute(XName.Get(@"cam_pitch"));
				if (pitchAttribute != null)
				{
					var pitch = pitchAttribute.ParseFloat();
					mPitch = pitch;
				}

				var yawAttribute = element.Attribute(XName.Get(@"cam_yaw"));
				if (yawAttribute != null)
				{
					var yaw = yawAttribute.ParseFloat();
					mYaw = yaw;
				}

				var heightAttribute = element.Attribute(XName.Get(@"cam_zpos"));
				if (heightAttribute != null)
				{
					var height = heightAttribute.ParseFloat();
					mHeight = height;
				}

				var dofAttribute = element.Attribute(XName.Get(@"cam_dof"));
				if (dofAttribute != null)
				{
					var dof = dofAttribute.ParseFloat();
					if (dof < 0)
					{
						throw new ApophysisException(Messages.FlameCameraDofRangeError);
					}
					mDepthOfField = dof;
				}

				var perspectiveAttribute = element.Attribute(XName.Get(@"cam_perspective"));
				if (perspectiveAttribute != null)
				{
					var perspective = perspectiveAttribute.ParseFloat();
					mPerspective = perspective;
				}

				var brightnessAttribute = element.Attribute(XName.Get(@"brightness"));
				if (brightnessAttribute != null)
				{
					var brightness = brightnessAttribute.ParseFloat();
					if (brightness <= 0)
					{
						throw new ApophysisException(Messages.FlameBrightnessRangeError);
					}
					mBrightness = brightness;
				}

				var gammaAttribute = element.Attribute(XName.Get(@"gamma"));
				if (gammaAttribute != null)
				{
					var gamma = gammaAttribute.ParseFloat();
					if (gamma < 0)
					{
						throw new ApophysisException(Messages.FlameGammaRangeError);
					}
					mGamma = gamma;
				}

				var gammaThresholdAttribute = element.Attribute(XName.Get(@"gamma_threshold"));
				if (gammaThresholdAttribute != null)
				{
					var gammaThreshold = gammaThresholdAttribute.ParseFloat();
					if (gammaThreshold < 0)
					{
						throw new ApophysisException(Messages.FlameGammaThresholdRangeError);
					}
					mGammaThreshold = gammaThreshold;
				}

				var vibrancyAttribute = element.Attribute(XName.Get(@"vibrancy"));
				if (vibrancyAttribute != null)
				{
					var vibrancy = vibrancyAttribute.ParseFloat();
					if (vibrancy < 0)
					{
						throw new ApophysisException(Messages.FlameVibrancyRangeError);
					}
					mVibrancy = vibrancy;
				}

				var colorAttribute = element.Attribute(XName.Get(@"background"));
				if (colorAttribute != null)
				{
					var background = colorAttribute.ParseColor();
					mBackground = background;
				}

				var iterators = element.Descendants(XName.Get(@"xform"));
				iterators = iterators.Concat(element.Descendants(XName.Get(@"finalxform")));
				Iterators.ReadXml(iterators);

				var palette = element.Descendants(XName.Get(@"palette")).FirstOrDefault();
				if (palette == null)
				{
					throw new ApophysisException(Messages.FlameMissingPaletteTagError);
				}

				Palette.ReadCondensedHexData(palette.Value);
				UpdateCalculatedValues();

				MessageCenter.SendMessage(Messages.FlameSuccessMessage);
			}
			catch (ApophysisException exception)
			{
				Trace.TraceError(exception.Message);
				MessageCenter.SendMessage(string.Format(Messages.FlameErrorMessage, exception.Message));

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