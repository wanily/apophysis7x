using System;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public abstract class TransformInputOperation
	{
		protected TransformInputOperation([NotNull] Transform transform)
		{
			if (transform == null) throw new ArgumentNullException("transform");
			Transform = transform;
		}

		[NotNull]
		public Transform Transform
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