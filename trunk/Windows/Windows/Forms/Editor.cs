using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		private Flame mFlame;
		private bool mInitializing;

		public Editor()
		{
			InitializeComponent();

			mCanvas.Settings = new EditorSettings
			{
				MoveAmount = ApophysisSettings.EditorMoveDistance,
				AngleSnap = ApophysisSettings.EditorRotateAngle,
				ScaleSnap = ApophysisSettings.EditorScaleRatio
			};

			mIteratorSelect.DrawItem += OnIteratorSelectDrawItem;
			mIteratorSelect.MeasureItem += OnIteratorMeasureItem;

			Flame = new Flame();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
					components = null;
				}

				if (mFlame != null)
				{
					mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
					mFlame = null;
				}
			}
			base.Dispose(disposing);
		}

		[NotNull]
		public Flame Flame
		{
			get { return mFlame; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				if (mFlame != null)
				{
					mFlame.Iterators.ContentChanged -= OnIteratorCollectionChanged;
				}

				mFlame = value;
				mFlame.Iterators.ContentChanged += OnIteratorCollectionChanged;

				mInitializing = true;

				mCanvas.Iterators = value.Iterators;
				OnIteratorCollectionChanged(this, new EventArgs());

				SelectedIterator = value.Iterators.First();

				mInitializing = false;
			}
		}
		public Iterator SelectedIterator
		{
			get { return mCanvas.SelectedIterator; }
			set
			{
				mCanvas.SelectedIterator = value;

				mInitializing = true;

				if (value != null)
				{
					mIteratorName.Text = value.Name;
					mIteratorWeightLabel.Value = value.Weight;
					mIteratorSelect.SelectedIndex = value.Index;
				}
				else
				{
					mIteratorName.Text = null;
					mIteratorWeightLabel.Value = 0.5;
					mIteratorSelect.SelectedIndex = -1;
				}

				mInitializing = false;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData & Keys.Control) == Keys.Control && (keyData & Keys.V) == Keys.V)
			{
				var clipboard = Clipboard.GetText();
				if (!string.IsNullOrEmpty(clipboard))
				{
					try
					{
						var element = XElement.Parse(clipboard, LoadOptions.None);
						mFlame.ReadXml(element);
					}
					catch (ApophysisException exception)
					{
						MessageBox.Show(exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch (XmlException exception)
					{
						MessageBox.Show("Invalid XML: " + exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void OnIteratorNameChanged(object sender, EventArgs e)
		{
			if (mCanvas.SelectedIterator == null || mInitializing)
				return;

			var textBox = sender as TextBox;
			if (textBox != null)
			{
				mCanvas.SelectedIterator.Name = textBox.Text;
				
				mInitializing = true;
				
				var selection = SelectedIterator;
				
				//todo there are better ways
				OnIteratorCollectionChanged(this, new EventArgs());
				SelectedIterator = selection;

				mInitializing = false;
			}
		}
		private void OnIteratorWeightChanged(object sender, EventArgs e)
		{
			if (mCanvas.SelectedIterator == null || mInitializing)
				return;

			var textBox = sender as DragPanel;
			if (textBox != null)
			{
				mCanvas.SelectedIterator.Weight = textBox.Value;
			}
		}
		private void OnIteratorSelectedByCanvas(object sender, EventArgs e)
		{
			if (mInitializing)
				return;

			var iterator = mCanvas.SelectedIterator;
			SelectedIterator = iterator;
		}
		private void OnIteratorCollectionChanged(object sender, EventArgs e)
		{
			mIteratorSelect.Items.Clear();
			foreach (var iterator in mFlame.Iterators)
			{
				var name = string.IsNullOrEmpty(iterator.Name)
					? (iterator.Index + 1).ToString(CultureInfo.CurrentCulture)
					: string.Format("{0} - {1}", iterator.Index + 1, iterator.Name);
				mIteratorSelect.Items.Add(name);
			}
		}
		private void OnIteratorSelectedFromCombo(object sender, EventArgs e)
		{
			if (mInitializing)
				return;

			var iterator = mFlame.Iterators[mIteratorSelect.SelectedIndex];
			SelectedIterator = iterator;
		}
		private void OnIteratorSelectDrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
				return;

			var item = mIteratorSelect.Items[e.Index] as string;
			var size = e.Graphics.MeasureString(item, mIteratorSelect.Font);

			using (var brush = new SolidBrush(IteratorVisual.GetColor(mCanvas.Iterators[e.Index])))
			{
				using (var background = new SolidBrush(mIteratorSelect.BackColor))
				{
					e.Graphics.FillRectangle(background, e.Bounds);
				}

				e.DrawFocusRectangle();

				var poly = new[]
				{
					new Point(e.Bounds.Left + 2, e.Bounds.Top + e.Bounds.Height - 2),
					new Point(e.Bounds.Left + e.Bounds.Height - 2, e.Bounds.Top + e.Bounds.Height - 2),
					new Point(e.Bounds.Left + e.Bounds.Height - 2, e.Bounds.Top + 2)
				};

				using (var pen = new Pen(brush))
				{
					e.Graphics.DrawPolygon(pen, poly);
				}

				using (var brush2 = new SolidBrush(Color.FromArgb(0x80, brush.Color)))
				{
					e.Graphics.FillPolygon(brush2, poly);
				}

				using (var pen2 = new SolidBrush(mIteratorSelect.ForeColor))
				{
					e.Graphics.DrawString(item, mIteratorSelect.Font, pen2,
						e.Bounds.Left + e.Bounds.Height + 2,
						e.Bounds.Top + e.Bounds.Height / 2.0f - size.Height / 2.0f);
				}
			}
		}
		private void OnIteratorMeasureItem(object sender, MeasureItemEventArgs e)
		{
			var item = mIteratorSelect.Items[e.Index] as string;
			var size = e.Graphics.MeasureString(item, mIteratorSelect.Font);

			e.ItemWidth = (int)size.Width;
			e.ItemHeight = (int) size.Height;
		}
	}
}
