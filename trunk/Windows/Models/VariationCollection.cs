using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Models
{
	[PublicAPI]
	public class VariationCollection : ReadOnlyCollection<Variation>
	{
		private readonly Dictionary<string, Variation> mVariationInstances;

		~VariationCollection()
		{
			foreach (var instance in mVariationInstances.Values)
			{
				instance.Dispose();
			}

			mVariationInstances.Clear();
		}
		public VariationCollection() : base(new List<Variation>())
		{
			mVariationInstances = new Dictionary<string, Variation>();

			foreach (var variation in VariationRegistry.GetVariationNames())
			{
				var instance = VariationRegistry.GetInstance(variation);

				mVariationInstances.Add(variation, instance);
				Items.Add(instance);

				foreach (var variable in instance.EnumerateVariables())
				{
					instance.ResetVariable(variable);
				}
			}
		}

		public double GetWeight(string name)
		{
			return mVariationInstances[name].Weight;
		}
		public double SetWeight(string name, double value)
		{
			mVariationInstances[name].Weight = value;
			return mVariationInstances[name].Weight;
		}

		public double GetVariable(string name)
		{
			var variationName = VariationRegistry.GetVariationNameForVariable(name);
			if (string.IsNullOrEmpty(variationName))
			{
				throw new KeyNotFoundException();
			}

			return mVariationInstances[variationName].GetVariable(name);
		}
		public double ResetVariable(string name)
		{
			var variationName = VariationRegistry.GetVariationNameForVariable(name);
			if (string.IsNullOrEmpty(variationName))
			{
				throw new KeyNotFoundException();
			}

			return mVariationInstances[variationName].ResetVariable(name);
		}
		public double SetVariable(string name, double value)
		{
			var variationName = VariationRegistry.GetVariationNameForVariable(name);
			if (string.IsNullOrEmpty(variationName))
			{
				throw new KeyNotFoundException();
			}

			return mVariationInstances[variationName].SetVariable(name, value);
		}

		public void ClearWeights()
		{
			foreach (var variation in Items)
			{
				variation.Weight = 0;
			}
		}

		public bool IsEqual([NotNull] VariationCollection variations)
		{
			if (variations == null) throw new ArgumentNullException("variations");

			if (variations.Items.Count != Items.Count || variations.mVariationInstances.Count != mVariationInstances.Count)
				return false;

			for (int i = 0; i < Items.Count; i++)
			{
				if (!Equals(variations.Items[i].Weight, Items[i].Weight))
					return false;

				var variablesOther = variations.Items[i].GetVariableCount();
				var variablesThis = Items[i].GetVariableCount();

				if (!Equals(variablesOther, variablesThis))
					return false;

				for (int j = 0; j < variablesThis; j++)
				{
					var nameOther = variations.Items[i].GetVariableNameAt(j);
					var nameThis = Items[i].GetVariableNameAt(j);

					if (!Equals(nameOther, nameThis))
						return false;

					var valueOther = variations.Items[i].GetVariable(nameOther);
					var valueThis = Items[i].GetVariable(nameThis);

					if (!Equals(valueOther, valueThis))
						return false;
				}

			}

			return true;
		}
	}
}