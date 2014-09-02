using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class EditorController : WindowController<Editor>
	{
		private static readonly double[] mMoveOffsetOptions = { 1.0, 0.5, 0.25, 0.1, 0.05, 0.025, 0.01 };
		private static readonly double[] mSnapAngleOptions = { 5.0, 15.0, 30.0, 45.0, 60.0, 90.0, 120.0, 180.0 };
		private static readonly double[] mScaleRatioOptions = { 110.0, 125.0, 150.0, 175.0, 200.0 };

		private Lock mInitialize = new Lock();
		private Flame mFlame;

		private TextBox[] mPointTextBoxes;

		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mInitialize != null)
				{
					mInitialize.Dispose();
					mInitialize = null;
				}

				if (mFlame != null)
				{
					mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
					mFlame = null;
				}
			}
		}

		protected override void AttachView()
		{
			View.IteratorCanvas.Edit += OnCanvasEdit;

			View.IteratorWeightDragPanel.ValueChanged += OnWeightChanged;
			View.IteratorNameTextBox.TextChanged += OnNameChanged;
			View.IteratorSelectionComboBox.SelectedIndexChanged += OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged += OnIteratorSelectedFromCanvas;

			View.IteratorColorDragPanel.ValueChanged += OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged += OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged += OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged += OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged += OnColorChanged;

			View.KeyHandler = OnKeyDown;

			View.IteratorCanvas.Settings = new EditorSettings
			{
				MoveAmount = ApophysisSettings.EditorMoveDistance,
				AngleSnap = ApophysisSettings.EditorRotateAngle,
				ScaleSnap = ApophysisSettings.EditorScaleRatio
			};

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

			SetFlame(new Flame());
		}
		protected override void DetachView()
		{
			View.IteratorCanvas.Edit -= OnCanvasEdit;

			View.IteratorWeightDragPanel.ValueChanged -= OnWeightChanged;
			View.IteratorNameTextBox.TextChanged -= OnNameChanged;
			View.IteratorSelectionComboBox.SelectedIndexChanged -= OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged -= OnIteratorSelectedFromCanvas;

			View.IteratorColorDragPanel.ValueChanged -= OnColorChanged;
			View.IteratorColorSpeedDragPanel.ValueChanged -= OnColorSpeedChanged;
			View.IteratorOpacityDragPanel.ValueChanged -= OnOpacityChanged;
			View.IteratorDirectColorDragPanel.ValueChanged -= OnDirectColorChanged;
			View.IteratorColorScrollBar.ValueChanged -= OnColorChanged;

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

			View.KeyHandler = null;
			mPointTextBoxes = null;
		}

		public void SetFlame([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			if (mFlame != null)
			{
				mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
			}

			mFlame = flame;
			mFlame.Iterators.ContentChanged += OnIteratorCollectionChanged;

			using (mInitialize.Enter())
			{
				View.IteratorCanvas.Iterators = mFlame.Iterators;
				
				BuildIteratorComboBox();
				SelectIterator(mFlame.Iterators.First());
			}
		}
		public void ReadFlameFromClipboard()
		{
			var clipboard = Clipboard.GetText();
			if (!string.IsNullOrEmpty(clipboard))
			{
				try
				{
					var flame = new Flame();
					
					flame.ReadXml(XElement.Parse(clipboard, LoadOptions.None));
					SetFlame(flame);
				}
				catch (ApophysisException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch (XmlException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void BuildIteratorComboBox()
		{
			View.IteratorSelectionComboBox.Items.Clear();
			View.IteratorSelectionComboBox.Items.AddRange(mFlame
				.Iterators
				.Select(GetIteratorDisplayName)
				.OfType<object>()
				.ToArray());
		}
		private void SelectIterator([CanBeNull] Iterator iterator)
		{
			View.IteratorCanvas.SelectedIterator = iterator;

			using (mInitialize.Enter())
			{
				View.IteratorSelectionComboBox.SelectedIndex = iterator == null ? -1 : iterator.Index;
				View.IteratorNameTextBox.Text = iterator == null ? null : iterator.Name;
				View.IteratorWeightDragPanel.Value = iterator == null ? 0.5 : iterator.Weight;
				View.IteratorColorDragPanel.Value = iterator == null ? 0.0 : iterator.Color;
				View.IteratorColorSpeedDragPanel.Value = iterator == null ? 0.0 : iterator.ColorSpeed;
				View.IteratorOpacityDragPanel.Value = iterator == null ? 1.0 : iterator.Opacity;
				View.IteratorDirectColorDragPanel.Value = iterator == null ? 1.0 : iterator.DirectColor;
				View.IteratorColorScrollBar.Value = iterator == null ? 0 : (int)(iterator.Color * 1000);
			}

			OnCanvasEdit(View.IteratorCanvas, new EventArgs());
		}

		private string GetIteratorDisplayName(Iterator iterator)
		{
			if (iterator == null)
				return null;

			var name = string.IsNullOrEmpty(iterator.Name)
				? (iterator.Index + 1).ToString(CultureInfo.CurrentCulture)
				: string.Format("{0} - {1}", iterator.Index + 1, iterator.Name);

			return name;
		}

		private void OnNameChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy || iterator == null)
				return;

			iterator.Name = View.IteratorNameTextBox.Text;

			using (mInitialize.Enter())
			{
				View.IteratorSelectionComboBox.Items.RemoveAt(iterator.Index); 
				View.IteratorSelectionComboBox.Items.Insert(iterator.Index, GetIteratorDisplayName(iterator));
				View.IteratorSelectionComboBox.SelectedIndex = iterator.Index;
			}
		}
		private void OnWeightChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy || iterator == null)
				return;

			iterator.Weight = View.IteratorWeightDragPanel.Value;
		}
		private void OnColorChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy || iterator == null)
				return;

			if (ReferenceEquals(sender, View.IteratorColorDragPanel))
			{
				iterator.Color = View.IteratorColorDragPanel.Value;
				using (mInitialize.Enter())
				{
					View.IteratorColorScrollBar.Value = (int)(iterator.Color * 1000);
				}
			}
			else if (ReferenceEquals(sender, View.IteratorColorScrollBar))
			{
				iterator.Color = View.IteratorColorScrollBar.Value / 1000.0;
				using (mInitialize.Enter())
				{
					View.IteratorColorDragPanel.Value = iterator.Color;
				}
			}
		}
		private void OnColorSpeedChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy || iterator == null)
				return;

			iterator.ColorSpeed = View.IteratorColorSpeedDragPanel.Value;
		}
		private void OnOpacityChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy || iterator == null)
				return;

			iterator.Opacity = View.IteratorOpacityDragPanel.Value;
		}
		private void OnDirectColorChanged(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy || iterator == null)
				return;

			iterator.DirectColor = View.IteratorDirectColorDragPanel.Value;
		}

		private void OnIteratorSelectedFromCanvas(object sender, EventArgs e)
		{
			var iterator = View.IteratorCanvas.SelectedIterator;
			if (mInitialize.IsBusy)
				return;

			SelectIterator(iterator);
		}
		private void OnIteratorSelectedFromComboBox(object sender, EventArgs e)
		{
			var iterator = View.IteratorSelectionComboBox.SelectedIndex < 0 || mFlame == null ? null : 
				mFlame.Iterators[View.IteratorSelectionComboBox.SelectedIndex];

			if (mInitialize.IsBusy)
				return;

			SelectIterator(iterator);
		}
		private void OnIteratorCollectionChanged(object sender, EventArgs e)
		{
			BuildIteratorComboBox();
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

		private void OnCanvasEdit(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mInitialize.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			var o = matrix.Origin;
			var x = matrix.Matrix.X + o;
			var y = matrix.Matrix.Y + o;

			using (mInitialize.Enter())
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
		private void OnPointChanged(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mInitialize.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			double ox, oy, xx, xy, yx, yy;

			using (mInitialize.Enter())
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
			if (View.IteratorCanvas.SelectedIterator == null || mInitialize.IsBusy)
				return;

			var textBox = sender as TextBox;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var matrix = View.IteratorCanvas.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			var o = matrix.Origin;
			var x = matrix.Matrix.X + o;
			var y = matrix.Matrix.Y + o;

			using (mInitialize.Enter()) 
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

		private void OnKeyDown(Keys keys)
		{
			if ((keys & Keys.Control) == Keys.Control && (keys & Keys.V) == Keys.V)
			{
				ReadFlameFromClipboard();
			}
		}
	}
}