using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xyrus.Apophysis.Strings;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public static class VariationRegistry
	{
		private static readonly List<Variation> mVariations;
		private static readonly Dictionary<string, Variation> mVariationsByName;

		static VariationRegistry()
		{
			mVariations = new List<Variation>();
			mVariationsByName = new Dictionary<string, Variation>();
		}

		public static string RegisterDll([NotNull] string dllPath)
		{
			Debug.Print(@"Loading ""{0}""...", dllPath);
			try
			{
				var variation = new ExternalVariation(dllPath);

				mVariations.Add(variation);

				try
				{
					mVariationsByName.Add(variation.Name, variation);
				}
				catch (ArgumentException exception)
				{
					variation.Dispose();
					throw new ApophysisException(string.Format(Messages.PluginAlreadyLoadedError, variation.Name), exception);
				}

				return variation.Name;
			}
			catch (Exception ex)
			{
				if (ex is ApophysisException)
					throw;

				throw new ApophysisException(string.Format(Messages.GenericPluginError, dllPath, ex.Message), ex);
			}
		}
		public static string Register<T>() where T : Variation, new()
		{
			var instance = new T();

			mVariationsByName.Add(instance.Name, instance);
			mVariations.Add(instance);

			return instance.Name;
		}

		public static Variation GetInstance(string name)
		{
			if (!mVariationsByName.ContainsKey(name))
				throw new KeyNotFoundException();

			var variation = mVariationsByName[name] as ExternalVariation;
			if (variation != null)
			{
				return variation.CreateInstance();
			}

			return (Variation)Activator.CreateInstance(mVariationsByName[name].GetType());
		}

		public static bool IsVariable(string name)
		{
			return mVariations.Any(x => x.EnumerateVariables().Any(y => Equals((y ?? string.Empty).ToLower(), name.ToLower())));
		}
		public static bool IsVariation(string name)
		{
			return mVariationsByName.ContainsKey(name);
		}

		public static IEnumerable<string> GetVariationNames()
		{
			return mVariationsByName.Keys;
		}
		public static string GetVariationNameForVariable(string variableName)
		{
			var variation = mVariations.FirstOrDefault(x => x.EnumerateVariables().Any(y => Equals((y ?? string.Empty).ToLower(), variableName.ToLower())));
			if (variation == null)
				return null;

			return variation.Name;
		}

		public static string GetName<T>() where T: Variation, new()
		{
			using (var instance = new T())
			{
				return instance.Name;
			}
		}
	}
}