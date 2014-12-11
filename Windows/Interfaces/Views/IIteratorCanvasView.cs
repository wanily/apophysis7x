using System;
using System.Drawing;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IIteratorCanvasView : IView
	{
		[NotNull]
		IteratorCollection Iterators { get; set; }

		Color GridZeroLineColor { get; set; }
		Color GridLineColor { get; set; }
		Color BackdropColor { get; set; }
		Color RulerGridLineColor { get; set; }
		Color RulerBackdropColor { get; set; }
		Color RulerBackgroundColor { get; set; }
		Color ReferenceColor { get; set; }

		bool ShowRuler { get; set; }
		bool HighlightOrigin { get; set; }
		float PreviewRange { get; set; }
		float PreviewDensity { get; set; }
		bool PreviewApplyPostTransform { get; set; }

		Iterator SelectedIterator { get; set; }

		IteratorMatrix ActiveMatrix { get; set; }

		[NotNull]
		EditorCommands Commands { get; }

		[NotNull]
		EditorSettings Settings { get; set; }

		void ZoomOptimally();

		void RaiseBeginEdit();
		void RaiseEdit();
		void RaiseEndEdit();

		event EventHandler BeginEdit;
		event EventHandler EndEdit;
		event EventHandler Edit;

		event EventHandler SelectionChanged;
		event EventHandler ActiveMatrixChanged;

		void LoadSettings();
		void SaveSettings();
	}
}