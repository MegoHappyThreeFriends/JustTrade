namespace JustTrade.Database
{
	using System;
	using System.Collections.Generic;

	public interface IRepository
	{
		ResultCollection<T> Find<T>(params RepoFiler[] filter);
		ResultCollection<T> FindById<T>(Guid id, bool selectAll = false);
		void AddList<T>(IEnumerable<T> items);
		void Add<T>(T item);
		void UpdateList<T>(IEnumerable<T> items);
		void Update<T>(T item);
		void RemoveList<T>(IEnumerable<T> items, bool finaly = false);
		void Remove<T>(T item, bool finaly = false);
	}
}
