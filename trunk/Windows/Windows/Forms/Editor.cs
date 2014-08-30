using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		private Flame mFlame;

		public Editor()
		{
			InitializeComponent();

			mCanvas.Settings = new EditorSettings
			{
				MoveAmount = ApophysisSettings.EditorMoveDistance,
				AngleSnap = ApophysisSettings.EditorRotateAngle,
				ScaleSnap = ApophysisSettings.EditorScaleRatio
			};

			Flame = new Flame();
		}

		[NotNull]
		public Flame Flame
		{
			get { return mFlame; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				mFlame = value;
				mCanvas.Iterators = value.Iterators;
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
	}
}
