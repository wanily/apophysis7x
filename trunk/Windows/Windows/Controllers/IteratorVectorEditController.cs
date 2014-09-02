using System;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorVectorEditController : Controller<Editor>
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
				iterator.PreAffine.Origin.X = 0;
				iterator.PreAffine.Origin.Y = 0;
			}
			if (ReferenceEquals(sender, View.IteratorResetPreAffineX) || ReferenceEquals(sender, View.IteratorResetPreAffine))
			{
				iterator.PreAffine.Matrix.X.X = 1;
				iterator.PreAffine.Matrix.X.Y = 0;
			}
			if (ReferenceEquals(sender, View.IteratorResetPreAffineY) || ReferenceEquals(sender, View.IteratorResetPreAffine))
			{
				iterator.PreAffine.Matrix.Y.X = 0;
				iterator.PreAffine.Matrix.Y.Y = 1;
			}

			if (ReferenceEquals(sender, View.IteratorResetPostAffineO) || ReferenceEquals(sender, View.IteratorResetPostAffine))
			{
				iterator.PostAffine.Origin.X = 0;
				iterator.PostAffine.Origin.Y = 0;
			}
			if (ReferenceEquals(sender, View.IteratorResetPostAffineX) || ReferenceEquals(sender, View.IteratorResetPostAffine))
			{
				iterator.PostAffine.Matrix.X.X = 1;
				iterator.PostAffine.Matrix.X.Y = 0;
			}
			if (ReferenceEquals(sender, View.IteratorResetPostAffineY) || ReferenceEquals(sender, View.IteratorResetPostAffine))
			{
				iterator.PostAffine.Matrix.Y.X = 0;
				iterator.PostAffine.Matrix.Y.Y = 1;
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

			double ox, oy, xx, xy, yx, yy;

			using (mParent.Initializer.Enter())
			{
				if (!double.TryParse(View.IteratorPreAffineOxTextBox.Text, NumberStyles.Float, InputController.Culture, out ox)) ox = pre.Origin.X;
				if (!double.TryParse(View.IteratorPreAffineOyTextBox.Text, NumberStyles.Float, InputController.Culture, out oy)) oy = pre.Origin.Y;
				if (!double.TryParse(View.IteratorPreAffineXxTextBox.Text, NumberStyles.Float, InputController.Culture, out xx)) xx = pre.Matrix.X.X;
				if (!double.TryParse(View.IteratorPreAffineXyTextBox.Text, NumberStyles.Float, InputController.Culture, out xy)) xy = pre.Matrix.X.Y;
				if (!double.TryParse(View.IteratorPreAffineYxTextBox.Text, NumberStyles.Float, InputController.Culture, out yx)) yx = pre.Matrix.Y.X;
				if (!double.TryParse(View.IteratorPreAffineYyTextBox.Text, NumberStyles.Float, InputController.Culture, out yy)) yy = pre.Matrix.Y.Y;
			}

			pre.Origin.X = ox;
			pre.Origin.Y = oy;
			pre.Matrix.X.X = xx;
			pre.Matrix.X.Y = xy;
			pre.Matrix.Y.X = yx;
			pre.Matrix.Y.Y = yy;

			using (mParent.Initializer.Enter())
			{
				if (!double.TryParse(View.IteratorPostAffineOxTextBox.Text, NumberStyles.Float, InputController.Culture, out ox)) ox = post.Origin.X;
				if (!double.TryParse(View.IteratorPostAffineOyTextBox.Text, NumberStyles.Float, InputController.Culture, out oy)) oy = post.Origin.Y;
				if (!double.TryParse(View.IteratorPostAffineXxTextBox.Text, NumberStyles.Float, InputController.Culture, out xx)) xx = post.Matrix.X.X;
				if (!double.TryParse(View.IteratorPostAffineXyTextBox.Text, NumberStyles.Float, InputController.Culture, out xy)) xy = post.Matrix.X.Y;
				if (!double.TryParse(View.IteratorPostAffineYxTextBox.Text, NumberStyles.Float, InputController.Culture, out yx)) yx = post.Matrix.Y.X;
				if (!double.TryParse(View.IteratorPostAffineYyTextBox.Text, NumberStyles.Float, InputController.Culture, out yy)) yy = post.Matrix.Y.Y;
			}

			post.Origin.X = ox;
			post.Origin.Y = oy;
			post.Matrix.X.X = xx;
			post.Matrix.X.Y = xy;
			post.Matrix.Y.X = yx;
			post.Matrix.Y.Y = yy;

			View.IteratorCanvas.Refresh();
		}
		private void OnVectorCommit(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var textBox = sender as TextBox;

			var iterator = View.IteratorCanvas.SelectedIterator;

			var preo = iterator.PreAffine.Origin;
			var prex = iterator.PreAffine.Matrix.X;
			var prey = iterator.PreAffine.Matrix.Y;

			var posto = iterator.PostAffine.Origin;
			var postx = iterator.PostAffine.Matrix.X;
			var posty = iterator.PostAffine.Matrix.Y;

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

			var preo = iterator.PreAffine.Origin;
			var prex = iterator.PreAffine.Matrix.X;
			var prey = iterator.PreAffine.Matrix.Y;

			var posto = iterator.PostAffine.Origin;
			var postx = iterator.PostAffine.Matrix.X;
			var posty = iterator.PostAffine.Matrix.Y;

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