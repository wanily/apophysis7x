using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class TransformCollectionVisual : CanvasVisual<Canvas>
	{
		private TransformCollection mCollection;

		public TransformCollectionVisual([NotNull] Control control, [NotNull] Canvas canvas, [NotNull] TransformCollection collection) : base(control, canvas)
		{
			if (collection == null) throw new ArgumentNullException("collection");

			mCollection = collection;
			mCollection.ContentChanged += OnCollectionChanged;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mCollection != null)
			{
				mCollection.ContentChanged -= OnCollectionChanged;
				mCollection = null;
			}
		}

		private void OnCollectionChanged(object sender, EventArgs eventArgs)
		{
			InvalidateControl();
		}
		protected override void OnControlPaint(Graphics graphics)
		{
			if (mCollection == null)
				return;

			foreach (var item in mCollection)
			{
				using (var visual = new TransformVisual(AttachedControl, Canvas, item))
				{
					visual.Paint(graphics);
				}
			}
		}
	}
}