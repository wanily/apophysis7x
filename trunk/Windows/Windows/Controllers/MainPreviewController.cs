using System;
using System.Drawing;
using System.Globalization;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainPreviewController : Controller<Main>
	{
		private ThreadedRenderer mRenderer;
		private TimeLock mPreviewTimeLock;
		private MainController mParent;
		private Bitmap mBitmap;
		private int mPreviewDensity;

		public MainPreviewController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mPreviewTimeLock = new TimeLock(OnPreviewSizeChangedCallback);
			mPreviewTimeLock.Delay = 250;

			mRenderer = new ThreadedRenderer();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mRenderer != null)
				{
					mRenderer.Dispose();
					mRenderer = null;
				}

				if (mPreviewTimeLock != null)
				{
					mPreviewTimeLock.Dispose();
					mPreviewTimeLock = null;
				}

				if (mBitmap != null)
				{
					mBitmap.Dispose();
					mBitmap = null;
				}
			}

			mParent = null;
		}

		protected override void AttachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged += OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus += OnDensityChanged;
			View.PreviewPicture.SizeChanged += OnPreviewSizeChanged;

			using (mParent.Initializer.Enter())
			{
				PreviewDensity = ApophysisSettings.MainPreviewDensity;
			}
		}
		protected override void DetachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged -= OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus -= OnDensityChanged;
			View.PreviewPicture.SizeChanged -= OnPreviewSizeChanged;

			View.PreviewPicture.Image = null;

			ApophysisSettings.MainPreviewDensity = PreviewDensity;
		}

		private void OnPreviewSizeChangedCallback()
		{
			UpdatePreview();
		}
		private void OnPreviewSizeChanged(object sender, EventArgs e)
		{
			mPreviewTimeLock.Enter();
		}

		private void OnDensityChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			int value;
			if (!int.TryParse(View.PreviewDensityComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
				return;

			if (value <= 0)
				return;

			using (mParent.Initializer.Enter())
			{
				PreviewDensity = value;
			}

			UpdatePreview();
		}
		private void OnRendererFinished(Bitmap bitmap)
		{
			if (bitmap == null)
				return;

			if (mBitmap != null)
			{
				mBitmap.Dispose();
			}

			mBitmap = bitmap;

			View.PreviewPicture.Invoke(new Action(() =>
			{
				View.PreviewPicture.Image = bitmap;
				View.PreviewPicture.Refresh();
			}));
		}

		public int PreviewDensity
		{
			get { return mPreviewDensity; }
			set
			{
				mPreviewDensity = value;
				View.PreviewDensityComboBox.Text = value.ToString(InputController.Culture);

				UpdatePreview();
			}
		}

		public void UpdatePreview()
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (mParent.Initializer.IsBusy || flame == null)
				return;

			var density = (double)PreviewDensity;
			var size = View.PreviewPicture.ClientSize;

			View.PreviewPicture.Image = null;

			mRenderer.Cancel();
			mRenderer.StartCreateBitmap(flame, density, size, OnRendererFinished);
		}
	}
}