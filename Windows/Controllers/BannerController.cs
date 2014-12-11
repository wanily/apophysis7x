using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class BannerController : Controller<IBannerView>, IBannerController
	{
		public BannerController()
		{
			View.Show();
		}

		protected override void DisposeOverride(bool disposing)
		{
			if (!disposing) 
				return;

			View.Close();
		}

		public string BannerText
		{
			get { return View.BannerText; }
			set { View.BannerText = value; }
		}
	}
}
