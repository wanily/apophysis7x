using System;
using System.Linq;
using System.Numerics;
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
				var terminator = new Iterator(mData.Flame);
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

		public float[][] C { get; private set; }
		public float[][] P { get; private set; }

		public Variation[] Variations { get; private set; }
		public IteratorData[] RefTable { get; private set; }

		public float ColorC1 { get; private set; }
		public float ColorC2 { get; private set; }

		private void PrepareColors()
		{
			ColorC1 = (1 + mIterator.ColorSpeed)*0.5f;
			ColorC2 = mIterator.Color*(1 - mIterator.ColorSpeed)*0.5f;
		}
		private void PrepareMatrices()
		{
			C = new[] { new float[2], new float[2], new float[2] };
			P = new[] { new float[2], new float[2], new float[2] };

			C[0][0] = mIterator.PreAffine.M11;
			C[0][1] = -mIterator.PreAffine.M12;
			C[1][0] = -mIterator.PreAffine.M21;
			C[1][1] = mIterator.PreAffine.M22;
			C[2][0] = mIterator.PreAffine.M31;
			C[2][1] = -mIterator.PreAffine.M32;

			P[0][0] = mIterator.PostAffine.M11;
			P[0][1] = -mIterator.PostAffine.M12;
			P[1][0] = -mIterator.PostAffine.M21;
			P[1][1] = mIterator.PostAffine.M22;
			P[2][0] = mIterator.PostAffine.M31;
			P[2][1] = -mIterator.PostAffine.M32;
		}
		private void PrepareVariations()
		{
			if (mIsTerminal)
			{
				Variations = new Variation[0];
				return;
			}

			Variations = mIterator.Variations
				.Where(x => System.Math.Abs(x.Weight) > float.Epsilon)
				.OrderBy(x => x.Priority)
				.ThenBy(x => x.Name)
				.ToArray();

			foreach (var variation in Variations)
				variation.Prepare();
		}
		private void PrepareRefTable()
		{
			if (mIsTerminal || mIterator.GroupIndex != 0)
			{
				SetTerminalIterator();
				return;
			}

			var iterators = mData.Iterators;

			var n = mData.Flame.Iterators.Count(x => x.GroupIndex == 0);
			var k = mIndex;

			var tp = new float[n];
			var total = 0.0f;

			RefTable = new IteratorData[mRefTableSize];

			for (int i = 0; i < n; i++)
			{
				tp[i] = mIterator.Weight * mData.Flame.GetChaosCoefficient(k, i);
				total += tp[i];
			}

			if (total > 0)
			{
				var loop = 0.0f;
				for (int i = 0; i < mRefTableSize; i++)
				{
					var sum = 0.0f;
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
			if (RefTable == null)
				return;

			for (int i = 0; i < mData.Flame.Iterators.Count(x => x.GroupIndex == 0); i++)
			{
				RefTable[i] = mTerminator;
			}
		}

		public IteratorData Next(Random random)
		{
			return RefTable[random.Next()%mRefTableSize];
		}
		public bool Process(Random random, Vector3 vector, ref float color)
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

			return random.NextFloat() < mIterator.Opacity;
		}
	}
}