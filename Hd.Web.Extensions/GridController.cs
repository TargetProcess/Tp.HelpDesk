// 
// Copyright (c) 2005-2012 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Hd.Portal;
using Hd.Portal.Components;
using Hd.Portal.Entities.Request;
using Hd.QueryExtensions;
using Parameter = Hd.QueryExtensions.Parameter;

namespace Hd.Web.Extensions
{
    public class GridControllerArgs : EventArgs
    {
        private readonly GridView _gridView;
        private readonly IList _collection;

        public GridControllerArgs(GridView gridView, IList collection)
        {
            _gridView = gridView;
            _collection = collection;
        }

        public IList SourceCollection
        {
            get { return _collection; }
        }

        public GridView Grid
        {
            get { return _gridView; }
        }
    }

    [ToolboxData("<{0}:GridController runat=server></{0}:GridController>")]
    public class GridController : WebControl, IPostBackEventHandler
    {
        #region Events declarations

        public delegate void OnQueryCreatedHandler();

        public event OnQueryCreatedHandler QueryCreated;

        public delegate void OnGridBindingCompleteHandler();

        public event OnGridBindingCompleteHandler GridBindingComplete;

        public delegate void OnGridBindingStartHandler(Object sender, GridControllerArgs args);

        public event OnGridBindingStartHandler GridBindingStart;

        public delegate void OnEntityOperationHandler(int? id, EventArgs args);

        public event OnEntityOperationHandler DeleteEntity;

        #endregion

        #region Fields

        private GridView _grid;
        private Pager _pager;
        private BusinessQuery _businessQuery;
        private Type entityBaseType = null;
        private readonly Collection<Control> _childControls = new Collection<Control>();

        #endregion

        #region Constructors

        public GridController()
        {
            IsRequiredToBindGrid = false;
        }

        #region Overloads

        public GridController(GridView grid, Pager pager, BusinessQuery businessQuery)
            : this(grid, pager, businessQuery, null)
        {
        }

        #endregion

        public GridController(GridView grid, Pager pager,
                              BusinessQuery businessQuery, Type entityBaseType)
        {
            IsRequiredToBindGrid = false;
            if (grid == null)
                throw new ArgumentNullException("grid");

            if (businessQuery == null)
                throw new ArgumentNullException("businessQuery");

            _businessQuery = businessQuery;
            this.entityBaseType = entityBaseType;

            _grid = grid;
            _childControls.Add(grid);

            if (pager != null)
            {
                _pager = pager;
                //if (pager is Control) 
                _childControls.Add(pager);
            }

            InitializeGridSettings();
            InitializeQuerySettings();
            InitializePagerSettings();
        }

        #endregion

        #region Properties

        private String _filterProject = "-1";

        public String FilterProject
        {
            set { _filterProject = value; }
        }

        private String _gridId;

        public String GridID
        {
            set { _gridId = value; }
        }

        private String _filterId;

        public String FilterID
        {
            set { _filterId = value; }
        }

        private String _pagerId;

        public String PagerID
        {
            set { _pagerId = value; }
        }

        private String _custDlgBoxId;

        public String CustomizeDialogBoxId
        {
            set { _custDlgBoxId = value; }
        }

        private String _queryType;

        public String QueryType
        {
            get { return _queryType; }
            set { _queryType = value; }
        }

        public BusinessQuery BusinessQuery
        {
            get { return _businessQuery; }
        }

        public String EntityType
        {
            set { entityBaseType = Type.GetType(value); }
        }

        public GridView Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        public Pager Pager
        {
            get
            {
                if (_pager == null)
                    throw new Exception("Pager is not defined");
                return _pager;
            }
        }

        #endregion

        #region Event Handlers

        #region Grid Event Handlers

        private void Grid_Sorting(Object sender, GridViewSortEventArgs e)
        {
            var sortDirection = _businessQuery.Query.OrderByTerms[0].Field == e.SortExpression &&
                                _businessQuery.Query.OrderByTerms[0].Direction == OrderByDirection.Ascending
                                    ? OrderByDirection.Descending
                                    : OrderByDirection.Ascending;

            _businessQuery.Query.OrderByTerms[0] = String.CompareOrdinal(e.SortExpression, "size(request.Requesters)") == 0
                                                                                            ? new OrderByTerm(e.SortExpression, sortDirection)
                                                                                            : new OrderByTerm(e.SortExpression, _businessQuery.Query.FromClause.BaseTable, sortDirection);

            ResetPageIndex();
            IsRequiredToBindGrid = true;
        }

        private void Grid_PreRender(Object sender, EventArgs e)
        {
            if (IsRequiredToBindGrid)
                BindGrid();
        }

        protected void Grid_RowDataBound(Object sender, GridViewRowEventArgs e)
        {
        }

        #endregion

        #region WebControl Event Handlers

