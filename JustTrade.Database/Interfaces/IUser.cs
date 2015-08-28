using System.Collections.Generic;

namespace JustTrade.Database.Interfaces
{
	public interface IUser
	{
		void Add(User user);
		void Update(User user);
		void Remove(User user);
		ICollection<User> GetAll();
	}
}
