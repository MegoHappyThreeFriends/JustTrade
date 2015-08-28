﻿namespace JustTrade.Database
{
	using System;
	using JustTrade.Database.Interfaces;

	public class Settings : IEntityDefault
	{
		private SettingsSection _settingsSection;

		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Type { get; set; }
		public virtual string Value { get; set; }
		public virtual SettingsSection Section
		{
			get { return _settingsSection; }
			set { _settingsSection = value; }
		}

	}
}
