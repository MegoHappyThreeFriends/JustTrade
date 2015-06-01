using System;
using JustTrade.Database;
using System.Web;

namespace JastTrade
{

	public static class UserSession
	{
		private static User _user;
		private static Session _session;

		public static User CurrentUser
		{
			get
			{
				if (_session == null)
				{
					var currentSession = HttpContext.Current.Session;
					var session = (Session)(currentSession["session"]==null ? null : currentSession["session"]);
					if (session == null) 
					{
						return null;
					}
					_session = session;
					_user = session.User;
				}
				return _user;
			}
		}

        public static void CreateSession(User user)
	    {
	        Session session = new Session();
	        session.User = user;
            session.SignUp = DateTime.UtcNow;
            var currentSession = HttpContext.Current.Session;
            currentSession["session"] = session;
	    }

	}
}

