using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class IteratorPointEditController : Controller<Editor>
	{
		private static readonly float[] mMoveOffsetOptions = { 1.0f, 0.5f, 0.25f, 0.1f, 0.05f, 0.025f, 0.01f };
		private static readonly float[] mSnapAngleOptions = { 5.0f, 15.0f, 30.0f, 45.0f, 60.0f, 90.0f, 120.0f, 180.0f };
		private static readonly float[] mScaleRatioOptions = { 110.0f, 125.0f, 150.0f, 175.0f, 200.0f };

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
			float angle;

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

			View.IteratorCanvas.Commands.RotateSelectedIterator(-angle * Float.Pi / 180.0f);
		}
		private void OnSnapAngleChanged(object sender, EventArgs e)
		{
			float value;
			if (!float.TryParse(View.IteratorSnapAngle.Text, NumberStyles.Float, InputController.Culture, out value))
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

			View.IteratorCanvas.Commands.MoveSelectedIterator(offset * View.IteratorCanvas.Settings.MoveAmount);
		}
		private void OnMoveOffsetChanged(object sender, EventArgs e)
		{
			float value;
			if (!float.TryParse(View.IteratorMoveOffset.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				value = View.IteratorCanvas.Settings.MoveAmount;
			}

			View.IteratorCanvas.Settings.MoveAmount = value;
		}

		private void OnIteratorScaleClick(object sender, EventArgs e)
		{
			float ratio;

			if (ReferenceEquals(sender, View.IteratorScaleDown))
			{
				ratio = 100.0f / View.IteratorCanvas.Settings.ScaleSnap;
			}
			else if (ReferenceEquals(sender, View.IteratorScaleUp))
			{
				ratio = View.IteratorCanvas.Settings.ScaleSnap / 100.0f;
			}
			else
			{
				return;
			}

			View.IteratorCanvas.Commands.ScaleSelectedIterator(ratio);
		}
		private void OnScaleRatioChanged(object sender, EventArgs e)
		{
			float value;
			if (!float.TryParse(View.IteratorScaleRatio.Text, NumberStyles.Float, InputController.Culture, out value))
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
				matrix = matrix.Alter(m31: 0, m32: 0);
			}
			else if (ReferenceEquals(sender, View.IteratorResetPointX))
			{
				matrix = matrix.Alter(1, 0);
			}
			else if (ReferenceEquals(sender, View.IteratorResetPointY))
			{
				matrix = matrix.Alter(m21: 0, m22: 1);
			}

			switch (View.IteratorCanvas.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = matrix;
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = matrix;
					break;
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

			float ox, oy, xx, xy, yx, yy;

			using (mParent.Initializer.Enter())
			{
				if (!float.TryParse(View.IteratorPointOxTextBox.Text, NumberStyles.Float, InputController.Culture, out ox)) ox = matrix.M31;
				if (!float.TryParse(View.IteratorPointOyTextBox.Text, NumberStyles.Float, InputController.Culture, out oy)) oy = matrix.M32;
				if (!float.TryParse(View.IteratorPointXxTextBox.Text, NumberStyles.Float, InputController.Culture, out xx)) xx = matrix.M11 + ox;
				if (!float.TryParse(View.IteratorPointXyTextBox.Text, NumberStyles.Float, InputController.Culture, out xy)) xy = matrix.M12 + oy;
				if (!float.TryParse(View.IteratorPointYxTextBox.Text, NumberStyles.Float, InputController.Culture, out yx)) yx = matrix.M21 + ox;
				if (!float.TryParse(View.IteratorPointYyTextBox.Text, NumberStyles.Float, InputController.Culture, out yy)) yy = matrix.M22 + oy;
			}

			switch (View.IteratorCanvas.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = new Matrix3x2(xx - ox, xy - oy, yx - ox, yy - oy, ox, oy);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = new Matrix3x2(xx - ox, xy - oy, yx - ox, yy - oy, ox, oy);
					break;
			}

			View.IteratorCanvas.Refresh();
		}
		private void OnPointCommit(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mParent.Initializer.IsBusy)
				return;

			var textBox = sender as TextBox;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			var o = new Vector2(matrix.M31, matrix.M32);
			var x = new Vector2(matrix.M11, matrix.M12) + o;
			var y = new Vector2(matrix.M21, matrix.M22) + o;

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

			var o = new Vector2(matrix.M31, matrix.M32);
			var x = new Vector2(matrix.M11, matrix.M12) + o;
			var y = new Vector2(matrix.M21, matrix.M22) + o;

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