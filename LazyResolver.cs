using System;
using Microsoft.Practices.Unity;

namespace Xyrus.Apophysis
{
	/// <summary>
	/// WARNING: do not use this as a readonly field! It will break the lazy-loading mechanism.
	/// http://blogs.msdn.com/b/ericlippert/archive/2008/05/14/mutating-readonly-structs.aspx
	/// </summary>
	public struct LazyResolver<T>
	{
		private T mResolved;
		private bool mIsResolved;

		public T Object
		{
			get
			{
				if (!mIsResolved)
				{
					mResolved = ApophysisApplication.Container.Resolve<T>();
					mIsResolved = true;
				}

				return mResolved;
			}
		}
		public void Reset()
		{
			var disposable = mResolved as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}

			mIsResolved = false;
			mResolved = default(T);
		}
	}
}