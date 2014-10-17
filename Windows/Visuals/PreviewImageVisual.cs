using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class PreviewImageVisual : ControlVisual<PictureBox>
	{
		private Bitmap mPreviewImage;

		public PreviewImageVisual([NotNull] PictureBox control) : base(control)
		{
		}

		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mPreviewImage != null)
				{
					mPreviewImage.Dispose();
					mPreviewImage = null;
				}
			}
		}

		protected override void RegisterEvents(PictureBox control)
		{
		}
		protected override void UnregisterEvents(PictureBox control)
		{
		}

		public Bitmap PreviewImage
		{
			get { return mPreviewImage; }
			set
			{
				mPreviewImage = value;
				InvalidateControl();
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			if (PreviewImage == null || PreviewImage.PixelFormat == PixelFormat.Undefined /* disposed */) 
				return;

			var size = PreviewImage.Size.FitToFrame(AttachedControl.ClientSize);

			graphics.DrawImage(PreviewImage, new Rectangle(new Point(
				AttachedControl.ClientSize.Width / 2 - size.Width / 2,
				AttachedControl.ClientSize.Height / 2 - size.Height / 2),
				size));
		}
	}
}