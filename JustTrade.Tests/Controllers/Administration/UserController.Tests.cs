namespace JustTrade.Tests.Controllers.Administration
{
	using System.Web.Mvc;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JustTrade.Controllers.Administration;
	using Database;
	using Tools;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class UserControllerTests : BaseTests
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
		    ActionResult result = controller.Add(user, null);
			Assert.IsTrue(result is EmptyResult);
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
			ActionResult result = controller.Add(user, null);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Add_RedirectToMessageController_WhenDuplicateUser()
		{
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			var user = new User()
			{
				Login = "login",
				Name = "name",
				Password = "password"
			};
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).Returns(new ResultCollection<User>(new List<User>() { user }, null));
			ActionResult result = controller.Add(user, null);
			CheckRedirectToMessageWithError(result);
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
			var result = controller.Update(user, null);
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.Update(It.Is<User>(y => y.Login == user.Login)), Times.Once);
		}

		[Test]
		public void Update_RedirectToMessageWithError_WhenUserDataNotSet() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			var user = new User();
			var oldUser = new User() {
				Login = "login1",
				Name = "name",
				Password = "password"
			};
			db.Setup(x => x.FindById<User>(It.IsAny<Guid>())).Returns(new ResultCollection<User>(new List<User>() { oldUser }, null));
			ActionResult result = controller.Update(user, null);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Update_RedirectToMessageWithError_WhenUserNotFound() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			var user = new User() {
				Login = "new_login",
				Name = "name",
				Password = "password"
			};
			db.Setup(x => x.FindById<User>(It.IsAny<Guid>())).Returns(new ResultCollection<User>(new List<User>(), null));
			ActionResult result = controller.Update(user, null);
			CheckRedirectToMessageWithError(result);
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
			var result = controller.Remove(new []{ Guid.Empty });
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.RemoveList(It.Is<ICollection<User>>(y => y.First().Login == user.Login)), Times.Once);
		}

		[Test]
		public void Remove_RedirectToMessageWithError_WhenUserCollectionIsNull() {
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
			ActionResult result = controller.Remove(null);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Remove_RedirectToMessageWithError_WhenUserNotFound() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			var db = MockTools.SetupDb();
			var controller = new UserController();
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).Returns(new ResultCollection<User>(new List<User>(), null));
			ActionResult result = controller.Remove(new[] { Guid.Empty });
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void UpdatePermission_UpdateRepmissionInDb() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
			Guid permissionTemplateId = Guid.NewGuid();
			Guid permissionTemplateIdNew = Guid.NewGuid();
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
				Password = "password",
				UserPermissionBindings = new[] { new UserPermissionBinding() {
					Id = permissionTemplateId
				} }
			};
			var template = new PermissionTemplate() {
				Id = permissionTemplateIdNew
			};
			db.Setup(x => x.FindById<User>(It.IsAny<Guid>())).
				Returns(new ResultCollection<User>(new List<User>() { oldUser }, null));
			db.Setup(x => x.Find<PermissionTemplate>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate>() { template }, null));
			var result = controller.Update(user, new []{ Guid.Empty });
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.RemoveList(
				It.Is<List<UserPermissionBinding>>(y => y.First().Id == permissionTemplateId)), Times.Once);
			db.Verify(x => x.AddList(
				It.Is<List<UserPermissionBinding>>(y => y.First().PermissionTemplate.Id == permissionTemplateIdNew)), Times.Once);
		}

		#region Methods: Private

		

		#endregion

	}
}

