﻿using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class About : Form
	{
		public About()
		{
			InitializeComponent();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			mVersionLabel.Text = string.Format(mVersionLabel.Text, Application.ProductVersion);
			mCopyright.Text = string.Format(mCopyright.Text, DateTime.Today.Year, Application.CompanyName);
		}

		private void OnCloseClick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
