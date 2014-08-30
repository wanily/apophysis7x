using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Xyrus.Apophysis.Windows.Models
{
	[PublicAPI]
	public class IteratorCollection : ReadOnlyCollection<Iterator>
	{
		private readonly Flame mFlame;
		private EventHandler mContentChanged;

		public IteratorCollection([NotNull] Flame hostingFlame) : base(new List<Iterator>())
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;
			Items.Add(new Iterator(mFlame));
		}

		private void RaiseContentChanged()
		{
			if (mContentChanged == null)
				return;

			mContentChanged(this, new EventArgs());
		}

		public int Add()
		{
			Items.Add(new Iterator(mFlame));
			RaiseContentChanged();

			return Count - 1;
		}
		public int Add([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			Items.Add(iterator);
			RaiseContentChanged();

			return Count - 1;
		}

		public bool Remove(int index)
		{
			if (Count <= 1) throw new ApophysisException("Can't remove last iterator of flame");

			if (index < 0 || index >= Count)
				return false;

			Items.RemoveAt(index);
			RaiseContentChanged();

			return true;
		}
		public bool Remove([NotNull] Iterator iterator)
		{
			if (Count <= 1) throw new ApophysisException("Can't remove last iterator of flame");

			if (!Contains(iterator)) 
				return false;

			Items.Remove(iterator);
			RaiseContentChanged();

			return true;
		}

		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
		}
	}
}