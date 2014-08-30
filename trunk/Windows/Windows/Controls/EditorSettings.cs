using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Xyrus.Apophysis.Windows.Controls
{
	[PublicAPI, DesignerSerializer(typeof(EditorSettingsSerializer), typeof(CodeDomSerializer))]
	public class EditorSettings : Component
	{
		private EditorGridContextMenu mContextMenu;

		protected override void Dispose(bool disposing)
		{
			UnbindContextMenu();
			base.Dispose(disposing);
		}

		internal void BindContextMenu([NotNull] EditorGridContextMenu menu)
		{
			if (menu == null) throw new ArgumentNullException("menu");

			mContextMenu = menu;
			mContextMenu.UpdateCheckedStates(this);
		}
		internal void UnbindContextMenu()
		{
			if (mContextMenu == null)
				return;

			mContextMenu.UpdateCheckedStates((EditorSettings)null);
			mContextMenu = null;
		}

		public double MoveAmount { get; set; }
		public double AngleSnap { get; set; }
		public double ScaleSnap { get; set; }

		public bool ZoomAutomatically { get; set; }
		public bool ShowVariationPreview { get; set; }
		public bool LockAxes { get; set; }
	}
}