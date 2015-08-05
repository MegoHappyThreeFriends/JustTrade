namespace JustTrade.Helpers.Interfaces
{
	using System;
	using System.Collections.Generic;
	using JustTrade.Database;

	public interface ISettings
	{
		List<SettingsSection> GetSectionList();
		Dictionary<string, List<Settings>> GetList();
		void AddSection(string name);
		void UpdateSection(Guid sectionId, string name);
		void AddSettigs(Guid sectionId, string name, string value);
		void UpdateSettigs(Guid settingsId , Guid sectionId, string name, string value);
		void RemoveSection(Guid id);
		void RemoveSettings(Guid id);
	}
}
