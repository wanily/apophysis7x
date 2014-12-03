using System;
using System.Collections.Generic;
using System.Numerics;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class Variation : IDisposable
	{
		private readonly string mDefaultName;
		private bool mIsDisposed;

		~Variation()
		{
			Dispose(false);
		}
		protected Variation()
		{
			mDefaultName = GetType().Name.ToLower();
			Priority = VariationPriority.Normal;
		}

		public static bool VariationsIn15CStyle
		{
			get; 
			set;
		}

		protected void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			DisposeOverride(disposing);

			mIsDisposed = true;
		}
		protected virtual void DisposeOverride(bool disposing)
		{
			
		}

		public virtual string Name
		{
			get { return mDefaultName; }
		}
		public float Weight
		{
			get; 
			set;
		}
		public VariationPriority Priority
		{
			get; 
			protected set;
		}

		public IEnumerable<string> EnumerateVariables()
		{
			var count = GetVariableCount();
			for (int i = 0; i < count; i++)
			{
				yield return GetVariableNameAt(i);
			}
		}

		public virtual void Prepare(Matrix3x2? affineMatrix = null)
		{
		}
		public abstract void Calculate(IterationData data);
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual float GetVariable([NotNull] string name)
		{
			return 0.0f;
		}
		public virtual float SetVariable([NotNull] string name, float value)
		{
			return 0.0f;
		}
		public virtual float ResetVariable([NotNull] string name)
		{
			return 0.0f;
		}

		public virtual int GetVariableCount()
		{
			return 0;
		}
		public virtual string GetVariableNameAt(int index)
		{
			return null;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}