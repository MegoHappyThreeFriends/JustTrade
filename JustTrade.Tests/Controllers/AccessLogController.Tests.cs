namespace JustTrade.Tests.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using JustTrade.Controllers;
	using JustTrade.Database;
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tests.Tools;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	class AccessLogControllerTests : BaseTests
	{
		[Test]
		public void Index_ReturnCorrectData() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var accessLog = new AccessLog() {
				Id = Guid.NewGuid()
			};
			db.Setup(x => x.Find<AccessLog>(It.IsAny<RepoFiler>()))
				.Returns(new ResultCollection<AccessLog>(new List<AccessLog>() { accessLog }, null));
			var controller = new AccessLogController();
			var result = controller.Index(Guid.Empty) as ViewResultBase;
			Assert.IsNotNull(result);
			var firstItem = ((List<AccessLog>)result.Model).First();
			Assert.AreEqual(firstItem.Id, accessLog.Id);
		}

	}
}
