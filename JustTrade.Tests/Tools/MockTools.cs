namespace JustTrade.Tests.Tools
{
	using System;
	using System.Collections.Generic;
	using JustTrade.Helpers;
	using Moq;
	using NHibernate;
	using NUnit.Framework;

	static class MockTools
	{

		#region Extend: ISession

		public static Mock<ICriteria> SetupCriteria(this Mock<ISession> sessionMock) {
			var criteria = new Mock<ICriteria>();
			sessionMock.Setup(x => x.CreateCriteria(It.IsAny<Type>())).Returns(criteria.Object);
			return criteria;
		}

		#endregion

		public static Mock<ISession> CreateSessionMock() {
			var session = new Mock<ISession>();
			var transaction = new Mock<ITransaction>();
			session.Setup(x => x.BeginTransaction()).Returns(transaction.Object);
			return session;
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
