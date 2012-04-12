// 
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Hd.Portal;
using Hd.QueryExtensions;
using NUnit.Framework;
using Tp.RequestServiceProxy;

namespace Hd.Tests
{
	[TestFixture]
	public class PortalTest
	{
		[SetUp]
		public void SetUp()
		{
			DataPortal.Initialize();
			DataPortal.Instance.SetFactory<Request>(new RequestDataFactory());
		}

		[TearDown]
		public void TearDown()
		{
		}

		[Test]
		public void RetrieveAll()
		{
			var query = new SelectQuery(typeof (Request));
			List<Request> requests = DataPortal.Instance.Retrieve<Request>(query);
			CollectionAssert.IsNotEmpty(requests);
		}

		[Test]
		public void RetrieveAllWithoutType()
		{
			var query = new SelectQuery(typeof (Request));
			IList requests = DataPortal.Instance.Retrieve(query);
			CollectionAssert.IsNotEmpty(requests);
		}

		[Test]
		public void RetrieveCount()
		{
			var query = new SelectQuery(typeof (Request));
			Assert.AreEqual(0, DataPortal.Instance.RetrieveCount(query));
		}

		[Test]
		public void RetrieveEntity()
		{
			var query = new SelectQuery(typeof (Request));
			IEntity entity = DataPortal.Instance.Retrieve(typeof (Request), 1);
			Assert.AreEqual(1, (entity as Request).ID);
		}

		[Test]
		public void ReflectionGetByID()
		{
			var factory = new RequestDataFactory();

			Object result = factory.GetType().InvokeMember("Retrieve", BindingFlags.InvokeMethod,
			                                               Type.DefaultBinder, factory, new object[] {1});

			Assert.AreEqual(1, ((RequestDTO) result).ID);
		}

		[Test]
		public void ReflectionCount()
		{
			var factory = new RequestDataFactory();

			var objects = new object[] {1, 2, 3};

			Object result = factory.GetType().InvokeMember("RetrieveCount", BindingFlags.InvokeMethod,
			                                               Type.DefaultBinder, factory,
			                                               new object[] {"select count(*) from Request", objects});

			Assert.AreEqual(0, result);
		}
	}
}