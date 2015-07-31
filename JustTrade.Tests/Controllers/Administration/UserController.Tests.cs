namespace JustTrade.Tests.Controllers.Administration
{
	using Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JustTrade.Controllers.Administration;
	using Database;
	using Tools;
	using Moq;
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
		public void Add_RedirectToMessageController_WhenUserDataNotSet()
		{
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			var user = new User();
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).Returns(new ResultCollection<User>(new List<User>(), null));
			var result = controller.Add(user, null);
			Message message = (Message)((System.Web.Mvc.RedirectToRouteResult) result).RouteValues["message"];
			Assert.IsTrue(message.Caption.Equals("Error"));
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
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).Returns(new ResultCollection<User>(new List<User>() { user }, null));
			controller.Remove(new []{ Guid.Empty });
			db.Verify(x => x.RemoveList(It.Is<ICollection<User>>(y => y.First().Login == user.Login)), Times.Once);
		}

	}
}

