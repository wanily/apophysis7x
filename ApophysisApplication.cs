using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Interfaces.Threading;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Threading;
using Xyrus.Apophysis.Variations;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;
using Messages = Xyrus.Apophysis.Strings.Messages;

namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class ApophysisApplication
	{
		private static BannerController mBanner;
		private static UnityContainer mContainer;

		static ApophysisApplication()
		{
			mContainer = new UnityContainer();
		}

		public static IMainController MainWindow { get; private set; }
		public static string BatchPathToOpen { get; private set; }

		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			RegisterSingletons();
			RegisterTypes();

			mBanner = new BannerController();
			mBanner.Initialize();

			SetupExceptionHandler();
			LoadVariations();
			ReadCommandLine(args);

			mBanner.BannerText = Messages.InitializationLoadingGuiMessage;

			using (MainWindow = Container.Resolve<IMainController>())
			{
				MainWindow.Initialize();

				mBanner.Dispose();
				mBanner = null;

				Application.Run((Form)MainWindow.View);
			}
		}

		static void RegisterTypes()
		{
			Container.RegisterType<NativeTimer, NativeTimer>();

			Container.RegisterType<IThreadController, ThreadController>();
			Container.RegisterType<IWaitImageController, WaitImageController>();

			Container.RegisterType<IAutosaveController, AutosaveController>();
			Container.RegisterType<IBatchListController, BatchListController>();
			Container.RegisterType<IEditorController, EditorController>();
			Container.RegisterType<IFlamePropertiesController, FlamePropertiesController>();
			Container.RegisterType<IFullscreenController, FullscreenController>();
			Container.RegisterType<IMainMenuController, MainMenuController>();
			Container.RegisterType<IMainPreviewController, MainPreviewController>();
			Container.RegisterType<IMainToolbarController, MainToolbarController>();
			Container.RegisterType<IMessagesController, MessagesController>();
			Container.RegisterType<IRenderController, RenderController>();
			Container.RegisterType<ISettingsController, SettingsController>();
			Container.RegisterType<IUndoController, UndoController>();
		}
		static void RegisterSingletons()
		{
			Container.RegisterType<IAboutView, About>(Singleton());
			Container.RegisterType<IBannerView, Banner>(Singleton());
			Container.RegisterType<IEditorView, Editor>(Singleton());
			Container.RegisterType<IFlamePropertiesView, FlameProperties>(Singleton());
			Container.RegisterType<IFullscreenView, Fullscreen>(Singleton());
			Container.RegisterType<IMainView, Main>(Singleton());
			Container.RegisterType<IMessagesView, Windows.Forms.Messages>(Singleton());
			Container.RegisterType<IRenderView, Render>(Singleton());
			Container.RegisterType<ISettingsView, Windows.Forms.Settings>(Singleton());

			Container.RegisterType<IMainController, MainController>(Singleton());
		}

		static void LoadVariations()
		{
			mBanner.BannerText = Messages.InitializationLoadingVariationsMessage;

			Variation.VariationsIn15CStyle = ApophysisSettings.Common.VariationsIn15CStyle;

			var types = typeof(Linear).Assembly.GetTypes();
			var registerMethod = typeof(VariationRegistry).GetMethod(@"Register", BindingFlags.Static | BindingFlags.Public);

			foreach (var type in types)
			{
				if (!typeof(Variation).IsAssignableFrom(type) || type.IsAbstract || type == typeof(ExternalVariation))
					continue;

				//special treatment for "linear3D"
				if (Variation.VariationsIn15CStyle && type == typeof (Linear3D))
					continue;

				var method = registerMethod.MakeGenericMethod(type);
				var result = method.Invoke(null, new object[0]) as string;

				Debug.Assert(!string.IsNullOrEmpty(result));
				mBanner.BannerText = result;

				Application.DoEvents();
			}

			var defaultPluginDir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty, "Plugins");
			var pluginDir = ApophysisSettings.Common.PluginDirectoryName;

			if (string.IsNullOrEmpty(pluginDir) || string.IsNullOrEmpty(pluginDir.Trim()))
			{
				if (!Directory.Exists(defaultPluginDir))
					Directory.CreateDirectory(defaultPluginDir);

				pluginDir = ApophysisSettings.Common.PluginDirectoryName = defaultPluginDir;
			}

			if (Directory.Exists(pluginDir))
			{
				var files = Directory.GetFiles(pluginDir, "*.dll");
				var errorList = new List<string>();

				foreach (var file in files)
				{
					if (File.Exists(file + @".bad"))
						continue;

					try
					{
						if (!ExternalVariation.FitsCurrentMachineType(file))
							continue;

						var name = VariationRegistry.RegisterDll(file);
						mBanner.BannerText = name;
#if DEBUG
						Thread.Sleep(10);
#endif
						Application.DoEvents();

					}
					catch (ApophysisException exception)
					{
						errorList.Add(string.Format(@"   - {0}: {1}", Path.GetFileName(file), exception.Message));
						MessageCenter.SendMessage(string.Format(Messages.LoadingPluginErrorLogMessage,  Path.GetFileName(file), exception.Message));

						File.WriteAllText(file + @".bad", Messages.BadPluginMessageFileContent);
					}
				}

				if (errorList.Count > 0)
				{
					var body = string.Join(Environment.NewLine, errorList.ToArray());
					if (errorList.Count > 10)
					{
						body = string.Join(Environment.NewLine, errorList.Take(10).ToArray()) + Environment.NewLine + @"   - ...";
						body += Environment.NewLine + Environment.NewLine + Messages.GenericProblemListExceedsMaxSizeMessage;
					}

					MessageBox.Show(
						string.Format(Messages.VariationInitializationCollectiveError, body),
						Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}
		static void SetupExceptionHandler()
		{
			mBanner.BannerText = Messages.InitializationMessage;

#if !DEBUG
			Application.ThreadException += OnThreadException;
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
#endif
		}
		static void ReadCommandLine(string[] args)
		{
			var batchPath = args.Length == 0 ? null : args[0];
			if (batchPath == null)
			{
				BatchPathToOpen = null;
				return;
			}
			
			BatchPathToOpen = batchPath;

			if (string.IsNullOrEmpty(Path.GetDirectoryName(batchPath)))
			{
				BatchPathToOpen = Path.Combine(Environment.CurrentDirectory, batchPath);
			}
		}

		static void HandleException(Exception exception)
		{
			if (exception == null)
				return;

			var timestamp = DateTime.Now;

			var fullData = exception.ToString();
			var savePath = string.Format(
				@"%userprofile%\\apophysis-exception-{0:0000}-{1:00}-{2:00}_{3:00}.{4:00}.{5:00}.txt",
				timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second);

			savePath = Environment.ExpandEnvironmentVariables(savePath);

			var saveMessage = string.Format(Messages.UnhandledExceptionSaveMessage, savePath);

			try
			{
				File.WriteAllText(savePath, fullData);
			}
			catch (Exception saveException)
			{
				saveMessage = string.Format(
					Messages.UnhandledExceptionSaveFailureMessage, savePath,
					saveException.Message);
			}

			var locationString = Common.UnknownPlaceholder;
			if (exception.TargetSite != null)
			{
				locationString = exception.TargetSite.ToString();
			}

			var message = string.Format(
				Messages.UnhandledExceptionMessage,
				saveMessage, exception.Message, locationString);

			MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		
		private static LifetimeManager Singleton()
		{
			return new ContainerControlledLifetimeManager();
		}

		public static UnityContainer Container
		{
			get { return mContainer; }
		}
		public static void ClearContainer()
		{
			mContainer = new UnityContainer();
		}

		public static void Reset()
		{
			if (MainWindow != null)
			{
				MainWindow.Dispose();
				MainWindow = null;
			}

			mContainer.Dispose();
			ClearContainer();
		}

		private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			HandleException(e.Exception);
		}
		private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Trace.TraceError(e.ExceptionObject.ToString());
			HandleException(e.ExceptionObject as Exception);
		}
	}
}