        protected override void OnInit(EventArgs e)
        {
            Control control;

            if (_grid == null)
            {
                control = FindNestedControl(_gridId);

                if (control != null && control is GridView)
                    _grid = control as GridView;
                else
                    throw new Exception(String.Format("GridView '{0}' not found", _gridId));
            }

            _businessQuery = BusinessQuery.CreateInstance(_queryType);

            if (!ReferenceEquals(QueryCreated, null))
                QueryCreated();

            if (_pager == null && _pagerId != null)
            {
                control = FindControl(_pagerId);
                if (control != null && control is Pager)
                {
                    _pager = control as Pager;
                    _pager.StateID = GetStateID();
                    _childControls.Add(_pager);
                }
                else
                    throw new Exception(String.Format("Pager '{0}' not found", _pagerId));
            }

            InitializeGridSettings();
            InitializeQuerySettings();

            if (IsPagingEnabled && Page.IsPostBack)
                _pager.PageSize = _businessQuery.Query.PageSettings.PageSize;

            InitializePagerSettings();

            if (!Page.IsPostBack)
                IsRequiredToBindGrid = true;
        }

        private Control FindNestedControl(String controlID)
        {
            if (String.IsNullOrEmpty(controlID))
                return null;

            if (controlID.IndexOf(".") < 0)
                return FindControl(controlID);

            String[] controlIDs = controlID.Split('.');

            Control control = FindControl(controlIDs[0]);

            for (Int32 i = 1; i < controlIDs.Length; i++)
                control = control.FindControl(controlIDs[i]);

            return control;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Initialize();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div id='{0}'></div>", ClientID);
        }

        #endregion

        private void Pager_PageIndexChanged(Object sender, PageIndexChangedEventArgs e)
        {
            if (!IsPagingEnabled)
                return;

            InitializeQueryPageSettings();
        }

        private void InitializeQueryPageSettings()
        {
            _businessQuery.Query.PageSettings.PageIndex = _pager.CurrentPageIndex;
            _businessQuery.Query.PageSettings.PageSize = _pager.PageSize;
            IsRequiredToBindGrid = true;
        }

        #endregion

        #region Public Methods

        public void InitializeFilter()
        {
            Initialize();
            // initialize filter
        }

        public void Initialize()
        {
            SelectQuery query = LoadQuery();

            if (!ReferenceEquals(query, null) && _grid.Rows.Count > 0)
            {
                UpdateSortDirectionSign();
            }

            if (IsPagingEnabled && (Page == null || !Page.IsPostBack))
                _pager.GotoPage(_pager.CurrentPageIndex);
        }

        public void ResetPageIndex()
        {
            if (IsPagingEnabled)
                _pager.CurrentPageIndex = 1;
        }

        public Boolean IsPagingEnabled
        {
            get { return !ReferenceEquals(_pager, null); }
        }

        public Boolean GridControlsVisibile
        {
            set
            {
                foreach (Control control in _childControls)
                {
                    control.Visible = value;
                }
            }
            get { return _grid.Visible; }
        }

        protected Boolean IsRequiredToBindGrid { get; set; }

        public void BindGrid()
        {
            BindGrid(IsRequiredToBindGrid);
        }

        public void RefreshGrid()
        {
            IsRequiredToBindGrid = true;
            UpdatePagerTotalCount();
        }
        private static void AppendLikeExpression(String words, SelectQuery selectQuery, WhereClause group, String columnName)
        {
            var parameter = new Parameter("%" + words + "%");
            group.Terms.Add(
                WhereTerm.CreateCompare(SqlExpression.Field(columnName, selectQuery.FromClause.BaseTable),
                                        SqlExpression.Parameter(), CompareOperator.Like));
            selectQuery.Parameters.Add(parameter);
        }

        public void BindGrid(Boolean bindFlag)
        {
            if (!bindFlag)
                return;

            if (_grid.DataKeyNames.Length == 0)
                throw new Exception(String.Format("The data key should be specified for grid '{0}'", _grid.ID));

            String fieldId = _grid.DataKeyNames[0];

            Int32 countOfOrderByFields = _businessQuery.Query.OrderByTerms.Cast<OrderByTerm>().Count(term => term.Field.ToLower() == fieldId.ToLower());

            if (countOfOrderByFields == 0)
                _businessQuery.Query.OrderByTerms.Add(new OrderByTerm(fieldId, OrderByDirection.Descending));

            if (countOfOrderByFields == 2)
                _businessQuery.Query.OrderByTerms.RemoveAt(1);

            // FILTER CODE STARTS
            if (_filterProject != "-1")
            {
                var groupName = new WhereClause(WhereClauseRelationship.And);

                //_filterProject = RemoveSpecialCharacters(_filterProject);
                AppendLikeExpression(_filterProject, _businessQuery.Query, groupName, "Project.Name");
                //AppendLikeExpression("Tau Product Web Site - Scrum #1", _businessQuery.Query, groupName, "Project.Name");
                
                _businessQuery.Query.WherePhrase.SubClauses.Add(groupName);
            }
            //else
            //{
            //    _businessQuery.Query.WherePhrase.SubClauses.Clear();
            //}
            // FILTER CODE ENDS

            IList list;
            // Search functionality
            if (HttpContext.Current.Request.QueryString["SearchString"] != null)
            {
                var searchFilter = new SearchFilter { SearchPattern = HttpContext.Current.Request.QueryString["SearchString"] };
                searchFilter.AppendSimpleSearchPattern(_businessQuery.Query);

                list = DataPortal.Instance.Retrieve(_businessQuery.Query);

                UpdatePagerTotalCount();
                _businessQuery.Reinitialize();
            }
            else
            {
                list = DataPortal.Instance.Retrieve(_businessQuery.Query);
            }
            // THE UPDATE QUERY SHOULD BE REINITIALIZED AND THE PAGER TOTAL COUNT SHOULD BE UPDATED
            if (_filterProject != "-1")
            {
                UpdatePagerTotalCount();
                _businessQuery.Reinitialize();
            }

            _grid.DataSource = list;

            if (list.Count > 0 && list[0] is Request)
            {
                var teamCache = new TeamCache();
                teamCache.PreloadChilds(list);

                var requestersCountCache = new RequestersCountCache();
                requestersCountCache.PreloadChilds(list);
            }

            if (GridBindingStart != null)
                GridBindingStart(this, new GridControllerArgs(_grid, list));

            _grid.DataBind();

            SaveQuery();

            UpdateSortDirectionSign();

            if (GridBindingComplete != null)
                GridBindingComplete();
        }

