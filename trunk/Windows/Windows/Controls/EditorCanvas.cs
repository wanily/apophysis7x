using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Visuals;
using Xyrus.Apophysis.Windows.Input;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class EditorCanvas : UserControl
	{
		private ControlVisualChain mVisual;
		private InputHandlerChain mInteraction;

		private GridVisual mGridPainter;
		private GridRulerVisual mRulerPainter;
		private IteratorCollectionVisual mIteratorPainter;
		private IteratorCollectionInputHandler mIteratorInteraction;

		private EditorGridContextMenu mGridContextMenu;
		private EditorCommands mCommands;

		private EventHandler mBeginEdit;
		private EventHandler mEndEdit;
		private EventHandler mEdit;
		private EventHandler mSelectionChanged;

		private bool mIsDisposed;

		public EditorCanvas()
		{
			InitializeComponent();
			SuspendLayout();

			var grid = new Grid(new Vector2(Width, Height));

			mVisual = new ControlVisualChain(this);
			mInteraction = new InputHandlerChain(this);
			
			mVisual.Add(mGridPainter = new GridVisual(this, grid));
			mVisual.Add(mIteratorPainter = new IteratorCollectionVisual(this, grid), 100);
			mVisual.Add(mRulerPainter = new GridRulerVisual(this, grid), int.MaxValue);

			mInteraction.Add(new GridInputStrategy(this, grid), int.MaxValue);
			mInteraction.Add(mIteratorInteraction = new IteratorCollectionInputHandler(this, mIteratorPainter, grid), 100);

			mIteratorInteraction.BeginEdit += OnBeginEdit;
			mIteratorInteraction.EndEdit += OnEndEdit;
			mIteratorInteraction.Edit += OnEdit;
			mIteratorInteraction.SelectionChanged += OnSelectionChanged;

			GridLineColor = Color.FromArgb(0xff, 0x66, 0x66, 0x66);
			BackdropColor = Color.Transparent;
			GridZeroLineColor = Color.FromArgb(0xff, 0xbb, 0xbb, 0xbb);

			RulerGridLineColor = Color.Gray;
			RulerBackgroundColor = Color.FromArgb(0xff, 0x33, 0x33, 0x33);
			RulerBackdropColor = Color.Transparent;

			ShowRuler = true;

			mCommands = new EditorCommands(this);
			mGridContextMenu = new EditorGridContextMenu(this);

			Settings.UnbindContextMenu();
			Settings.BindContextMenu(mGridContextMenu);

			MouseClick += OnCanvasMouseClick;
			ResumeLayout(false);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && !mIsDisposed)
			{
				MouseClick -= OnCanvasMouseClick;

				if (components != null)
				{
					components.Dispose();
				}

				if (mIteratorInteraction != null)
				{
					mIteratorInteraction.SelectionChanged -= OnSelectionChanged;
					mIteratorInteraction.BeginEdit -= OnBeginEdit;
					mIteratorInteraction.EndEdit -= OnEndEdit;
					mIteratorInteraction.Edit -= OnEdit;
				}

				if (mInteraction != null)
				{
					mInteraction.Dispose();
					mInteraction = null;
				}

				if (mVisual != null)
				{
					mVisual.Dispose();
					mVisual = null;
				}

				if (mGridContextMenu != null)
				{
					mGridContextMenu.Dispose();
					mGridContextMenu = null;
				}

				if (mCommands != null)
				{
					mCommands.Dispose();
					mCommands = null;
				}

				mIsDisposed = true;
			}

			mGridPainter = null;
			mRulerPainter = null;
			mIteratorPainter = null;
			mIteratorInteraction = null;

			base.Dispose(disposing);
		}

		public IteratorCollection Iterators
		{
			get { return mIteratorPainter.Collection; }
			set { mIteratorPainter.Collection = value; }
		}

		public Color GridZeroLineColor
		{
			get { return mGridPainter.GridZeroLineColor; }
			set { mGridPainter.GridZeroLineColor = value; }
		}
		public Color GridLineColor
		{
			get { return mGridPainter.GridLineColor; }
			set { mGridPainter.GridLineColor = value; }
		}
		public Color BackdropColor
		{
			get { return mGridPainter.BackdropColor; }
			set { mGridPainter.BackdropColor = value; }
		}

		public Color RulerGridLineColor
		{
			get { return mRulerPainter.GridLineColor; }
			set { mRulerPainter.GridLineColor = value; }
		}
		public Color RulerBackdropColor
		{
			get { return mRulerPainter.BackdropColor; }
			set { mRulerPainter.BackdropColor = value; }
		}
		public Color RulerBackgroundColor
		{
			get { return mRulerPainter.BackgroundColor; }
			set { mRulerPainter.BackgroundColor = value; }
		}

		public Color ReferenceColor
		{
			get { return mIteratorPainter.ReferenceColor; }
			set { mIteratorPainter.ReferenceColor = value; }
		}

		public bool ShowRuler
		{
			get { return mRulerPainter.ShowLabels || mRulerPainter.ShowVertical || mRulerPainter.ShowHorizontal; }
			set
			{
				mRulerPainter.ShowLabels = value;
				mRulerPainter.ShowHorizontal = value;
				mRulerPainter.ShowVertical = value;

				Point lt, rb;

				if (value)
				{
					lt = new Point(mRulerPainter.RulerSize + 10, mRulerPainter.RulerSize + 10);
					rb = new Point(10, 10);
				}
				else
				{
					lt = new Point(10, 10);
					rb = new Point(10, 10);
				}

				mIteratorPainter.HintTextRectangle = new Rectangle(lt.X, lt.Y, rb.X - lt.X, rb.Y - lt.Y);
				Refresh();
			}
		}
		public bool HighlightOrigin
		{
			get { return mGridPainter.HighlightOrigin; }
			set { mGridPainter.HighlightOrigin = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Iterator SelectedIterator
		{
			get { return mIteratorInteraction.SelectedIterator; }
			set
			{
				mIteratorInteraction.SelectedIterator = value;
			}
		}
		public IteratorMatrix ActiveMatrix
		{
			get { return mIteratorInteraction.ActiveMatrix; }
			set
			{
				mIteratorInteraction.ActiveMatrix = value;
				mGridContextMenu.UpdateCheckedStates(this);
				Refresh();
			}
		}

		[NotNull, Browsable(false)]
		public EditorCommands Commands
		{
			get { return mCommands; }
		}

		[NotNull]
		public EditorSettings Settings
		{
			get { return mIteratorInteraction.Settings; }
			set
			{
				mIteratorInteraction.Settings.UnbindContextMenu();
				mIteratorInteraction.Settings = value;
				mIteratorInteraction.Settings.BindContextMenu(mGridContextMenu);
			}
		}

		public void ZoomOptimally()
		{
			if (mIteratorInteraction == null)
				return;

			mIteratorInteraction.ZoomOptimally();
		}

		private void OnBeginEdit(object sender, EventArgs args)
		{
			RaiseBeginEdit();
		}
		internal void RaiseBeginEdit()
		{
			if (mBeginEdit != null)
				mBeginEdit(this, new EventArgs());
		}

		private void OnEdit(object sender, EventArgs args)
		{
			RaiseEdit();
		}
		internal void RaiseEdit()
		{
			if (mEdit != null)
				mEdit(this, new EventArgs());
		}

		private void OnEndEdit(object sender, EventArgs args)
		{
			RaiseEndEdit();
		}
		internal void RaiseEndEdit()
		{
			if (mEndEdit != null)
				mEndEdit(this, new EventArgs());
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			RaiseSelectionChanged();
		}
		internal void RaiseSelectionChanged()
		{
			if (mSelectionChanged != null)
				mSelectionChanged(this, new EventArgs());
		}

		public event EventHandler BeginEdit
		{
			add { mBeginEdit += value; }
			remove { mBeginEdit -= value; }
		}
		public event EventHandler EndEdit
		{
			add { mEndEdit += value; }
			remove { mEndEdit -= value; }
		}
		public event EventHandler Edit
		{
			add { mEdit += value; }
			remove { mEdit -= value; }
		}
		public event EventHandler SelectionChanged
		{
			add { mSelectionChanged += value; }
			remove { mSelectionChanged -= value; }
		}

		private void OnCanvasMouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			var hit = mIteratorInteraction == null ? null : mIteratorInteraction.HitTestIterator(new Vector2(e.X, e.Y));
			if (hit != null)
			{
				//todo
			}

			if (hit == null && mGridContextMenu != null)
			{
				mGridContextMenu.Show(this, new Point(e.X, e.Y), ToolStripDropDownDirection.Default);
			}
		}
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			mInteraction.TriggerKeyPress(keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
