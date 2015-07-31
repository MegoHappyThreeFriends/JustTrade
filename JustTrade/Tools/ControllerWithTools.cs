using System;

namespace JustTrade.Tools
{
	using System.Web.Mvc;
	using JustTrade.Helpers;
	using JustTrade.Models;

	public class ControllerWithTools : Controller
	{

		#region Message

		protected ActionResult GenerateErrorMessage(Exception ex) {
			var message = new Message(Lang.Get("Error"), ex.Message, ex.StackTrace, true);
			return RedirectToAction("Index", "Message", new { message });
		}

		protected ActionResult GenerateErrorMessage(string message, Exception ex) {
			var messageObj = new Message(Lang.Get("Error"), ex.Message, ex.StackTrace, true);
			return RedirectToAction("Index", "Message", new { @message = messageObj });
		}

		protected ActionResult GenerateErrorMessage(string message, string description) {
			var messageObj = new Message(Lang.Get("Error"), message, description, true);
			return RedirectToAction("Index", "Message", new { @message = messageObj });
		}

		protected ActionResult GenerateInformMessage(string message, string description) {
			var messageObj = new Message(Lang.Get("Information"), message, description);
			return RedirectToAction("Index", "Message", new { @message = messageObj });
		}

		#endregion

	}
}