using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorPointEditController : Controller<Editor>
	{
		private static readonly double[] mMoveOffsetOptions = { 1.0, 0.5, 0.25, 0.1, 0.05, 0.025, 0.01 };
		private static readonly double[] mSnapAngleOptions = { 5.0, 15.0, 30.0, 45.0, 60.0, 90.0, 120.0, 180.0 };
		private static readonly double[] mScaleRatioOptions = { 110.0, 125.0, 150.0, 175.0, 200.0 };

		private EditorController mParent;
		private TextBox[] mPointTextBoxes;

		public IteratorPointEditController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			mPointTextBoxes = new[]
			{
				View.IteratorPointOxTextBox, View.IteratorPointOyTextBox,
				View.IteratorPointXxTextBox, View.IteratorPointXyTextBox,
				View.IteratorPointYxTextBox, View.IteratorPointYyTextBox
			};

			foreach (var box in mPointTextBoxes)
			{
				box.TextChanged += OnPointChanged;
				box.LostFocus += OnPointCommit;
			}

			View.IteratorRotate90CCW.Click += OnIteratorRotateClick;
			View.IteratorRotate90CW.Click += OnIteratorRotateClick;
			View.IteratorRotateCCW.Click += OnIteratorRotateClick;
			View.IteratorRotateCW.Click += OnIteratorRotateClick;

			View.IteratorMoveUp.Click += OnIteratorMoveClick;
			View.IteratorMoveDown.Click += OnIteratorMoveClick;
			View.IteratorMoveLeft.Click += OnIteratorMoveClick;
			View.IteratorMoveRight.Click += OnIteratorMoveClick;

			View.IteratorScaleUp.Click += OnIteratorScaleClick;
			View.IteratorScaleDown.Click += OnIteratorScaleClick;

			View.IteratorSnapAngle.Items.AddRange(mSnapAngleOptions.OfType<object>().ToArray());
			View.IteratorSnapAngle.Text = View.IteratorCanvas.Settings.AngleSnap.ToString(InputController.Culture);
			View.IteratorSnapAngle.TextChanged += OnSnapAngleChanged;

			View.IteratorMoveOffset.Items.AddRange(mMoveOffsetOptions.OfType<object>().ToArray());
			View.IteratorMoveOffset.Text = View.IteratorCanvas.Settings.MoveAmount.ToString(InputController.Culture);
			View.IteratorMoveOffset.TextChanged += OnMoveOffsetChanged;

			View.IteratorScaleRatio.Items.AddRange(mScaleRatioOptions.OfType<object>().ToArray());
			View.IteratorScaleRatio.Text = View.IteratorCanvas.Settings.ScaleSnap.ToString(InputController.Culture);
			View.IteratorScaleRatio.TextChanged += OnScaleRatioChanged;

			View.IteratorResetPointO.Click += OnResetPointClick;
			View.IteratorResetPointX.Click += OnResetPointClick;
			View.IteratorResetPointY.Click += OnResetPointClick;
		}
		protected override void DetachView()
		{
			foreach (var box in mPointTextBoxes)
			{
				box.TextChanged -= OnPointChanged;
				box.LostFocus -= OnPointCommit;
			}

			View.IteratorRotate90CCW.Click -= OnIteratorRotateClick;
			View.IteratorRotate90CW.Click -= OnIteratorRotateClick;
			View.IteratorRotateCCW.Click -= OnIteratorRotateClick;
			View.IteratorRotateCW.Click -= OnIteratorRotateClick;

			View.IteratorMoveUp.Click -= OnIteratorMoveClick;
			View.IteratorMoveDown.Click -= OnIteratorMoveClick;
			View.IteratorMoveLeft.Click -= OnIteratorMoveClick;
			View.IteratorMoveRight.Click -= OnIteratorMoveClick;

			View.IteratorScaleUp.Click -= OnIteratorScaleClick;
			View.IteratorScaleDown.Click -= OnIteratorScaleClick;

			View.IteratorSnapAngle.TextChanged -= OnSnapAngleChanged;
			View.IteratorSnapAngle.Items.Clear();

			View.IteratorMoveOffset.TextChanged -= OnMoveOffsetChanged;
			View.IteratorMoveOffset.Items.Clear();

			View.IteratorScaleRatio.TextChanged -= OnScaleRatioChanged;
			View.IteratorScaleRatio.Items.Clear();

			View.IteratorResetPointO.Click -= OnResetPointClick;
			View.IteratorResetPointX.Click -= OnResetPointClick;
			View.IteratorResetPointY.Click -= OnResetPointClick;

			mPointTextBoxes = null;
		}

		private void OnIteratorRotateClick(object sender, EventArgs e)
		{
			double angle;

			if (ReferenceEquals(sender, View.IteratorRotate90CCW))
			{
				angle = -90;
			}
			else if (ReferenceEquals(sender, View.IteratorRotate90CW))
			{
				angle = 90;
			}
			else if (ReferenceEquals(sender, View.IteratorRotateCCW))
			{
				angle = -View.IteratorCanvas.Settings.AngleSnap;
			}
			else if (ReferenceEquals(sender, View.IteratorRotateCW))
			{
				angle = View.IteratorCanvas.Settings.AngleSnap;
			}
			else
			{
				return;
			}

			while (angle < 0)
			{
				angle = 360 + angle;
			}

			View.IteratorCanvas.Commands.RotateSelected(-angle * System.Math.PI / 180.0);
		}
		private void OnSnapAngleChanged(object sender, EventArgs e)
		{
			double value;
			if (!double.TryParse(View.IteratorSnapAngle.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				value = View.IteratorCanvas.Settings.AngleSnap;
			}

			View.IteratorCanvas.Settings.AngleSnap = value;
		}

		private void OnIteratorMoveClick(object sender, EventArgs e)
		{
			Vector2 offset;

			if (ReferenceEquals(sender, View.IteratorMoveLeft))
			{
				offset = new Vector2(-1, 0);
			}
			else if (ReferenceEquals(sender, View.IteratorMoveRight))
			{
				offset = new Vector2(1, 0);
			}
			else if (ReferenceEquals(sender, View.IteratorMoveUp))
			{
				offset = new Vector2(0, 1);
			}
			else if (ReferenceEquals(sender, View.IteratorMoveDown))
			{
				offset = new Vector2(0, -1);
			}
			else
			{
				return;
			}

			View.IteratorCanvas.Commands.MoveSelected(offset * View.IteratorCanvas.Settings.MoveAmount);
		}
		private void OnMoveOffsetChanged(object sender, EventArgs e)
		{
			double value;
			if (!double.TryParse(View.IteratorMoveOffset.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				value = View.IteratorCanvas.Settings.MoveAmount;
			}

			View.IteratorCanvas.Settings.MoveAmount = value;
		}

		private void OnIteratorScaleClick(object sender, EventArgs e)
		{
			double ratio;

			if (ReferenceEquals(sender, View.IteratorScaleDown))
			{
				ratio = 100.0 / View.IteratorCanvas.Settings.ScaleSnap;
			}
			else if (ReferenceEquals(sender, View.IteratorScaleUp))
			{
				ratio = View.IteratorCanvas.Settings.ScaleSnap / 100.0;
			}
			else
			{
				return;
			}

			View.IteratorCanvas.Commands.ScaleSelected(ratio);
		}
		private void OnScaleRatioChanged(object sender, EventArgs e)
		{
			double value;
			if (!double.TryParse(View.IteratorScaleRatio.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				value = View.IteratorCanvas.Settings.ScaleSnap;
			}

			View.IteratorCanvas.Settings.ScaleSnap = value;
		}

		private void OnResetPointClick(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			if (ReferenceEquals(sender, View.IteratorResetPointO))
			{
				matrix.Origin.X = 0;
				matrix.Origin.Y = 0;
			}
			else if (ReferenceEquals(sender, View.IteratorResetPointX))
			{
				matrix.Matrix.X.X = 1;
				matrix.Matrix.X.Y = 0;
			}
			else if (ReferenceEquals(sender, View.IteratorResetPointY))
			{
				matrix.Matrix.Y.X = 0;
				matrix.Matrix.Y.Y = 1;
			}

			View.IteratorCanvas.Refresh();
			mParent.UpdateCoordinates();
		}
		private void OnPointChanged(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			double ox, oy, xx, xy, yx, yy;

			using (mParent.Initializer.Enter())
			{
				if (!double.TryParse(View.IteratorPointOxTextBox.Text, NumberStyles.Float, InputController.Culture, out ox)) ox = matrix.Origin.X;
				if (!double.TryParse(View.IteratorPointOyTextBox.Text, NumberStyles.Float, InputController.Culture, out oy)) oy = matrix.Origin.Y;
				if (!double.TryParse(View.IteratorPointXxTextBox.Text, NumberStyles.Float, InputController.Culture, out xx)) xx = matrix.Matrix.X.X + ox;
				if (!double.TryParse(View.IteratorPointXyTextBox.Text, NumberStyles.Float, InputController.Culture, out xy)) xy = matrix.Matrix.X.Y + oy;
				if (!double.TryParse(View.IteratorPointYxTextBox.Text, NumberStyles.Float, InputController.Culture, out yx)) yx = matrix.Matrix.Y.X + ox;
				if (!double.TryParse(View.IteratorPointYyTextBox.Text, NumberStyles.Float, InputController.Culture, out yy)) yy = matrix.Matrix.Y.Y + oy;
			}

			matrix.Origin.X = ox;
			matrix.Origin.Y = oy;
			matrix.Matrix.X.X = xx - ox;
			matrix.Matrix.X.Y = xy - oy;
			matrix.Matrix.Y.X = yx - ox;
			matrix.Matrix.Y.Y = yy - oy;

			View.IteratorCanvas.Refresh();
		}
		private void OnPointCommit(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var textBox = sender as TextBox;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			var o = matrix.Origin;
			var x = matrix.Matrix.X + o;
			var y = matrix.Matrix.Y + o;

			using (mParent.Initializer.Enter())
			{
				if (ReferenceEquals(sender, View.IteratorPointOxTextBox)) View.IteratorPointOxTextBox.Text = o.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointOyTextBox)) View.IteratorPointOyTextBox.Text = o.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointXxTextBox)) View.IteratorPointXxTextBox.Text = x.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointXyTextBox)) View.IteratorPointXyTextBox.Text = x.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointYxTextBox)) View.IteratorPointYxTextBox.Text = y.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointYyTextBox)) View.IteratorPointYyTextBox.Text = y.Y.ToString(InputController.PreciseFormat, InputController.Culture);
			}

			if (textBox != null)
			{
				textBox.Refresh();
			}
		}

		public void UpdatePointControls()
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			var o = matrix.Origin;
			var x = matrix.Matrix.X + o;
			var y = matrix.Matrix.Y + o;

			using (mParent.Initializer.Enter())
			{
				View.IteratorPointOxTextBox.Text = o.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointOyTextBox.Text = o.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointXxTextBox.Text = x.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointXyTextBox.Text = x.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointYxTextBox.Text = y.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointYyTextBox.Text = y.Y.ToString(InputController.PreciseFormat, InputController.Culture);
			}

			if (ReferenceEquals(View.Tabs.SelectedTab, View.PointTab))
			{
				foreach (var box in mPointTextBoxes)
				{
					box.Refresh();
				}
			}
		}
	}
}