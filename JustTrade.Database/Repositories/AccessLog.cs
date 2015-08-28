namespace JustTrade.Database
{
	using System;
	using JustTrade.Database.Interfaces;

	public class AccessLog : IBaseEntity
	{
		private User _user;

		public virtual Guid Id {
			get;
			set;
		}

		public virtual DateTime Time {
			get;
			set;
		}

		public virtual string Action {
			get;
			set;
		}

		public virtual string Type {
			get;
			set;
		}

		public virtual string Data {
			get;
			set;
		}

		public virtual Guid Reference {
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
	}
}
