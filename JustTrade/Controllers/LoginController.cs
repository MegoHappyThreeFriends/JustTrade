using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JastTrade;
using JustTrade.Database;
using JustTrade.Tools;

namespace JustTrade.Controllers
{
	using JustTrade.Models;

	public class LoginController : Controller
	{
		//
		// GET: /Login/
		public ActionResult Index() {
			return View();
		}

		public ActionResult Login(string login, string password) {
			User user = Repository<User>.FindBy("Login", login);
			if (user != null) {
				if (user.Password == password) {
					UserSession.CreateSession(user);
					//return RedirectToAction("Index", "Home");
					return Redirect("/Home");
				} else {
					TempData["Message"] = new Message("Test caption","Mego text");
					
				}
			} else {
				//return Json(JsonData.Create(false, "User not found"), JsonRequestBehavior.AllowGet);
				TempData["Message"] = new Message("Test caption", "Mego text");
			}
			return RedirectToAction("Index", "Message");
		}

	}

}