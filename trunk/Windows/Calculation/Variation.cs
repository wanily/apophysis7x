using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class Variation : IDisposable
	{
		private ICollection<Variable> mEmptyCollection;

		private string mDefaultName;
		private bool mIsDisposed;

		~Variation()
		{
			Dispose(false);
		}

		protected Variation()
		{
			mDefaultName = GetType().Name.ToLower();
			mEmptyCollection = new Collection<Variable>();
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

		[NotNull]
		protected virtual ICollection<Variable> GetVariables()
		{
			return mEmptyCollection;
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
		public bool HasVariable([NotNull] string name)
		{
			if (string.IsNullOrEmpty(name))
				return false;

			return GetVariables().Any(x => Equals((x.Name ?? string.Empty).ToLower(), name.ToLower()));
		}

		public abstract void Calculate(IterationData data);
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}