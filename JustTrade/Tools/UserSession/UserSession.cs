using System;
using JustTrade.Database;
using System.Web;

namespace JastTrade
{
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json.Linq;

	public class ActUserSession
	{
		public User  User {
			get;
			set;
		}

		public List<string> PermissionList {
			get;
			set;
		}
	}

	public static class UserSession
	{
		public static User CurrentUser {
			get {
				var currentSession = HttpContext.Current.Session;
				var session = (ActUserSession)(currentSession["session"]);
				if (session == null) {
					return null;
				}
				return session.User;
			}
		}

		public static ActUserSession CurrentSession {
			get {
				var currentSession = HttpContext.Current.Session;
				var session = (ActUserSession)(currentSession["session"]);
				if (session == null) {
					return null;
				}
				return session;
			}
		}

		public static void CreateSession(User user) {
			var session = new Session {
				User = user,
				SignUp = DateTime.UtcNow
			};

			var permissionList = new List<string>();
			using (var users = Repository<User>.FindById(user.Id)) {
				var permissionBindings = users.First().UserPermissionBindings;
				foreach (var userPermissionBinding in permissionBindings) {
					if (!string.IsNullOrEmpty(userPermissionBinding.PermissionTemplate.PermissionRules)) {
						permissionList.AddRange(
							JArray.Parse(userPermissionBinding.PermissionTemplate.PermissionRules).ToObject<string[]>().ToArray());
						permissionList = permissionList.Distinct().ToList();
					}
				}
			}

			var nsession = new ActUserSession() {
				User = user,
				PermissionList = permissionList
			};
			
			Repository<Session>.Add(session);
			var currentSession = HttpContext.Current.Session;
			currentSession["session"] = nsession;
		}
	}
}

