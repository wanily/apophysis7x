using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class FlameCollection : ReadOnlyCollection<Flame>
	{
		private EventHandler mContentChanged;
		private string mName;

		public FlameCollection() : base(new List<Flame>())
		{
			mName = null;
			Append();
		}
		public FlameCollection([NotNull] IEnumerable<Flame> flames) : base(flames.ToList())
		{
			if (Items.Count == 0)
			{
				throw new ArgumentException("Batch can't be empty", @"flames");
			}

			mName = null;
		}

		[CanBeNull]
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}

		[NotNull]
		public string CalculatedName
		{
			get
			{
				if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Name.Trim()))
				{
					return Items.First().CalculatedName;
				}

				return Name;
			}
		}

		public int Append()
		{
			return Append(new Flame());
		}
		public int Append([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException(@"flame");

			Items.Add(flame);
			RaiseContentChanged();

			return Count - 1;
		}

		public bool Remove(int index)
		{
			if (index < 0 || index >= Count) return false;
			if (!CanRemove()) throw new ApophysisException("Can't remove last flame from batch");

			Items.RemoveAt(index);
			RaiseContentChanged();

			return true;
		}
		public bool Remove([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException(@"flame");
			if (!CanRemove()) throw new ApophysisException("Can't remove last flame from batch");

			if (!Contains(flame))
				return false;

			Items.Remove(flame);
			RaiseContentChanged();

			return true;
		}
		public bool CanRemove()
		{
			return Count > 1;
		}

		public bool Replace([NotNull] Flame flame, [NotNull] Flame substituteFlame)
		{
			if (flame == null) throw new ArgumentNullException(@"flame");
			if (substituteFlame == null) throw new ArgumentNullException(@"substituteFlame");

			if (!Contains(flame))
				return false;

			var index = IndexOf(flame);
			Debug.Assert(index >= 0);

			Items[index] = substituteFlame;
			RaiseContentChanged();

			return true;
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

		public void ReadXml([NotNull] XElement element)
		{
			if (element == null) throw new ArgumentNullException(@"element");

			var elementName = element.Name.ToString().ToLower();
			var elementNames = new[] { @"flame", @"flames" };
			var flameNodes = new List<XElement>();

			switch (elementName)
			{
				case @"flame":
					flameNodes.Add(element);
					break;
				case @"flames":
					flameNodes.AddRange(element.Descendants(XName.Get(@"flame")).Where(x => x != null));
					break;
				default:
					var expectedNameString = elementNames.Length == 1
						? elementNames[0]
						: string.Format("{0} or {1}",
							string.Join(@", ", elementNames.Select(x => @"""" + x + @"""").Take(elementNames.Length - 2).ToArray()),
							@"""" + elementNames.Last() + @"""");
					throw new ApophysisException(string.Format("Expected XML node {0} but received \"{1}\"", expectedNameString, elementName));
			}

			if (!flameNodes.Any())
			{
				throw new ApophysisException("No flames in batch");
			}

			var nameAttribute = element.Attribute(XName.Get(@"name"));
			Name = nameAttribute == null ? null : nameAttribute.Value;

			var counter = 1;
			var newCollection = new List<Flame>();

			foreach (var flameNode in flameNodes)
			{
				var flameNameAttribute = flameNode.Attribute(XName.Get(@"name"));
				var flameName = flameNameAttribute == null ? null : flameNameAttribute.Value;

				if (string.IsNullOrEmpty(flameName) || string.IsNullOrEmpty(flameName.Trim()))
				{
					flameName = string.Format("Untitled flame #{0}", counter);
				}

				var flame = new Flame();

				try
				{
					flame.ReadXml(flameNode);
					newCollection.Add(flame);

					if (!string.IsNullOrEmpty(flame.Name))
					{
						Flame.ReduceCounter();
					}
				}
				catch (ApophysisException exception)
				{
					throw new ApophysisException(string.Format("Flame \"{0}\": {1}", flameName, exception.Message), exception);
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
	}
}