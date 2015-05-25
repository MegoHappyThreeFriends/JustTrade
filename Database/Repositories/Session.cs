using System;
using System.Collections.Generic;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;

namespace JustTrade.Database
{
	public class Session
	{
		private User _user;

		void OnCreated(){
		}

		public Session(){
			OnCreated ();
		}

		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime SignUp { get ; set; }
		public virtual DateTime LastActivity { get ; set; }
		public virtual User User { 
			get { return _user; }
			set { _user = value; }
		}
	}

}
