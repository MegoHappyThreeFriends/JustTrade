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
			var messageObj = new Message(Lang.Get("Error"), ex.Message, ex.StackTrace, true);
			return RedirectToAction("Index", "Message", new {
				message = messageObj.Serialize()
			});
		}

		protected ActionResult GenerateErrorMessage(string msg, Exception ex) {
			var messageObj = new Message(Lang.Get("Error"), msg, ex.Message + "\n" + ex.StackTrace, true);
			return RedirectToAction("Index", "Message", new {
				message = messageObj.Serialize()
			});
		}

		protected ActionResult GenerateErrorMessage(string msg, string description) {
			var messageObj = new Message(Lang.Get("Error"), msg, description, true);
			return RedirectToAction("Index", "Message", new {
				message = messageObj.Serialize()
			});
		}

		protected ActionResult GenerateInformMessage(string msg, string description) {
			var messageObj = new Message(Lang.Get("Information"), msg, description);
			return RedirectToAction("Index", "Message", new {
				message = messageObj.Serialize()
			});
		}

		#endregion

	}
}