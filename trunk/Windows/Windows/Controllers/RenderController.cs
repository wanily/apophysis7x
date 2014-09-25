using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Threading;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class RenderController : DataInputController<Render>
	{
		private int mCounter;
		private NativeTimer mElapsedTimer;
		private ThreadedRenderer mRenderer;
		private Stack<Flame> mRenderStack;

		private MainController mParent;
		private bool mBatchMode;
		private Flame mCurrentlyRenderingFlame;
		private bool mIsRendering;
		private bool mIsPaused;

		public RenderController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mRenderer = new ThreadedRenderer();
			mRenderStack = new Stack<Flame>();
			mElapsedTimer = new NativeTimer();

			mRenderer.InvokeCallbackMode = InvokeCallbackMode.AfterReset;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mRenderer != null)
				{
					mRenderer.Dispose();
					mRenderer = null;
				}

				if (mRenderStack != null)
				{
					mRenderStack.Clear();
					mRenderStack = null;
				}
			}

			mParent = null;
			mElapsedTimer = null;
		}

		protected override void AttachView()
		{
			View.StartButton.Click += OnStartClick;
			View.PauseButton.Click += OnPauseClick;
			View.CancelButton.Click += OnCancelClick;

			mRenderer.Progress += OnProgress;
		}
		protected override void DetachView()
		{
			View.StartButton.Click -= OnStartClick;
			View.PauseButton.Click -= OnPauseClick;
			View.CancelButton.Click -= OnCancelClick;

			mRenderer.Progress -= OnProgress;
		}

		private void OnProgress(object sender, ProgressEventArgs args)
		{
			try
			{
				SetProgress(args.Progress);
				SetRemaining(args.TimeRemaining);
				SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			}
			catch (ObjectDisposedException) { }
			
		}
		private void OnStartClick(object sender, EventArgs e)
		{
			IsRendering = true;
			mCounter = 0;

			if (mBatchMode)
			{
				foreach (var flame in mParent.Flames.Reverse())
				{
					mRenderStack.Push(flame);
				}
			}
			else
			{
				mRenderStack.Push(mParent.BatchListController.GetSelectedFlame());
			}

			NextFlame();
		}
		private void OnPauseClick(object sender, EventArgs e)
		{
			if (!IsRendering)
				return;

			IsPaused = !IsPaused;

			if (IsPaused)
			{
				mRenderer.Suspend();
			}
			else
			{
				mRenderer.Resume();
			}
		}
		private void OnCancelClick(object sender, EventArgs e)
		{
			if (IsRendering)
			{
				mRenderer.Cancel();

				IsRendering = false;
				CurrentlyRenderingFlame = null;

				ResetStatus();
				mRenderStack.Clear();
			}
			else
			{
				View.Close();
			}
		}

		private void SetSingleMode(Flame flame)
		{
			Debug.Assert(flame != null);

			var header = string.Format(ControlResources.RenderSingleWindowTitleWithFlame, flame.CalculatedName);

			View.Text = header;
			CurrentlyRenderingFlame = null;
		}
		private void SetBatchMode(FlameCollection batch)
		{
			Debug.Assert(batch != null);

			var header = string.Format(ControlResources.RenderBatchWindowTitleWithFlames, batch.CalculatedName);

			View.Text = header;
			CurrentlyRenderingFlame = null;
		}
		private void UpdateMode()
		{
			if (mBatchMode)
			{
				SetBatchMode(mParent.Flames);
			}
			else
			{
				SetSingleMode(mParent.BatchListController.GetSelectedFlame());
			}
		}

		private void NextFlame()
		{
			if (View == null)
				return;

			if (mRenderStack.Count == 0)
			{
				View.Invoke(new Action(() =>
				{
					IsRendering = false;
					CurrentlyRenderingFlame = null;
					ResetStatus();

					mCounter = 0;
				}));

				return;
			}

			mCounter++;

			mElapsedTimer.SetStartingTime();

			View.Invoke(new Action(() =>
			{
				CurrentlyRenderingFlame = mRenderStack.Pop();
			}));

			SetInfoString(mCurrentlyRenderingFlame.CalculatedName);

			var flame = mCurrentlyRenderingFlame;

			//todo
			mRenderer.StartCreateBitmap(flame, 25, flame.CanvasSize, SaveBitmap);
		}
		private void SaveBitmap(Bitmap bitmap)
		{
			//todo
			bitmap.Dispose();
			NextFlame();
		}

		private void ResetStatus()
		{
			View.Invoke(new Action(() =>
			{
				View.ProgressBar.Value = 0;
				View.RemainingLabel.Text = null;
				View.ElapsedLabel.Text = null;
				View.InfoLabel.Text = null;
			}));
		}
		private void SetProgress(double progress)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.ProgressBar.Value = (int)(progress * 100)));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.ElapsedLabel.Text = string.Format("Elapsed: {0}", GetTimespanString(elapsed))));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.RemainingLabel.Text = string.Format("Remaining: {0}", remaining == null ? "calculating..." : GetTimespanString(remaining.Value))));
		}
		private void SetInfoString(string info)
		{
			if (View == null)
				return;

			View.Invoke(new Action(() => View.InfoLabel.Text = info));
		}
		private string GetTimespanString(TimeSpan time)
		{
			return string.Format("{0}:{1}:{2}",
				time.Hours.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Minutes.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Seconds.ToString("#0", InputController.Culture).PadLeft(2, '0'));
		}

		public Flame CurrentlyRenderingFlame
		{
			get { return mCurrentlyRenderingFlame; }
			private set
			{
				mCurrentlyRenderingFlame = value;
			}
		}
		public bool BatchMode
		{
			get { return mBatchMode; }
			set
			{
				if (Equals(value, mBatchMode))
					return;

				mBatchMode = value;
				UpdateMode();
			}
		}

		public bool IsRendering
		{
			get { return mIsRendering; }
			private set
			{
				if (Equals(value, mIsRendering))
					return;

				var oldValue = mIsRendering;
				mIsRendering = value;

				View.StartButton.Enabled = !value;
				View.PauseButton.Enabled = value;
				View.CancelButton.Text = value ? ControlResources.Cancel : ControlResources.Close;

				View.SizeGroupBox.Enabled = !value;
				View.PropertiesGroupBox.Enabled = !value;
				View.SaveParametersCheckBox.Enabled = !value;
				View.DestinationLabel.Enabled = !value;
				View.DestinationTextBox.Enabled = !value;
				View.SelectFolderButton.Enabled = !value;
				View.GoToFolderButton.Enabled = !value;

				if (!oldValue && value)
				{
					View.Tabs.SelectedTab = View.MessagesTab;
				}
				else if (!value && oldValue)
				{
					//
				}

				View.ProgressBar.Value = 0;
				IsPaused = false;
			}
		}
		public bool IsPaused
		{
			get { return mIsPaused; }
			private set
			{
				if (Equals(value, mIsPaused))
					return;

				mIsPaused = value;

				View.PauseButton.Text = value ? ControlResources.Resume : ControlResources.Pause;
			}
		}

		public override void Update()
		{
			UpdateMode();
			IsRendering = false;

			base.Update();
		}
	}
}