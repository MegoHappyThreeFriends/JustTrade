﻿namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Web.Mvc;
	using JustTrade.Helpers;
	using JustTrade.Models;
	using JustTrade.Tools;
	using JustTrade.Tools.Attributes;
	using JustTrade.Tools.Security;

	public class MessageController : ControllerWithTools
	{
		[FreeAccess]
		[HttpGet]
		public ActionResult Index(string message) {
			var msg = Message.Deserialize(message);
			return PartialView("../Administrator/Message/_Index", msg);
		}

		[FreeAccess]
		[HttpGet]
		public ActionResult SendReport(string head, string body) {
			try {
				JTSecurity.Session.Mail.Send("JastTrage@gmail.com", head, body);
			} catch (Exception ex) {
				return GenerateErrorMessage(Lang.Get("Error send mail"), ex);
			}
			return new EmptyResult();
		}

	}
}