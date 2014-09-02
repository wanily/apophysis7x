using System.Globalization;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows
{
	static class Extensions
	{
		public static string GetDisplayName(this Iterator iterator)
		{
			if (iterator == null)
				return null;

			var name = string.IsNullOrEmpty(iterator.Name)
				? (iterator.Index + 1).ToString(CultureInfo.CurrentCulture)
				: string.Format("{0} - {1}", iterator.Index + 1, iterator.Name);

			return name;
		}
	}
}
