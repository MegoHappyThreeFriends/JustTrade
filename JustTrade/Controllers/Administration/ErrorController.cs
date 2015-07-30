namespace JustTrade.Controllers.Administration
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