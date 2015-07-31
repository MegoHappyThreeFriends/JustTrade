namespace JustTrade.Tests.Tools
{
	using System;
	using System.Collections.Generic;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Tools.Security;
	using Moq;

	static class MockTools
	{

		public static Mock<IRepository> SetupDb() {
			var dbMock = new Mock<IRepository>();
			var userSession = new UserSession();
			userSession.Db = dbMock.Object;
			JTSecurity.Session = userSession;
			return dbMock;
		}

		public static void SetupSysSettings(Dictionary<string, string> dictionary) {
			AppSettings.MockSettings = dictionary;
		}

		public static void SetupDefaultSysSettings() {
			var dictionary = new Dictionary<string, string>();
			dictionary.Add("lang","");
			dictionary.Add("workspace", "");
			AppSettings.MockSettings = dictionary;
		}

		public static void SetupLanguage() {
			Lang.IsMock = true;
		}

	}
}
