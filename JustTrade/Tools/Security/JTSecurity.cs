namespace JustTrade.Tools.Security
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using JustTrade.Database;
	using JustTrade.Helpers;
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

		private IMail _mail;
		public IMail Mail {
			get {
				if (_mail == null) {
					_mail = new Mail();
				}
				return _mail;
			}
			internal set {
				_mail = value;
			}
		}

		private IRepository _repository;
		public IRepository Db {
			get {
				return _repository ?? (_repository = new Repository());
			}
			internal set {
				_repository = value;
			}
		}
	}

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
					session = new UserSession() {
						User = null,
						PermissionList = null
					};
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
			var newSession = new UserSession() {
				User = user,
				PermissionList = permissionList
			};
			var currentSession = HttpContext.Current.Session;
			currentSession["session"] = newSession;
		}
	}
}

