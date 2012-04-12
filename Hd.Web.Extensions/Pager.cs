using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Hd.Web.Extensions
{
	public class PageIndexChangedEventArgs : EventArgs
	{
		public int OldPageNumber;
		public int NewPageNumber;
	}

	public class PageSizeChangedEventArgs : EventArgs
	{
		public int PageSize;
	}

	[ToolboxData("<{0}:Pager runat=server></{0}:Pager>"), ParseChildren(true)]
	public class Pager : WebControl, INamingContainer
	{
		public delegate void PageIndexChangedEventHandler(object sender, PageIndexChangedEventArgs e);

		public delegate void PageSizeChangedEventHandler(object sender, PageSizeChangedEventArgs e);

		public event PageIndexChangedEventHandler PageIndexChanged;

		public event PageSizeChangedEventHandler PageSizeChanged;

		protected virtual void RaisePageIndexChanged()
		{
			var e = new PageIndexChangedEventArgs { NewPageNumber = CurrentPageIndex };
			if (PageIndexChanged != null)
			{
				PageIndexChanged(this, e);
			}
		}

		public Pager()
		{
			_pageSize = int.MinValue;
			TotalRecordsCount = 0;
		}

		private string _stateID;

		public string StateID
		{
			get
			{
				if (string.IsNullOrEmpty(_stateID))
				{
					_stateID = "PageNumber";
				}

				return "Pager_" + _stateID;
			}
			set { _stateID = value; }
		}

		protected override void CreateChildControls()
		{
			Controls.Clear();
			BuildControlHierarchy();
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			Refresh();
		}

		private void Refresh()
		{
			CreateChildControls();
		}

		private string pageDescription = "Page {0} of {1}";
		private string totalCountDescription = "Total records {0}";

		[DefaultValue("Page {0} of {1}"),
		 Description("Gets and sets the description about the displayed page. {0} - page number, {1} - count of pages.")]
		public string PageDescription
		{
			get { return pageDescription; }
			set { pageDescription = value; }
		}

		[DefaultValue("Total records {0}"),
		 Description("Gets and sets the description about the count of records. {0} - count of records.")]
		public string TotalRecordsCountDescription
		{
			get { return totalCountDescription; }
			set { totalCountDescription = value; }
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.Div; }
		}

		private void BuildControlHierarchy()
		{
			Table table = new Table();
			table.CssClass = CssClass;
			table.Font.Name = Font.Name;
			table.Font.Size = Font.Size;
			table.BorderStyle = BorderStyle;
			table.BorderWidth = BorderWidth;
			table.BorderColor = BorderColor;
			table.HorizontalAlign = HorizontalAlign.Right;
			table.BackColor = BackColor;
			table.ForeColor = ForeColor;

			TableRow row = new TableRow();
			table.Rows.Add(row);

			TableCell prevCell = new TableCell();
			BuildPrevButton(prevCell);
			row.Cells.Add(prevCell);

			TableCell cellPageDescription = new TableCell();
			BuildDescription(cellPageDescription);
			cellPageDescription.HorizontalAlign = HorizontalAlign.Right;
			row.Cells.Add(cellPageDescription);

			TableCell nextCell = new TableCell();
			BuildNextButton(nextCell);
			row.Cells.Add(nextCell);

			Controls.Add(table);
		}

		private void BuildDescription(TableCell cell)
		{
			int startCount = 0;
			int endCount = 0;

			if (TotalRecordsCount == 0)
			{
				cell.Text = string.Empty;
				return;
			}

			if (IsNavigationAvailable)
			{
				startCount = PageSize * (_currentPageIndex - 1) + 1;
				endCount = PageSize * _currentPageIndex;

				if (endCount > TotalRecordsCount)
				{
					endCount = TotalRecordsCount;
				}
			}
			else
			{
				startCount = 1;
				endCount = TotalRecordsCount;
			}

			string recordsText = string.Format("<b>{0}</b> - <b>{1}</b> of <b>{2}</b>", startCount, endCount, TotalRecordsCount);

			cell.Text = recordsText;
		}

		private void BuildNextButton(TableCell cell)
		{
			bool isRequiredToMakeVisible = IsNavigationToNextAvalaible && IsNavigationAvailable;

			if (isRequiredToMakeVisible)
			{
				cell.Controls.Add(new LiteralControl("&nbsp;"));
			}

			LinkButton next = new LinkButton();
			next.ID = "Next";
			next.Click += navigateToNext;
			next.Text = "Next >";
			next.ToolTip = "Next page";
			next.CausesValidation = false;
			next.Visible = isRequiredToMakeVisible;
			cell.Controls.Add(next);
		}

		private void BuildPrevButton(TableCell cell)
		{
			bool isRequiredToMakeVisible = IsNavigationToPreviousAvalaible && IsNavigationAvailable;

			LinkButton prev = new LinkButton();
			prev.ID = "Prev";
			prev.Click += navigateToPreviuos;
			prev.ToolTip = "Previous page";
			prev.Text = "< Prev";
			prev.CausesValidation = false;
			prev.Visible = isRequiredToMakeVisible;
			cell.Controls.Add(prev);

			if (isRequiredToMakeVisible)
			{
				cell.Controls.Add(new LiteralControl("&nbsp;"));
			}
		}

		#region navidate* Methods

		private void navigateToPreviuos(object sender, EventArgs args)
		{
			GotoPage(_currentPageIndex - 1);
		}

		private void navigateToNext(object sender, EventArgs args)
		{
			GotoPage(_currentPageIndex + 1);
		}

		#endregion

		protected override void Render(HtmlTextWriter output)
		{
			if (Site != null && Site.DesignMode)
			{
				CreateChildControls();
			}

			base.Render(output);
		}

		private int _currentPageIndex
		{
			get
			{
				object value = StateKeeperManager.State.GetValue(StateID);

				if (ReferenceEquals(value, null))
				{
					return 1;
				}

				int pageIndex = (int)value;

				if (pageIndex <= 0)
				{
					return 1;
				}

				return pageIndex;
			}
			set { StateKeeperManager.State.SetValue(StateID, value); }
		}

		[Browsable(false)]
		public int CurrentPageIndex
		{
			get { return _currentPageIndex; }
			set
			{
				if (value <= 0)
				{
					_currentPageIndex = 1;
				}

				_currentPageIndex = value;
				RaisePageIndexChanged();
			}
		}

		public void GotoPage(int pageNumber)
		{
			CurrentPageIndex = pageNumber;
		}

		private int _pageSize;
		private int _defaultPageSize = int.MinValue;

		// TODO: this property is useless since we have another default PageSize in PageSettings class
		public int DefaultPageSize
		{
			set { _defaultPageSize = value; }
		}

		[Browsable(false)]
		[Personalizable()]
		public int PageSize
		{
			get
			{
				if (_pageSize == int.MinValue)
				{
					return _defaultPageSize == int.MinValue ? 20 : _defaultPageSize;
				}

				return _pageSize;
			}
			set
			{
				bool isRequiredToInvokeEvent = _pageSize != value && _pageSize != int.MinValue;
				_pageSize = value;

				if (PagesCount > 0 && _currentPageIndex > PagesCount)
				{
					_currentPageIndex = PagesCount;
				}

				if (isRequiredToInvokeEvent && PageSizeChanged != null)
				{
					var args = new PageSizeChangedEventArgs { PageSize = value };
					PageSizeChanged(this, args);
				}
			}
		}

		private int _totalRecordsCount;

		[Browsable(false)]
		public int TotalRecordsCount
		{
			get { return _totalRecordsCount; }
			set
			{
				_totalRecordsCount = value;

				if (PagesCount > 0 && _currentPageIndex > PagesCount)
				{
					CurrentPageIndex = PagesCount;
				}

				if (PagesCount == 0)
				{
					CurrentPageIndex = 1;
				}
			}
		}

		[Browsable(false)]
		public bool IsNavigationToPreviousAvalaible
		{
			get { return _currentPageIndex != 1; }
		}

		[Browsable(false)]
		public int PagesCount
		{
			get
			{
				int count = 0;

				try
				{
					count = TotalRecordsCount / PageSize;
					if ((double)TotalRecordsCount / PageSize > 1
						&& TotalRecordsCount * PageSize != count
						&& TotalRecordsCount != count * PageSize)
					{
						count++;
					}

					count = count == 0 && TotalRecordsCount >= 1 ? 1 : count;
				}
				catch { }

				return count;
			}
		}

		[Browsable(false)]
		public bool IsNavigationAvailable
		{
			get { return PageSize > 0; }
		}

		[Browsable(false)]
		public bool IsNavigationToNextAvalaible
		{
			get { return ((double)TotalRecordsCount / (_currentPageIndex * PageSize)) > 1; }
		}
	}
}