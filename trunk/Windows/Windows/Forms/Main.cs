using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		private ControlVisualChain<PictureBox> mPainter;
		private InputHandlerChain mInput;

		private PreviewBackgroundVisual mBackgroundVisual;
		private PreviewImageVisual mImageVisual;
		private PreviewGuidelinesVisual mGuidelinesVisual;
		private PreviewInputVisual mInputVisual;
		private CameraEditInputHandler mCameraEditHandler;

		private InputController mInputController;

		private bool mShowGuidelines;
		private bool mShowTransparency;

		private CameraEditMode mCameraEditMode;
		private bool mCameraEditUseScale;

		public Main()
		{
			InitializeComponent();
			mInputController = new InputController();

			mPainter = new ControlVisualChain<PictureBox>(PreviewPicture);
			mPainter.Add(mBackgroundVisual = new PreviewBackgroundVisual(PreviewPicture), 100);
			mPainter.Add(mImageVisual = new PreviewImageVisual(PreviewPicture), 200);
			mPainter.Add(mGuidelinesVisual = new PreviewGuidelinesVisual(PreviewPicture), 300);
			mPainter.Add(mInputVisual = new PreviewInputVisual(PreviewPicture), 400);

			mInput = new InputHandlerChain(PreviewPicture);
			mInput.Add(mCameraEditHandler = new CameraEditInputHandler(PreviewPicture, mInputVisual));

			mCameraEditHandler.BeginEdit += OnCameraBeginEdit;
			mCameraEditHandler.CameraChanged += OnCameraChanged;
			mCameraEditHandler.EndEdit += OnCameraEndEdit;

			// hack http://stackoverflow.com/questions/2646606/c-sharp-winforms-statusstrip-how-do-i-reclaim-the-space-from-the-grip
			StatusBar.Padding = new Padding(StatusBar.Padding.Left, StatusBar.Padding.Top, StatusBar.Padding.Left, StatusBar.Padding.Bottom);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mCameraEditHandler != null)
				{
					mCameraEditHandler.BeginEdit -= OnCameraBeginEdit;
					mCameraEditHandler.CameraChanged -= OnCameraChanged;
					mCameraEditHandler.EndEdit -= OnCameraEndEdit;
				}

				if (mPainter != null)
				{
					mPainter.Dispose();
					mPainter = null;
				}

				if (mInput != null)
				{
					mInput.Dispose();
					mInput = null;
				}

				if (components != null)
				{
					components.Dispose();
				}
			}

			base.Dispose(disposing);

			mInputController = null;
			mBackgroundVisual = null;
			mGuidelinesVisual = null;
			mImageVisual = null;
			mInputVisual = null;
			mCameraEditHandler = null;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void OnWindowLoaded(object sender, EventArgs e)
		{
			UpdateBatchListColumnSize();
		}
		private void OnDensityKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputController.HandleKeyPressForIntegerTextBox(e);
		}
		private void OnCameraChanged(object sender, CameraChangedEventArgs args)
		{
			if (CameraChanged != null)
				CameraChanged(this, args);
		}
		private void OnCameraBeginEdit(object sender, EventArgs e)
		{
			if (CameraBeginEdit != null)
				CameraBeginEdit(this, new EventArgs());
		}
		private void OnCameraEndEdit(object sender, EventArgs e)
		{
			if (CameraEndEdit != null)
				CameraEndEdit(this, new EventArgs());
		}

		public bool ShowGuidelines
		{
			get { return mShowGuidelines; }
			set
			{
				mGuidelinesVisual.Visible = mShowGuidelines = value;
			}
		}
		public bool ShowTransparency
		{
			get { return mShowTransparency; }
			set
			{
				mBackgroundVisual.ShowTransparency = mShowTransparency = value;
			}
		}
		public bool FitFrame
		{
			get { return mInputVisual.FitFrame; }
			set
			{
				mInputVisual.FitFrame = value;
			}
		}

		public Flame PreviewedFlame
		{
			get { return mCameraEditHandler.Flame; }
			set
			{
				mCameraEditHandler.Flame = value;
				mInputVisual.ImageSize = value == null? new Size() : value.CanvasSize;
				mGuidelinesVisual.ImageSize = mInputVisual.ImageSize;
			}
		}
		public Bitmap PreviewImage
		{
			get { return mImageVisual.PreviewImage; }
			set
			{
				mImageVisual.PreviewImage = value;
			}
		}

		public CameraEditMode CameraEditMode
		{
			get { return mCameraEditMode; }
			set { mCameraEditHandler.EditMode = mCameraEditMode = value; }
		}
		public bool CameraEditUseScale
		{
			get { return mCameraEditUseScale; }
			set { mCameraEditHandler.UseScale = mCameraEditUseScale = value; }
		}

		public event EventHandler CameraBeginEdit;
		public event EventHandler CameraEndEdit;
		public event CameraChangedEventHandler CameraChanged;

		internal void UpdateBatchListColumnSize()
		{
			BatchListView.Columns[0].Width = BatchListView.ClientSize.Width - 3;
		}
	}
}
