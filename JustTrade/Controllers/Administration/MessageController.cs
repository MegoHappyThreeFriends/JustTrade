namespace JustTrade.Controllers.Administration
{
	using System.Web.Mvc;
	using JustTrade.Models;
	using JustTrade.Tools.Attributes;

	public class MessageController : Controller
	{
		[FreeAccess]
		[HttpGet]
		public ActionResult Index(Message message) {
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