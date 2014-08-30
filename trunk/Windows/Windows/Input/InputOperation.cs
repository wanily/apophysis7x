using System;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	abstract class InputOperation
	{
		protected InputOperation([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			Iterator = iterator;
		}

		[NotNull]
		public Iterator Iterator
		{
			get; 
			private set;
		}

		protected abstract string GetInfoString();

		public override string ToString()
		{
			return GetInfoString();
		}
	}
}