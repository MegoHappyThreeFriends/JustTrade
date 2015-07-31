﻿namespace JustTrade.Database
{
	using System;
	using NHibernate;
	using System.Collections.Generic;
	using NHibernate.Criterion;
	using System.Collections.ObjectModel;
	using System.Linq;

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

	public class ResultCollection<T> : Collection<T>, IDisposable
	{
		private bool _isDisposed;
		private ISession _session;

		public ResultCollection(IList<T> list, ISession session)
			: base(list) {
			_session = session;
		}

		public void Dispose() {
			if (!_isDisposed) {
				_isDisposed = true;
				if (_session != null) {
					if (_session.IsOpen) {
						_session.Clear();
					}
					_session.Dispose();
				}
			}
		}
	}

	public class Repository : IRepository
	{

		private void GenerateExpression(IEnumerable<RepoFiler> filters, ref ICriteria criteria) {
			foreach (var repoFiler in filters) {
				switch (repoFiler.Expr) {
					case RepoFilerExpr.In:
						var data = ((Array)repoFiler.Value).Cast<object>().ToArray();
						criteria.Add(Restrictions.In(repoFiler.Name, data));
						break;
					case RepoFilerExpr.Like:
						criteria.Add(Restrictions.Like(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Lt:
						criteria.Add(Restrictions.Lt(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Le:
						criteria.Add(Restrictions.Le(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.IsNull:
						criteria.Add(Restrictions.IsNull(repoFiler.Name));
						break;
					case RepoFilerExpr.IsNotNull:
						criteria.Add(Restrictions.IsNotNull(repoFiler.Name));
						break;
					case RepoFilerExpr.IsNotEmpty:
						criteria.Add(Restrictions.IsNotEmpty(repoFiler.Name));
						break;
					case RepoFilerExpr.IsEmpty:
						criteria.Add(Restrictions.IsEmpty(repoFiler.Name));
						break;
					case RepoFilerExpr.InsensitiveLike:
						criteria.Add(Restrictions.InsensitiveLike(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Ge:
						criteria.Add(Restrictions.Ge(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Gt:
						criteria.Add(Restrictions.Gt(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.Eq:
						criteria.Add(Restrictions.Eq(repoFiler.Name, repoFiler.Value));
						break;
					case RepoFilerExpr.NotEq:
						criteria.Add(Restrictions.Not(Restrictions.Eq(repoFiler.Name, repoFiler.Value)));
						break;
					default:
						throw new Exception("Incorrect expression !");
				}
			}
		}

		public ResultCollection<T> Find<T>(params RepoFiler[] filter) {
			ISession session = NHibernateHelper.OpenSession();
			using (ITransaction transaction = session.BeginTransaction()) {
				ICriteria criteria = session.CreateCriteria(typeof(T));
				GenerateExpression(filter, ref criteria);
				IList<T> matchingObjects = criteria.List<T>();
				transaction.Commit();
				return new ResultCollection<T>(matchingObjects, session);
			}
		}

		public ResultCollection<T> FindById<T>(Guid id) {
			return Find<T>(new RepoFiler("id", id));
		}

		public void Add<T>(IEnumerable<T> items) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				foreach (var item in items) {
					session.SaveOrUpdate(item);
				}
				transaction.Commit();
			}
		}

		public void Add<T>(T item) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				session.SaveOrUpdate(item);
				transaction.Commit();
			}
		}

		public void Update<T>(IEnumerable<T> items) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				foreach (var item in items) {
					session.Update(item);
				}
				transaction.Commit();
			}
		}

		public void Update<T>(T item) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				session.Update(item);
				transaction.Commit();
			}
		}

		public void Remove<T>(IEnumerable<T> items) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				foreach (var item in items) {
					session.Delete(item);
				}
				transaction.Commit();
			}
		}

		public void Remove<T>(T item) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				session.Delete(item);
				transaction.Commit();
			}
		}

	}
}
