using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Database;

namespace JustTrade.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";

			return View ();
		}

		public ActionResult Test()
		{
			
			if (!System.IO.File.Exists (@"C:\nhibernate.db"))
			{
				NHibernateHelper.CreateDb();
			}

			UserRepository repository = new UserRepository ();
			repository.Add(new User { Name = "Bwm", Password = "25000" });
			repository.Add(new User { Name = "Opel", Password = "20000" });
			repository.Add(new User { Name = "Ford", Password = "15000" });
			var users = repository.GetAll ();
			ViewBag.Users = users;
			return View ("Index");
		}

	}
}

