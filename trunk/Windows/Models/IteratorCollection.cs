using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
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

		public void Reset()
		{
			Items.Clear();
			Items.Add(new Iterator(mFlame));

			RaiseContentChanged();
		}

		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
		}

		internal IteratorCollection Copy([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			var copy = new IteratorCollection(flame);

			copy.Items.Clear();
			foreach (var item in Items)
			{
				copy.Items.Add(item.Copy(flame));
			}

			return copy;
		}

		public void ReadXml(IEnumerable<XElement> elements)
		{
			var array = (elements ?? new XElement[0]).ToArray();

			if (!array.Any())
			{
				throw new ApophysisException("No transforms in flame");
			}

			var counter = 1;
			var newCollection = new List<Iterator>();

			foreach (var element in array)
			{
				var iterator = new Iterator(mFlame);

				try
				{
					iterator.ReadXml(element);
					newCollection.Add(iterator);
				}
				catch (ApophysisException exception)
				{
					throw new ApophysisException("Transform #" + counter + ": " + exception.Message, exception);
				}

				counter++;
			}

			Items.Clear();
			foreach (var item in newCollection)
			{
				Items.Add(item);
			}

			RaiseContentChanged();
		}

		public bool IsEqual([NotNull] IteratorCollection iterators)
		{
			if (iterators == null) throw new ArgumentNullException("iterators");

			if (!Equals(iterators.Count, Count))
				return false;

			for (int i = 0; i < iterators.Count; i++)
			{
				if (!iterators[i].IsEqual(this[i]))
					return false;
			}

			return true;
		}
	}
}