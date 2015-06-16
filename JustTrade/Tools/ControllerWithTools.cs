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
			TempData["Message"] = new Message(Lang.Get("Error"), ex.Message, ex.StackTrace, true);
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateErrorMessage(string message, Exception ex) {
			TempData["Message"] = new Message(Lang.Get("Error"), ex.Message, ex.StackTrace, true);
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateErrorMessage(string message, string description) {
			TempData["Message"] = new Message(Lang.Get("Error"), message, description, true);
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateInformMessage(string message, string description) {
			TempData["Message"] = new Message(Lang.Get("Information"), message, description);
			return RedirectToAction("Index", "Message");
		}

		protected ActionResult GenerateConfirmMessage(string message, string description, string urlOnYes, string data) {
			TempData["Message"] = new Message(Lang.Get("Information"), message, description, false, Message.Button.YesNo);
			TempData["RequestUrlOnYes"] = urlOnYes;
			TempData["RequestUrlOnYesData"] = data;
			return RedirectToAction("Index", "Message");
		}

		#endregion

	}
}