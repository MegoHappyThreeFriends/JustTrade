using System.Net;

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
			if (login.IsNullOrEmptyValue() || password.IsNullOrEmptyValue()) {
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
				if (user.Password == password.GetHashPassword())
				{
					string ipAddress = GetInternalIp();
					// allow localhost and test
					if (!(ipAddress == "127.0.0.1" || ipAddress == "::1" || ipAddress == null))
					{
						var ipAddressList = user.AllowIPAdress.ParceIpAddresses();
						if (!ipAddressList.Contains(IPAddress.Parse(ipAddress)))
						{
							return GenerateErrorMessage(Lang.Get("No entry for your ip"), string.Empty);
						}
					}
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

		private string GetInternalIp()
		{
			if (Request == null)
			{
				return null;
			}
			string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (!string.IsNullOrEmpty(ipAddress))
			{
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0)
				{
					return addresses[0];
				}
			}
			return Request.ServerVariables["REMOTE_ADDR"];
		}


	}

}