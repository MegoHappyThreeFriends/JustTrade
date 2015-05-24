using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using JustTrade.Database;

namespace JastTrade.Controllers
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
			
			var s = new Session ();
			s.Name = "sdfsdf";
			s.User = Repository<User>.FindByName ("Ford");
			if (s.User != null) {
				Repository<Session>.Add (s);
			}
			var session = Repository<Session>.FindByName ("sdfsdf");

			User uu;
			if (session != null) {
				uu = session.User;
			}

			//Repository<User>.Add(new User { Name = "Bwm", Password = "25000" });
			//Repository<User>.Add(new User { Name = "Opel", Password = "20000" });
			//Repository<User>.Add(new User { Name = "Ford", Password = "15000" });
			var users = Repository<User>.GetAll ();
			ViewBag.Users = users;
			return View ("Index");
		}

	}

}

