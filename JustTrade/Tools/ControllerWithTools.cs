using System;
using System.Web.Routing;

namespace JustTrade.Tools
{
	using System.Web.Mvc;
	using JustTrade.Helpers;
	using JustTrade.Models;

	public class ControllerWithTools : Controller
	{

		#region Message

		protected ActionResult GenerateErrorMessage(Exception ex) {
			var messageObj = new Message(Lang.Get("Error"), ex.Message, ex.StackTrace, true);
			TempData["message"] = messageObj;
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateErrorMessage(string msg, Exception ex) {
			var messageObj = new Message(Lang.Get("Error"), msg, ex.Message + "\n" + ex.StackTrace, true);
			TempData["message"] = messageObj;
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateErrorMessage(string msg, string description) {
			var messageObj = new Message(Lang.Get("Error"), msg, description, true);
			TempData["message"] = messageObj;
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateInformMessage(string msg, string description) {
			var messageObj = new Message(Lang.Get("Information"), msg, description);
			TempData["message"] = messageObj;
			return RedirectToAction("Index", "Message");
		}

		#endregion

	}
}