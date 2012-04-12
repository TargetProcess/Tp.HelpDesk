using Hd.Portal;
using NUnit.Framework;

namespace Hd.Tests
{
	[TestFixture]
	public class GeneralUserTest
	{
		[Test]
		public void ShouldRemoveEmailFromFirstName()
		{
			var user = new GeneralUser {FirstName = "user@mail.com"};
			Assert.AreEqual("user", user.FullName);
		}

		[Test]
		public void ShouldRemoveEmailFromLastName()
		{
			var user = new GeneralUser {LastName = "user@mail.com"};
			Assert.AreEqual("user", user.FullName);
		}

		[Test]
		public void ShouldRemoveAllAfterFirstEmailFromLastName()
		{
			var user = new GeneralUser
			{
				LastName = "user@mail.com user2@secondmail.com"
			};
			Assert.AreEqual("user", user.FullName);
		}

		[Test]
		public void ShouldRemoveEmailFromBothFirstAndLastName()
		{
			var user = new GeneralUser
			{
				FirstName = "user1@mail.com",
				LastName = "user2@secondmail.com"
			};
			Assert.AreEqual("user1 user2", user.FullName);
		}
	}
}
