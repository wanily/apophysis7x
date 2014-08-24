using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Xyrus.Apophysis.Windows.Models
{
	public class TransformCollection : ReadOnlyCollection<Transform>
	{
		private readonly Flame mFlame;
		private EventHandler mContentChanged;

		public TransformCollection(Flame hostingFlame) : base(new List<Transform>())
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;
			Items.Add(new Transform(mFlame));
		}

		private void RaiseContentChanged()
		{
			if (mContentChanged == null)
				return;

			mContentChanged(this, new EventArgs());
		}

		public int Add()
		{
			Items.Add(new Transform(mFlame));
			RaiseContentChanged();

			return Count - 1;
		}
		public int Add(Transform transform)
		{
			if (transform == null) throw new ArgumentNullException("transform");

			Items.Add(transform);
			RaiseContentChanged();

			return Count - 1;
		}

		public bool Remove(int index)
		{
			if (Count <= 1) throw new ApophysisException("Can't remove last transform of flame");

			if (index < 0 || index >= Count)
				return false;

			Items.RemoveAt(index);
			RaiseContentChanged();

			return true;
		}
		public bool Remove(Transform transform)
		{
			if (Count <= 1) throw new ApophysisException("Can't remove last transform of flame");

			if (!Contains(transform)) 
				return false;

			Items.Remove(transform);
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