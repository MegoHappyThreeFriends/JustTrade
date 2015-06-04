using System;
using JustTrade.Database;
using System.Web;

namespace JastTrade
{

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
			var session = new Session {
				User = user,
				SignUp = DateTime.UtcNow
			};
			Repository<Session>.Add(session);
			var currentSession = HttpContext.Current.Session;
			currentSession["session"] = session;
		}
	}
}

