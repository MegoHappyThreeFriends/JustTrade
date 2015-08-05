namespace JustTrade.Tests.Tools
{
	using System.Collections.Generic;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Helpers.Interfaces;
	using JustTrade.Tools.Security;
	using Moq;

	static class MockTools
	{

		public static UserSession SetupSession() {
			var userSession = new UserSession();
			JTSecurity.Session = userSession;
			return userSession;
		}

		public static Mock<IRepository> SetupDb(this UserSession session) {
			var dbMock = new Mock<IRepository>();
			session.Db = dbMock.Object;
			return dbMock;
		}

		public static Mock<IMail> SetupMail(this UserSession session) {
			var mailMock = new Mock<IMail>();
			session.Mail = mailMock.Object;
			return mailMock;
		}

		public static void SetupSysSettings(Dictionary<string, string> dictionary) {
			AppSettings.MockSettings = dictionary;
		}

		public static void SetupDefaultAppSettings() {
			var dictionary = new Dictionary<string, string> {
				{ "lang", "" }, 
				{ "workspace", "" }
			};
			AppSettings.MockSettings = dictionary;
		}

		public static void SetupLanguage() {
			Lang.IsMock = true;
		}

	}
}
