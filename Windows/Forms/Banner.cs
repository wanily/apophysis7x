using System.Drawing;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Forms
{
	public sealed partial class Banner : Form
	{
		private string mBannerText;

		public Banner()
		{
			InitializeComponent();
			Text = Application.ProductName;
		}

		public string BannerText
		{
			get { return mBannerText; }
			set
			{
				mBannerText = value;
				Refresh();
			}
		}
		public string VersionText
		{
			get { return Application.ProductName + " " + Application.ProductVersion; }
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			const float margin = 15f;

			using (var brush = new SolidBrush(ForeColor))
			{
				var bannerSize = e.Graphics.MeasureString(BannerText, Font);
				var versionSize = e.Graphics.MeasureString(VersionText, Font);

				e.Graphics.DrawString(BannerText, Font, brush, ClientSize.Width - margin - bannerSize.Width, ClientSize.Height - margin - bannerSize.Height);
				e.Graphics.DrawString(VersionText, Font, brush, margin, ClientSize.Height - margin - versionSize.Height);

			}
		}
	}
}
