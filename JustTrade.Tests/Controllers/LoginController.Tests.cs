namespace JustTrade.Tests.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using JustTrade.Controllers;
	using JustTrade.Database;
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tests.Tools;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	public class LoginControllerTests : BaseTests
	{
		[Test]
		public void Login_RedirectToMessageWithError_WhenNotFound() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new LoginController();
			var user = new User {
				Login = "login",
				Password = "password"
			};

			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<User>(new List<User>(), null));
			var result = controller.Login(user.Login, user.Password);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Login_RedirectToMessageWithError_WhenLoginOrPasswordIsNullOrEmpty() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new LoginController();
			var user = new User {
				Login = "login",
				Password = "password"
			};
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<User>(new List<User> { user }, null));
			controller.TempData = new TempDataDictionary();
			var result = controller.Login(string.Empty, user.Password);
			CheckRedirectToMessageWithError(result);
			result = controller.Login(user.Login, string.Empty);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Login_RedirectToMessageWithError_WhenPasswordNotCorrect() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new LoginController();
			var user = new User {
				Login = "login",
				Password = "password"
			};
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<User>(new List<User> { user }, null));
			var result = controller.Login(user.Login, user.Password);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Login_ReturnEmptyResult_WhenAllGood() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new LoginController();
			var user = new User {
				Login = "login",
				Password = "password".GetHashPassword()
			};
			var user2 = new User {
				UserPermissionBindings = new List<UserPermissionBinding>{ 
					new UserPermissionBinding {
						PermissionTemplate = new PermissionTemplate {
							PermissionRules = "[\"Login.Index\"]"
						}
					}
				}
			};
			db.Setup(x => x.FindById<User>(It.IsAny<Guid>(), false)).
				Returns(new ResultCollection<User>(new List<User> { user2 }, null));
			db.Setup(x => x.Find<User>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<User>(new List<User> { user }, null));
			var result = controller.Login(user.Login, "password");
			Assert.IsTrue(result is EmptyResult);
		}

	}
}
