
namespace JustTrade.Tests.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Models;
	using NUnit.Framework;

	public class BaseTests
	{
		protected void CheckRedirectToMessageWithError(ActionResult result) {
			if (result is RedirectToRouteResult) {
				var routeValues = ((RedirectToRouteResult)result).RouteValues;
				var message = (Message)routeValues["message"];
				Assert.IsTrue(routeValues["controller"].Equals("Message") && message.Caption.Equals("Error"));
			} else {
				Assert.Fail("ActionResult is not redirect");
			}
		}
	}
}
