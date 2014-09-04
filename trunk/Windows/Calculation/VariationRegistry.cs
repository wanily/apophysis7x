using System;
using System.Collections.Generic;
using System.Linq;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public static class VariationRegistry
	{
		private static List<Variation> mVariations;
		private static Dictionary<string, Variation> mVariationsByName;

		static VariationRegistry()
		{
			mVariations = new List<Variation>();
			mVariationsByName = new Dictionary<string, Variation>();
		}

		public static void Register<T>() where T : Variation, new()
		{
			var instance = new T();

			mVariationsByName.Add(instance.Name, instance);
			mVariations.Add(instance);
		}
		public static Variation GetInstance(string name)
		{
			if (!mVariationsByName.ContainsKey(name))
				throw new KeyNotFoundException();

			return (Variation)Activator.CreateInstance(mVariationsByName[name].GetType());
		}

		public static bool IsVariable(string name)
		{
			return mVariations.Any(x => x.HasVariable(name));
		}
		public static bool IsVariation(string name)
		{
			return mVariationsByName.ContainsKey(name);
		}
	}
}