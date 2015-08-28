using System;

namespace JustTrade.Tools.Security
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using Newtonsoft.Json.Linq;

	public static class JTSecurity
	{
		private static UserSession _mockUserSession;

		public static bool AccessIsAllowed(string permission) {
			var session = Session;
			if (session.PermissionList == null) {
				return false;
			}
			return session.PermissionList.Contains(permission);
		}

		public static UserSession Session {
			get {
				if (_mockUserSession != null) {
					return _mockUserSession;
				}
				var currentSession = HttpContext.Current.Session;
				var session = (UserSession)(currentSession["session"]);
				if (session == null) {
					session = new UserSession(null, null);
					currentSession["session"] = session;
				}
				return session;
			}
			internal set {
				_mockUserSession = value;
			}
		}

		public static void CreateSession(User user) {
			var permissionList = new HashSet<string>();
			using (var users = Session.Db.FindById<User>(user.Id)) {
				var permissionBindings = users.First().UserPermissionBindings;
				foreach (var userPermissionBinding in permissionBindings) {
					if (!string.IsNullOrEmpty(userPermissionBinding.PermissionTemplate.PermissionRules)) {
						var list = JArray.Parse(userPermissionBinding.PermissionTemplate.PermissionRules).ToObject<string[]>().ToList();
						list = list.Distinct().ToList();
						foreach (var item in list) {
							permissionList.Add(item);
						}
					}
				}
			}
			var newSession = new UserSession(user, permissionList);
			// For unit test
			if (HttpContext.Current != null) {
				var currentSession = HttpContext.Current.Session;
				currentSession["session"] = newSession;
			}
		}
	}
}

