using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainPreviewController : Controller<Main>
	{
		private TimeLock mPreviewTimeLock;
		private MainController mParent;
		private int mPreviewDensity;

		public MainPreviewController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mPreviewTimeLock = new TimeLock(OnPreviewSizeChangedCallback);
			mPreviewTimeLock.Delay = 250;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mPreviewTimeLock != null)
				{
					mPreviewTimeLock.Dispose();
					mPreviewTimeLock = null;
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

		private void OnPreviewSizeChangedCallback()
		{
			UpdatePreview();
		}
		private void OnPreviewSizeChanged(object sender, EventArgs e)
		{
			mPreviewTimeLock.Enter();
		}

		protected override void DetachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged -= OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus -= OnDensityChanged;

			ApophysisSettings.MainPreviewDensity = PreviewDensity;
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

			//todo
			Trace.TraceInformation(@"Updating preview ({3} - {0}x{1} / {2})", size.Width, size.Height, density, flame.CalculatedName);
		}
	}
}