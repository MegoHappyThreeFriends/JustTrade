using System;

namespace JustTrade.Database
{
	using JustTrade.Database.Interfaces;

	public class UserPermissionBinding : IBaseEntity
	{
		private User _user;
		private PermissionTemplate _permissionTemplate;

		public UserPermissionBinding() {
		}

		public virtual Guid Id {
			get;
			set;
		}
		public virtual User User {
			get {
				return _user;
			}
			set {
				_user = value;
			}
		}

		public virtual PermissionTemplate PermissionTemplate {
			get {
				return _permissionTemplate;
			}
			set {
				_permissionTemplate = value;
			}
		}

	}

}
