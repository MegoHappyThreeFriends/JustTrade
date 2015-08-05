namespace JustTrade.Helpers
{
	using System;
	using System.Collections.Generic;
	using JustTrade.Database;
	using JustTrade.Helpers.Interfaces;

	public class JTSettings : ISettings
	{
		public List<SettingsSection> GetSectionList() {
			throw new NotImplementedException();
		}

		public Dictionary<string, List<Settings>> GetList() {
			throw new NotImplementedException();
		}

		public void AddSection(string name) {
			throw new NotImplementedException();
		}

		public void UpdateSection(Guid sectionId, string name) {
			throw new NotImplementedException();
		}

		public void AddSettigs(Guid sectionId, string name, string value) {
			throw new NotImplementedException();
		}

		public void UpdateSettigs(Guid settingsId, Guid sectionId, string name, string value) {
			throw new NotImplementedException();
		}

		public void RemoveSection(Guid id) {
			throw new NotImplementedException();
		}

		public void RemoveSettings(Guid id) {
			throw new NotImplementedException();
		}
	}
}
