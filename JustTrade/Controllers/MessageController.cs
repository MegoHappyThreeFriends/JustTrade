namespace JustTrade.Controllers
{
	using System.Web.Mvc;
	using JustTrade.Models;
	using JustTrade.Tools.Attributes;

	public class MessageController : Controller
	{
		[FreeAccess]
		[HttpGet]
		public ActionResult Index() {
			var message = (Message)TempData["Message"];
			return PartialView(message);
		}

		[FreeAccess]
		[HttpGet]
		public ActionResult SendReport() {
			//TODO: Реализовать отправку сообщений
			return new EmptyResult();
		}

	}
}