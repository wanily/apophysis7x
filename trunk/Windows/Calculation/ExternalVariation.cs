using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Xyrus.Apophysis.Strings;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public unsafe sealed class ExternalVariation : Variation
	{
		private const CallingConvention mConvention = CallingConvention.Cdecl;

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

		[DllImport(@"kernel32", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport(@"kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		//[DllImport(@"kernel32.dll", SetLastError = true)]
		//[return: MarshalAs(UnmanagedType.Bool)]
		//private static extern bool FreeLibrary(IntPtr hModule);

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
		private delegate byte* PluginVarGetVariableNameAtDelegate(int index);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarResetVariableDelegate(void* vp, byte* name);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarGetVariableDelegate(void* vp, byte* name, double* value);

		[UnmanagedFunctionPointer(mConvention)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private delegate bool PluginVarSetVariableDelegate(void* vp, byte* name, double* value);

		[UnmanagedFunctionPointer(mConvention)]
		private delegate byte* PluginVarGetNameDelegate();

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

		private List<string> mVariables;

		private readonly string mDllPath;
		private IntPtr mHModule;
		private string mName;
		private void* mVp;

		private double* mPreX, mPostX, mPreY, mPostY, mPreZ, mPostZ, mColor;

		private ExternalVariation([NotNull] string dllPath, IntPtr hModule)
		{
			mDllPath = dllPath;
			mHModule = hModule;
		}
		public ExternalVariation([NotNull] string dllPath)
		{
			if (string.IsNullOrEmpty(dllPath)) throw new ArgumentNullException(@"dllPath");
			if (!File.Exists(dllPath)) throw new FileNotFoundException(Messages.PluginModuleNotFoundError, dllPath);

			if (!FitsCurrentMachineType(dllPath))
			{
				var type = GetDllMachineType(dllPath);
				throw new ApophysisException(string.Format(Messages.PluginUnsupportedArchitectureError, type));
			}

			mDllPath = dllPath;
			LoadModule();
		}

		public static bool FitsCurrentMachineType([NotNull] string dllPath)
		{
			if (string.IsNullOrEmpty(dllPath)) throw new ArgumentNullException(@"dllPath");
			if (!File.Exists(dllPath)) throw new FileNotFoundException(Messages.PluginModuleNotFoundError, dllPath);

			var is64Bit = IntPtr.Size == 8;
			var type = GetDllMachineType(dllPath);

			return (is64Bit && type == MachineType.IMAGE_FILE_MACHINE_AMD64) ||
			       (!is64Bit && type == MachineType.IMAGE_FILE_MACHINE_I386);
		}
		private static MachineType GetDllMachineType([NotNull] string dllPath)
		{
			var file = new FileStream(dllPath, FileMode.Open, FileAccess.Read);
			var reader = new BinaryReader(file);

			MachineType machineType;

			try
			{
				file.Seek(0x3c, SeekOrigin.Begin);
				var peOffset = reader.ReadInt32();

				file.Seek(peOffset, SeekOrigin.Begin);
				var peHead = reader.ReadUInt32();

				if (peHead != 0x00004550)
					throw new ApophysisException(string.Format(Messages.PluginUnknownPeHeaderError));

				machineType = (MachineType) reader.ReadUInt16();
			}
			catch
			{
				throw new ApophysisException(string.Format(Messages.PluginUnknownArchitectureError));
			}
			finally
			{
				reader.Close();
				file.Close();
			}

			return machineType;
		}

		private static string GetStringFromPointer(byte* ptr)
		{
			if (ptr == null)
			{
				return null;
			}

			const int bufferSize = 1024;

			var array = new byte[bufferSize];
			var count = 0;

			for (var c = 0; c < bufferSize; c++)
			{
				byte b = *(ptr + c);

				if (b == 0)
				{
					count = c; 
					break;
				}

				array[c] = b;
			}

			if (count <= 0)
			{
				return string.Empty;
			}

			var str = new byte[count];
			Array.Copy(array, str, count);

			return Encoding.ASCII.GetString(str);
		}
		private static byte[] GetArrayFromString(string str)
		{
			return Encoding.ASCII.GetBytes(str);
		}

		public override double GetVariable(string name)
		{
			double value = 0;
			byte[] nbytes = GetArrayFromString(name);

			fixed(byte* nptr = &nbytes[0])
			{
				double* vptr = &value;
				bool result = mGetVariable(mVp, nptr, vptr);

				Debug.Assert(result, @"PluginVarGetVariable");

				return value;
			}
		}
		public override double SetVariable(string name, double value)
		{
			byte[] nbytes = GetArrayFromString(name);

			fixed(byte* nptr = &nbytes[0])
			{
				double* vptr = &value;
				bool result = mSetVariable(mVp, nptr, vptr);

				Debug.Assert(result, @"PluginVarSetVariable");
			}

			return GetVariable(name);
		}
		public override double ResetVariable(string name)
		{
			byte[] nbytes = GetArrayFromString(name);

			fixed (byte* nptr = &nbytes[0])
			{
				mResetVariable(mVp, nptr);
			}

			return GetVariable(name);
		}

		public override int GetVariableCount()
		{
			return mVariables.Count;
		}
		public override string GetVariableNameAt(int index)
		{
			return mVariables[index];
		}

		public override string Name
		{
			get { return mName; }
		}

		public override void Prepare(IterationData data)
		{
			if (mInit != null)
			{
				mInit(mVp, mPostX, mPostY, mPostZ, mPreX, mPreY, mPreZ, mColor, Weight, 1, 0, 0, 1, 0, 0);
			}
			else if (mInitLegacy3D != null)
			{
				mInitLegacy3D(mVp, mPostX, mPostY, mPostZ, mPreX, mPreY, mPreZ, Weight);
			}
			else
			{
				mInitLegacy(mVp, mPostX, mPostY, mPreX, mPreY, Weight);
			}

			mPrepare(mVp);
		}
		public override void Calculate(IterationData data)
		{
			*mPreX = data.PreX;
			*mPreY = data.PreY;
			*mPreZ = data.PreZ;

			*mPostX = data.PostX;
			*mPostY = data.PostY;
			*mPostZ = data.PostZ;

			*mColor = data.Color;

			mCalculate(mVp);

			data.PreX = *mPreX;
			data.PreY = *mPreY;
			data.PreZ = *mPreZ;

			data.PostX = *mPostX;
			data.PostY = *mPostY;
			data.PostZ = *mPostZ;

			data.Color = *mColor;
		}

		private T LoadProc<T>(IntPtr hModule, string name)
		{
			var ptr = GetProcAddress(hModule, name);
			if (ptr == IntPtr.Zero)
				return (T)(object)null;

			return (T)(object)Marshal.GetDelegateForFunctionPointer(ptr, typeof (T));
		}
		private void LoadModule()
		{
			if (mHModule != IntPtr.Zero)
			{
				DisposeOverride(true);
			}

			mHModule = LoadLibrary(mDllPath);

			mCreate = LoadProc<PluginVarCreateDelegate>(mHModule, @"PluginVarCreate");
			mDestroy = LoadProc<PluginVarDestroyDelegate>(mHModule, @"PluginVarDestroy");
			mGetVariableNameAt = LoadProc<PluginVarGetVariableNameAtDelegate>(mHModule, @"PluginVarGetVariableNameAt");
			mResetVariable = LoadProc<PluginVarResetVariableDelegate>(mHModule, @"PluginVarResetVariable");
			mGetVariable = LoadProc<PluginVarGetVariableDelegate>(mHModule, @"PluginVarGetVariable");
			mSetVariable = LoadProc<PluginVarSetVariableDelegate>(mHModule, @"PluginVarSetVariable");
			mGetName = LoadProc<PluginVarGetNameDelegate>(mHModule, @"PluginVarGetName");
			mGetNrVariables = LoadProc<PluginVarGetNrVariablesDelegate>(mHModule, @"PluginVarGetNrVariables");
			mCalculate = LoadProc<PluginVarCalculateDelegate>(mHModule, @"PluginVarCalc");
			mPrepare = LoadProc<PluginVarPrepareDelegate>(mHModule, @"PluginVarPrepare");

			if (mCreate == null || mDestroy == null || mGetVariableNameAt == null || mResetVariable == null ||
			    mGetVariable == null || mSetVariable == null || mGetName == null || mGetNrVariables == null || 
				mCalculate == null || mPrepare == null)
			{
				throw new ApophysisException(Messages.PluginMethodNotFoundError);
			}

			Initialize();

			var init = GetProcAddress(mHModule, @"PluginVarInitDC");
			if (init != IntPtr.Zero)
			{
				mInit = (PluginVarInitDcDelegate) Marshal.GetDelegateForFunctionPointer(init, typeof (PluginVarInitDcDelegate));
			}
			else
			{
				init = GetProcAddress(mHModule, @"PluginVarInit3D");
				if (init != IntPtr.Zero)
				{
					mInitLegacy3D = (PluginVarInit3DDelegate) Marshal.GetDelegateForFunctionPointer(init, typeof (PluginVarInit3DDelegate));
				}
				else
				{
					mInitLegacy = (PluginVarInitDelegate) Marshal.GetDelegateForFunctionPointer(GetProcAddress(mHModule, @"PluginVarInit"), typeof(PluginVarInitDelegate));
					if (mInitLegacy == null)
					{
						throw new ApophysisException(Messages.PluginMethodNotFoundError);
					}
				}
			}

			var namePtr = mGetName();

			mName = GetStringFromPointer(namePtr);
			mVariables = new List<string>();

			var count = mGetNrVariables();

			for (int i = 0; i < count; i++)
			{
				namePtr = mGetVariableNameAt(i);
				mVariables.Add(GetStringFromPointer(namePtr));
			}

			//hack: pretty bad but for now we clone Apophysis' behavior...
			if (mName.ToLower().StartsWith(@"pre_"))
			{
				Priority = VariationPriority.Pre;
			}
			else if (mName.ToLower().StartsWith(@"post_"))
			{
				Priority = VariationPriority.Post;
			}
		}
		private void Initialize()
		{
			mVp = mCreate();

			mPreX = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();
			mPreY = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();
			mPreZ = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();

			mPostX = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();
			mPostY = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();
			mPostZ = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();

			mColor = (double*)Marshal.AllocHGlobal(sizeof(double)).ToPointer();
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
			if (mHModule == IntPtr.Zero)
			{
				throw new ObjectDisposedException(GetType().Name);
			}

			var instance = new ExternalVariation(mDllPath, mHModule)
			{
				mCreate = mCreate,
				mDestroy = mDestroy,
				mGetName = mGetName,
				mGetNrVariables = mGetNrVariables,
				mGetVariableNameAt = mGetVariableNameAt,
				mInit = mInit,
				mInitLegacy = mInitLegacy,
				mInitLegacy3D = mInitLegacy3D,
				mResetVariable = mResetVariable,
				mGetVariable = mGetVariable,
				mSetVariable = mSetVariable,
				mPrepare = mPrepare,
				mCalculate = mCalculate
			};

			instance.Initialize();
			
			instance.mName = mName;
			instance.mVariables = new List<string>();
			instance.mVariables.AddRange(mVariables);

			instance.Priority = Priority;

			return instance;
		}
	}
}