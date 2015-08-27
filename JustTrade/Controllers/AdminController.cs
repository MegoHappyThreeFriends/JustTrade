namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Web.Mvc;
	using Database;
	using Helpers;

	public class AdminController : Controller
	{

		[HttpGet]
		public ActionResult Index() {
			return View();
		}

	}
}
