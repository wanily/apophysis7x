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
			View.BatchListView.SelectedIndexChanged += OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit += OnListLabelEdited;
		}
		protected override void DetachView()
		{
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
				
				item.Tag = flame;
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
				var index = mParent.Flames.IndexOf(flame);
				if (index < 0)
					return;

				View.BatchListView.Items[index].Selected = true;
			}

			mParent.LoadFlameAndEraseHistory(flame);
		}
		public Flame GetSelectedFlame()
		{
			var item = View.BatchListView.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Selected);
			if (item == null)
				return null;

			return item.Tag as Flame;
		}

		private void OnListSelectionChanged(object sender, EventArgs e)
		{
			var flame = GetSelectedFlame();
			if (flame == null)
				return;

			mParent.LoadFlameAndEraseHistory(flame);
		}
		private void OnListLabelEdited(object sender, LabelEditEventArgs e)
		{
			var item = View.BatchListView.Items[e.Item];
			var flame = item.Tag as Flame;

			if (flame == null)
				return;

			flame.Name = e.Label;
			mParent.NotifyFlameNameChanged(flame);
		}

	}
}