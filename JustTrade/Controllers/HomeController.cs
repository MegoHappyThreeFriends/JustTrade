[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("JustTrade.Tests")]
namespace JustTrade.Controllers
{
	using System;
	using System.Web.Mvc;
	using JastTrade;

	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index() {
			if (JustTradeSecurity.CurrentSession == null) {
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

