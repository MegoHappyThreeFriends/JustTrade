using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JustTrade.Database
{
	using JustTrade.Database.Interfaces;

	public class User : BaseEntity, IEntityWithDeleted, IBaseEntity
	{
		IList<Session> _sessions = new List<Session>();
		IList<UserPermissionBinding> _userPermissionBindings = new List<UserPermissionBinding>();

		void OnCreated() {
		}

		public User() {
			OnCreated();
		}

		public virtual Guid Id {
			get;
			set;
		}
		public virtual string Name {
			get;
			set;
		}
		public virtual string Login {
			get;
			set;
		}
		public virtual string Password {
			get;
			set;
		}
		public virtual string AllowIPAdress
		{
			get;
			set;
		}
		public virtual bool IsSuperuser {
			get;
			set;
		}
		public virtual bool Deleted {
			get;
			set;
		}

		public virtual IList<UserPermissionBinding> UserPermissionBindings {
			get {
				return _userPermissionBindings;
			}

			set {
				_userPermissionBindings = value;
			}
		}

		public virtual IList<Session> Sessions {
			get {
				return _sessions;
			}

			set {
				_sessions = value;
			}
		}

		public override bool Equals(object obj) {
			User item = obj as User;
			if (item != null && item.Id == Id && item.IsSuperuser == IsSuperuser &&
				item.Login == Login && item.Name == Name && item.Password == Password) {
				return true;
			}
			return false;
		}

	}

}

