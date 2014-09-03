using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;

namespace Xyrus.Apophysis.Windows.Controls
{
	[PublicAPI, DesignerSerializer(typeof(EditorSettingsSerializer), typeof(CodeDomSerializer))]
	public class EditorSettings : Component
	{
		private EditorGridContextMenu mContextMenu;
		private bool mZoomAutomatically;
		private bool mShowVariationPreview;
		private bool mLockAxes;
		private double mMoveAmount;
		private double mAngleSnap;
		private double mScaleSnap;

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

		public double MoveAmount
		{
			get { return mMoveAmount; }
			set
			{
				mMoveAmount = value;
				RaiseSettingsChanged();
			}
		}
		public double AngleSnap
		{
			get { return mAngleSnap; }
			set
			{
				mAngleSnap = value;
				RaiseSettingsChanged();
			}
		}
		public double ScaleSnap
		{
			get { return mScaleSnap; }
			set
			{
				mScaleSnap = value;
				RaiseSettingsChanged();
			}
		}

		public bool ZoomAutomatically
		{
			get { return mZoomAutomatically; }
			set
			{
				mZoomAutomatically = value;
				RaiseSettingsChanged();
			}
		}
		public bool ShowVariationPreview
		{
			get { return mShowVariationPreview; }
			set
			{
				mShowVariationPreview = value;
				RaiseSettingsChanged();
			}
		}
		public bool LockAxes
		{
			get { return mLockAxes; }
			set
			{
				mLockAxes = value;
				RaiseSettingsChanged();
			}
		}

		public event EventHandler SettingsChanged;
		private void RaiseSettingsChanged()
		{
			if (SettingsChanged != null)
			{
				SettingsChanged(this, new EventArgs());
			}
		}
	}
}