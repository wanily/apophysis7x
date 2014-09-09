using System;
using System.Collections.Generic;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class Variation : IDisposable
	{
		private string mDefaultName;
		private bool mIsDisposed;

		~Variation()
		{
			Dispose(false);
		}

		protected Variation()
		{
			mDefaultName = GetType().Name.ToLower();
		}

		protected void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				Release();
			}

			DisposeOverride();

			mIsDisposed = true;
		}
		protected void DisposeOverride()
		{
			
		}

		public virtual void Prepare()
		{
		}
		public virtual void Release()
		{
			
		}

		public virtual string Name
		{
			get { return mDefaultName; }
		}
		public double Weight
		{
			get; 
			set;
		}

		public IEnumerable<string> EnumerateVariables()
		{
			var count = GetVariableCount();
			for (int i = 0; i < count; i++)
			{
				yield return GetVariableNameAt(i);
			}
		}

		public abstract void Calculate(IterationData data);
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual double GetVariable([NotNull] string name)
		{
			return 0.0;
		}
		public virtual double SetVariable([NotNull] string name, double value)
		{
			return 0.0;
		}
		public virtual double ResetVariable([NotNull] string name)
		{
			return 0.0;
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