using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows
{
	static class Extensions
	{
		public static string GetDisplayName(this Iterator iterator)
		{
			if (iterator == null)
				return null;

			var groupName = string.Empty;
			var formatString = @"{1}";

			if (iterator.GroupIndex > 0)
			{
				groupName = "Final";
				formatString = iterator.IsSingleInGroup ? @"{0}" : @"{0} {1}";
			}

			if (!string.IsNullOrEmpty(iterator.Name))
			{
				formatString += " - {2}";
			}

			return string.Format(formatString, groupName, iterator.GroupItemIndex + 1, iterator.Name);
		}
		public static string GetVerboseDisplayName(this Iterator iterator)
		{
			if (iterator == null)
				return null;

			var groupName = "Transform";
			var formatString = @"{0} #{1}";

			if (iterator.GroupIndex > 0)
			{
				groupName = "Final transform";
				formatString = iterator.IsSingleInGroup ? @"{0}" : @"{0} #{1}";
			}

			if (!string.IsNullOrEmpty(iterator.Name))
			{
				formatString += " - {2}";
			}

			return string.Format(formatString, groupName, iterator.GroupItemIndex + 1, iterator.Name);
		}
		public static Color GetColor(this Iterator iterator)
		{
			if (iterator == null) 
				return Color.Transparent;

			Color[] colors =
			{
				Color.Red,
				Color.Yellow,
				Color.LightGreen,
				Color.Cyan,
				Color.Blue,
				Color.Magenta,
				Color.Orange,
				Color.LightSkyBlue,
				Color.MediumOrchid,
				Color.Salmon
			};

			if (iterator.GroupIndex < 0)
			{
				return Color.Transparent;
			}

			if (iterator.GroupIndex > 0)
			{
				return Color.White;
			}

			return colors[iterator.GroupItemIndex % colors.Length];
		}

		public static void Sort<T>([NotNull] this IList<T> list, params Expression<Func<T, object>>[] sortExpressions)
		{
			if (list == null) throw new ArgumentNullException("list");

			var funcs = (sortExpressions ?? new Expression<Func<T, object>>[0]).Select(x => x.Compile());
			var result = list.ToArray().AsEnumerable();

			foreach (var expression in funcs)
			{
				result = result.OrderBy(expression);
			}

			list.Clear();
			foreach (var item in result)
			{
				list.Add(item);
			}
		}
	}
}
