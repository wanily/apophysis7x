using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class TransformCollectionVisual : CanvasVisual<Canvas>
	{
		private TransformCollection mCollection;
		private List<TransformVisual> mVisuals;
		private bool mIsDisposed;

		public TransformCollectionVisual([NotNull] Canvas canvas, [NotNull] TransformCollection collection) : base(canvas)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			mCollection = collection;
			mVisuals = new List<TransformVisual>();

			mCollection.ContentChanged += OnCollectionChanged;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				DestroyVisuals();
			}

			mVisuals = null;
			mCollection = null;
			mIsDisposed = true;
		}

		private void OnCollectionChanged(object sender, EventArgs eventArgs)
		{
			CreateVisuals();
			InvalidateControl();
		}
		protected override void OnControlPaint(Graphics graphics)
		{
			if (mIsDisposed)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Paint(graphics);
			}
		}

		protected override void RegisterEvents(Control control)
		{
			base.RegisterEvents(control);
			if (mIsDisposed)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Detach();
				visual.Attach(control);
			}
		}
		protected override void UnregisterEvents(Control control)
		{
			base.UnregisterEvents(control);
			if (mIsDisposed)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Detach();
			}
		}

		private void CreateVisuals()
		{
			if (mIsDisposed)
				return;

			DestroyVisuals();

			foreach (var transform in mCollection)
			{
				var visual = new TransformVisual(Canvas, transform);
				if (AttachedControl != null)
				{
					visual.Attach(AttachedControl);
				}
				mVisuals.Add(visual);
			}
		}
		private void DestroyVisuals()
		{
			if (mIsDisposed)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Dispose();
			}

			mVisuals.Clear();
		}
	}
}