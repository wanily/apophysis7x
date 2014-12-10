using System;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IMainMenuController : IViewController
	{
		void UpdateButtonStates();

		void OnNewFlameClick(object sender, EventArgs e);
		void OnOpenBatchClick(object sender, EventArgs e);
		void OnRestoreAutosaveClick(object sender, EventArgs e);
		void OnSaveFlameClick(object sender, EventArgs e);
		void OnSaveBatchClick(object sender, EventArgs e);
		void OnPaletteFromImageClick(object sender, EventArgs e);
		void OnBrowsePalettesClick(object sender, EventArgs e);
		void OnRandomBatchClick(object sender, EventArgs e);
		void OnRenderFlameClick(object sender, EventArgs e);
		void OnRenderBatchClick(object sender, EventArgs e);
		void OnExitClick(object sender, EventArgs e);

		void OnUndoClick(object sender, EventArgs e);
		void OnRedoClick(object sender, EventArgs e);
		void OnCopyClick(object sender, EventArgs e);
		void OnPasteClick(object sender, EventArgs e);

		void OnFullscreenClick(object sender, EventArgs e);
		void OnEditorClick(object sender, EventArgs e);
		void OnFlamePropertiesClick(object sender, EventArgs e);
		void OnPalettePropertiesClick(object sender, EventArgs e);
		void OnCanvasPropertiesClick(object sender, EventArgs e);
		void OnMessagesClick(object sender, EventArgs e);
		void OnSettingsClick(object sender, EventArgs e);
		void OnShowToolbarClick(object sender, EventArgs e);
		void OnShowStatusbarClick(object sender, EventArgs e);
		void OnShowBatchListClick(object sender, EventArgs e);

		void OnResetLayoutClick(object sender, EventArgs e);
		void OnResetCameraClick(object sender, EventArgs e);
		void OnRandomizeClick(object sender, EventArgs e);
		void OnSummarizeClick(object sender, EventArgs e);

		void OnAboutClick(object sender, EventArgs e);
	}
}