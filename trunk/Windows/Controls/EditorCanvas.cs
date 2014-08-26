using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Drawing;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class EditorCanvas : UserControl
	{
		private Grid mGrid;
		private TransformCollection mTransforms;

		private GridNavigationStrategy mNavigation;
		private ControlPaintingChain mPainting;

		private GridVisual mGridPainter;
		private GridRulerVisual mRulerPainter;
		private TransformCollectionVisual mTransformPainter;

		public EditorCanvas()
		{
			InitializeComponent();

			mGrid = new Grid(new Vector2(Width, Height));

			mPainting = new ControlPaintingChain(this);
			
			mPainting.Add(mGridPainter = new GridVisual(this, mGrid));
			mPainting.Add(mRulerPainter = new GridRulerVisual(this, mGrid), int.MaxValue);

			mNavigation = new GridNavigationStrategy(this, mGrid);

			RebuildInterceptors();

			GridLineColor = Color.FromArgb(0xff, 0x66, 0x66, 0x66);
			BackdropColor = Color.Transparent;
			GridZeroLineColor = Color.FromArgb(0xff, 0xbb, 0xbb, 0xbb);

			RulerGridLineColor = Color.Gray;
			RulerBackgroundColor = Color.FromArgb(0xff, 0x33, 0x33, 0x33);
			RulerBackdropColor = Color.Transparent;

			ShowRuler = true;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				if (mNavigation != null)
				{
					mNavigation.Dispose();
					mNavigation = null;
				}

				if (mPainting != null)
				{
					mPainting.Dispose();
				}

				if (mTransforms != null)
				{
					DestroyInterceptors();
				}
			}

			base.Dispose(disposing);
		}

		public TransformCollection Transforms
		{
			get { return mTransforms; }
			set
			{
				mTransforms = value;
				RebuildInterceptors();
			}
		}
		public Color GridZeroLineColor
		{
			get { return mGridPainter.GridZeroLineColor; }
			set { mGridPainter.GridZeroLineColor = value; }
		}
		public Color GridLineColor
		{
			get { return mGridPainter.GridLineColor; }
			set { mGridPainter.GridLineColor = value; }
		}
		public Color BackdropColor
		{
			get { return mGridPainter.BackdropColor; }
			set { mGridPainter.BackdropColor = value; }
		}

		public Color RulerGridLineColor
		{
			get { return mRulerPainter.GridLineColor; }
			set { mRulerPainter.GridLineColor = value; }
		}
		public Color RulerBackdropColor
		{
			get { return mRulerPainter.BackdropColor; }
			set { mRulerPainter.BackdropColor = value; }
		}
		public Color RulerBackgroundColor
		{
			get { return mRulerPainter.BackgroundColor; }
			set { mRulerPainter.BackgroundColor = value; }
		}

		public bool ShowRuler
		{
			get { return mRulerPainter.ShowLabels || mRulerPainter.ShowVertical || mRulerPainter.ShowHorizontal; }
			set
			{
				mRulerPainter.ShowLabels = value;
				mRulerPainter.ShowHorizontal = value;
				mRulerPainter.ShowVertical = value;
			}
		}

		private void RebuildInterceptors()
		{
			DestroyInterceptors();

			if (mTransforms == null)
			{
				mTransformPainter = null;
				return;
			}

			mPainting.Add(mTransformPainter = new TransformCollectionVisual(this, mGrid, mTransforms), 100);
		}
		private void DestroyInterceptors()
		{
			if (mTransformPainter != null)
			{
				mPainting.Remove(mTransformPainter);

				mTransformPainter.Dispose();
				mTransformPainter = null;
			}
		}
	}
}
