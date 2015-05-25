using System;
using JustTrade.Database;
using System.Web;
using System.ServiceModel.Activation;

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
                    _session = session;
                    _user = session.User;
                }
                return _user;
            }
        }

        public static void SetSessionUser(Session session)
        {
            session.SignUp = DateTime.UtcNow;
            var currentSession = HttpContext.Current.Session;
            currentSession["session"] = session;
        }

    }
}

