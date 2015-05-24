using System;

namespace JustTrade.Database
{
	public class Permition
	{
		private User _user;

		public Permition ()
		{
		}

		public virtual Guid Id { get; set;}
		public virtual string Name { get; set;}
		public virtual User User { 
			get { return _user; }
			set { _user = value; }
		}
	}
}

