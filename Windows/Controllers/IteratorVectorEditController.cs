using System;
using System.Globalization;
using System.Numerics;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class IteratorVectorEditController : Controller<Editor>, IIteratorVectorEditController
	{
		private EditorController mParent;
		private TextBox[] mVectorTextBoxes;

		public IteratorVectorEditController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			mVectorTextBoxes = new[]
			{
				View.IteratorPreAffineOxTextBox, View.IteratorPreAffineOyTextBox,
				View.IteratorPreAffineXxTextBox, View.IteratorPreAffineXyTextBox,
				View.IteratorPreAffineYxTextBox, View.IteratorPreAffineYyTextBox,

				View.IteratorPostAffineOxTextBox, View.IteratorPostAffineOyTextBox,
				View.IteratorPostAffineXxTextBox, View.IteratorPostAffineXyTextBox,
				View.IteratorPostAffineYxTextBox, View.IteratorPostAffineYyTextBox
			};

			foreach (var box in mVectorTextBoxes)
			{
				box.TextChanged += OnVectorChanged;
				box.LostFocus += OnVectorCommit;
			}

			View.IteratorResetPreAffineO.Click += OnResetVectorClick;
			View.IteratorResetPreAffineX.Click += OnResetVectorClick;
			View.IteratorResetPreAffineY.Click += OnResetVectorClick;
			View.IteratorResetPreAffine.Click += OnResetVectorClick;

			View.IteratorResetPostAffineO.Click += OnResetVectorClick;
			View.IteratorResetPostAffineX.Click += OnResetVectorClick;
			View.IteratorResetPostAffineY.Click += OnResetVectorClick;
			View.IteratorResetPostAffine.Click += OnResetVectorClick;

		}
		protected override void DetachView()
		{
			foreach (var box in mVectorTextBoxes)
			{
				box.TextChanged -= OnVectorChanged;
				box.LostFocus -= OnVectorCommit;
			}

			View.IteratorResetPreAffineO.Click -= OnResetVectorClick;
			View.IteratorResetPreAffineX.Click -= OnResetVectorClick;
			View.IteratorResetPreAffineY.Click -= OnResetVectorClick;
			View.IteratorResetPreAffine.Click -= OnResetVectorClick;

			View.IteratorResetPostAffineO.Click -= OnResetVectorClick;
			View.IteratorResetPostAffineX.Click -= OnResetVectorClick;
			View.IteratorResetPostAffineY.Click -= OnResetVectorClick;
			View.IteratorResetPostAffine.Click -= OnResetVectorClick;

			mVectorTextBoxes = null;
		}

		private void OnResetVectorClick(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;

			if (ReferenceEquals(sender, View.IteratorResetPreAffineO) || ReferenceEquals(sender, View.IteratorResetPreAffine))
			{
				iterator.PreAffine = iterator.PreAffine.Alter(m31: 0, m32: 0);
			}
			if (ReferenceEquals(sender, View.IteratorResetPreAffineX) || ReferenceEquals(sender, View.IteratorResetPreAffine))
			{
				iterator.PreAffine = iterator.PreAffine.Alter(1, 0);
			}
			if (ReferenceEquals(sender, View.IteratorResetPreAffineY) || ReferenceEquals(sender, View.IteratorResetPreAffine))
			{
				iterator.PreAffine = iterator.PreAffine.Alter(m21: 0, m22: 1);
			}

			if (ReferenceEquals(sender, View.IteratorResetPostAffineO) || ReferenceEquals(sender, View.IteratorResetPostAffine))
			{
				iterator.PostAffine = iterator.PostAffine.Alter(m31: 0, m32: 0);
			}
			if (ReferenceEquals(sender, View.IteratorResetPostAffineX) || ReferenceEquals(sender, View.IteratorResetPostAffine))
			{
				iterator.PostAffine = iterator.PostAffine.Alter(1, 0);
			}
			if (ReferenceEquals(sender, View.IteratorResetPostAffineY) || ReferenceEquals(sender, View.IteratorResetPostAffine))
			{
				iterator.PostAffine = iterator.PostAffine.Alter(m21: 0, m22: 1);
			}

			View.IteratorCanvas.Refresh();
			mParent.UpdateCoordinates();
		}
		private void OnVectorChanged(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var pre = iterator.PreAffine;
			var post = iterator.PostAffine;

			float ox, oy, xx, xy, yx, yy;

			using (mParent.Initializer.Enter())
			{
				if (!float.TryParse(View.IteratorPreAffineOxTextBox.Text, NumberStyles.Float, InputController.Culture, out ox)) ox = pre.M31;
				if (!float.TryParse(View.IteratorPreAffineOyTextBox.Text, NumberStyles.Float, InputController.Culture, out oy)) oy = pre.M32;
				if (!float.TryParse(View.IteratorPreAffineXxTextBox.Text, NumberStyles.Float, InputController.Culture, out xx)) xx = pre.M11;
				if (!float.TryParse(View.IteratorPreAffineXyTextBox.Text, NumberStyles.Float, InputController.Culture, out xy)) xy = pre.M12;
				if (!float.TryParse(View.IteratorPreAffineYxTextBox.Text, NumberStyles.Float, InputController.Culture, out yx)) yx = pre.M21;
				if (!float.TryParse(View.IteratorPreAffineYyTextBox.Text, NumberStyles.Float, InputController.Culture, out yy)) yy = pre.M22;
			}

			iterator.PreAffine = new Matrix3x2(xx, xy, yx, yy, ox, oy);

			using (mParent.Initializer.Enter())
			{
				if (!float.TryParse(View.IteratorPostAffineOxTextBox.Text, NumberStyles.Float, InputController.Culture, out ox)) ox = post.M31;
				if (!float.TryParse(View.IteratorPostAffineOyTextBox.Text, NumberStyles.Float, InputController.Culture, out oy)) oy = post.M32;
				if (!float.TryParse(View.IteratorPostAffineXxTextBox.Text, NumberStyles.Float, InputController.Culture, out xx)) xx = post.M11;
				if (!float.TryParse(View.IteratorPostAffineXyTextBox.Text, NumberStyles.Float, InputController.Culture, out xy)) xy = post.M12;
				if (!float.TryParse(View.IteratorPostAffineYxTextBox.Text, NumberStyles.Float, InputController.Culture, out yx)) yx = post.M21;
				if (!float.TryParse(View.IteratorPostAffineYyTextBox.Text, NumberStyles.Float, InputController.Culture, out yy)) yy = post.M22;
			}

			iterator.PostAffine = new Matrix3x2(xx, xy, yx, yy, ox, oy);

			View.IteratorCanvas.Refresh();
		}
		private void OnVectorCommit(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var textBox = sender as TextBox;

			var iterator = View.IteratorCanvas.SelectedIterator;

			var preo = new Vector2(iterator.PreAffine.M31, iterator.PreAffine.M32);
			var prex = new Vector2(iterator.PreAffine.M11, iterator.PreAffine.M12);
			var prey = new Vector2(iterator.PreAffine.M21, iterator.PreAffine.M22);

			var posto = new Vector2(iterator.PostAffine.M31, iterator.PostAffine.M32);
			var postx = new Vector2(iterator.PostAffine.M11, iterator.PostAffine.M12);
			var posty = new Vector2(iterator.PostAffine.M21, iterator.PostAffine.M22);

			using (mParent.Initializer.Enter())
			{
				if (ReferenceEquals(sender, View.IteratorPreAffineOxTextBox)) View.IteratorPreAffineOxTextBox.Text = preo.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPreAffineOyTextBox)) View.IteratorPreAffineOyTextBox.Text = preo.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPreAffineXxTextBox)) View.IteratorPreAffineXxTextBox.Text = prex.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPreAffineXyTextBox)) View.IteratorPreAffineXyTextBox.Text = prex.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPreAffineYxTextBox)) View.IteratorPreAffineYxTextBox.Text = prey.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPreAffineYyTextBox)) View.IteratorPreAffineYyTextBox.Text = prey.Y.ToString(InputController.PreciseFormat, InputController.Culture);

				if (ReferenceEquals(sender, View.IteratorPostAffineOxTextBox)) View.IteratorPostAffineOxTextBox.Text = posto.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPostAffineOyTextBox)) View.IteratorPostAffineOyTextBox.Text = posto.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPostAffineXxTextBox)) View.IteratorPostAffineXxTextBox.Text = postx.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPostAffineXyTextBox)) View.IteratorPostAffineXyTextBox.Text = postx.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPostAffineYxTextBox)) View.IteratorPostAffineYxTextBox.Text = posty.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPostAffineYyTextBox)) View.IteratorPostAffineYyTextBox.Text = posty.Y.ToString(InputController.PreciseFormat, InputController.Culture);
			}

			if (textBox != null)
			{
				textBox.Refresh();
			}
		}

		public void UpdateVectorControls()
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;

			var preo = new Vector2(iterator.PreAffine.M31, iterator.PreAffine.M32);
			var prex = new Vector2(iterator.PreAffine.M11, iterator.PreAffine.M12);
			var prey = new Vector2(iterator.PreAffine.M21, iterator.PreAffine.M22);

			var posto = new Vector2(iterator.PostAffine.M31, iterator.PostAffine.M32);
			var postx = new Vector2(iterator.PostAffine.M11, iterator.PostAffine.M12);
			var posty = new Vector2(iterator.PostAffine.M21, iterator.PostAffine.M22);

			using (mParent.Initializer.Enter())
			{
				View.IteratorPreAffineOxTextBox.Text = preo.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPreAffineOyTextBox.Text = preo.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPreAffineXxTextBox.Text = prex.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPreAffineXyTextBox.Text = prex.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPreAffineYxTextBox.Text = prey.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPreAffineYyTextBox.Text = prey.Y.ToString(InputController.PreciseFormat, InputController.Culture);

				View.IteratorPostAffineOxTextBox.Text = posto.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPostAffineOyTextBox.Text = posto.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPostAffineXxTextBox.Text = postx.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPostAffineXyTextBox.Text = postx.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPostAffineYxTextBox.Text = posty.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPostAffineYyTextBox.Text = posty.Y.ToString(InputController.PreciseFormat, InputController.Culture);
			}
			if (ReferenceEquals(View.Tabs.SelectedTab, View.VectorTab))
			{
				foreach (var box in mVectorTextBoxes)
				{
					box.Refresh();
				}
			}
		}
	}
}