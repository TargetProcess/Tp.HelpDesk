using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Compilation;
using System.Reflection;
using System.ComponentModel;
using System.Web.UI;
using System.Collections;

namespace Hd.Web.Extensions
{
    internal class DataSourceWrapper
    {
        public DataSourceWrapper(object methodProvider)
        {
            this.methodProvider = methodProvider;
        }
        internal object methodProvider;
        private Type type;
        public Type Type
        {
            get
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    type = methodProvider.GetType();
                }
                if (type == null)
                {
                    type = BuildManager.GetType(typeName, false, true);
                }
                return type;
            }
        }
        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        private string selectMethod;
        public string SelectMethod
        {
            get { return selectMethod; }
            set { selectMethod = value; }
        }
        private string updateMethod;
        public string UpdateMethod
        {
            get { return updateMethod; }
            set { updateMethod = value; }
        }
        private string insertMethod;
        public string InsertMethod
        {
            get { return insertMethod; }
            set { insertMethod = value; }
        }
        private string deleteMethod;
        public string DeleteMethod
        {
            get { return deleteMethod; }
            set { deleteMethod = value; }
        }
        private string dataObjectTypeName;
        public string DataObjectTypeName
        {
            get { return dataObjectTypeName; }
            set { dataObjectTypeName = value; }
        }
        public void Update(object obj)
        {
            Invoke(GetUpdateMethod(), new object[] { obj });
        }
        public object Select(IDictionary keys)
        {
            ArrayList a = new ArrayList(keys.Values);
            return Invoke(GetSelectMethod(1), a.ToArray());
        }
        public object GetDataFromSource(object[] parametes)
        {
            MethodInfo method = GetSelectMethod(parametes.Length);
            if (method == null)
            {
                return null;
            }
            return Invoke(method, parametes);
        }
        public void Insert(object obj)
        {
            Invoke(GetInsertMethod(), new object[] { obj });
        }
        public void Delete(IDictionary keys)
        {
            ArrayList a = new ArrayList(keys.Values);
            Invoke(GetDeleteMethod(), a.ToArray());            
        }
        private Dictionary<int, MethodInfo> selectMethods = new Dictionary<int, MethodInfo>();
        public MethodInfo GetSelectMethod(int parameterCount)
        {
            MethodInfo method;
            if (selectMethods.ContainsKey(parameterCount))
            {
                method = selectMethods[parameterCount];
            }
            else
            {
                method = GetMethod(SelectMethod, DataObjectMethodType.Select, parameterCount);
            }
            if (method == null)
            {
                throw new MissingMethodException("No select method found on " + TypeName);
            }
            return method;
        }
        public MethodInfo GetUpdateMethod()
        {
            MethodInfo method = GetMethod(UpdateMethod, DataObjectMethodType.Update, 1);
            if (method == null)
            {
                throw new MissingMethodException("No update method found on " + TypeName);
            }
            return method;
        }
        public MethodInfo GetInsertMethod()
        {
            MethodInfo method = GetMethod(InsertMethod, DataObjectMethodType.Insert, 1);
            if (method == null)
            {
                throw new MissingMethodException("No insert method found on " + TypeName);
            }
            return method;
        }
        public MethodInfo GetDeleteMethod()
        {
            MethodInfo method = GetMethod(DeleteMethod, DataObjectMethodType.Delete, 1);
            if (method == null)
            {
                throw new MissingMethodException("No delete method found on " + TypeName);
            }
            return method;
        }
        private MethodInfo GetMethod(string methodName, DataObjectMethodType methodType, int parametersCount)
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            if (!string.IsNullOrEmpty(methodName))
            {
                foreach (MethodInfo method in Type.GetMethods())
                {
                    if (String.Compare(method.Name, methodName, true) == 0)
                    {
                        methods.Add(method);
                    }
                }
            }
            else
            {
                foreach (MethodInfo method in Type.GetMethods())
                {
                    object[] attributes = method.GetCustomAttributes(typeof(DataObjectMethodAttribute), true);
                    if (attributes.Length > 0)
                    {
                        foreach (DataObjectMethodAttribute attribute in attributes)
                        {
                            if (attribute.MethodType == methodType)
                            {
                                methods.Add(method);
                            }
                        }
                    }
                }
            }
            if (methods.Count == 0)
            {
                return null;
            }
            foreach (MethodInfo method in methods)
            {
                if (method.GetParameters().Length == parametersCount)
                {
                    return method;
                }
            }
            return methods[0];
        }

        public object Invoke(MethodInfo method, object[] parameters)
        {
            if (method.IsStatic)
            {
                 return method.Invoke(null, parameters);
            }
        	object obj;
        	if (string.IsNullOrEmpty(TypeName))
        	{
        		obj = methodProvider;                    
        	}
        	else
        	{
        		obj = Activator.CreateInstance(Type);
        	}
        	return method.Invoke(obj, parameters);
        }
        public object CreateDataObjectInstance()
        {
            Type type = BuildManager.GetType(dataObjectTypeName, false, true);
            return Activator.CreateInstance(type);
        }
    }
}
