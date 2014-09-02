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

		private TextBox[] mPointTextBoxes, mVectorTextBoxes;

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
			View.IteratorCanvas.ActiveMatrixChanged += OnActiveMatrixChanged;

			OnActiveMatrixChanged(View.IteratorCanvas, new EventArgs());

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

			mVectorTextBoxes = new[]
			{
				View.IteratorPreAffineOxTextBox, View.IteratorPreAffineOyTextBox,
				View.IteratorPreAffineXxTextBox, View.IteratorPreAffineXyTextBox,
				View.IteratorPreAffineYxTextBox, View.IteratorPreAffineYyTextBox,

				View.IteratorPostAffineOxTextBox, View.IteratorPostAffineOyTextBox,
				View.IteratorPostAffineXxTextBox, View.IteratorPostAffineXyTextBox,
				View.IteratorPostAffineYxTextBox, View.IteratorPostAffineYyTextBox
			};

			foreach (var box in mPointTextBoxes)
			{
				box.TextChanged += OnPointChanged;
				box.LostFocus += OnPointCommit;
			}

			foreach (var box in mVectorTextBoxes)
			{
				box.TextChanged += OnVectorChanged;
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

			View.IteratorResetPreAffineO.Click += OnResetVectorClick;
			View.IteratorResetPreAffineX.Click += OnResetVectorClick;
			View.IteratorResetPreAffineY.Click += OnResetVectorClick;
			View.IteratorResetPreAffine.Click += OnResetVectorClick;

			View.IteratorResetPostAffineO.Click += OnResetVectorClick;
			View.IteratorResetPostAffineX.Click += OnResetVectorClick;
			View.IteratorResetPostAffineY.Click += OnResetVectorClick;
			View.IteratorResetPostAffine.Click += OnResetVectorClick;

			SetFlame(new Flame());
		}
		protected override void DetachView()
		{
			View.IteratorCanvas.Edit -= OnCanvasEdit;
			View.IteratorCanvas.ActiveMatrixChanged -= OnActiveMatrixChanged;

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

			foreach (var box in mVectorTextBoxes)
			{
				box.TextChanged -= OnVectorChanged;
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

			View.IteratorResetPreAffineO.Click -= OnResetVectorClick;
			View.IteratorResetPreAffineX.Click -= OnResetVectorClick;
			View.IteratorResetPreAffineY.Click -= OnResetVectorClick;
			View.IteratorResetPreAffine.Click -= OnResetVectorClick;

			View.IteratorResetPostAffineO.Click -= OnResetVectorClick;
			View.IteratorResetPostAffineX.Click -= OnResetVectorClick;
			View.IteratorResetPostAffineY.Click -= OnResetVectorClick;
			View.IteratorResetPostAffine.Click -= OnResetVectorClick;

			View.KeyHandler = null;
			mPointTextBoxes = null;
			mVectorTextBoxes = null;
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

			View.Text = string.Format("Editor - {0}", mFlame.CalculatedName);
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
			OnCanvasEdit(View.IteratorCanvas, new EventArgs());
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
			OnCanvasEdit(View.IteratorCanvas, new EventArgs());
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

			var preo = iterator.PreAffine.Origin;
			var prex = iterator.PreAffine.Matrix.X;
			var prey = iterator.PreAffine.Matrix.Y;

			var posto = iterator.PostAffine.Origin;
			var postx = iterator.PostAffine.Matrix.X;
			var posty = iterator.PostAffine.Matrix.Y;

			using (mInitialize.Enter())
			{
				View.IteratorPointOxTextBox.Text = o.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointOyTextBox.Text = o.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointXxTextBox.Text = x.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointXyTextBox.Text = x.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointYxTextBox.Text = y.X.ToString(InputController.PreciseFormat, InputController.Culture);
				View.IteratorPointYyTextBox.Text = y.Y.ToString(InputController.PreciseFormat, InputController.Culture);

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

			if (ReferenceEquals(View.Tabs.SelectedTab, View.PointTab))
			{
				foreach (var box in mPointTextBoxes)
				{
					box.Refresh();
				}
			}
			else if (ReferenceEquals(View.Tabs.SelectedTab, View.VectorTab))
			{
				foreach (var box in mVectorTextBoxes)
				{
					box.Refresh();
				}
			}
		}
		private void OnActiveMatrixChanged(object sender, EventArgs e)
		{
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
		private void OnVectorChanged(object sender, EventArgs e)
		{
			if (View.IteratorCanvas.SelectedIterator == null || mInitialize.IsBusy)
				return;

			var iterator = View.IteratorCanvas.SelectedIterator;
			var pre = iterator.PreAffine;
			var post = iterator.PostAffine;

			double ox, oy, xx, xy, yx, yy;

			using (mInitialize.Enter())
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

			using (mInitialize.Enter())
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

			var preo = iterator.PreAffine.Origin;
			var prex = iterator.PreAffine.Matrix.X;
			var prey = iterator.PreAffine.Matrix.Y;

			var posto = iterator.PostAffine.Origin;
			var postx = iterator.PostAffine.Matrix.X;
			var posty = iterator.PostAffine.Matrix.Y;

			using (mInitialize.Enter()) 
			{
				if (ReferenceEquals(sender, View.IteratorPointOxTextBox)) View.IteratorPointOxTextBox.Text = o.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointOyTextBox)) View.IteratorPointOyTextBox.Text = o.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointXxTextBox)) View.IteratorPointXxTextBox.Text = x.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointXyTextBox)) View.IteratorPointXyTextBox.Text = x.Y.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointYxTextBox)) View.IteratorPointYxTextBox.Text = y.X.ToString(InputController.PreciseFormat, InputController.Culture);
				if (ReferenceEquals(sender, View.IteratorPointYyTextBox)) View.IteratorPointYyTextBox.Text = y.Y.ToString(InputController.PreciseFormat, InputController.Culture);

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

		private void OnKeyDown(Keys keys)
		{
			if ((keys & Keys.Control) == Keys.Control && (keys & Keys.V) == Keys.V)
			{
				ReadFlameFromClipboard();
			}
		}
	}
}