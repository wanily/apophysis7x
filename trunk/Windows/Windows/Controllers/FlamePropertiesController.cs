using System;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FlamePropertiesController : Controller<FlameProperties>
	{
		private readonly Lock mInitialize = new Lock();
		private MainController mParent;
		private Flame mFlame;

		private FlamePropertiesUndoController mUndoController;
		private FlamePropertiesPreviewController mPreviewController;
		private FlamePropertiesToolbarController mToolbarController;

		private FlameProperties3DCameraController m3DCameraController;
		private FlamePropertiesCameraController mCameraController;
		private FlamePropertiesImagingController mImagingController;
		private FlamePropertiesCanvasController mCanvasController;

		public FlamePropertiesController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;

			mUndoController = new FlamePropertiesUndoController(View, this);
			mPreviewController = new FlamePropertiesPreviewController(View, this);
			mToolbarController = new FlamePropertiesToolbarController(View, this);

			m3DCameraController = new FlameProperties3DCameraController(View, this);
			mCameraController = new FlamePropertiesCameraController(View, this);
			mImagingController = new FlamePropertiesImagingController(View, this);
			mCanvasController = new FlamePropertiesCanvasController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mCanvasController != null)
				{
					mCanvasController.Dispose();
					mCanvasController = null;
				}

				if (m3DCameraController != null)
				{
					m3DCameraController.Dispose();
					m3DCameraController = null;
				}

				if (mCameraController != null)
				{
					mCameraController.Dispose();
					mCameraController = null;
				}

				if (mImagingController != null)
				{
					mImagingController.Dispose();
					mImagingController = null;
				}

				if (mToolbarController != null)
				{
					mToolbarController.Dispose();
					mToolbarController = null;
				}

				if (mPreviewController != null)
				{
					mPreviewController.Dispose();
					mPreviewController = null;
				}

				if (mUndoController != null)
				{
					mUndoController.Dispose();
					mUndoController = null;
				}
			}

			mFlame = null;
			mParent = null;
		}

		protected override void AttachView()
		{
			mPreviewController.Initialize();
			mToolbarController.Initialize();
			mUndoController.Initialize();

			m3DCameraController.Initialize();
			mCameraController.Initialize();
			mImagingController.Initialize();
			mCanvasController.Initialize();
		}
		protected override void DetachView()
		{
		}

		public void UpdateToolbar()
		{
			mToolbarController.UpdateButtonStates();
		}
		public void UpdateWindowTitle()
		{
			View.Text = string.Format("Adjustment - {0}", mFlame.CalculatedName);
		}
		public void UpdatePreview()
		{
			mPreviewController.UpdatePreview();
		}
		public void UpdateCamera()
		{
			mCameraController.Update();
		}

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}

		public FlamePropertiesPreviewController PreviewController
		{
			get { return mPreviewController; }
		}
		public UndoController UndoController
		{
			get { return mParent.UndoController; }
		}
		public Flame Flame
		{
			get { return mFlame; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				mFlame = value;

				using (mInitialize.Enter())
				{
					AfterReset();
				}

				UpdateWindowTitle();
			}
		}

		private void AfterReset()
		{
			mImagingController.Update();
			mCameraController.Update();
			m3DCameraController.Update();
			mCanvasController.Update();
			
			UpdateToolbar();
			UpdatePreview();
		}

		internal event EventHandler FlameChanged;
		internal void RaiseFlameChanged()
		{
			if (FlameChanged != null)
				FlameChanged(this, new EventArgs());
		}

		internal void ApplyCanvas(bool withResize)
		{
			if (mFlame == null)
				return;

			mPreviewController.UpdatePreview();

			if (withResize)
			{
				mParent.ResizeWithoutUpdatingPreview();
			}

			mParent.UndoController.CommitChange(mFlame);
			mParent.FlamePropertiesController.RaiseFlameChanged();
			mParent.FlamePropertiesController.UpdateCamera();

			mParent.BatchListController.UpdateSelectedPreview();
		}
	}
}