        #endregion

        #region Private Methods

        private void InitializeGridSettings()
        {
            _grid.PreRender += Grid_PreRender;
            _grid.Sorting += Grid_Sorting;
            _grid.RowDataBound += Grid_RowDataBound;
        }

        private void InitializeQuerySettings()
        {
            SelectQuery query = LoadQuery();

            if (!ReferenceEquals(query, null))
            {
                _businessQuery.Query = query;
            }
        }

        private void InitializePagerSettings()
        {
            if (!IsPagingEnabled)
                return;

            _pager.PageIndexChanged += Pager_PageIndexChanged;
            _pager.PageSizeChanged += _pager_PageSizeChanged;
            UpdatePagerTotalCount();
        }

        private void _pager_PageSizeChanged(Object sender, PageSizeChangedEventArgs e)
        {
            if (_pager.PageSize != _businessQuery.Query.PageSettings.PageSize)
            {
                ResetPageIndex();
                InitializeQueryPageSettings();
            }
        }

        private void UpdatePagerTotalCount()
        {
            if (!IsPagingEnabled)
                return;

            if (_pager.PageSize != _businessQuery.Query.PageSettings.PageSize)
            {
                _pager.PageSize = _businessQuery.Query.PageSettings.PageSize;
            }

            _pager.TotalRecordsCount = DataPortal.Instance.RetrieveCount(_businessQuery.Query);
        }

        private void SaveQuery()
        {
            StateKeeperManager.State.SetValue(GetStateID(), _businessQuery.Query);
        }

        public SelectQuery LoadQuery()
        {
            return StateKeeperManager.State.GetValue(GetStateID()) as SelectQuery;
        }

        public String GetStateID()
        {
            return "STATE" + _grid.ClientID + "_" + Requester.LoggedUserID;
        }

        /// <summary>
        /// 	Draw sort direction image. TODO: Maybe refactored into Sorter class
        /// </summary>
        private void UpdateSortDirectionSign()
        {
            if (ReferenceEquals(_grid.HeaderRow, null))
                return;

            TableCellCollection cells = _grid.HeaderRow.Cells;
            foreach (TableCell cell in cells)
            {
                if (cell.Controls.Count > 0 && cell.Controls[0] is LinkButton)
                {
                    var link = (LinkButton)cell.Controls[0];
                    cell.Wrap = false;
                    if (link.CommandArgument == _businessQuery.Query.OrderByTerms[0].Field)
                    {
                        var labelTitle = new Label { Text = link.Text + " " };
                        // OBSOLETE
                        //var img = new Image
                        //            {
                        //                ImageUrl = _businessQuery.Query.OrderByTerms[0].Direction == OrderByDirection.Ascending
                        //                            ? "~/img/sort_up.gif"
                        //                            : "~/img/sort_down.gif"
                        //            };

                        link.CssClass = _businessQuery.Query.OrderByTerms[0].Direction
                            == OrderByDirection.Ascending
                            ? "tau-unit-asc_sorted"
                            : "tau-unit-desc_sorted";
                        link.Controls.Clear();


                        link.Controls.Add(labelTitle);
                        link.Controls.Add(new HtmlGenericControl("i"));
                        // link.Controls.Add(img);
                    }
                }
            }
        }

        #endregion

        public void RaisePostBackEvent(String eventArgument)
        {
            String[] parts = eventArgument.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                return;

            if (parts[0] == "D")
            {
                if (DeleteEntity != null)
                    DeleteEntity(Int32.Parse(parts[1]), new EventArgs());
            }
        }

        public String GetDeletePostbackScript(Object argument)
        {
            return Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}", argument));
        }

    }
}