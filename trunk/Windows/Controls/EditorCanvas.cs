using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Drawing;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class EditorCanvas : UserControl
	{
		private ControlVisualChain mVisual;
		private InteractionHandlerChain mInteraction;

		private GridVisual mGridPainter;
		private GridRulerVisual mRulerPainter;
		private TransformCollectionVisual mTransformPainter;

		public EditorCanvas()
		{
			InitializeComponent();

			var grid = new Grid(new Vector2(Width, Height));

			mVisual = new ControlVisualChain(this);
			mInteraction = new InteractionHandlerChain(this);
			
			mVisual.Add(mGridPainter = new GridVisual(this, grid));
			mVisual.Add(mTransformPainter = new TransformCollectionVisual(this, grid), 100);
			mVisual.Add(mRulerPainter = new GridRulerVisual(this, grid), int.MaxValue);

			mInteraction.Add(new GridInteractionStrategy(this, grid), int.MaxValue);
			mInteraction.Add(new TransformCollectionInteractionHandler(this, mTransformPainter, grid), 100);

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

				if (mInteraction != null)
				{
					mInteraction.Dispose();
					mInteraction = null;
				}

				if (mVisual != null)
				{
					mVisual.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		public TransformCollection Transforms
		{
			get { return mTransformPainter.Collection; }
			set { mTransformPainter.Collection = value; }
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
	}
}
