using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Visuals;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class EditorCanvas : UserControl
	{
		private ControlVisualChain mVisual;
		private InputHandlerChain mInteraction;

		private GridVisual mGridPainter;
		private GridRulerVisual mRulerPainter;
		private TransformCollectionVisual mTransformPainter;

		private GridInputStrategy mGridInteraction;
		private TransformCollectionInputHandler mTransformInteraction;

		private EventHandler mBeginEdit;
		private EventHandler mEndEdit;

		public EditorCanvas()
		{
			InitializeComponent();

			var grid = new Grid(new Vector2(Width, Height));

			mVisual = new ControlVisualChain(this);
			mInteraction = new InputHandlerChain(this);
			
			mVisual.Add(mGridPainter = new GridVisual(this, grid));
			mVisual.Add(mTransformPainter = new TransformCollectionVisual(this, grid), 100);
			mVisual.Add(mRulerPainter = new GridRulerVisual(this, grid), int.MaxValue);

			mInteraction.Add(mGridInteraction = new GridInputStrategy(this, grid), int.MaxValue);
			mInteraction.Add(mTransformInteraction = new TransformCollectionInputHandler(this, mTransformPainter, grid), 100);

			mTransformInteraction.BeginEdit += OnBeginEdit;
			mTransformInteraction.EndEdit += OnEndEdit;

			GridLineColor = Color.FromArgb(0xff, 0x66, 0x66, 0x66);
			BackdropColor = Color.Transparent;
			GridZeroLineColor = Color.FromArgb(0xff, 0xbb, 0xbb, 0xbb);

			RulerGridLineColor = Color.Gray;
			RulerBackgroundColor = Color.FromArgb(0xff, 0x33, 0x33, 0x33);
			RulerBackdropColor = Color.Transparent;

			ShowRuler = true;

			Settings.MoveAmount = 0.5;
			Settings.AngleSnap = 15;
			Settings.ScaleSnap = 125;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				if (mTransformInteraction != null)
				{
					mTransformInteraction.BeginEdit -= OnBeginEdit;
					mTransformInteraction.EndEdit -= OnEndEdit;
				}

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

		public Color ReferenceColor
		{
			get { return mTransformPainter.ReferenceColor; }
			set { mTransformPainter.ReferenceColor = value; }
		}

		public EditorSettings Settings
		{
			get { return mTransformInteraction.Settings; }
		}
		public Vector2 CursorPosition
		{
			get { return mGridInteraction.Canvas.CanvasToWorld(mInteraction.CursorPosition); }
		}

		public bool ShowRuler
		{
			get { return mRulerPainter.ShowLabels || mRulerPainter.ShowVertical || mRulerPainter.ShowHorizontal; }
			set
			{
				mRulerPainter.ShowLabels = value;
				mRulerPainter.ShowHorizontal = value;
				mRulerPainter.ShowVertical = value;

				Point lt, rb;

				if (value)
				{
					lt = new Point(mRulerPainter.RulerSize + 10, mRulerPainter.RulerSize + 10);
					rb = new Point(10, 10);
				}
				else
				{
					lt = new Point(10, 10);
					rb = new Point(10, 10);
				}

				mTransformPainter.HintTextRectangle = new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
				Refresh();
			}
		}
		public bool HighlightOrigin
		{
			get { return mGridPainter.HighlightOrigin; }
			set { mGridPainter.HighlightOrigin = value; }
		}

		private void OnBeginEdit(object sender, EventArgs args)
		{
			RaiseBeginEdit();
		}
		protected void RaiseBeginEdit()
		{
			if (mBeginEdit != null)
				mBeginEdit(this, new EventArgs());
		}

		private void OnEndEdit(object sender, EventArgs args)
		{
			RaiseEndEdit();
		}
		protected void RaiseEndEdit()
		{
			if (mEndEdit != null)
				mEndEdit(this, new EventArgs());
		}

		public event EventHandler BeginEdit
		{
			add { mBeginEdit += value; }
			remove { mBeginEdit -= value; }
		}
		public event EventHandler EndEdit
		{
			add { mEndEdit += value; }
			remove { mEndEdit -= value; }
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			mInteraction.TriggerKeyPress(keyData);

			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
