using System;
using System.Linq;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class IteratorData : IDisposable
	{
		private FlameData mData;
		private Iterator mIterator;

		private IteratorData mTerminator;
		private bool mIsTerminal;
		private int mIndex;

		private const int mRefTableSize = 1024;

		~IteratorData()
		{
			Dispose(false);
		}
		private IteratorData([NotNull] FlameData data, [NotNull] Iterator iterator, bool terminal)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (iterator == null) throw new ArgumentNullException("iterator");

			mData = data;
			mIndex = iterator.Index;
			mIterator = iterator.Copy();

			if (terminal)
			{
				mTerminator = this;
			}
			else
			{
				var terminator = new Iterator(mData.Renderer.Flame);
				terminator.Variations.ClearWeights();

				mTerminator = new IteratorData(data, terminator, true);
			}

			mIsTerminal = terminal;
		}
		public IteratorData([NotNull] FlameData data, [NotNull] Iterator iterator): this(data, iterator, false)
		{
		}

		public void Initialize()
		{
			PrepareColors();
			PrepareMatrices();
			PrepareVariations();
			PrepareRefTable();
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mTerminator != null && !mIsTerminal)
				{
					mTerminator.Dispose();
				}
			}

			mData = null;
			mIterator = null;
		}

		public double[][] C { get; private set; }
		public double[][] P { get; private set; }

		public Variation[] Variations { get; private set; }
		public IteratorData[] RefTable { get; private set; }

		public double ColorC1 { get; private set; }
		public double ColorC2 { get; private set; }

		private void PrepareColors()
		{
			ColorC1 = (1 + mIterator.ColorSpeed)*0.5;
			ColorC2 = mIterator.Color*(1 - mIterator.ColorSpeed)*0.5;
		}
		private void PrepareMatrices()
		{
			C = new[] { new double[2], new double[2], new double[2] };
			P = new[] { new double[2], new double[2], new double[2] };

			C[0][0] = mIterator.PreAffine.Matrix.X.X;
			C[0][1] = -mIterator.PreAffine.Matrix.X.Y;
			C[1][0] = -mIterator.PreAffine.Matrix.Y.X;
			C[1][1] = mIterator.PreAffine.Matrix.Y.Y;
			C[2][0] = mIterator.PreAffine.Origin.X;
			C[2][1] = -mIterator.PreAffine.Origin.Y;

			P[0][0] = mIterator.PostAffine.Matrix.X.X;
			P[0][1] = -mIterator.PostAffine.Matrix.X.Y;
			P[1][0] = -mIterator.PostAffine.Matrix.Y.X;
			P[1][1] = mIterator.PostAffine.Matrix.Y.Y;
			P[2][0] = mIterator.PostAffine.Origin.X;
			P[2][1] = -mIterator.PostAffine.Origin.Y;
		}
		private void PrepareVariations()
		{
			if (mIsTerminal)
			{
				Variations = new Variation[0];
				return;
			}

			Variations = mIterator.Variations
				.Where(x => System.Math.Abs((double) x.Weight) > double.Epsilon)
				.OrderBy(x => x.Priority)
				.ThenBy(x => x.Name)
				.ToArray();

			foreach (var variation in Variations)
				variation.Prepare();
		}
		private void PrepareRefTable()
		{
			if (mIsTerminal)
			{
				SetTerminalIterator();
				return;
			}

			var iterators = mData.Iterators;

			var n = mData.Renderer.Flame.Iterators.Count;
			var k = mIndex;

			var tp = new double[n];
			var total = 0.0;

			RefTable = new IteratorData[mRefTableSize];

			for (int i = 0; i < n; i++)
			{
				tp[i] = mIterator.Weight * mData.Renderer.Flame.GetChaosCoefficient(k, i);
				total += tp[i];
			}

			if (total > 0)
			{
				var loop = 0.0;
				for (int i = 0; i < mRefTableSize; i++)
				{
					var sum = 0.0;
					var j = -1;

					do
					{
						j++;
						sum += tp[j];
					} while (sum <= loop && j != n - 1);

					RefTable[i] = iterators[j];
					loop += total/mRefTableSize;
				}
			}
			else
			{
				SetTerminalIterator();
			}
		}

		private void SetTerminalIterator()
		{
			for (int i = 0; i < mData.Renderer.Flame.Iterators.Count; i++)
			{
				RefTable[i] = mTerminator;
			}
		}

		public IteratorData Next(Random random)
		{
			return RefTable[random.Next()%mRefTableSize];
		}
		public bool Process(Random random, Vector3 vector, ref double color)
		{
			var spreadColor = color*ColorC1 + ColorC2;

			var preTransformedVector = new Vector3(
				C[0][0] * vector.X + C[1][0] * vector.Y + C[2][0],
				C[0][1] * vector.X + C[1][1] * vector.Y + C[2][1],
				vector.Z);

			var data = new IterationData
			{
				Color = spreadColor,
				PreX = preTransformedVector.X,
				PreY = preTransformedVector.Y,
				PreZ = preTransformedVector.Z
			};

			for (int i = 0; i < Variations.Length; i++)
			{
				Variations[i].Calculate(data);
			}

			var postTransformedVector = new Vector3(
				P[0][0] * data.PostX + P[1][0] * data.PostY + P[2][0],
				P[0][1] * data.PostX + P[1][1] * data.PostY + P[2][1],
				data.PostZ);

			color += mIterator.DirectColor*(data.Color - color);

			vector.X = postTransformedVector.X;
			vector.Y = postTransformedVector.Y;
			vector.Z = postTransformedVector.Z;

			return random.NextDouble() < mIterator.Opacity;
		}
	}
}