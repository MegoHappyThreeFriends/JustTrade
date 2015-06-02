using System;
using JustTrade.Database;
using System.Web;

namespace JastTrade
{

	/*public static class SessionExtention
	{
		public static User CurrentUser(this HttpSessionState session) {
			//var currentSession = HttpContext.Current.Session;
			var siteSession = (Session)(session["session"]);
			if (siteSession == null) {
				return null;
			}
			return siteSession.User;
		}
	}*/

	public static class UserSession
	{
		public static User CurrentUser {
			get {
				var currentSession = HttpContext.Current.Session;
				var session = (Session)(currentSession["session"]);
				if (session == null) {
					return null;
				}
				return session.User;
			}
		}

		public static void CreateSession(User user) {
			Session session = new Session();
			session.User = user;
			session.SignUp = DateTime.UtcNow;
			var currentSession = HttpContext.Current.Session;
			currentSession["session"] = session;
		}

	}
}

