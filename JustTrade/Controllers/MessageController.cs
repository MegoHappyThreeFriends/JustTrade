using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustTrade.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Models;

	public class MessageController : Controller
	{
		public ActionResult Index() {
			var message = (Message)TempData["Message"];
			return View(message);
		}


	}
}