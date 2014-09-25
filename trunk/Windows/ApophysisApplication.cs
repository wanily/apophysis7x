using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Variations;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class ApophysisApplication
	{
		private static BannerController mBanner;

		public static MainController MainWindow { get; private set; }

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

			mBanner = new BannerController();
			mBanner.Initialize();

			SetupExceptionHandler();
			LoadVariations();

			mBanner.BannerText = Messages.InitializationLoadingGuiMessage;

			using (MainWindow = new MainController())
			{
				MainWindow.Initialize();

				mBanner.Dispose();
				mBanner = null;

				Application.Run(MainWindow.View);
			}
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
