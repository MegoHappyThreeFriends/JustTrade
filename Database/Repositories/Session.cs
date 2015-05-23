using System;
using System.Collections.Generic;

namespace JustTrade.Database
{
	public class Session
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual User UserBySession { get; set;}
	}
}
