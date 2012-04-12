//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyUtil;

namespace Hd.Web.Extensions
{
	internal class TpObjectDataSourceView : DataSourceView, IStateManager
	{
		private readonly TpObjectDataSource owner;

		public TpObjectDataSourceView(TpObjectDataSource owner, string viewName) : base(owner, viewName)
		{
			this.owner = owner;
		}

		protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
		{
			var args = new TpObjectDataSourceEventArgs();
			owner.OnSelecting(args);
			if (args.Cancel)
			{
				return null;
			}

			object obj = args.BusinessObject ?? Source.GetDataFromSource(GetParametersForSelect(arguments));

			if (arguments.RetrieveTotalRowCount && CanRetrieveTotalRowCount)
			{
				arguments.TotalRowCount = GetRowCount(obj);
			}

			args.SelectParams = SelectParameters.GetValues(HttpContext.Current, owner);

			if (obj == null)
			{
				return null;
			}

			if (!(obj is IEnumerable))
			{
				obj = new[] {obj};
			}
			if (InternalSort && (!string.IsNullOrEmpty(arguments.SortExpression)))
			{
				obj = Sort((IEnumerable) obj, arguments.SortExpression);
			}

			args.BusinessObject = obj;
			owner.OnSelected(args);

			return (IEnumerable) obj;
		}

		private static IEnumerable Sort(IEnumerable collection, string sortExpression)
		{
			string propertyName = sortExpression;
			bool reverse = false;
			if (propertyName.EndsWith("DESC"))
			{
				reverse = true;
				propertyName = propertyName.Substring(0, propertyName.Length - 5);
			}
			return SortUtility.Sort(collection, propertyName, reverse);
		}

		private object[] GetParametersForSelect(DataSourceSelectArguments arguments)
		{
			var parameters = new List<object>();
			foreach (DictionaryEntry entry in SelectParameters.GetValues(HttpContext.Current, owner))
			{
				parameters.Add(entry.Value);
			}
			if (CanPage)
			{
				parameters.Add(arguments.MaximumRows);
				parameters.Add(arguments.StartRowIndex);
			}
			if ((!string.IsNullOrEmpty(arguments.SortExpression)) && (!InternalSort))
			{
				parameters.Add(arguments.SortExpression);
			}
			return parameters.ToArray();
		}

		protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
		{
			var args = new TpObjectDataSourceEventArgs();

			owner.OnUpdating(args);

			if (args.Cancel)
			{
				return 0;
			}

			object obj = args.BusinessObject;

			if ((obj == null) && (keys.Count == 1))
			{
				obj = Source.Select(keys);
			}

			if (obj == null)
			{
				throw new InvalidOperationException("Cannot perform an update operation. No object to update.");
			}

			CopyValues(obj, values);
			Source.Update(obj);
			args.BusinessObject = obj;
			owner.OnUpdated(args);

			return 1;
		}

		protected override int ExecuteInsert(IDictionary values)
		{
			var args = new TpObjectDataSourceEventArgs();
			owner.OnInserting(args);
			if (args.Cancel)
			{
				return 0;
			}
			object obj = Source.CreateDataObjectInstance();
			CopyValues(obj, values);
			Source.Insert(obj);
			return 1;
		}

		protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
		{
			Source.Delete(keys);
			return 1;
		}

		private static void CopyValues(object target, IDictionary values)
		{
			Type type = target.GetType();
			PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(type);
			foreach (string key in values.Keys)
			{
				PropertyDescriptor descriptor = descriptors[key];
				object value = values[key];

				object valueToSet = BuildObjectValue(value, descriptor.PropertyType);
				descriptor.SetValue(target, valueToSet);
			}
		}

		private static object ConvertType(object value, Type type)
		{
			var invariantString = value as string;

			if (invariantString != null)
			{
				TypeConverter typeConverter = TypeDescriptor.GetConverter(type);

				if (typeConverter == null)
					return value;

				try
				{
					value = typeConverter.ConvertFromString(null, CultureInfo.CurrentCulture, invariantString);
				}
				catch (NotSupportedException)
				{
					throw new InvalidOperationException("Can not convert type");
				}
				catch (FormatException)
				{
					throw new InvalidOperationException("Can not convert type");
				}
			}

			return value;
		}


		private static object BuildObjectValue(object value, Type destinationType)
		{
			if ((value != null) && !destinationType.IsInstanceOfType(value))
			{
				Type elementType = destinationType;
				bool formedFromGeneric = false;

				if (destinationType.IsGenericType && (destinationType.GetGenericTypeDefinition() == typeof (Nullable<>)))
				{
					elementType = destinationType.GetGenericArguments()[0];
					formedFromGeneric = true;
				}
				else if (destinationType.IsByRef)
				{
					elementType = destinationType.GetElementType();
				}

				value = ConvertType(value, elementType);

				if (formedFromGeneric)
				{
					Type type = value.GetType();

					if (elementType != type)
						throw new InvalidOperationException("Can not convert type");
				}
			}

			return value;
		}


		private static int GetRowCount(object obj)
		{
			int result;
			if (obj == null)
			{
				result = 0;
			}
			else if (obj is IList)
			{
				result = ((IList) obj).Count;
			}
			else if (obj is List<object>)
			{
				result = ((List<object>) obj).Count;
			}
			else if (obj is IEnumerable)
			{
				var temp = ((IEnumerable) obj);
				int count = 0;
				foreach (object item in temp)
				{
					count++;
				}
				result = count;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public bool EnablePaging { get; set; }

		private DataSourceWrapper source;

		internal DataSourceWrapper Source
		{
			get
			{
				if (source == null)
				{
					source = new DataSourceWrapper(owner.TemplateControl);
				}
				if ((owner.TemplateControl != null) && (source.methodProvider == null))
				{
					source.methodProvider = owner.TemplateControl;
				}
				return source;
			}
		}

		private ParameterCollection selectParameters;

		public ParameterCollection SelectParameters
		{
			get
			{
				if (selectParameters == null)
				{
					selectParameters = new ParameterCollection();
					if (tracking)
					{
						((IStateManager) selectParameters).TrackViewState();
					}
				}
				return selectParameters;
			}
		}

		public override bool CanPage
		{
			get { return EnablePaging; }
		}

		public override bool CanRetrieveTotalRowCount
		{
			get { return true; }
		}

		public override bool CanSort
		{
			get { return true; }
		}

		public bool InternalSort { get; set; }

		#region IStateManager Members

		bool IStateManager.IsTrackingViewState
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		void IStateManager.LoadViewState(object state)
		{
			if (state != null)
			{
				((IStateManager) SelectParameters).LoadViewState(state);
			}
		}

		object IStateManager.SaveViewState()
		{
			return (selectParameters != null) ? ((IStateManager) selectParameters).SaveViewState() : null;
		}

		private bool tracking;

		void IStateManager.TrackViewState()
		{
			tracking = true;
			if (selectParameters != null)
			{
				((IStateManager) selectParameters).TrackViewState();
			}
		}

		#endregion
	}
}