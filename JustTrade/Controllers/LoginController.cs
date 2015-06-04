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
	using JustTrade.Helpers;
	using JustTrade.Models;

	public class LoginController : ControllerWithTools
	{
		//
		// GET: /Login/
		public ActionResult Index() {
			return View();
		}

		public ActionResult Login(string login, string password) {
			User user;
			try {
				user = Repository<User>.FindBy("Login", login);
			} catch (Exception ex) {
				return GenerateErrorMessage(Lang.Get("Error connect to db"), ex);
			}
			if (user != null) {
				if (user.Password == password) {
					try {
						UserSession.CreateSession(user);
					} catch (Exception ex) {
						return GenerateErrorMessage(Lang.Get("Error create session"), ex);
					}
					return new EmptyResult();
				}
			}
			return GenerateErrorMessage(Lang.Get("Login or password is incorrect"), string.Empty);
		}

	}

}