// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections.Generic;
using System.Text;

namespace Hd.QueryExtensions
{
	/// <summary>
	/// Data type of a constant value
	/// </summary>
	public enum SqlDataType
	{
		/// <summary>String value</summary>
		String,
		/// <summary>Numeric value (int, double, float, decimal)</summary>
		Number,
		/// <summary>DateTime object</summary>
		Date
	}

	/// <summary>
	/// Represents a typed constant value.
	/// </summary>
	[Serializable]
	public class SqlConstant
	{
		private SqlDataType type;
		private object val;

		/// <summary>
		/// Creates a new SqlConstant instance
		/// </summary>
		/// <param name="type">Constant's date type</param>
		/// <param name="val">Constant's value</param>
		public SqlConstant(SqlDataType type, object val)
		{
			this.type = type;
			this.val = val;
		}

		public SqlConstant() : this(SqlDataType.String, null) {}

		/// <summary>
		/// Creates a SqlConstant which represents a numeric value.
		/// </summary>
		/// <param name="val">Value of the expression</param>
		/// <returns>A SqlConstant which represents a floating point value</returns>
		public static SqlConstant Number(double val)
		{
			return new SqlConstant(SqlDataType.Number, val);
		}

		/// <summary>
		/// Creates a SqlConstant which represents a numeric value.
		/// </summary>
		/// <param name="val">Value of the expression</param>
		/// <returns>A SqlConstant which represents a decimal value</returns>
		public static SqlConstant Number(decimal val)
		{
			return new SqlConstant(SqlDataType.Number, val);
		}

		/// <summary>
		/// Creates a SqlConstant which represents a numeric value.
		/// </summary>
		/// <param name="val">Value of the expression</param>
		/// <returns>A SqlConstant which represents a numeric value</returns>
		public static SqlConstant Number(int val)
		{
			return new SqlConstant(SqlDataType.Number, val);
		}

		/// <summary>
		/// Creates a SqlConstant which represents a textual value.
		/// </summary>
		/// <param name="val">Value of the expression</param>
		/// <returns>A SqlConstant which represents a textual value</returns>
		public static SqlConstant String(string val)
		{
			return new SqlConstant(SqlDataType.String, val);
		}

		/// <summary>
		/// Creates a SqlConstant which represents a date value.
		/// </summary>
		/// <param name="val">Value of the expression</param>
		/// <returns>A SqlConstant which represents a date value</returns>
		public static SqlConstant Date(DateTime val)
		{
			return new SqlConstant(SqlDataType.Date, val);
		}

		public SqlDataType Type
		{
			get { return type; }
			set { type = value; }
		}

		public object Value
		{
			get { return val; }
			set { val = value; }
		}
	}
}