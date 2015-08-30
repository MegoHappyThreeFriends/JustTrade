using System.Web.Mvc;
using JustTrade.Tools;
using JustTrade.Tools.Attributes;

namespace JustTrade.Controllers
{
	public class AccessLogController : ControllerWithTools
	{
		[FreeAccess]
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
	}
}