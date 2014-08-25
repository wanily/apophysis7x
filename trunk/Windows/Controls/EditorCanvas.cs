using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Drawing;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class EditorCanvas : UserControl
	{
		private TransformCollection mTransforms;

		private Vector2 mDragStart;
		private Vector2 mOffsetStart;
		private bool mIsDragging;

		private GridNavigationStrategy mNavigation;
		private GridVisual mVisual;
		private GridRulerVisual mRulers;

		public EditorCanvas()
		{
			InitializeComponent();

			var grid = new Grid(new Vector2(Width, Height));

			mNavigation = new GridNavigationStrategy(grid);
			mNavigation.Attach(this);

			mVisual = new GridVisual(grid);
			mVisual.Attach(this);

			mRulers = new GridRulerVisual(grid);
			mRulers.Attach(this);
			mRulers.ShowVertical = true;

			GridLineColor = Color.Gray;
			BackdropColor = Color.Transparent;

			RulerGridLineColor = Color.Gray;
			RulerBackgroundColor = Color.FromArgb(0xff, 0x33, 0x33, 0x33);
			RulerBackdropColor = Color.FromArgb(0xff, 0x55, 0x55, 0x55);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				if (mTransforms != null)
				{
					mTransforms.ContentChanged -= OnTransformCollectionChanged;
					mTransforms = null;
				}

				if (mNavigation != null)
				{
					mNavigation.Dispose();
					mNavigation = null;
				}

				if (mVisual != null)
				{
					mVisual.Dispose();
					mVisual = null;
				}

				if (mRulers != null)
				{
					mRulers.Dispose();
					mRulers = null;
				}
			}

			base.Dispose(disposing);
		}

		public TransformCollection Transforms
		{
			get { return mTransforms; }
			set
			{
				if (mTransforms != null)
					mTransforms.ContentChanged -= OnTransformCollectionChanged;

				mTransforms = value;

				if (value != null)
					value.ContentChanged += OnTransformCollectionChanged;
			}
		}
		public Color GridLineColor
		{
			get { return mVisual.GridLineColor; }
			set { mVisual.GridLineColor = value; }
		}
		public Color BackdropColor
		{
			get { return mVisual.BackdropColor; }
			set { mVisual.BackdropColor = value; }
		}

		public Color RulerGridLineColor
		{
			get { return mRulers.GridLineColor; }
			set { mRulers.GridLineColor = value; }
		}
		public Color RulerBackdropColor
		{
			get { return mRulers.BackdropColor; }
			set { mRulers.BackdropColor = value; }
		}
		public Color RulerBackgroundColor
		{
			get { return mRulers.BackgroundColor; }
			set { mRulers.BackgroundColor = value; }
		}

		private void OnTransformCollectionChanged(object sender, EventArgs e)
		{
			Refresh();
		}
	}
}
