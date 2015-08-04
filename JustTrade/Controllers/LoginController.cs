namespace JustTrade.Controllers
{
	using System;
	using System.Linq;
	using System.Web.Mvc;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tools;
	using JustTrade.Tools.Attributes;
	using JustTrade.Tools.Security;

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
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) {
				return GenerateErrorMessage(Lang.Get("Login or password is incorrect"), string.Empty);
			}
			try {
				using (var users = 
					JTSecurity.Session.Db.Find<User>(new RepoFiler("Login", login))) {
					user = users.FirstOrDefault();
				}
			} catch (Exception ex) {
				return GenerateErrorMessage(Lang.Get("Error connect to db"), ex);
			}
			if (user != null) {
				if (user.Password == password.GetHashPassword()) {
					try {
						JTSecurity.CreateSession(user);
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