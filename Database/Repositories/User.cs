using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Linq;
using System.Linq;


namespace Database
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
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Password { get; set; }
	}

	public class UserRepository : iUser
	{
		public void Add(User user)
		{
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Save(user);
				transaction.Commit();
			}
		}

		public void Update(User user)
		{
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Update (user);
				transaction.Commit();
			}
		}

		public void Remove(User user)
		{
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Delete (user);
				transaction.Commit();
			}
		}

		public ICollection<User> GetAll()
		{
			ICollection<User> list;
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				list = session.Query<User>().ToList();
				transaction.Commit();
			}
			return list;
		}
	}
}

