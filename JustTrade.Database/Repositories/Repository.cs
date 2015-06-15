using System;
using NHibernate;
using System.Collections.Generic;
using NHibernate.Linq;
using System.Linq;
using NHibernate.Criterion;

namespace JustTrade.Database
{
	
	public static class Repository <T>
	{

		public static T FindById(Guid id)
		{
			return FindBy ("id", id);
		}

		public static T FindByName(string name)
		{
			return FindBy ("Name", name);
		}

		public static T FindBy(string propertyName, object propertyValue){
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				ICriteria criteria = session.CreateCriteria(typeof(T));
				criteria.Add(Expression.Eq(propertyName, propertyValue));
				IList<T> matchingObjects = criteria.List<T>();
				transaction.Commit();
				return matchingObjects.FirstOrDefault ();
			}
		}

		public static void Add(T item)
		{
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.SaveOrUpdate(item);
				transaction.Commit();
			}
		}

		public static void Update(T item)
		{
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Update (item);
				transaction.Commit();
			}
		}

		public static void Remove(T item)
		{
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Delete (item);
				transaction.Commit();
			}
		}

		public static ICollection<T> GetAll()
		{
			ICollection<T> list;
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				ICriteria targetObjects = session.CreateCriteria(typeof(T));
				list = targetObjects.List<T>();
				transaction.Commit();
			}
			return list;
		}

	}
}

