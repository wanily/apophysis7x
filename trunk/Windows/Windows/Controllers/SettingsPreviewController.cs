using System;
using System.Globalization;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class SettingsPreviewController : Controller<Forms.Settings>
	{
		private SettingsController mParent;

		public SettingsPreviewController([NotNull] Forms.Settings view, [NotNull] SettingsController parent)
			: base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mParent = null;
		}

		protected override void AttachView()
		{
		}
		protected override void DetachView()
		{
		}

		public void Update()
		{
			View.ThreadsComboBox.SelectedIndex = ApophysisSettings.Preview.ThreadCount.GetValueOrDefault();
		}
		public void WriteSettings()
		{
			if (View.ThreadsComboBox.SelectedIndex == 0)
			{
				ApophysisSettings.Preview.ThreadCount = null;
			}
			else
			{
				ApophysisSettings.Preview.ThreadCount = int.Parse(View.ThreadsComboBox.Text, NumberStyles.Integer, InputController.Culture);
			}
		}
	}
}