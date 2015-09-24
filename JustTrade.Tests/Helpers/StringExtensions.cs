namespace JustTrade.Tests.Helpers
{
	using JustTrade.Helpers.ExtensionMethods;
	using NUnit.Framework;

	[TestFixture]
	class StringExtensionsTests
	{
		[Test]
		public void GetHashPassword_ReturnCorrectValue() {
			const string testPassword = "Sergey Ratkin";
			var pass = testPassword.GetHashPassword();
			Assert.AreEqual(pass, "bc533e0726e2a9efad53c39cdc27dfdc");
		}
	}
}
