using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JastTrade.Controllers
{
	public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }

		public ActionResult Database()
		{

			return PartialView ();
		}

    }
}
