using System;
using System.Collections;
using System.Collections.Generic;
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
		private GridVisual mVisual;
		private GridRulerVisual mRulers;
		private List<TransformVisual> mTransformVisuals;

		public EditorCanvas()
		{
			InitializeComponent();

			mGrid = new Grid(new Vector2(Width, Height));

			mNavigation = new GridNavigationStrategy(mGrid);
			mNavigation.Attach(this);

			mVisual = new GridVisual(mGrid);
			mVisual.Attach(this);

			mRulers = new GridRulerVisual(mGrid);
			mRulers.Attach(this);

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

				if (mTransforms != null)
				{
					mTransforms.ContentChanged -= OnTransformCollectionChanged;
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
				if (mTransforms != null)
					mTransforms.ContentChanged -= OnTransformCollectionChanged;

				mTransforms = value;

				if (value != null)
					value.ContentChanged += OnTransformCollectionChanged;

				RebuildInterceptors();
			}
		}
		public Color GridZeroLineColor
		{
			get { return mVisual.GridZeroLineColor; }
			set { mVisual.GridZeroLineColor = value; }
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

		public bool ShowRuler
		{
			get { return mRulers.ShowLabels || mRulers.ShowVertical || mRulers.ShowHorizontal; }
			set
			{
				mRulers.ShowLabels = value;
				mRulers.ShowHorizontal = value;
				mRulers.ShowVertical = value;
			}
		}

		private void RebuildInterceptors()
		{
			DestroyInterceptors();

			mTransformVisuals = new List<TransformVisual>();
			foreach (var transform in (IEnumerable<Transform>)mTransforms ?? new Transform[0])
			{
				var visual = new TransformVisual(mGrid, transform);

				visual.Attach(this);
				mTransformVisuals.Add(visual);
			}
		}
		private void DestroyInterceptors()
		{
			if (mTransformVisuals != null)
			{
				foreach (var visual in mTransformVisuals)
					visual.Dispose();

				mTransformVisuals.Clear();
				mTransformVisuals = null;
			}
		}

		private void OnTransformCollectionChanged(object sender, EventArgs e)
		{
			//RebuildInterceptors();
			Refresh();
		}
	}
}
