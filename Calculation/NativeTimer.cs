using System;
using System.Runtime.InteropServices;
using Xyrus.Apophysis.Interfaces.Calculation;

namespace Xyrus.Apophysis.Calculation
{
	public class NativeTimer : INativeTimer
	{
		long mStartTime;
		readonly long mFreq;

		static class NativeMethods
		{
			[DllImport(@"Kernel32", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

			[DllImport(@"Kernel32", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool QueryPerformanceFrequency(out long lpFrequency);
		}

		public NativeTimer()
		{
			mStartTime = 0;
			if (NativeMethods.QueryPerformanceFrequency(out mFreq) == false)
				throw new NotSupportedException();
		}
		public virtual void SetStartingTime()
		{
			NativeMethods.QueryPerformanceCounter(out mStartTime);
		}
		public virtual float GetElapsedTimeInSeconds()
		{
			long time; NativeMethods.QueryPerformanceCounter(out time);
			float delta = (time - mStartTime) / (float)mFreq;
			return delta;
		}
	}
}