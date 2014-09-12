using System;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class BatchListController : Controller<Main>
	{
		private MainController mParent;

		public BatchListController([NotNull] Main view, [NotNull] MainController parent) : base(view)
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
			mParent.UndoController.CurrentReplaced += OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged += OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit += OnListLabelEdited;
		}
		protected override void DetachView()
		{
			mParent.UndoController.CurrentReplaced -= OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged -= OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit -= OnListLabelEdited;
		}

		public void BuildFlameList()
		{
			View.BatchListView.Items.Clear();
			if (mParent.Flames == null)
				return;

			var index = 0;
			foreach (var flame in mParent.Flames)
			{
				var item = View.BatchListView.Items.Add(flame.CalculatedName);

				SetItemProperties(item, flame);
				item.ImageIndex = index++;
			}
		}
		public void SelectFlame([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (mParent.Initializer.IsBusy || mParent.Flames == null)
				return;

			using (mParent.Initializer.Enter())
			{
				SetListSelection(flame);
			}

			mParent.LoadFlameAndEraseHistory(flame);
			mParent.EditorController.Flame = flame;
		}
		public Flame GetSelectedFlame()
		{
			var item = GetSelectedItem();
			if (item == null)
				return null;

			return item.Tag as Flame;
		}

		private void SetItemProperties(ListViewItem item, Flame flame)
		{
			item.Text = flame.CalculatedName;
			item.Tag = flame;
		}
		private ListViewItem GetSelectedItem()
		{
			return View.BatchListView.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Selected);
		}

		internal void SetListSelection([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			var index = mParent.Flames.IndexOf(flame);
			if (index < 0)
				return;

			View.BatchListView.Items[index].Selected = true;
		}

		private void OnListSelectionChanged(object sender, EventArgs e)
		{
			var flame = GetSelectedFlame();
			if (flame == null)
				return;

			mParent.LoadFlameAndEraseHistory(flame);
			mParent.UpdateToolbar();
			mParent.UpdateMenu();
		}
		private void OnListLabelEdited(object sender, LabelEditEventArgs e)
		{
			var item = View.BatchListView.Items[e.Item];
			var flame = item.Tag as Flame;

			if (flame == null)
				return;

			if (!string.IsNullOrEmpty(e.Label) && !string.IsNullOrEmpty(e.Label.Trim()))
			{
				flame.Name = e.Label;
				mParent.NotifyFlameNameChanged(flame);
			}
			else
			{
				e.CancelEdit = true;
			}
		}
		private void OnCurrentFlameReplaced(object sender, EventArgs e)
		{
			var selected = GetSelectedItem();
			if (selected == null)
				return;

			var flame = selected.Tag as Flame;
			if (flame == null)
				return;

			using (mParent.Initializer.Enter())
			{
				mParent.Flames.Replace(flame, mParent.UndoController.Current);
				SetItemProperties(selected, mParent.UndoController.Current);
				mParent.EditorController.Flame = mParent.UndoController.Current;
			}
		}

	}
}