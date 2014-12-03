using System;
using System.Numerics;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	class MoveOperation : IteratorInputOperation
	{
		public MoveOperation([NotNull] Iterator iterator, Vector2 origin, Vector2 current) : base(iterator)
		{
			if (origin == null) throw new ArgumentNullException("origin");
			if (current == null) throw new ArgumentNullException("current");

			Origin = origin;
			Current = current;
		}

		public Vector2 Origin
		{
			get; 
			private set;
		}

		public Vector2 Current
		{
			get;
			private set;
		}

		public Vector2 Delta
		{
			get { return (Current - Origin); }
		}

		protected override string GetInfoString()
		{
			return string.Format("Move:\t {0}\t {1}",
				Delta.X.ToString("0.000", InputController.Culture).PadLeft(6),
				Delta.Y.ToString("0.000", InputController.Culture).PadLeft(6));
		}
	}
}