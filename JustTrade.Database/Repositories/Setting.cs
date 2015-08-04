namespace JustTrade.Database
{
	using System;

	public class Setting
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
