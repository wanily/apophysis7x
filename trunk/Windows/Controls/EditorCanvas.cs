using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Visuals;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

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

		private TransformUpdatedEventHandler mTransformUpdated;
		private TransformHitEventHandler mTransformHit;
		private EventHandler mTransformHitCleared;

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
			}
		}
		public bool HighlightOrigin
		{
			get { return mGridPainter.HighlightOrigin; }
			set { mGridPainter.HighlightOrigin = value; }
		}

		private void OnTransformUpdated(object sender, TransformUpdatedEventArgs args)
		{
			RaiseTransformUpdated(args.Operation);
		}
		protected void RaiseTransformUpdated([NotNull] TransformInputOperation operation)
		{
			if (mTransformUpdated != null)
				mTransformUpdated(this, new TransformUpdatedEventArgs(operation));
		}

		private void OnTransformHit(object sender, TransformHitEventArgs args)
		{
			RaiseTransformHit(args.Operation);
		}
		protected void RaiseTransformHit([NotNull] TransformMouseOverOperation operation)
		{
			if (mTransformHit != null)
				mTransformHit(this, new TransformHitEventArgs(operation));
		}

		private void OnTransformHitCleared(object sender, EventArgs args)
		{
			RaiseTransformHitCleared();
		}
		protected void RaiseTransformHitCleared()
		{
			if (mTransformHitCleared != null)
				mTransformHitCleared(this, new EventArgs());
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

		public event TransformUpdatedEventHandler TransformUpdated
		{
			add { mTransformUpdated += value; }
			remove { mTransformUpdated -= value; }
		}
		public event TransformHitEventHandler TransformHit
		{
			add { mTransformHit += value; }
			remove { mTransformHit -= value; }
		}
		public event EventHandler TransformHitCleared
		{
			add { mTransformHitCleared += value; }
			remove { mTransformHitCleared -= value; }
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
	}
}
