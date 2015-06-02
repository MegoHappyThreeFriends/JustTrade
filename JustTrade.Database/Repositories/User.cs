using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Linq;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;


namespace JustTrade.Database
{

	public interface iUser
	{
		void Add (User user);
		void Update (User user);
		void Remove (User user);
		ICollection<User> GetAll();
	}

	public class User 
	{
		IList<Session> _sessions = new List<Session>();
		IList<Permition> _permition = new List<Permition>();

		void OnCreated(){
		}

		public User() {
			OnCreated();
		}

		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Login { get; set; }
		public virtual string Password { get; set; }
		public virtual bool IsSuperuser { get; set; }

		public virtual IList<Permition> Permitions { 
			get{ return _permition; }

			set{ _permition = value; }
		}

		public virtual IList<Session> Sessions { 
			get{ return _sessions; }

			set{ _sessions = value; }
		}

		public override bool Equals(object obj)
	    {
	        User item = obj as User;
	        if (item != null && item.Id == Id && item.IsSuperuser == IsSuperuser && 
                item.Login == Login && item.Name == Name && item.Password == Password)
	        {
	            return true;
	        }
	        return false;
	    }
	}

}

