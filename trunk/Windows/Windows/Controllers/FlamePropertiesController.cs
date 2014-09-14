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
			mUndoController.Initialize();
			mPreviewController.Initialize();
			mToolbarController.Initialize();
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
			//todo set all the fields :D

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