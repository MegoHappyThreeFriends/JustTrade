namespace JustTrade.Tests.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JustTrade.Controllers.Administration;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Tests.Tools;
	using Moq;
	using NHibernate;
	using NUnit.Framework;

	[TestFixture]
	public class UserControllerTests
	{
	    [Test]
		public void Add_CallAddInRepository ()
	    {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController ();
			var user = new User () {
				Login = "login",
				Name = "name",
				Password = "password"
			};
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).Returns(new ResultCollection<User>(new List<User>(), null));
		    controller.Add(user, null);
			db.Verify(x => x.Add(It.Is<User>(y=>y.Login == user.Login)), Times.Once);
		}

		[Test]
		public void Update_CallUpdateInRepository() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			var user = new User() {
				Login = "new_login",
				Name = "name",
				Password = "password"
			};
			var oldUser = new User() {
				Login = "login1",
				Name = "name",
				Password = "password"
			};
			db.Setup(x => x.FindById<User>(It.IsAny<Guid>())).Returns(new ResultCollection<User>(new List<User>() { oldUser }, null));
			controller.Update(user, null);
			db.Verify(x => x.Update(It.Is<User>(y => y.Login == user.Login)), Times.Once);
		}

		[Test]
		public void Remove_CallRemoveInRepository() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			var user = new User() {
				Login = "new_login",
				Name = "name",
				Password = "password"
			};
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).Returns(new ResultCollection<User>(new List<User>() { user, user }, null));
			controller.Remove(new []{ Guid.Empty }, true);
			db.Verify(x => x.Remove(It.Is<List<User>>(y => y[0].Login == user.Login)), Times.Once);
		}

	}
}

