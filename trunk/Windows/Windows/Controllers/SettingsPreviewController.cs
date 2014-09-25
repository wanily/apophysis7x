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
			if (!ApophysisSettings.Preview.ThreadCount.HasValue)
				View.ThreadsComboBox.SelectedIndex = 0;
			else View.ThreadsComboBox.Text = ApophysisSettings.Preview.ThreadCount.GetValueOrDefault().ToString(InputController.Culture);
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