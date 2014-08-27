using System;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformMoveOperation : TransformInputOperation
	{
		public TransformMoveOperation([NotNull] Transform transform, [NotNull] Vector2 origin, [NotNull] Vector2 current) : base(transform)
		{
			if (origin == null) throw new ArgumentNullException("origin");
			if (current == null) throw new ArgumentNullException("current");

			Origin = origin.Freeze();
			Current = current.Freeze();
		}

		[NotNull]
		public ImmutableVector2 Origin
		{
			get; 
			private set;
		}

		[NotNull]
		public ImmutableVector2 Current
		{
			get;
			private set;
		}

		[NotNull]
		public ImmutableVector2 Delta
		{
			get { return (Current - Origin).Freeze(); }
		}

		protected override string GetInfoString()
		{
			return string.Format("Move: {0:0.000}; {1:0.000}", Delta.X, Delta.Y);
		}
	}
}