namespace JustTrade.Database
{
	using System;
	using System.ComponentModel.Design;
	using System.Text;
	using JustTrade.Database.Interfaces;
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

	public class RepoFiler : IEquatable<RepoFiler>
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

		public bool Equals(RepoFiler other) {
			if (Name == other.Name && Value == other.Value && Expr == other.Expr) {
				return true;
			}
			return false;
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
		private enum AccessType
		{
			Add,
			Update,
			Remove
		}

		private readonly User _currentUser;

		public Repository(User user) {
			_currentUser = user;
		}

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
			if (typeof(T).GetProperties().Any(x => x.Name == "Deleted")) {
				if (filter.All(x => x.Name != "Deleted")) {
					Array.Resize(ref filter, filter.Length + 1);
					filter[filter.Length - 1] = new RepoFiler("Deleted", false);
				}
			}
			ISession session = NHibernateHelper.OpenSession();
			using (ITransaction transaction = session.BeginTransaction()) {
				ICriteria criteria = session.CreateCriteria(typeof(T));
				GenerateExpression(filter, ref criteria);
				IList<T> matchingObjects = criteria.List<T>();
				transaction.Commit();
				return new ResultCollection<T>(matchingObjects, session);
			}
		}

		public ResultCollection<T> FindById<T>(Guid id, bool selectAll = false) {
			var repoFilterList = new List<RepoFiler>();
			if (selectAll) {
				repoFilterList.Add(new RepoFiler("Deleted", new[] { true, false }, RepoFilerExpr.In));
			}
			repoFilterList.Add(new RepoFiler("id", id));
			return Find<T>(repoFilterList.ToArray());
		}

		public void AddList<T>(IEnumerable<T> items) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				foreach (var item in items) {
					session.Save(item);
				}
				transaction.Commit();
			}
			foreach (var item in items) {
				AddToAccessLog(item, AccessType.Add);
			}
		}

		public void Add<T>(T item) {
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				session.Save(item);
				transaction.Commit();
			}
			AddToAccessLog(item, AccessType.Add);
		}

		public void UpdateList<T>(IEnumerable<T> items) {
			foreach (var item in items) {
				AddToAccessLog(item, AccessType.Update);
			}
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				foreach (var item in items) {
					session.Update(item);
				}
				transaction.Commit();
			}
		}

		public void Update<T>(T item) {
			AddToAccessLog(item, AccessType.Update);
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				session.Update(item);
				transaction.Commit();
			}
		}

		public void RemoveList<T>(IEnumerable<T> items, bool finaly = false) {
			var itemsArray = items as T[] ?? items.ToArray();
			if (itemsArray.First() is IEntityWithDeleted) {
				foreach (var item in itemsArray) {
					((IEntityWithDeleted)item).Deleted = true;
					AddToAccessLog(item, AccessType.Update);
				}
			} else {
				foreach (var item in itemsArray) {
					AddToAccessLog(item, AccessType.Remove);
				}
			}
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				if (itemsArray.First() is IEntityWithDeleted) {
					foreach (var item in itemsArray) {
						((IEntityWithDeleted)item).Deleted = true;
						session.Update(item);
					}
				} else {
					foreach (var item in itemsArray) {
						session.Delete(item);
					}
				}
				transaction.Commit();
			}
		}

		public void Remove<T>(T item, bool finaly = false) {
			if (item is IEntityWithDeleted) {
				((IEntityWithDeleted)item).Deleted = true;
				AddToAccessLog(item, AccessType.Update);
			} else {
				AddToAccessLog(item, AccessType.Remove);
			}
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				if (item is IEntityWithDeleted) {
					((IEntityWithDeleted)item).Deleted = true;
					session.Update(item);
				} else {
					session.Delete(item);
				}
				transaction.Commit();
			}
		}

		private void AddToAccessLog<T>(T item, AccessType accessType) {
			var filter = new List<RepoFiler>();
			filter.Add(new RepoFiler("id", ((IEntityDefault)item).Id));
			IEntityDefault selectedItem = null;
			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction()) {
				ICriteria criteria = session.CreateCriteria(typeof(T));
				GenerateExpression(filter, ref criteria);
				IList<T> matchingObjects = criteria.List<T>();
				if (matchingObjects.Any()) {
					selectedItem = (IEntityDefault)matchingObjects.First();
				}
				transaction.Commit();
			}

			if (accessType == AccessType.Add) {
				var accessLog = new AccessLog() {
					Reference = ((IEntityDefault)item).Id,
					Time = DateTime.Now,
					Type = typeof(T).Name,
					User = _currentUser,
					Data = Newtonsoft.Json.JsonConvert.SerializeObject(item),
					Action = "Add"
				};
				using (ISession session = NHibernateHelper.OpenSession())
				using (ITransaction transaction = session.BeginTransaction()) {
					session.Save(accessLog);
					transaction.Commit();
				}
			} else {
				if (selectedItem == null) {
					throw new Exception("Error insert to AccessLog data. Can't extract entity data.");
				}
				var accessLog = new AccessLog() {
					Reference = selectedItem.Id,
					Time = DateTime.Now,
					Type = typeof(T).Name,
					User = _currentUser,
					Data = (
						accessType == AccessType.Remove ?
						Newtonsoft.Json.JsonConvert.SerializeObject(item) :
						GetDiff(selectedItem, item)
						),
					Action = accessType.ToString()
				};
				using (ISession session = NHibernateHelper.OpenSession())
				using (ITransaction transaction = session.BeginTransaction()) {
					session.Save(accessLog);
					transaction.Commit();
				}
			}
		}

		public string GetDiff(object old, object newo) {
			var builder = new StringBuilder();
			builder.Append("{");
			foreach (var propertyInfoFirst in old.GetType().GetProperties()) {
				var firstValue = propertyInfoFirst.GetValue(old);
				if (!(firstValue is ValueType || firstValue is string || firstValue is Guid)) {
					continue;
				}
				var secondProperty = newo.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyInfoFirst.Name);
				if (secondProperty == null) {
					continue;
				}
				var secondValue = secondProperty.GetValue(newo);
				if (!secondValue.Equals(firstValue)) {
					builder.Append(string.Format("{2}\"{0}\":\"{1}\"", propertyInfoFirst.Name, secondValue,
						(builder.Length > 3 ? "," : string.Empty)));
				}
			}
			builder.Append("}");
			return builder.ToString();
		}

	}
}

