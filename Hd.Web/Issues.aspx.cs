// 
// Copyright (c) 2005-2013 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Linq;
using System.Text;
using Hd.QueryExtensions;
using Hd.Web.Extensions;

public partial class Issues : PersisterBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        controller.RefreshGrid();
    }

    private static void AppendLikeExpression(String words, SelectQuery selectQuery, WhereClause group, String columnName)
    {
        var parameter = new Parameter("%" + RemoveSpecialCharacters(words) + "%");
        group.Terms.Add(
            WhereTerm.CreateCompare(SqlExpression.Field(columnName, selectQuery.FromClause.BaseTable),
                                    SqlExpression.Parameter(), CompareOperator.Like));
        selectQuery.Parameters.Add(parameter);
    }
    private static String RemoveSpecialCharacters(String str)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Char c in str.Where(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
        {
            sb.Append(c);
        }
        return sb.ToString();
    }

}