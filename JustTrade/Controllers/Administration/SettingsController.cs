using System.Web.Mvc;

namespace JustTrade.Controllers.Administration
{
	using Tools;

	public class SettingsController : ControllerWithTools
	{
		[HttpGet]
		public ActionResult Index()
		{
			return PartialView("../Administrator/Settings/_Index");
		}
	}

}