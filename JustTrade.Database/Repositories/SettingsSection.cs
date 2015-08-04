namespace JustTrade.Database
{
	using System;
	using System.Collections.Generic;

	public class SettingsSection
	{
		IList<Setting> _settings = new List<Setting>();

		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList<Setting> Settings
		{
			get { return _settings; }

			set { _settings = value; }
		}

	}
}
