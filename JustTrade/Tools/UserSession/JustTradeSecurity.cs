using JustTrade.Database;
using System.Web;

namespace JastTrade
{
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json.Linq;

	public class UserSession
	{
		public User User {
			get;
			set;
		}

		public HashSet<string> PermissionList {
			get;
			set;
		}
	}

	public static class JustTradeSecurity
	{
		public static bool AccessIsAllowed(string permission) {
			var currentSession = HttpContext.Current.Session;
			var session = (UserSession)(currentSession["session"]);
			if (session == null) {
				return false;
			}
			return session.PermissionList.Contains(permission);
		}

		public static UserSession CurrentSession {
			get {
				var currentSession = HttpContext.Current.Session;
				var session = (UserSession)(currentSession["session"]);
				if (session == null) {
					return null;
				}
				return session;
			}
		}

		public static void CreateSession(User user) {
			var permissionList = new HashSet<string>();
			using (var users = Repository<User>.FindById(user.Id)) {
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
			var nsession = new UserSession() {
				User = user,
				PermissionList = permissionList
			};
			var currentSession = HttpContext.Current.Session;
			currentSession["session"] = nsession;
		}
	}
}

