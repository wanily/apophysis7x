using System;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FlamePropertiesController : WindowController<FlameProperties>
	{
		private Lock mInitialize = new Lock();
		private MainController mParent;
		private Flame mFlame;

		private FlamePropertiesUndoController mUndoController;
		private FlamePropertiesPreviewController mPreviewController;
		private FlamePropertiesToolbarController mToolbarController;

		public FlamePropertiesController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;

			mUndoController = new FlamePropertiesUndoController(View, this);
			mPreviewController = new FlamePropertiesPreviewController(View, this);
			mToolbarController = new FlamePropertiesToolbarController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
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
			View.DepthBlurDragPanel.ValueChanged += OnDepthBlurChanged;
			View.PitchDragPanel.ValueChanged += OnPitchChanged;
			View.YawDragPanel.ValueChanged += OnYawChanged;
			View.HeightDragPanel.ValueChanged += OnHeightChanged;
			View.PerspectiveDragPanel.ValueChanged += OnPerspectiveChanged;
			View.ScaleDragPanel.ValueChanged += OnScaleChanged;

			mUndoController.Initialize();
			mPreviewController.Initialize();
			mToolbarController.Initialize();
		}
		protected override void DetachView()
		{
			View.DepthBlurDragPanel.ValueChanged -= OnDepthBlurChanged;
			View.PitchDragPanel.ValueChanged -= OnPitchChanged;
			View.YawDragPanel.ValueChanged -= OnYawChanged;
			View.HeightDragPanel.ValueChanged -= OnHeightChanged;
			View.PerspectiveDragPanel.ValueChanged -= OnPerspectiveChanged;
			View.ScaleDragPanel.ValueChanged -= OnScaleChanged;
		}

		private void OnDepthBlurChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.DepthOfField = View.DepthBlurDragPanel.Value;
		}
		private void OnPitchChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Pitch = View.PitchDragPanel.Value * System.Math.PI / 180.0;
		}
		private void OnYawChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Yaw = View.YawDragPanel.Value * System.Math.PI / 180.0;
		}
		private void OnHeightChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Height = View.HeightDragPanel.Value;
		}
		private void OnPerspectiveChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Perspective = View.PerspectiveDragPanel.Value;
		}
		private void OnScaleChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.PixelsPerUnit = View.ScaleDragPanel.Value * mFlame.CanvasSize.Width / 100;
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

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}

		internal void UpdatePreviewsGlobally()
		{
			mParent.UpdatePreviews(false);
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
			View.DepthBlurDragPanel.Value = mFlame.DepthOfField;
			View.PitchDragPanel.Value = mFlame.Pitch * 180.0 / System.Math.PI;
			View.YawDragPanel.Value = mFlame.Yaw * 180.0 / System.Math.PI;
			View.HeightDragPanel.Value = mFlame.Height;
			View.PerspectiveDragPanel.Value = mFlame.Perspective;
			View.ScaleDragPanel.Value = mFlame.PixelsPerUnit * 100.0 / mFlame.CanvasSize.Width;

			UpdateToolbar();
			UpdatePreview();
		}

		internal event EventHandler FlameChanged;
		internal void RaiseFlameChanged()
		{
			if (FlameChanged != null)
				FlameChanged(this, new EventArgs());
		}
	}
}