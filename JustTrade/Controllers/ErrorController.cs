using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustTrade.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Tools;

	public class ErrorController : ControllerWithTools
	{
		[HttpGet]
		public ActionResult Index() {
			return View("Index");
		}

		[HttpGet]
		public ActionResult Permission() {
			return View("Permission");
		}

	}
}