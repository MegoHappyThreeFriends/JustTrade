namespace JustTrade.Database.Interfaces
{
	using System;

	public interface IBaseEntity
	{
		Guid Id {
			get;
			set;
		}
	}
}
