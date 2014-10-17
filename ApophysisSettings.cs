using Xyrus.Apophysis.Settings.Providers;

namespace Xyrus.Apophysis
{
	public static class ApophysisSettings
	{
		static ApophysisSettings()
		{
			Editor = new EditorSettingsProvider();
			View = new ViewSettingsProvider();
			Preview = new PreviewSettingsProvider();
			Autosave = new AutosaveSettingsProvider();
			Common = new CommonSettingsProvider();
			Render = new RenderSettingsProvider();
		}

		public static EditorSettingsProvider Editor
		{
			get; 
			private set;
		}
		public static ViewSettingsProvider View
		{
			get;
			private set;
		}
		public static PreviewSettingsProvider Preview
		{
			get;
			private set;
		}
		public static AutosaveSettingsProvider Autosave
		{
			get;
			private set;
		}
		public static CommonSettingsProvider Common
		{
			get;
			private set;
		}
		public static RenderSettingsProvider Render
		{
			get;
			private set;
		}

		public static void Serialize()
		{
			Editor.Serialize();
			View.Serialize();
			Preview.Serialize();
			Autosave.Serialize();
			Common.Serialize();
			Render.Serialize();
		}
	}
}
