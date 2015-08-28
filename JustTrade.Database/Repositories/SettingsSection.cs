using System.Linq;

namespace JustTrade.Database
{
	using System;
	using System.Collections.Generic;
	using JustTrade.Database.Interfaces;

	public class SettingsSection : IEntityDefault
	{
		IList<Settings> _settings = new List<Settings>();

		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList<Settings> Settings
		{
			get { return _settings; }

			set { _settings = value; }
		}
		
	}
}
