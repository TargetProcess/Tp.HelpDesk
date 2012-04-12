//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using System.Collections.Generic;
using System.Linq;
using Hd.Portal;
using NUnit.Framework;
using Tp.RequestServiceProxy;

namespace Hd.Tests.Portal.Entities.Request
{
	public class RequestContext
	{
		private Requester _requester;

		public void UserWorksForCompany(int userCompanyId)
		{
			_requester = new Requester {CompanyID = userCompanyId};
			_products.Clear();
			_retrievedProducts = null;
		}

		public void UserDoesNotWorkForAnyCompany()
		{
			_requester = new Requester();
			_products.Clear();
			_retrievedProducts = null;
		}

		private readonly List<Product> _products = new List<Product>();

		public void ProductBelongsToCompany(string productName, int companyId)
		{
			_products.Add(new Product {Name = productName, CompanyID = companyId});
		}

		public void ProductIsPublic(string productName)
		{
			_products.Add(new Product {Name = productName});
		}

		private IList<Product> _retrievedProducts;

		public void RetrieveProductsForUser()
		{
			_retrievedProducts = Hd.Portal.Request.RetrieveProducts(_products.ToArray(), _requester);
		}

		public void RetrievedProductsAre(Strings expectedProducts)
		{
			Assert.That(_retrievedProducts.Count, Is.EqualTo(expectedProducts.Values.Length));

			foreach (var productName in expectedProducts.Values)
			{
				var name = productName;
				Assert.That(_retrievedProducts.First(x => x.Name == name), Is.Not.Null);
			}
		}
	}
}