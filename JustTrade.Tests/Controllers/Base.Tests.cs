
namespace JustTrade.Tests.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Models;
	using NUnit.Framework;

	public class BaseTests
	{
		protected void CheckRedirectToMessageWithError(ActionResult result, TempDataDictionary tempData) {
			if (result is RedirectToRouteResult) {
				var message = tempData["message"] as Message;
				if (message == null) {
					Assert.Fail("TempData not contained message model");
				}
				Assert.IsTrue(message.Caption.Equals("Error"));
			} else {
				Assert.Fail("ActionResult is not redirect");
			}
		}
	}
}
