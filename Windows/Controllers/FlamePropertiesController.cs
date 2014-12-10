using System;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FlamePropertiesController : Controller<FlameProperties>, IFlamePropertiesController
	{
		private readonly Lock mInitialize = new Lock();
		private MainController mParent;
		private Flame mFlame;

		private FlamePropertiesPreviewController mPreviewController;
		private FlamePropertiesToolbarController mToolbarController;

		private FlameProperties3DCameraController m3DCameraController;
		private FlamePropertiesCameraController mCameraController;
		private FlamePropertiesImagingController mImagingController;
		private FlamePropertiesCanvasController mCanvasController;
		private FlamePropertiesPaletteController mPaletteController;

		public FlamePropertiesController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;

			mPreviewController = new FlamePropertiesPreviewController(View, this);
			mToolbarController = new FlamePropertiesToolbarController(View, this);
			m3DCameraController = new FlameProperties3DCameraController(View, this);
			mCameraController = new FlamePropertiesCameraController(View, this);
			mImagingController = new FlamePropertiesImagingController(View, this);
			mCanvasController = new FlamePropertiesCanvasController(View, this);
			mPaletteController = new FlamePropertiesPaletteController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mPaletteController != null)
				{
					mPaletteController.Dispose();
					mPaletteController = null;
				}

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
			}

			mFlame = null;
			mParent = null;
		}

		protected override void AttachView()
		{
			mPreviewController.Initialize();
			mToolbarController.Initialize();

			m3DCameraController.Initialize();
			mCameraController.Initialize();
			mImagingController.Initialize();
			mCanvasController.Initialize();
			mPaletteController.Initialize();

			UndoController.StackChanged += OnUndoStackChanged;
		}
		protected override void DetachView()
		{
			UndoController.StackChanged -= OnUndoStackChanged;
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
		public void UpdatePalette()
		{
			mPaletteController.Update();
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
		public IUndoController UndoController
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
			mPaletteController.Update();
			
			UpdateToolbar();
			UpdatePreview();
		}

		public event EventHandler FlameChanged;
		public void RaiseFlameChanged()
		{
			if (FlameChanged != null)
				FlameChanged(this, new EventArgs());
		}

		public void ApplyCanvas(bool withResize)
		{
			if (mFlame == null)
				return;

			mPreviewController.UpdatePreview();

			if (withResize)
			{
				mParent.ResizeWithoutUpdatingPreview();
			}

			CommitValue();
			UpdateCamera();

			mParent.BatchListController.UpdateSelectedPreview();
		}
		public void CommitValue()
		{
			UndoController.CommitChange(Flame);
			PreviewController.DelayedUpdatePreview();
			RaiseFlameChanged();
		}

		private void OnUndoStackChanged(object sender, EventArgs e)
		{
			UpdateToolbar();
		}
	}
}