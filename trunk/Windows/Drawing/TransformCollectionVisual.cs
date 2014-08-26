using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class TransformCollectionVisual : CanvasVisual<Canvas>, IEnumerable<TransformVisual>
	{
		private TransformCollection mCollection;
		private List<TransformVisual> mVisuals;

		private EventHandler mContentChanged;

		public TransformCollectionVisual([NotNull] Control control, [NotNull] Canvas canvas) : base(control, canvas)
		{
			mVisuals = new List<TransformVisual>();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mCollection != null)
			{
				mCollection.ContentChanged -= OnCollectionChanged;
				mCollection = null;
			}

			if (mVisuals != null)
			{
				foreach (var visual in mVisuals)
				{
					visual.Dispose();
				}

				mVisuals.Clear();
				mVisuals = null;
			}
		}

		public TransformCollection Collection
		{
			get { return mCollection; }
			set
			{
				if (mCollection != null)
				{
					mCollection.ContentChanged -= OnCollectionChanged;
				}

				if (value != null)
				{
					value.ContentChanged += OnCollectionChanged;
				}

				mCollection = value;
				OnCollectionChanged(mCollection, new EventArgs());
			}
		}
		public TransformVisual this[int index]
		{
			get
			{
				if (mVisuals == null)
					throw new IndexOutOfRangeException();

				if (index < 0 || index >= mVisuals.Count)
					throw new IndexOutOfRangeException();

				return mVisuals[index];
			}
		}

		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
		}

		private void OnCollectionChanged(object sender, EventArgs eventArgs)
		{
			if (mVisuals == null || mCollection == null)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Dispose();
			}

			mVisuals.Clear();

			foreach (var transform in mCollection)
			{
				mVisuals.Add(new TransformVisual(AttachedControl, Canvas, transform));
			}

			if (mContentChanged != null)
				mContentChanged(this, new EventArgs());

			InvalidateControl();
		}
		protected override void OnControlPaint(Graphics graphics)
		{
			if (mVisuals == null)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Paint(graphics);
			}
		}

		public IEnumerator<TransformVisual> GetEnumerator()
		{
			if (mVisuals == null)
				return new List<TransformVisual>().GetEnumerator();

			return mVisuals.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}