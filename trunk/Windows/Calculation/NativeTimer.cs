using System;
using System.Runtime.InteropServices;

namespace Xyrus.Apophysis.Calculation
{
	class NativeTimer
	{
		long mStartTime;
		readonly long mFreq;

		static class NativeMethods
		{
			[DllImport("Kernel32", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

			[DllImport("Kernel32", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool QueryPerformanceFrequency(out long lpFrequency);
		}

		public NativeTimer()
		{
			mStartTime = 0;
			if (NativeMethods.QueryPerformanceFrequency(out mFreq) == false)
				throw new NotSupportedException();
		}
		public void SetStartingTime()
		{
			NativeMethods.QueryPerformanceCounter(out mStartTime);
		}
		public double GetElapsedTimeInSeconds()
		{
			long time; NativeMethods.QueryPerformanceCounter(out time);
			double delta = (time - mStartTime) / (double)mFreq;
			return delta;
		}
	}
}