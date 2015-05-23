using System;

namespace JustTrade.Database
{
	public class Permition
	{
		public Permition ()
		{
		}

		public virtual Guid PermitionId { get; set;}
		public virtual string Name { get; set;}
		public virtual User UserByPermition { get; set;}
	}
}

