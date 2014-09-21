using System;
using System.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class EditorController : Controller<Editor>
	{
		private MainController mParent;

		private EditorUndoController mEditorUndoController;
		private EditorPreviewController mEditorPreviewController;
		private EditorToolbarController mToolbarController;
		private IteratorPropertiesController mPropertiesController;
		private IteratorSelectionController mSelectionController;
		private IteratorColorController mColorController;
		private IteratorPointEditController mPointEditController;
		private IteratorVectorEditController mVectorEditController;
		private IteratorVariationsController mVariationsController;
		private IteratorVariablesController mVariablesController;

		private Lock mInitialize = new Lock();
		private EditorSettings mSettings;
		private Flame mFlame;

		public EditorController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mEditorUndoController = new EditorUndoController(View, this);
			mEditorPreviewController = new EditorPreviewController(View, this);
			mToolbarController = new EditorToolbarController(View, this);
			mPropertiesController = new IteratorPropertiesController(View, this);
			mSelectionController = new IteratorSelectionController(View, this);
			mColorController = new IteratorColorController(View, this);
			mPointEditController = new IteratorPointEditController(View, this);
			mVectorEditController = new IteratorVectorEditController(View, this);
			mVariationsController = new IteratorVariationsController(View, this);
			mVariablesController = new IteratorVariablesController(View);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mVariablesController != null)
				{
					mVariablesController.Dispose();
					mVariablesController = null;
				}

				if (mVariationsController != null)
				{
					mVariationsController.Dispose();
					mVariationsController = null;
				}

				if (mVectorEditController != null)
				{
					mVectorEditController.Dispose();
					mVectorEditController = null;
				}

				if (mPointEditController != null)
				{
					mPointEditController.Dispose();
					mPointEditController = null;
				}

				if (mColorController != null)
				{
					mColorController.Dispose();
					mColorController = null;
				}

				if (mSelectionController != null)
				{
					mSelectionController.Dispose();
					mSelectionController = null;
				}

				if (mPropertiesController != null)
				{
					mPropertiesController.Dispose();
					mPropertiesController = null;
				}

				if (mToolbarController != null)
				{
					mToolbarController.Dispose();
					mToolbarController = null;
				}

				if (mEditorPreviewController != null)
				{
					mEditorPreviewController.Dispose();
					mEditorPreviewController = null;
				}

				if (mEditorUndoController != null)
				{
					mEditorUndoController.Dispose();
					mEditorUndoController = null;
				}

				if (mInitialize != null)
				{
					mInitialize.Dispose();
					mInitialize = null;
				}

				if (mFlame != null)
				{
					mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
					mFlame = null;
				}
			}

			mParent = null;
		}

		protected override void AttachView()
		{
			View.IteratorCanvas.Settings = mSettings = new EditorSettings
			{
				MoveAmount = ApophysisSettings.Editor.MoveDistance,
				AngleSnap = ApophysisSettings.Editor.RotateAngle,
				ScaleSnap = ApophysisSettings.Editor.ScaleRatio,
				LockAxes = ApophysisSettings.Editor.LockAxes,
				ShowVariationPreview = ApophysisSettings.Editor.ShowVariationPreview,
				ZoomAutomatically = ApophysisSettings.Editor.AutoZoom
			};

			View.IteratorCanvas.ShowRuler = ApophysisSettings.Editor.ShowRulers;
			View.IteratorCanvas.PreviewDensity = ApophysisSettings.Editor.VariationPreviewDensity;
			View.IteratorCanvas.PreviewRange = ApophysisSettings.Editor.VariationPreviewRange;
			View.IteratorCanvas.PreviewApplyPostTransform = ApophysisSettings.Editor.VariationPreviewApplyPostTransform;

			mToolbarController.Initialize();
			mPropertiesController.Initialize();
			mSelectionController.Initialize();
			mColorController.Initialize();
			mPointEditController.Initialize();
			mVectorEditController.Initialize();
			mVariationsController.Initialize();
			mVariablesController.Initialize();
			mEditorUndoController.Initialize();
			mEditorPreviewController.Initialize();

			View.IteratorCanvas.Edit += OnCanvasEdit;
			View.IteratorCanvas.ActiveMatrixChanged += OnActiveMatrixChanged;

			UpdateActiveMatrix();
		}
		protected override void DetachView()
		{
			ApophysisSettings.Editor.LockAxes = mSettings.LockAxes;
			ApophysisSettings.Editor.MoveDistance = mSettings.MoveAmount;
			ApophysisSettings.Editor.RotateAngle = mSettings.AngleSnap;
			ApophysisSettings.Editor.ScaleRatio = mSettings.ScaleSnap;
			ApophysisSettings.Editor.ShowRulers = View.IteratorCanvas.ShowRuler;
			ApophysisSettings.Editor.ShowVariationPreview = mSettings.ShowVariationPreview;
			ApophysisSettings.Editor.AutoZoom = mSettings.ZoomAutomatically;
			ApophysisSettings.Editor.VariationPreviewDensity = View.IteratorCanvas.PreviewDensity;
			ApophysisSettings.Editor.VariationPreviewRange = View.IteratorCanvas.PreviewRange;
			ApophysisSettings.Editor.VariationPreviewApplyPostTransform = View.IteratorCanvas.PreviewApplyPostTransform;

			ApophysisSettings.Serialize();

			View.IteratorCanvas.Edit -= OnCanvasEdit;
			View.IteratorCanvas.ActiveMatrixChanged -= OnActiveMatrixChanged;
		}

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}

		public void UpdateCoordinates()
		{
			mPointEditController.UpdatePointControls();
			mVectorEditController.UpdateVectorControls();
		}
		public void UpdateActiveMatrix()
		{
			
		}
		public void UpdateToolbar()
		{
			mToolbarController.UpdateButtonStates();
		}
		public void UpdateVariations()
		{
			mVariationsController.UpdateList();
		}
		public void UpdateVariables()
		{
			mVariablesController.UpdateList();
		}
		public void UpdateColor()
		{
			mColorController.UpdateControls();
		}
		public void UpdateWindowTitle()
		{
			View.Text = string.Format("Editor - {0}", mFlame.CalculatedName);
		}
		public void UpdatePreview()
		{
			mEditorPreviewController.UpdatePreview();
		}

		private void AfterReset()
		{
			mSelectionController.BuildIteratorComboBox();
			mSelectionController.SelectIterator(mFlame.Iterators.First());

			UpdateToolbar();
			UpdatePreview();
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

				if (mFlame != null)
				{
					mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
				}

				mFlame = value;
				mFlame.Iterators.ContentChanged += OnIteratorCollectionChanged;

				using (mInitialize.Enter())
				{
					View.IteratorCanvas.Iterators = mFlame.Iterators;
					AfterReset();
				}

				UpdateWindowTitle();
			}
		}
		public Iterator Iterator
		{
			get { return View.IteratorCanvas.SelectedIterator; }
		}

		internal event EventHandler FlameChanged;
		internal void RaiseFlameChanged()
		{
			if (FlameChanged != null)
				FlameChanged(this, new EventArgs());
		}

		private void OnIteratorCollectionChanged(object sender, EventArgs e)
		{
			if (mInitialize.IsBusy)
				return;

			mSelectionController.BuildIteratorComboBox();
			UpdateToolbar();
		}
		private void OnCanvasEdit(object sender, EventArgs e)
		{
			UpdateCoordinates();
		}
		private void OnActiveMatrixChanged(object sender, EventArgs e)
		{
			UpdateActiveMatrix();
		}
	}
}