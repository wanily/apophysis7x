using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Strings;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class FlameCollection : ReadOnlyCollection<Flame>
	{
		private EventHandler mContentChanged;

		private FlameCollection() : base(new List<Flame>())
		{
			Name = null;
		}
		public FlameCollection([NotNull] IEnumerable<Flame> flames) : base(flames.ToList())
		{
			if (Items.Count == 0)
			{
				throw new ArgumentException(Messages.EmptyBatchError, @"flames");
			}

			Name = null;
		}

		[CanBeNull]
		public string Name
		{
			get; 
			set;
		}

		[NotNull]
		public string CalculatedName
		{
			get
			{
				if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty((Name??string.Empty).Trim()))
				{
					return Items.First().CalculatedName;
				}

				return (Name ?? string.Empty);
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
			if (!CanRemove()) throw new ApophysisException(Messages.EmptyFlameCollectionError);

			Items.RemoveAt(index);
			RaiseContentChanged();

			return true;
		}
		public bool Remove([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException(@"flame");
			if (!CanRemove()) throw new ApophysisException(Messages.EmptyFlameCollectionError);

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

			MessageCenter.FlameParsing.Enter();

			try
			{
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
							: string.Format(Common.OrConcatenation,
								string.Join(@", ", elementNames.Select(x => @"""" + x + @"""").Take(elementNames.Length - 2).ToArray()),
								@"""" + elementNames.Last() + @"""");
						throw new ApophysisException(string.Format(Messages.UnexpectedXmlTagError, expectedNameString,
							elementName));
				}

				if (!flameNodes.Any())
				{
					throw new ApophysisException(Messages.FlameCollectionNoChildTagsError);
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
						flameName = string.Format(Common.NumberedUntitledFlame, counter);
					}

					var flame = new Flame();

					try
					{
						flame.ReadXml(flameNode, true);
						newCollection.Add(flame);

						if (!string.IsNullOrEmpty(flame.Name))
						{
							Flame.ReduceCounter();
						}
					}
					catch (ApophysisException exception)
					{
						throw new ApophysisException(string.Format(Messages.FlameErrorWrapper, flameName, exception.Message), exception);
					}

					counter++;
				}

				Items.Clear();

				foreach (var item in newCollection)
				{
					Items.Add(item);
				}
			}
			finally
			{
				MessageCenter.FlameParsing.Dispose();
			}
			

			RaiseContentChanged();
		}
		public void WriteXml([NotNull] out XElement element)
		{
			element = new XElement(XName.Get(@"Flames"));
			element.Add(new XAttribute(XName.Get(@"name"), CalculatedName));

			foreach (var flame in this)
			{
				XElement flameElement;
				flame.WriteXml(out flameElement);
				element.Add(flameElement);
			}
		}

		[NotNull]
		public static FlameCollection LoadFromXml([NotNull] XElement element)
		{
			var collection = new FlameCollection();
			collection.ReadXml(element);
			return collection;
		}
	}
}