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
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Models;
	using JustTrade.Tools.Attributes;

	public class LoginController : ControllerWithTools
	{
		
		[FreeAccess]
		[HttpGet]
		public ActionResult Index() {
			return View();
		}

		[FreeAccess]
		[HttpGet]
		public ActionResult Login(string login, string password) {
			User user;
			try {
				using (var users = Repository<User>.Find(new RepoFiler("Login", login))) {
					user = users.FirstOrDefault();
				}
			} catch (Exception ex) {
				return GenerateErrorMessage(Lang.Get("Error connect to db"), ex);
			}
			if (user != null) {
				if (user.Password == password.GetHashPassword()) {
					try {
						JustTradeSecurity.CreateSession(user);
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