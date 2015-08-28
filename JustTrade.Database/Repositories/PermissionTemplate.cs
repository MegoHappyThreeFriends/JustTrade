using System;

namespace JustTrade.Database
{
	using System.Collections.Generic;
	using JustTrade.Database.Interfaces;

	public class PermissionTemplate : IEntityWithDeleted, IBaseEntity
	{
		IList<UserPermissionBinding> _userPermissionBindings = new List<UserPermissionBinding>();

		public PermissionTemplate() {
		}

		public virtual Guid Id {
			get;
			set;
		}
		public virtual string Name {
			get;
			set;
		}
		public virtual string PermissionRules {
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

	}
}
