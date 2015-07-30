namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Web.Mvc;
	using JustTrade.Database;
	using JustTrade.Helpers;

	public class AdminController : Controller
	{

		[HttpGet]
		public ActionResult Index() {
			return View();
		}

		[HttpGet]
		public ActionResult Database() {
			return PartialView();
		}

		[HttpGet]
		public ActionResult GenerateDatabase() {
			try {
				NHibernateHelper.CreateDb();
			} catch (Exception ex) {
				ViewBag.Header = Lang.Get("Error generate database");
				ViewBag.Description = ex.ToString();
				return PartialView("_ErrorGenerateDatabase");
			}
			return PartialView("_SuccessGeneratedDatabase");
		}

	}
}
