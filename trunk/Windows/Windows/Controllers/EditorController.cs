using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class EditorController : WindowController<Editor>
	{
		private Lock mInitialize = new Lock();
		private Flame mFlame;

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
			View.IteratorWeightDragPanel.ValueChanged += OnWeightChanged;
			View.IteratorNameTextBox.TextChanged += OnNameChanged;
			View.IteratorSelectionComboBox.SelectedIndexChanged += OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged += OnIteratorSelectedFromCanvas;

			View.KeyDown += OnKeyDown;

			View.IteratorCanvas.Settings = new EditorSettings
			{
				MoveAmount = ApophysisSettings.EditorMoveDistance,
				AngleSnap = ApophysisSettings.EditorRotateAngle,
				ScaleSnap = ApophysisSettings.EditorScaleRatio
			};

			SetFlame(new Flame());
		}
		protected override void DetachView()
		{
			View.IteratorWeightDragPanel.ValueChanged -= OnWeightChanged;
			View.IteratorNameTextBox.TextChanged -= OnNameChanged;
			View.IteratorSelectionComboBox.SelectedIndexChanged -= OnIteratorSelectedFromComboBox;
			View.IteratorCanvas.SelectionChanged -= OnIteratorSelectedFromCanvas;

			View.KeyDown -= OnKeyDown;
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
			}
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

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && (e.KeyCode & Keys.V) == Keys.V)
			{
				ReadFlameFromClipboard();
			}
		}
	}
}