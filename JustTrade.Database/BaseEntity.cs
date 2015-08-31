using System;

namespace JustTrade.Database
{
	public abstract class BaseEntity: ICloneable
	{
		public virtual object Clone() {
			return this.MemberwiseClone();
		}
	}
}
