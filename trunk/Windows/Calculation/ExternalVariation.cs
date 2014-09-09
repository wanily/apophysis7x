using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public unsafe sealed class ExternalVariation : Variation
	{
		private const CallingConvention mConvention = CallingConvention.StdCall;

		enum MachineType : ushort
		{
			// ReSharper disable InconsistentNaming
			// ReSharper disable UnusedMember.Local
			IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
			IMAGE_FILE_MACHINE_AM33 = 0x1d3,
			IMAGE_FILE_MACHINE_AMD64 = 0x8664,
			IMAGE_FILE_MACHINE_ARM = 0x1c0,
			IMAGE_FILE_MACHINE_EBC = 0xebc,
			IMAGE_FILE_MACHINE_I386 = 0x14c,
			IMAGE_FILE_MACHINE_IA64 = 0x200,
			IMAGE_FILE_MACHINE_M32R = 0x9041,
			IMAGE_FILE_MACHINE_MIPS16 = 0x266,
			IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
			IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
			IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
			IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
			IMAGE_FILE_MACHINE_R4000 = 0x166,
			IMAGE_FILE_MACHINE_SH3 = 0x1a2,
			IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
			IMAGE_FILE_MACHINE_SH4 = 0x1a6,
			IMAGE_FILE_MACHINE_SH5 = 0x1a8,
			IMAGE_FILE_MACHINE_THUMB = 0x1c2,
			IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
			// ReSharper restore UnusedMember.Local
			// ReSharper restore InconsistentNaming
		}

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FreeLibrary(IntPtr hModule);

		[UnmanagedFunctionPointer(mConvention)]
		private delegate void* PluginVarCreateDelegate();

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarDestroyDelegate(void** vpp);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarInitDelegate(void* vp, void* pFPx, void* pFPy, void* pFTx, void* pFTy, double vvar);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarInit3DDelegate(void* vp, void* pFPx, void* pFPy, void* pFPz, void* pFTx, void* pFTy, void* pFTz, double vvar);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarInitDcDelegate(void* vp, void* pFPx, void* pFPy, void* pFPz, void* pFTx, void* pFTy, void* pFTz, void* pColor, double vvar, double a, double b, double c, double d, double e, double f);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private delegate string PluginVarGetVariableNameAtDelegate(int index);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarResetVariableDelegate(void* vp, [MarshalAs(UnmanagedType.LPStr)] string name);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarGetVariableDelegate(void* vp, [MarshalAs(UnmanagedType.LPStr)] string name, double* value);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarSetVariableDelegate(void* vp, [MarshalAs(UnmanagedType.LPStr)] string name, double* value);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private delegate string PluginVarGetNameDelegate();

		[UnmanagedFunctionPointer(mConvention)]
		private delegate int PluginVarGetNrVariablesDelegate();

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarPrepareDelegate(void* vp);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarCalculateDelegate(void* vp);

		private PluginVarCreateDelegate mCreate;
		private PluginVarDestroyDelegate mDestroy;
		private PluginVarInitDelegate mInitLegacy;
		private PluginVarInit3DDelegate mInitLegacy3D;
		private PluginVarInitDcDelegate mInit;
		private PluginVarGetVariableNameAtDelegate mGetVariableNameAt;
		private PluginVarResetVariableDelegate mResetVariable;
		private PluginVarGetVariableDelegate mGetVariable;
		private PluginVarSetVariableDelegate mSetVariable;
		private PluginVarGetNameDelegate mGetName;
		private PluginVarGetNrVariablesDelegate mGetNrVariables;
		private PluginVarPrepareDelegate mPrepare;
		private PluginVarCalculateDelegate mCalculate;
		
		private readonly string mDllPath;
		private IntPtr mHModule;
		private void* mVp;

		private double* mPreX, mPostX, mPreY, mPostY, mPreZ, mPostZ, mColor;

		public ExternalVariation([NotNull] string dllPath)
		{
			if (string.IsNullOrEmpty(dllPath)) throw new ArgumentNullException("dllPath");
			if (!File.Exists(dllPath)) throw new FileNotFoundException("Module not found", dllPath);

			if (!FitsCurrentMachineType(dllPath))
			{
				var type = GetDllMachineType(dllPath);
				throw new ApophysisException(string.Format("Unsupported plugin architecture ({0})", type));
			}

			mDllPath = dllPath;
			LoadModule();
		}

		public static bool FitsCurrentMachineType([NotNull] string dllPath)
		{
			if (string.IsNullOrEmpty(dllPath)) throw new ArgumentNullException("dllPath");
			if (!File.Exists(dllPath)) throw new FileNotFoundException("Module not found", dllPath);

			var is64Bit = IntPtr.Size == 8;
			var type = GetDllMachineType(dllPath);

			return (is64Bit && type == MachineType.IMAGE_FILE_MACHINE_AMD64) ||
			       (!is64Bit && type == MachineType.IMAGE_FILE_MACHINE_I386);
		}
		private static MachineType GetDllMachineType([NotNull] string dllPath)
		{
			var file = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
			var reader = new BinaryReader(file);

			file.Seek(0x3c, SeekOrigin.Begin);
			var peOffset = reader.ReadInt32();

			file.Seek(peOffset, SeekOrigin.Begin);
			var peHead = reader.ReadUInt32();

			if (peHead != 0x00004550)
				throw new ApophysisException(string.Format("Unable to find PE header of library \"{0}\"", dllPath));

			var machineType = (MachineType)reader.ReadUInt16();

			reader.Close();
			file.Close();

			return machineType;
		}

		public override double GetVariable(string name)
		{
			double ptr = 0;
			mGetVariable(mVp, name, &ptr);
			return ptr;
		}
		public override double SetVariable(string name, double value)
		{
			mSetVariable(mVp, name, &value);
			return value;
		}
		public override double ResetVariable(string name)
		{
			mResetVariable(mVp, name);
			return GetVariable(name);
		}

		public override int GetVariableCount()
		{
			return mGetNrVariables();
		}
		public override string GetVariableNameAt(int index)
		{
			return mGetVariableNameAt(index);
		}

		public override string Name
		{
			get { return mGetName(); }
		}

		public override void Prepare(IterationData data)
		{
			if (mInit != null)
			{
				mInit(mVp, mPostX, mPostY, mPostZ, mPreX, mPreY, mPreZ, mColor, data.Weight, 1, 0, 0, 1, 0, 0);
			}
			else if (mInitLegacy3D != null)
			{
				mInitLegacy3D(mVp, mPostX, mPostY, mPostZ, mPreX, mPreY, mPreZ, data.Weight);
			}
			else
			{
				mInitLegacy(mVp, mPostX, mPostY, mPreX, mPreY, data.Weight);
			}

			mPrepare(mVp);
		}
		public override void Calculate(IterationData data)
		{
			mCalculate(mVp);
		}

		private void LoadModule()
		{
			if (mHModule != IntPtr.Zero)
			{
				DisposeOverride(true);
			}

			mHModule = LoadLibrary(mDllPath);

			mCreate = (PluginVarCreateDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarCreate"), typeof(PluginVarCreateDelegate));
			mDestroy = (PluginVarDestroyDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarDestroy"), typeof(PluginVarDestroyDelegate));
			mGetVariableNameAt = (PluginVarGetVariableNameAtDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarGetVariableNameAt"), typeof(PluginVarGetVariableNameAtDelegate));
			mResetVariable = (PluginVarResetVariableDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarResetVariable"), typeof(PluginVarResetVariableDelegate));
			mGetVariable = (PluginVarGetVariableDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarGetVariable"), typeof(PluginVarGetVariableDelegate));
			mSetVariable = (PluginVarSetVariableDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarSetVariable"), typeof(PluginVarSetVariableDelegate));
			mGetName = (PluginVarGetNameDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarGetName"), typeof(PluginVarGetNameDelegate));
			mGetNrVariables = (PluginVarGetNrVariablesDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarGetNrVariables"), typeof(PluginVarGetNrVariablesDelegate));
			mCalculate = (PluginVarCalculateDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarCalc"), typeof(PluginVarCalculateDelegate));
			mPrepare = (PluginVarPrepareDelegate)Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarPrepare"), typeof(PluginVarPrepareDelegate));

			mVp = mCreate();

			mPreX = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();
			mPreY = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();
			mPreZ = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();

			mPostX = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();
			mPostY = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();
			mPostZ = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();

			mColor = (double*) Marshal.AllocHGlobal(sizeof (double)).ToPointer();

			var init = GetProcAddress(mHModule, "PluginVarInitDC");
			if (init != IntPtr.Zero)
			{
				mInit = (PluginVarInitDcDelegate) Marshal.GetDelegateForFunctionPointer(init, typeof (PluginVarInitDcDelegate));
			}
			else
			{
				init = GetProcAddress(mHModule, "PluginVarInit3D");
				if (init != IntPtr.Zero)
				{
					mInitLegacy3D = (PluginVarInit3DDelegate) Marshal.GetDelegateForFunctionPointer(init, typeof (PluginVarInit3DDelegate));
				}
				else
				{
					mInitLegacy = (PluginVarInitDelegate) Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, "PluginVarInit"), typeof(PluginVarInitDelegate));
				}
			}
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mHModule != IntPtr.Zero && disposing)
			{
				fixed (void** vpp = &mVp)
				{
					mDestroy(vpp);
				}

				Marshal.FreeHGlobal(new IntPtr(mPreX));
				Marshal.FreeHGlobal(new IntPtr(mPreY));
				Marshal.FreeHGlobal(new IntPtr(mPreZ));

				Marshal.FreeHGlobal(new IntPtr(mPostX));
				Marshal.FreeHGlobal(new IntPtr(mPostY));
				Marshal.FreeHGlobal(new IntPtr(mPostZ));

				Marshal.FreeHGlobal(new IntPtr(mColor));

				FreeLibrary(mHModule);
				mHModule = IntPtr.Zero;
			}

			mCreate = null;
			mDestroy = null;
			mInitLegacy = null;
			mInitLegacy3D = null;
			mInit = null;
			mGetVariableNameAt = null;
			mResetVariable = null;
			mGetVariable = null;
			mSetVariable = null;
			mGetName = null;
			mGetNrVariables = null;
			mPrepare = null;
			mCalculate = null;

			mPreX = null;
			mPreY = null;
			mPreZ = null;

			mPostX = null;
			mPostY = null;
			mPostZ = null;

			mColor = null;
		}

		public Variation CreateInstance()
		{
			return new ExternalVariation(mDllPath);
		}
	}
}