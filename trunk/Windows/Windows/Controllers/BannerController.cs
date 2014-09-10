using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class BannerController : WindowController<Banner>
	{
		protected override void AttachView()
		{
			View.Show();
		}
		protected override void DetachView()
		{
			View.Close();
		}

		public string BannerText
		{
			get { return View.BannerText; }
			set { View.BannerText = value; }
		}
	}
}
