using System;
using System.Drawing;
using System.Linq;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Rectangle = Xyrus.Apophysis.Math.Rectangle;

namespace Xyrus.Apophysis.Calculation
{
	public class FlameData : IDisposable
	{
		private Renderer mRenderer;
		private Flame mFlame;

		private const double mWhiteLevel = 200;
		private const int mMaxFilterWidth = 25;
		private const double mFilterCutoff = 1.8;

		~FlameData()
		{
			Dispose(false);
		}
		public FlameData([NotNull] Renderer renderer, [NotNull] Flame flame)
		{
			if (renderer == null) throw new ArgumentNullException(@"renderer");
			if (flame == null) throw new ArgumentNullException(@"flame");

			mRenderer = renderer;
			mFlame = flame;
		}

		public void Initialize()
		{
			CalculateFilterKernel();
			CalculateBufferMetrics();
			CalculateCamera();
			CalculateColorMap();
			PrepareIterators();
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Iterators != null)
				{
					foreach (var iterator in Iterators)
						iterator.Dispose();

					Iterators = null;
				}

				if (Finals != null)
				{
					foreach (var iterator in Finals)
						iterator.Dispose();

					Finals = null;
				}
			}

			mRenderer = null;
			Iterators = null;
			Finals = null;
			mFlame = null;
		}

		[NotNull]
		public Flame Flame
		{
			get { return mFlame; }
		}

		/// <summary>
		/// 2 ^ zoom
		/// </summary>
		public double TwoPowerZoom
		{
			get; 
			private set;
		}

		/// <summary>
		/// Points per pixel ("scale") adjusted to the output size
		/// flame_ppu * render_width / flame_canvas_width
		/// </summary>
		public double AdjustedPixelsPerUnit
		{
			get; 
			private set;
		}

		/// <summary>
		/// Color map as a 2D array
		/// </summary>
		public double[][] ColorMap
		{
			get; 
			private set;
		}

		/// <summary>
		/// AdjustedPixelsPerUnit * TwoPowerZoom
		/// 2 ^ zoom * flame_ppu * render_width / flame_canvas_width
		/// </summary>
		[NotNull]
		public Vector2 PointsPerUnit
		{
			get; 
			private set;
		}

		/// <summary>
		/// Maximum width of the outer margin of the buffer which contains the points being important to correctly blur the border of the image
		/// 0.5 * (25 - oversample)
		/// </summary>
		public int MaxGutterWidth
		{
			get; 
			private set;
		}

		/// <summary>
		/// Actual width of the outer margin of the buffer which contains the points being important to correctly blur the border of the image
		/// 0.5 * (filter_size - oversample)
		/// </summary>
		public int GutterWidth
		{
			get; 
			private set;
		}

		/// <summary>
		/// Total size of the histogram
		/// x = oversample * render_width + 2 * max_gutter_width
		/// y = oversample * render_height + 2 * max_gutter_width
		/// </summary>
		public Size HistogramSize
		{
			get; 
			private set;
		}

		/// <summary>
		/// Size of the histogram in memory
		/// histogram_size_x * histogram_size_y
		/// </summary>
		public int BufferLength
		{
			get; 
			private set;
		}

		/// <summary>
		/// Size of the filter kernel
		/// 3.6 * oversample * filter_radius
		/// </summary>
		public int FilterSize
		{
			get; 
			private set;
		}

		/// <summary>
		/// The filter kernel to blur the result image after oversampling
		/// </summary>
		public double[][] FilterKernel
		{
			get; 
			private set;
		}

		/// <summary>
		/// The rectangle in which points are visible
		/// </summary>
		public Rectangle CameraRectangle
		{
			get;
			private set;
		}

		/// <summary>
		/// The vector which is used to transform a point from units to pixels
		/// </summary>
		public Vector2 UnitToPixelFactor
		{
			get; 
			private set;
		}

		/// <summary>
		/// The vector which describes the zero position relative to the canvas
		/// </summary>
		public Vector2 ZeroPoint
		{
			get; 
			private set;
		}

		/// <summary>
		/// The cosine of the rotation angle
		/// </summary>
		public double CosAngle
		{
			get; 
			private set;
		}

		/// <summary>
		/// The sine of the rotation angle
		/// </summary>
		public double SinAngle
		{
			get;
			private set;
		}

		/// <summary>
		/// The 3D camera matrix (3x3)
		/// </summary>
		public double[][] Camera3D { get; private set; }

		/// <summary>
		/// The coefficient at which the blur strength of the depth of field effect is scaled
		/// 0.1 * dof
		/// </summary>
		public double DofCoeff { get; private set; }

		/// <summary>
		/// The value above which a point is considered "white" 
		/// constant at 200
		/// </summary>
		public double WhiteLevel
		{
			get { return mWhiteLevel; }
		}

		/// <summary>
		/// The array of affine iterators in the flame
		/// </summary>
		public IteratorData[] Iterators { get; private set; }

		/// <summary>
		/// The array of final iterators in the flame
		/// </summary>
		public IteratorData[] Finals { get; private set; }

		public bool ProjectPoint(ref Vector3 vector, Random random)
		{
			Project3DWithPitchYawDof(ref vector, random);

			var canvasProjection = System.Math.Abs(mFlame.Angle) < double.Epsilon
				? new Vector3(
					vector.X - CameraRectangle.TopLeft.X,
					vector.Y - CameraRectangle.TopLeft.Y,
					0)
				: new Vector3(
					(vector.X * CosAngle + vector.Y * SinAngle + ZeroPoint.X),
					(vector.Y * CosAngle - vector.X * SinAngle + ZeroPoint.Y),
					0);

			if (canvasProjection.X < 0 || canvasProjection.Y < 0 || 
				canvasProjection.X > CameraRectangle.Size.X ||
			    canvasProjection.Y > CameraRectangle.Size.Y)
				return false;

			vector = canvasProjection;
			return true;
		}
		public void AdjustPixelsPerUnit(Size size)
		{
			AdjustedPixelsPerUnit = mFlame.PixelsPerUnit * size.Width / mFlame.CanvasSize.Width;
		}

		private void CalculateFilterKernel()
		{
			var fw = (int)(2 * mFilterCutoff * mRenderer.Oversample * mRenderer.FilterRadius);
			
			FilterSize = fw + 1;
			if ((FilterSize + mRenderer.Oversample)%2 != 0)
				FilterSize ++;

			FilterKernel = new double[FilterSize][];

			var adjust = fw > 0 ? (mFilterCutoff*FilterSize)/fw : 1;
			var total = 0.0;

			for (int i = 0; i < FilterSize; i++)
			{
				FilterKernel[i] = new double[FilterSize];
				for (int j = 0; j < FilterSize; j++)
				{
					var ii = ((2.0 * i + 1) / FilterSize - 1) * adjust;
					var jj = ((2.0 * j + 1) / FilterSize - 1) * adjust;

					FilterKernel[i][j] = System.Math.Exp(-2.0*(ii*ii + jj*jj));
					total += FilterKernel[i][j];
				}
			}

			for (int i = 0; i < FilterSize; i++)
				for (int j = 0; j < FilterSize; j++)
					FilterKernel[i][j] /= total;

		}
		private void CalculateBufferMetrics()
		{
			MaxGutterWidth = (mMaxFilterWidth - mRenderer.Oversample)/2;
			GutterWidth = (FilterSize - mRenderer.Oversample)/2;

			HistogramSize = new Size(
				mRenderer.Oversample * mRenderer.Size.Width + 2 * MaxGutterWidth,
				mRenderer.Oversample * mRenderer.Size.Height + 2 * MaxGutterWidth);
			BufferLength = HistogramSize.Width*HistogramSize.Height;
		}
		private void CalculateCamera()
		{
			if (AdjustedPixelsPerUnit <= 0)
			{
				AdjustPixelsPerUnit(mRenderer.Size);
			}

			UpdateCamera();
		}
		private void CalculateColorMap()
		{
			var colors = mFlame.Palette;
			ColorMap = new double[colors.Length][];

			for (int i = 0; i < colors.Length; i++)
			{
				ColorMap[i] = new double[3];
				ColorMap[i][0] = (colors[i].R * mWhiteLevel) / 256.0;
				ColorMap[i][1] = (colors[i].G * mWhiteLevel) / 256.0;
				ColorMap[i][2] = (colors[i].B * mWhiteLevel) / 256.0;
			}
		}

		private void PrepareIterators()
		{
			Iterators = mFlame.Iterators.Where(x => x.GroupIndex == 0).Select(x => new IteratorData(this, x)).ToArray();
			Finals = mFlame.Iterators.Where(x => x.GroupIndex == 1).Select(x => new IteratorData(this, x)).ToArray();

			foreach (var iterator in Iterators.Concat(Finals))
				iterator.Initialize();
		}
		private void UpdateCamera()
		{
			TwoPowerZoom = System.Math.Pow(2, mFlame.Zoom);

			PointsPerUnit = new Vector2
			{
				X = AdjustedPixelsPerUnit * TwoPowerZoom,
				Y = AdjustedPixelsPerUnit * TwoPowerZoom
			};

			var corner = new Vector2
			{
				X = mFlame.Origin.X - mRenderer.Size.Width / PointsPerUnit.X / 2.0,
				Y = mFlame.Origin.Y - mRenderer.Size.Height / PointsPerUnit.Y / 2.0
			};

			var t = new[]
			{
				GutterWidth / (mRenderer.Oversample * PointsPerUnit.X),
				GutterWidth / (mRenderer.Oversample * PointsPerUnit.Y),
				(2 * MaxGutterWidth - GutterWidth) / (mRenderer.Oversample * PointsPerUnit.X),
				(2 * MaxGutterWidth - GutterWidth) / (mRenderer.Oversample * PointsPerUnit.Y)
			};

			CameraRectangle = new Rectangle(
				corner - new Vector2(t[0], t[1]),
				new Vector2(
					mRenderer.Size.Width / PointsPerUnit.X + t[2] - t[0],
					mRenderer.Size.Height / PointsPerUnit.Y + t[3] - t[1]));

			UnitToPixelFactor = new Vector2
			{
				X = (HistogramSize.Width - 0.5) * ((System.Math.Abs(CameraRectangle.Size.X) > double.Epsilon) ? 1.0 / CameraRectangle.Size.X : 1),
				Y = (HistogramSize.Height - 0.5) * ((System.Math.Abs(CameraRectangle.Size.Y) > double.Epsilon) ? 1.0 / CameraRectangle.Size.Y : 1)
			};

			SinAngle = System.Math.Sin(mFlame.Angle);
			CosAngle = System.Math.Cos(mFlame.Angle);

			ZeroPoint = System.Math.Abs(mFlame.Angle) < double.Epsilon ? new Vector2() : new Vector2
			{
				X = mFlame.Origin.X * (1 - CosAngle) - mFlame.Origin.Y * SinAngle - CameraRectangle.TopLeft.X,
				Y = mFlame.Origin.Y * (1 - CosAngle) + mFlame.Origin.X * SinAngle - CameraRectangle.TopLeft.Y
			};

			Camera3D = new[]
			{
				new[]
				{
					System.Math.Cos(-mFlame.Yaw), 
					-System.Math.Sin(-mFlame.Yaw), 
					0
				},
				new[]
				{
					System.Math.Cos(mFlame.Pitch) * System.Math.Sin(-mFlame.Yaw), 
					System.Math.Cos(mFlame.Pitch) * System.Math.Cos(-mFlame.Yaw), 
					-System.Math.Sin(mFlame.Pitch)
				},
				new[]
				{
					System.Math.Sin(mFlame.Pitch) * System.Math.Sin(-mFlame.Yaw), 
					System.Math.Sin(mFlame.Pitch) * System.Math.Cos(-mFlame.Yaw), 
					System.Math.Cos(mFlame.Pitch)
				}
			};

			DofCoeff = 0.1 * mFlame.DepthOfField;
		}

		private void Project3DWithPitchYawDof(ref Vector3 vector, Random random)
		{
			var z = vector.Z - mFlame.Height;
			var projectionVector = new Vector3(
				Camera3D[0][0] * vector.X + Camera3D[1][0] * vector.Y,
				Camera3D[0][1] * vector.X + Camera3D[1][1] * vector.Y + Camera3D[2][1] * z,
				Camera3D[0][2] * vector.X + Camera3D[1][2] * vector.Y + Camera3D[2][2] * z);
			var zr = 1 - mFlame.Perspective * projectionVector.Z;
			var t = random.NextDouble() * 2 * System.Math.PI;
			var dsin = System.Math.Sin(t);
			var dcos = System.Math.Cos(t);
			var dr = random.NextDouble() * DofCoeff * z;

			var camera3DProjection = new Vector3(
				(projectionVector.X + dr * dcos) / zr,
				(projectionVector.Y + dr * dsin) / zr,
				vector.Z - mFlame.Height);

			vector = camera3DProjection;
		}
	}
}