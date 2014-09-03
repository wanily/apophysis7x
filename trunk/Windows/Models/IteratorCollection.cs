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
		private Iterator mFinalIterator;
		private bool mUseFinalIterator;

		public IteratorCollection([NotNull] Flame hostingFlame) : base(new List<Iterator>())
		{
			if (hostingFlame == null) throw new ArgumentNullException("hostingFlame");

			mFlame = hostingFlame;
			mFinalIterator = new Iterator(mFlame);
			mUseFinalIterator = false;

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

		[NotNull]
		public Iterator FinalIterator
		{
			get { return mFinalIterator; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mFinalIterator = value;
			}
		}
		public bool UseFinalIterator
		{
			get { return mUseFinalIterator; }
			set { mUseFinalIterator = value; }
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

			copy.mFinalIterator = mFinalIterator.Copy();
			copy.mUseFinalIterator = mUseFinalIterator;

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

			var finalIterator = new Iterator(mFlame);
			var finalIteratorElement = array.FirstOrDefault(x => x.Name.ToString().ToLower() == "finalxform");
			var finalIteratorEnabled = true;

			if (finalIteratorElement != null)
			{
				try
				{
					finalIterator.ReadXmlFinal(finalIteratorElement, ref finalIteratorEnabled);
				}
				catch (ApophysisException exception)
				{
					throw new ApophysisException("Final transform: " + exception.Message, exception);
				}
			}

			mFinalIterator = finalIterator;
			mUseFinalIterator = finalIteratorEnabled;
			
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

			if (!Equals(iterators.mUseFinalIterator, mUseFinalIterator))
				return false;

			if (!iterators.mFinalIterator.IsEqual(mFinalIterator))
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