using System;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace JustTrade.Database
{
	using System.Collections;

	public enum RepoFilerExpr
	{
		NotEq,
		Eq,
		Ge,
		Gt,
		In,
		InsensitiveLike,
		IsEmpty,
		IsNotEmpty,
		IsNotNull,
		IsNull,
		Le,
		Lt,
		Like
	}

	public class RepoFiler
	{

		public RepoFiler(string name, object value, RepoFilerExpr expr = RepoFilerExpr.Eq) {
			Name = name;
			Value = value;
			Expr = expr;
		}

		public string Name {
			get;
			set;
		}

		public object Value {
			get;
			set;
		}

		public RepoFilerExpr Expr {
			get;
			set;
		}
	}

	public static class Repository <T>
	{
		private static void GenerateExpression(IEnumerable<RepoFiler> filters, ref ICriteria criteria) {
			foreach (var repoFiler in filters) {
				switch (repoFiler.Expr) {
					case RepoFilerExpr.In:
						criteria.Add(Expression.In(repoFiler.Name, ((object[]) repoFiler.Value) ));
						break;
					case RepoFilerExpr.Like:
						criteria.Add(Expression.Like(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Lt:
						criteria.Add(Expression.Lt(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Le:
						criteria.Add(Expression.Le(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.IsNull:
						criteria.Add(Expression.IsNull(repoFiler.Name));
						break;
					case RepoFilerExpr.IsNotNull:
						criteria.Add(Expression.IsNotNull(repoFiler.Name));
						break;
					case RepoFilerExpr.IsNotEmpty:
						criteria.Add(Expression.IsNotEmpty(repoFiler.Name));
						break;
					case RepoFilerExpr.IsEmpty:
						criteria.Add(Expression.IsEmpty(repoFiler.Name));
						break;
					case RepoFilerExpr.InsensitiveLike:
						criteria.Add(Expression.InsensitiveLike(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Ge:
						criteria.Add(Expression.Ge(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Gt:
						criteria.Add(Expression.Gt(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Eq:
						criteria.Add(Expression.Eq(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.NotEq:
						criteria.Add(Expression.Not(Expression.Eq(repoFiler.Name, repoFiler.Value)));
						break;
					default: throw new Exception("Incorrect expression !");
				}
			}
		}

		public static ICollection<T> Find(params RepoFiler[] filter) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				ICriteria criteria = session.CreateCriteria(typeof(T));
				GenerateExpression(filter,ref criteria);
				IList<T> matchingObjects = criteria.List<T>();
				transaction.Commit();
				return matchingObjects;
			}
		}

		public static ICollection<T> FindById(Guid id)
		{
			return Find (new RepoFiler("id", id));
		}

		//public static T FindBy(string propertyName, object propertyValue){
		//	using (ISession session = NHibernateHelper.OpenSession())
		//	using (ITransaction transaction = session.BeginTransaction())
		//	{
		//		ICriteria criteria = session.CreateCriteria(typeof(T));
		//		criteria.Add(Expression.Eq(propertyName, propertyValue));
		//		IList<T> matchingObjects = criteria.List<T>();
		//		transaction.Commit();
		//		return matchingObjects.FirstOrDefault ();
		//	}
		//}

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

	}
}

