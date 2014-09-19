using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Xyrus.Apophysis.Windows;

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

		public int Add()
		{
			return Add(0);
		}
		public int Add(int groupIndex)
		{
			Items.Add(new Iterator(mFlame) { GroupIndex = groupIndex });
			Items.Sort(x => x.GroupIndex, x => x.GroupItemIndex);

			RaiseContentChanged();

			return Count - 1;
		}
		public int Add([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			Items.Add(iterator);
			Items.Sort(x => x.GroupIndex, x => x.GroupItemIndex);

			RaiseContentChanged();

			return Count - 1;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= Count) return false;
			if (!CanRemove(this[index].GroupIndex)) throw new ApophysisException("Can't remove last primary iterator of flame");

			Items.RemoveAt(index);
			RaiseContentChanged();

			return true;
		}
		public bool Remove([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			if (!CanRemove(iterator.GroupIndex)) throw new ApophysisException("Can't remove last primary iterator of flame");

			if (!Contains(iterator)) 
				return false;

			Items.Remove(iterator);
			RaiseContentChanged();

			return true;
		}
		public bool CanRemove(int groupIndex)
		{
			if (groupIndex == 0)
			{
				return this.Count(x => x.GroupIndex == 0) > 1;
			}

			return true;
		}

		[NotNull]
		internal Iterator ConvertIterator([NotNull] Iterator iterator, int groupIndex)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			var copy = iterator.Copy();

			copy.GroupIndex = groupIndex;
			copy.Weight = groupIndex == 1 ? 0.5 : copy.Weight;
			copy.ColorSpeed = groupIndex == 1 ? 0 : copy.ColorSpeed;
			copy.Opacity = groupIndex == 1 ? 1 : copy.Opacity;

			Items.Remove(iterator);
			Items.Add(copy);
			Items.Sort(x => x.GroupIndex, x => x.GroupItemIndex);
			
			RaiseContentChanged();
			return copy;
		}

		public void Reset()
		{
			Items.Clear();
			Items.Add(new Iterator(mFlame));

			RaiseContentChanged();
		}

		private void RaiseContentChanged()
		{
			if (mContentChanged == null)
				return;

			mContentChanged(this, new EventArgs());
		}
		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
		}

		[NotNull]
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

		public void ReadXml([NotNull] IEnumerable<XElement> elements)
		{
			// ReSharper disable once ConstantNullCoalescingCondition
			var array = (elements ?? new XElement[0]).Where(x => x != null).ToArray();

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

			newCollection.Sort(x => x.GroupIndex, x => x.GroupItemIndex);

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

			return !iterators.Where((t, i) => !t.IsEqual(this[i])).Any();
		}

		public void NormalizeWeights()
		{
			var total = 0.0;
			for (int i = 0; i < Count; i++)
			{
				total += this[i].Weight;
			}

			if (total < 0.001)
			{
				for (int i = 0; i < Count; i++)
				{
					total = 1.0 / total;
				}
			}
			else
			{
				for (int i = 0; i < Count; i++)
				{
					total /= total;
				}
			}
		}

		public void WriteXml([NotNull] Collection<XElement> iteratorElements)
		{
			if (iteratorElements == null) throw new ArgumentNullException("iteratorElements");

			foreach (var iterator in this.OrderBy(x => x.GroupIndex))
			{
				XElement element;
				iterator.WriteXml(out element);
				iteratorElements.Add(element);
			}
		}
	}
}