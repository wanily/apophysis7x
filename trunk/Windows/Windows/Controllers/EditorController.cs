using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class EditorController : WindowController<Editor>
	{
		private UndoController mUndoController;
		private EditorUndoController mEditorUndoController;
		private EditorKeyboardController mKeyboardController;
		private EditorToolbarController mToolbarController;
		private IteratorPropertiesController mPropertiesController;
		private IteratorSelectionController mSelectionController;
		private IteratorColorController mColorController;
		private IteratorPointEditController mPointEditController;
		private IteratorVectorEditController mVectorEditController;

		private Lock mInitialize = new Lock();
		private EditorSettings mSettings;
		private Flame mFlame;

		public EditorController([NotNull] UndoController undoController)
		{
			if (undoController == null) throw new ArgumentNullException("undoController");
			mUndoController = undoController;

			mEditorUndoController = new EditorUndoController(View, this);
			mKeyboardController = new EditorKeyboardController(View, this);
			mToolbarController = new EditorToolbarController(View, this);
			mPropertiesController = new IteratorPropertiesController(View, this);
			mSelectionController = new IteratorSelectionController(View, this);
			mColorController = new IteratorColorController(View, this);
			mPointEditController = new IteratorPointEditController(View, this);
			mVectorEditController = new IteratorVectorEditController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
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

				if (mKeyboardController != null)
				{
					mKeyboardController.Dispose();
					mKeyboardController = null;
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

			mUndoController = null;
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

			mEditorUndoController.Initialize();
			mKeyboardController.Initialize();
			mToolbarController.Initialize();
			mPropertiesController.Initialize();
			mSelectionController.Initialize();
			mColorController.Initialize();
			mPointEditController.Initialize();
			mVectorEditController.Initialize();

			View.IteratorCanvas.Edit += OnCanvasEdit;
			View.IteratorCanvas.ActiveMatrixChanged += OnActiveMatrixChanged;

			UpdateActiveMatrix();
			Flame = new Flame();
			mUndoController.Initialize(Flame);
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

			ApophysisSettings.Serialize();

			View.IteratorCanvas.Edit -= OnCanvasEdit;
			View.IteratorCanvas.ActiveMatrixChanged -= OnActiveMatrixChanged;
		}

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}
		internal void UpdateCoordinates()
		{
			mPointEditController.UpdatePointControls();
			mVectorEditController.UpdateVectorControls();
		}
		internal void UpdateActiveMatrix()
		{
			
		}
		internal void UpdateToolbar()
		{
			mToolbarController.UpdateButtonStates();
		}
		internal void AfterReset()
		{
			mSelectionController.BuildIteratorComboBox();
			mSelectionController.SelectIterator(mFlame.Iterators.First());
			UpdateToolbar();
		}

		[NotNull]
		public UndoController UndoController
		{
			get { return mUndoController; }
		}

		[NotNull]
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

				View.Text = string.Format("Editor - {0}", mFlame.CalculatedName);
			}
		}
		public void ReadFlameFromClipboard()
		{
			var clipboard = Clipboard.GetText();
			if (!string.IsNullOrEmpty(clipboard))
			{
				try
				{
					var flame = new Flame();
					
					flame.ReadXml(XElement.Parse(clipboard, LoadOptions.None));
					Flame = flame;
				}
				catch (ApophysisException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch (XmlException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
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