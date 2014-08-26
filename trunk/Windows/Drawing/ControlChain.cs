using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public abstract class ControlChain<T> : ControlEventInterceptor where T : ChainItem
	{
		class PriorizedChainItem
		{
			[NotNull]
			public T Handler;
			public int Priority;
		}

		private List<PriorizedChainItem> mChain;

		protected ControlChain([NotNull] Control control) : base(control)
		{
			mChain = new List<PriorizedChainItem>();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mChain != null)
			{
				foreach (var item in mChain)
					item.Handler.Dispose();

				mChain.Clear();
				mChain = null;
			}
		}

		protected IEnumerable<T> GetChainItems()
		{
			if (mChain == null)
				return new T[0];

			return mChain.OrderBy(x => x.Priority).Select(x => x.Handler);
		}

		public void Add([NotNull] T handler, int priority = 1)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			if (priority < 1) throw new ArgumentOutOfRangeException("priority");

			mChain.Add(new PriorizedChainItem { Handler = handler, Priority = priority });
		}
		public void Remove([NotNull] T painter)
		{
			if (painter == null) throw new ArgumentNullException("painter");

			if (mChain == null)
				return;

			var itemsToRemove = mChain.Where(x => ReferenceEquals(painter, x.Handler));
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