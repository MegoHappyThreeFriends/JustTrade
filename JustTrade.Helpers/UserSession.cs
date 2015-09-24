namespace JustTrade.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JustTrade.Database;
	using JustTrade.Helpers.Interfaces;

	public class UserSession
	{
		public UserSession(User user, HashSet<string> permissionList) {
			User = user;
			PermissionList = permissionList;
		}

		public User User {
			get;
			private set;
		}

		public HashSet<string> PermissionList {
			get;
			private set;
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
				return _repository ?? (_repository = new Repository(User));
			}
			internal set {
				_repository = value;
			}
		}

		private Dictionary<string, Dictionary<string, string>> sysSettingsMock = null;
		internal void MockSysSettings(Dictionary<string, Dictionary<string, string>> dictionary) {
			sysSettingsMock = dictionary;
		}

		public T GetSysSettings<T>(string section, string name) {
			string value;
			if (sysSettingsMock != null) {
				value = sysSettingsMock[section][name];
			} else {
				using (var settingsList = _repository.Find<Settings>(new RepoFiler("Name", name))) {
					var settings = settingsList.FirstOrDefault(x => x.Section.Name == section);
					if (settings == null) {
						throw new Exception(Lang.Get("Settings not found"));
					}
					value = settings.Value;
				}
			}

			return (T)Convert.ChangeType(value, typeof(T));
		}

	}

}
