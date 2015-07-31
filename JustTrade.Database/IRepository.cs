namespace JustTrade.Database
{
	using System;
	using System.Collections.Generic;

	public interface IRepository
	{
		ResultCollection<T> Find<T>(params RepoFiler[] filter);
		ResultCollection<T> FindById<T>(Guid id);
		void Add<T>(IEnumerable<T> items);
		void Add<T>(T item);
		void Update<T>(IEnumerable<T> items);
		void Update<T>(T item);
		void Remove<T>(IEnumerable<T> items);
		void Remove<T>(T item);
	}
}
