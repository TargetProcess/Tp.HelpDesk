//  
// Copyright (c) 2005-2009 TargetProcess. All rights reserved.
// TargetProcess proprietary/confidential. Use is subject to license terms. Redistribution of this file is strictly forbidden.
// 
using NBehave.Narrator.Framework;
using NBehave.Spec.Extensions;
using NUnit.Framework;

namespace Hd.Tests.Portal.Entities.Request
{
	[TestFixture]
	public class RequestSpec
	{
		[Test]
		public void ShouldRetrieveProducts()
		{
			var context = new RequestContext();

			var story = new Feature("Should retrieve products");
			story.AddScenario("Should retrieve all public and Company's products when User works for Company")
				.Given("User works for Company 1", () => context.UserWorksForCompany(1))
				.And("$productName belongs to Company $companyId", () => context.ProductBelongsToCompany("Product 1", 1))
				.And("$productName belongs to Company $companyId", () => context.ProductBelongsToCompany("Product 2", 2))
				.And("$productName is public", () => context.ProductIsPublic("Product 3"))
				.When("retrieve products for User", context.RetrieveProductsForUser)
				.Then("retrieved products are: $productNames",
				      () => context.RetrievedProductsAre(Strings.Create("Product 1", "Product 3")));

			story.AddScenario("Should retrieve only public products when User does not work for any Company")
				.Given("user does not work for any company", context.UserDoesNotWorkForAnyCompany)
				.And("$productName belongs to company $companyId", () => context.ProductBelongsToCompany("Product 1", 1))
				.And("$productName belongs to company $companyId", () => context.ProductBelongsToCompany("Product 2", 2))
				.And("$productName is public", () => context.ProductIsPublic("Product 3"))
				.When("retrieve products for User", context.RetrieveProductsForUser)
				.Then("retrieved products are: $productNames", () => context.RetrievedProductsAre(Strings.Create("Product 3")));
		}
	}
}