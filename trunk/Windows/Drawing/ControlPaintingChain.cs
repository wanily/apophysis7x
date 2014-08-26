using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Drawing
{
	public class ControlPaintingChain : ControlEventInterceptor
	{
		class PriorizedChainItem
		{
			[NotNull]
			public ControlPainter Painter;
			public int Priority;
		}

		private List<PriorizedChainItem> mChain;

		public ControlPaintingChain([NotNull] Control control) : base(control)
		{
			mChain = new List<PriorizedChainItem>();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mChain != null)
			{
				foreach (var item in mChain)
					item.Painter.Dispose();

				mChain.Clear();
				mChain = null;
			}
		}

		protected sealed override void RegisterEvents(Control control)
		{
			control.Paint += OnControlPaint;
		}
		protected sealed override void UnregisterEvents(Control control)
		{
			control.Paint -= OnControlPaint;
		}

		private void OnControlPaint(object sender, PaintEventArgs e)
		{
			OnControlPaint(e.Graphics);
		}
		private void OnControlPaint(Graphics graphics)
		{
			if (mChain == null)
				return;

			var ordered = mChain.OrderBy(x => x.Priority);
			foreach (var item in ordered)
			{
				item.Painter.Paint(graphics);
			}
		}

		public void Paint()
		{
			if (AttachedControl == null)
				return;

			using (var graphics = AttachedControl.CreateGraphics())
			{
				OnControlPaint(graphics);
			}
		}
		public void Paint([NotNull] Graphics graphics)
		{
			if (graphics == null) throw new ArgumentNullException("graphics");
			OnControlPaint(graphics);
		}

		public void Add([NotNull] ControlPainter painter, int priority = 1)
		{
			if (painter == null) throw new ArgumentNullException("painter");
			if (priority < 1) throw new ArgumentOutOfRangeException("priority");

			mChain.Add(new PriorizedChainItem {Painter = painter, Priority = priority});
		}
		public void Remove([NotNull] ControlPainter painter)
		{
			if (painter == null) throw new ArgumentNullException("painter");

			if (mChain == null)
				return;

			var itemsToRemove = mChain.Where(x => ReferenceEquals(painter, x.Painter));
			foreach (var item in itemsToRemove)
			{
				mChain.Remove(item);
			}
		}
		public void Clear()
		{
			mChain.Clear();
		}
	}
}