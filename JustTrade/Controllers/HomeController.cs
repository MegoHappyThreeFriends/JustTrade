using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using JastTrade;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("JustTrade.Tests")]
namespace JustTrade.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index() {
			if (UserSession.CurrentUser == null) {
				return RedirectToAction("Index", "Login");
			}

			var mvcName = typeof(Controller).Assembly.GetName();
			var isMono = Type.GetType("Mono.Runtime") != null;

			ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData["Runtime"] = isMono ? "Mono" : ".NET";

			return View();
		}
	}
}

