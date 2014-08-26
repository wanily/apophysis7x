using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class TransformCollectionInteractionHandler : InteractionHandler, IEnumerable<TransformInteractionHandler>
	{
		private TransformCollectionVisual mVisualCollection;
		private List<TransformInteractionHandler> mHandlers;

		public TransformCollectionInteractionHandler([NotNull] Control control, [NotNull] TransformCollectionVisual visualCollection) : base(control)
		{
			if (visualCollection == null) throw new ArgumentNullException("visualCollection");

			mVisualCollection = visualCollection;
			mVisualCollection.ContentChanged += OnCollectionChanged;

			mHandlers = new List<TransformInteractionHandler>();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mVisualCollection != null)
			{
				mVisualCollection.ContentChanged += OnCollectionChanged;

				//disposed somewhere else
				mVisualCollection = null;
			}
		}

		public TransformCollection Collection
		{
			get { return mVisualCollection.Collection; }
		}
		public TransformInteractionHandler this[int index]
		{
			get
			{
				if (mHandlers == null)
					throw new IndexOutOfRangeException();

				if (index < 0 || index >= mHandlers.Count)
					throw new IndexOutOfRangeException();

				return mHandlers[index];
			}
		}

		private void OnCollectionChanged(object sender, EventArgs eventArgs)
		{
			if (mHandlers == null || mVisualCollection == null)
				return;

			foreach (var handler in mHandlers)
			{
				handler.Dispose();
			}

			mHandlers.Clear();

			foreach (var visual in mVisualCollection.Reverse())
			{
				mHandlers.Add(new TransformInteractionHandler(AttachedControl, visual));
			}

			InvalidateControl();
		}

		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mHandlers == null)
				return false;

			foreach (var visual in mVisualCollection)
			{
				visual.Reset();
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseMove(cursor, button))
					return true;
			}

			return false;
		}
		protected override bool OnAttachedControlMouseWheel(double delta, MouseButtons button)
		{
			if (mHandlers == null)
				return false;

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseWheel(delta, button))
					return true;
			}

			return false;
		}

		protected override bool OnAttachedControlMouseDown(Vector2 cursor)
		{
			if (mHandlers == null)
				return false;

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseDown(cursor))
					return true;
			}

			return false;
		}
		protected override bool OnAttachedControlMouseUp()
		{
			if (mHandlers == null)
				return false;

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseUp())
					return true;
			}

			return false;
		}

		protected override bool OnAttachedControlMouseDoubleClick()
		{
			if (mHandlers == null)
				return false;

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseDoubleClick())
					return true;
			}

			return false;
		}

		public IEnumerator<TransformInteractionHandler> GetEnumerator()
		{
			if (mHandlers == null)
				return new List<TransformInteractionHandler>().GetEnumerator();

			return mHandlers.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}