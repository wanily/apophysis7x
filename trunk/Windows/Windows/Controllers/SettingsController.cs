using System;

using Options = Xyrus.Apophysis.Windows.Forms.Settings;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class SettingsController : Controller<Options>
	{
		private MainController mParent;

		private SettingsCommonController mCommonController;
		private SettingsEditorController mEditorController;
		private SettingsViewController mViewController;
		private SettingsPreviewController mPreviewController;
		private SettingsAutosaveController mAutosaveController;

		public SettingsController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mCommonController = new SettingsCommonController(View, this);
			mEditorController = new SettingsEditorController(View, this);
			mViewController = new SettingsViewController(View, this);
			mPreviewController = new SettingsPreviewController(View, this);
			mAutosaveController = new SettingsAutosaveController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mAutosaveController != null)
				{
					mAutosaveController.Dispose();
					mAutosaveController = null;
				}

				if (mPreviewController != null)
				{
					mPreviewController.Dispose();
					mPreviewController = null;
				}

				if (mViewController != null)
				{
					mViewController.Dispose();
					mViewController = null;
				}

				if (mEditorController != null)
				{
					mEditorController.Dispose();
					mEditorController = null;
				}

				if (mCommonController != null)
				{
					mCommonController.Dispose();
					mCommonController = null;
				}
			}

			mParent = null;
		}

		protected override void AttachView()
		{
			mCommonController.Initialize();
			mEditorController.Initialize();
			mViewController.Initialize();
			mPreviewController.Initialize();
			mAutosaveController.Initialize();

			View.OkButton.Click += OnOkClick;
			View.CancelButton.Click += OnCancelClick;
		}
		protected override void DetachView()
		{
			View.OkButton.Click -= OnOkClick;
			View.CancelButton.Click -= OnCancelClick;
		}

		private void OnOkClick(object sender, EventArgs e)
		{
			mCommonController.WriteSettings();
			mEditorController.WriteSettings();
			mViewController.WriteSettings();
			mPreviewController.WriteSettings();
			mAutosaveController.WriteSettings();

			mParent.ReloadSettings();
			View.Close();
		}
		private void OnCancelClick(object sender, EventArgs e)
		{
			View.Close();
		}
	}
}
