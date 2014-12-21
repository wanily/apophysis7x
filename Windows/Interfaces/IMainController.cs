using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces
{
	public interface IMainController : IViewController
	{
		IUndoController UndoController { get; }
		IEditorController EditorController { get; }
		IFullscreenController FullscreenController { get; }
		IFlamePropertiesController FlamePropertiesController { get; }
		IMessagesController MessagesController { get; }
		ISettingsController SettingsController { get; }
		IRenderController RenderController { get; }
		IBatchListController BatchListController { get; }
		IMainMenuController MainMenuController { get; }
		IMainPreviewController MainPreviewController { get; }
		IMainToolbarController ToolbarController { get; }
		IAutosaveController AutosaveController { get; }

		[NotNull]
		Lock Initializer { get; }

		FlameCollection Flames { get; set; }

		void UpdateToolbar();
		void UpdateMenu();

		void UpdatePreviews(bool withMainPreview = true);
		void ResizeWithoutUpdatingPreview();

		void AppendFlame([NotNull] Flame flame);
		void RestoreAutosaveBatch(string targetPath);
		void ReadBatchFromFile(string path, bool withMessaging = true);
		void ReadFlameFromClipboard();
		void WriteCurrentFlameToClipboard();

		void LoadFlameAndEraseHistory([NotNull] Flame flame);
		void NotifyFlameNameChanged(Flame flame);

		void GenerateRandomFlames(int count, bool confirm = true);

		bool ConfirmExit();
		bool ConfirmReplaceBatch();

		void ReplaceBatchWithConfirm([NotNull] FlameCollection batch);
		void DeleteFlameIfPossibleWithConfirm([NotNull] Flame flame);

		void SetDirty();

		void SaveCurrentFlame(string path, int maxBatchSize = int.MaxValue);
		void SaveFlame([NotNull] Flame flame, string path, int maxBatchSize = int.MaxValue, string batchName = null);
		void SaveCurrentBatch(string path);

		void ShowEditor();
		void ShowFlameProperties();
		void ShowImagingProperties();
		void ShowPaletteProperties();
		void ShowCanvasProperties();
		void ShowMessages();
		void ShowSettings();
		void ShowRender();
		void ShowRenderAll();

		void ReloadSettings();
	}
}