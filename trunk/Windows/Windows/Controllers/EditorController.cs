using System;
using System.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class EditorController : WindowController<Editor>
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
			mVariablesController = new IteratorVariablesController(View, this);
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
				MoveAmount = ApophysisSettings.EditorMoveDistance,
				AngleSnap = ApophysisSettings.EditorRotateAngle,
				ScaleSnap = ApophysisSettings.EditorScaleRatio,
				LockAxes = ApophysisSettings.EditorLockAxes,
				ShowVariationPreview = ApophysisSettings.EditorShowVariationPreview,
				ZoomAutomatically = ApophysisSettings.EditorAutoZoom
			};

			View.IteratorCanvas.ShowRuler = ApophysisSettings.EditorShowRulers;
			View.IteratorCanvas.PreviewDensity = ApophysisSettings.EditorVariationPreviewDensity;
			View.IteratorCanvas.PreviewRange = ApophysisSettings.EditorVariationPreviewRange;
			View.IteratorCanvas.PreviewApplyPostTransform = ApophysisSettings.EditorVariationPreviewApplyPostTransform;

			mEditorUndoController.Initialize();
			mEditorPreviewController.Initialize();
			mToolbarController.Initialize();
			mPropertiesController.Initialize();
			mSelectionController.Initialize();
			mColorController.Initialize();
			mPointEditController.Initialize();
			mVectorEditController.Initialize();
			mVariationsController.Initialize();
			mVariablesController.Initialize();

			View.IteratorCanvas.Edit += OnCanvasEdit;
			View.IteratorCanvas.ActiveMatrixChanged += OnActiveMatrixChanged;

			UpdateActiveMatrix();
		}
		protected override void DetachView()
		{
			ApophysisSettings.EditorLockAxes = mSettings.LockAxes;
			ApophysisSettings.EditorMoveDistance = mSettings.MoveAmount;
			ApophysisSettings.EditorRotateAngle = mSettings.AngleSnap;
			ApophysisSettings.EditorScaleRatio = mSettings.ScaleSnap;
			ApophysisSettings.EditorShowRulers = View.IteratorCanvas.ShowRuler;
			ApophysisSettings.EditorShowVariationPreview = mSettings.ShowVariationPreview;
			ApophysisSettings.EditorAutoZoom = mSettings.ZoomAutomatically;
			ApophysisSettings.EditorVariationPreviewDensity = View.IteratorCanvas.PreviewDensity;
			ApophysisSettings.EditorVariationPreviewRange = View.IteratorCanvas.PreviewRange;
			ApophysisSettings.EditorVariationPreviewApplyPostTransform = View.IteratorCanvas.PreviewApplyPostTransform;

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

		internal void UpdatePreviewsGlobally()
		{
			mParent.UpdatePreviews(false);
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