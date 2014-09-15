using System;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	class MoveOperation : IteratorInputOperation
	{
		public MoveOperation([NotNull] Iterator iterator, [NotNull] Vector2 origin, [NotNull] Vector2 current) : base(iterator)
		{
			if (origin == null) throw new ArgumentNullException("origin");
			if (current == null) throw new ArgumentNullException("current");

			Origin = origin.Freeze();
			Current = current.Freeze();
		}

		[NotNull]
		public ReadOnlyVector2 Origin
		{
			get; 
			private set;
		}

		[NotNull]
		public ReadOnlyVector2 Current
		{
			get;
			private set;
		}

		[NotNull]
		public ReadOnlyVector2 Delta
		{
			get { return (Current - Origin).Freeze(); }
		}

		protected override string GetInfoString()
		{
			return string.Format("Move:\t {0}\t {1}",
				Delta.X.ToString("0.000", InputController.Culture).PadLeft(6),
				Delta.Y.ToString("0.000", InputController.Culture).PadLeft(6));
		}
	}
}