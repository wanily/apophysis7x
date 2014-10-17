using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlameProperties3DCameraController : DataInputController<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlameProperties3DCameraController(FlameProperties view, [NotNull] FlamePropertiesController parent) 
			: base(view, parent.Initializer)
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
			Register(View.DepthBlurDragPanel, 
				xx => mParent.Flame.DepthOfField = xx, 
				() => mParent.Flame.DepthOfField);
			Register(View.PitchDragPanel, 
				xx => mParent.Flame.Pitch = xx * System.Math.PI / 180.0, 
				() => mParent.Flame.Pitch * 180.0 / System.Math.PI);
			Register(View.YawDragPanel, 
				xx => mParent.Flame.Yaw = xx * System.Math.PI / 180.0, 
				() => mParent.Flame.Yaw * 180.0 / System.Math.PI);
			Register(View.HeightDragPanel, 
				xx => mParent.Flame.Height = xx, 
				() => mParent.Flame.Height);
			Register(View.PerspectiveDragPanel, 
				xx => mParent.Flame.Perspective = xx, 
				() => mParent.Flame.Perspective);
		}
		protected override void DetachView()
		{
			Cleanup();
		}

		protected override void OnValueCommittedOverride(object control)
		{
			mParent.CommitValue();
		}
		protected override void OnValueChangedOverride(object control)
		{
			mParent.PreviewController.DelayedUpdatePreview();
		}
	}
}