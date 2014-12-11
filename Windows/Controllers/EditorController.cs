using System;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class EditorController : Controller<IEditorView>, IEditorController
	{
		private Resolver<IMainController> mMainController;
		private Resolver<IEditorPreviewController> mPreviewController;
		private Resolver<IEditorToolbarController> mToolbarController;
		private Resolver<IIteratorController> mIteratorController;

		private Lock mInitialize = new Lock();
		
		private Flame mFlame;
		private IIteratorCanvasView mCanvas;

		public EditorController()
		{
			mIteratorController.Resolve();

			mFlame = mMainController.Object.Flame;
			mFlame.Iterators.ContentChanged += OnIteratorCollectionChanged;

			mMainController.Object.UndoController.StackChanged += OnUndoStackChanged;
			mMainController.Object.FlameChanged += OnFlameChanged;

			mCanvas = View.Canvas;
			mCanvas.Edit += OnCanvasEdit;
			mCanvas.LoadSettings();

			View.UndoEvent += OnUndoEvent;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mMainController.IsResolved)
				{
					mMainController.Object.UndoController.StackChanged -= OnUndoStackChanged;
					mMainController.Object.FlameChanged -= OnFlameChanged;
				}

				if (mCanvas != null)
				{
					mCanvas.Edit -= OnCanvasEdit;
					mCanvas.SaveSettings();
				}

				if (mFlame != null)
				{
					mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
				}

				if (View != null)
				{
					View.UndoEvent += OnUndoEvent;
				}

				mPreviewController.Reset();
				mToolbarController.Reset();
				mIteratorController.Reset();

				mMainController.Reset(false);

				if (mInitialize != null)
				{
					mInitialize.Dispose();
				}
			}

			mInitialize = null;
			mCanvas = null;
			mFlame = null;
		}

		public void UpdateWindowTitle()
		{
			View.SetHeader(mFlame);
		}

		public IEditorPreviewController Preview
		{
			get { return mPreviewController.Object; }
		}
		public IEditorToolbarController Toolbar
		{
			get { return mToolbarController.Object; }
		}

		public IIteratorController IteratorControls
		{
			get { return mIteratorController.Object; }
		}
		
		private void OnIteratorCollectionChanged(object sender, EventArgs e)
		{
			if (mInitialize.IsBusy)
				return;

			mIteratorController.Object.BuildIteratorComboBox();
			mToolbarController.Object.UpdateButtonStates();
		}
		private void OnFlameChanged(object sender, EventArgs e)
		{
			mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
			mFlame = mMainController.Object.Flame;
			mFlame.Iterators.ContentChanged += OnIteratorCollectionChanged;

			using (mInitialize.Enter())
			{
				mCanvas.Iterators = mFlame.Iterators;

				mIteratorController.Object.BuildIteratorComboBox();
				mToolbarController.Object.UpdateButtonStates();
				mPreviewController.Object.UpdatePreview();
			}

			UpdateWindowTitle();
		}
		
		private void OnCanvasEdit(object sender, EventArgs e)
		{
			mToolbarController.Object.UpdateButtonStates();
		}
		
		private void OnUndoStackChanged(object sender, EventArgs e)
		{
			mToolbarController.Object.UpdateButtonStates();
		}
		private void OnUndoEvent(object sender, EventArgs e)
		{
			mMainController.Object.RaiseUndoEvent();
		}
	}
}