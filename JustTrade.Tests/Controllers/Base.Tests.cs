
namespace JustTrade.Tests.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Models;
	using NUnit.Framework;

	public class BaseTests
	{
		protected void CheckRedirectToMessageWithError(ActionResult result, string messageText = null, string description = null) {
			if (result is RedirectToRouteResult) {
				var messageData = (string)((RedirectToRouteResult)(result)).RouteValues["message"];
				var message = Message.Deserialize(messageData);
				if (messageText != null) {
					Assert.AreEqual(message.MessageText, messageText);
					if (description != null) {
						Assert.AreEqual(message.Description, description);
					}
				}
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
