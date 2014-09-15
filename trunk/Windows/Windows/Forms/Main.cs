using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Main : Form
	{
		private ControlVisualChain<PictureBox> mPainter;
		private PreviewBackgroundVisual mBackgroundVisual;
		private PreviewImageVisual mImageVisual;
		private PreviewGuidelinesVisual mGuidelinesVisual;

		private InputController mInputController;

		private bool mShowGuidelines;
		private bool mShowTransparency;

		public Main()
		{
			InitializeComponent();
			mInputController = new InputController();

			mPainter = new ControlVisualChain<PictureBox>(PreviewPicture);
			mPainter.Add(mBackgroundVisual = new PreviewBackgroundVisual(PreviewPicture), 100);
			mPainter.Add(mImageVisual = new PreviewImageVisual(PreviewPicture), 200);
			mPainter.Add(mGuidelinesVisual = new PreviewGuidelinesVisual(PreviewPicture), 300);

			// hack http://stackoverflow.com/questions/2646606/c-sharp-winforms-statusstrip-how-do-i-reclaim-the-space-from-the-grip
			StatusBar.Padding = new Padding(StatusBar.Padding.Left, StatusBar.Padding.Top, StatusBar.Padding.Left, StatusBar.Padding.Bottom);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (mPainter != null)
				{
					mPainter.Dispose();
					mPainter = null;
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
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void OnWindowLoaded(object sender, System.EventArgs e)
		{
			UpdateBatchListColumnSize();
		}
		private void OnDensityKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputController.HandleKeyPressForIntegerTextBox(e);
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

		public Bitmap PreviewImage
		{
			get { return mImageVisual.PreviewImage; }
			set
			{
				mImageVisual.PreviewImage = value;
			}
		}

		internal void UpdateBatchListColumnSize()
		{
			BatchListView.Columns[0].Width = BatchListView.ClientSize.Width - 3;
		}
	}
}
