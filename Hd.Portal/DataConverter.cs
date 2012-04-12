// 
// Copyright (c) 2005-2008 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Hd.Portal
{
	public class DataConverter<T> where T : new()
	{
		public List<T> Convert(IList listOfEntities)
		{
			List<T> list = new List<T>();

			foreach (object entity in listOfEntities)
			{
				list.Add(Convert(entity));
			}

			return list;
		}

		public static List<object> Convert(IList listOfEntities, Type type)
		{
			List<object> list = new List<object>();

			foreach (object entity in listOfEntities)
			{
				object instance = Activator.CreateInstance(type);
				list.Add(Convert(entity, instance));
			}

			return list;
		}

		public static T Convert(object dto)
		{
			T value = new T();
			return (T) Convert(dto, value);
		}

		public DTO Convert<DTO>(T entity)
		{
			object instance = Activator.CreateInstance(typeof (DTO));

			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(instance);
			PropertyDescriptorCollection sourceDescriptors = TypeDescriptor.GetProperties(entity);

			foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
			{
				object valueToSet = sourceDescriptors[propertyDescriptor.Name].GetValue(entity);
				propertyDescriptor.SetValue(instance, valueToSet);
			}

			return (DTO) instance;
		}

		public static object Convert(object source, object instance)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(instance);
			PropertyDescriptorCollection sourceDescriptors = TypeDescriptor.GetProperties(source);

			foreach (PropertyDescriptor propertyDescriptor in sourceDescriptors)
			{
				object valueToSet = sourceDescriptors[propertyDescriptor.Name].GetValue(source);

				PropertyDescriptor descriptor = propertyDescriptorCollection[propertyDescriptor.Name];

				if (!ReferenceEquals(descriptor, null))
				{
					descriptor.SetValue(instance, valueToSet);
				}
			}

			return instance;
		}
	}
}