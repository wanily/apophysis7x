using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public abstract class FileDialogController : Controller
	{
		public static readonly string BatchFilesFilter = "Fractal Flame batches (*.flame)|*.flame";
		public static readonly string PaletteFilesFilter = "Fractal Flame batches (*.gradient;*.ugr)|*.gradient;*.ugr";
		public static readonly string AllFilesFilter = "All files (*.*)|*.*";
	}
	public class FileDialogController<T> : FileDialogController where T : FileDialog, new()
	{
		private T mDialog;

		public FileDialogController(string title, params string[] filters)
		{
			mDialog = new T
			{
				Filter = string.Join("|", filters), 
				Title = title
			};
		} 
		protected override void Dispose(bool disposing)
		{
			if (disposing && mDialog != null)
			{
				mDialog.Dispose();
				mDialog = null;
			}
		}

		public string GetFileName(bool? overwriteWarning = null)
		{
			if (mDialog == null)
			{
				throw new ObjectDisposedException(GetType().Name);
			}

			if (overwriteWarning.HasValue && mDialog is SaveFileDialog)
			{
				var saveDialog = (SaveFileDialog)(object)mDialog;
				saveDialog.OverwritePrompt = overwriteWarning.Value;
			}

			var result = mDialog.ShowDialog();
			if (result != DialogResult.OK)
				return null;

			return mDialog.FileName;
		}
	}
}
