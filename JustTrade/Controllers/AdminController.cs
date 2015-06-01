using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using JustTrade.Database;
using System.Text;
using System.Threading;
using JustTrade.Tools;

namespace JastTrade.Controllers
{
	public class AdminController : Controller
	{

		public ActionResult Index()
		{
			var d = HttpContext;
			var u = UserSession.CurrentUser;
			return View();
		}

		public ActionResult Database()
		{
			return PartialView();
		}

		public ActionResult GenerateDatabase()
		{
			try
			{
				NHibernateHelper.CreateDb();
			}
			catch (Exception ex)
			{
				return Json(JsonData.Create(ex), JsonRequestBehavior.AllowGet);
			}
			return Json(JsonData.Create(true,"Database created"), JsonRequestBehavior.AllowGet);
		}

	}
}
