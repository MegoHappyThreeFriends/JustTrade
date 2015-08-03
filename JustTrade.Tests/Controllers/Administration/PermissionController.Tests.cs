namespace JustTrade.Tests.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using jsTree3.Models;
	using JustTrade.Controllers.Administration;
	using JustTrade.Database;
	using JustTrade.Tests.Tools;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	class PermissionControllerTests : BaseTests
	{
		[Test]
		public void ShowAddUpdateTemplate_ResultCorrectModel() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid()
			};
			db.Setup(x => x.Find<PermissionTemplate>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			var result = (ViewResultBase)controller.ShowAddUpdateTemplate(new Guid?[] { Guid.Empty });
			if (!(result.Model is PermissionTemplate)) {
				Assert.Fail("Result model is not PermissionTemplate");
			}
			Assert.IsTrue(((PermissionTemplate)result.Model).Id == permissionTemplate.Id);
		}

		[Test]
		public void GetTemlateList_ResultCorrectModel() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid()
			};
			db.Setup(x => x.Find<PermissionTemplate>()).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			var result = (ViewResultBase)controller.GetTemlateList();
			if (!(result.Model is List<PermissionTemplate>)) {
				Assert.Fail("Result model is not PermissionTemplate");
			}
			Assert.IsTrue(((List<PermissionTemplate>)result.Model).First().Id == permissionTemplate.Id);
		}

		[Test]
		public void AddTemplate_CallDatabaseAdd() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			const string templateName = "New template";
			var controller = new PermissionController();
			db.Setup(x => x.Find<PermissionTemplate>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate>(), null));
			ActionResult result = controller.AddTemplate(templateName);
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.Add(It.Is<PermissionTemplate>(y => y.Name == templateName)), Times.Once);
		}

		[Test]
		public void AddTemplate_RedirectToMessageWithError_WhenDuplicateItem() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			const string templateName = "New template";
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid()
			};
			db.Setup(x => x.Find<PermissionTemplate>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			ActionResult result = controller.AddTemplate(templateName);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void UpdateTemplate_CallDatabaseUpdate() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			const string templateName = "New template";
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid(),
				Name = "Old template name"
			};
			db.Setup(x => x.FindById<PermissionTemplate>(It.IsAny<Guid>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			ActionResult result = controller.UpdateTemplate(permissionTemplate.Id, templateName);
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.Update(It.Is<PermissionTemplate>(y => y.Name == templateName)), Times.Once);
		}

		[Test]
		public void UpdateTemplate_RedirectToMessageWithError_WhenRecordNotFound() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			const string templateName = "New template";
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid(),
				Name = "Old template name"
			};
			db.Setup(x => x.FindById<PermissionTemplate>(It.IsAny<Guid>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate>(), null));
			ActionResult result = controller.UpdateTemplate(permissionTemplate.Id, templateName);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void RemoveTemplate_CallDatabaseRemove() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid(),
				Name = "Old template name"
			};
			db.Setup(x => x.Find<PermissionTemplate>(It.IsAny<RepoFiler>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			ActionResult result = controller.RemoveTemplates(new[] { permissionTemplate.Id });
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.RemoveList(It.Is<PermissionTemplate[]>(y => y.First().Id == permissionTemplate.Id)), Times.Once);
		}

		[Test]
		public void UpdateTemplateParameter_CallDatabaseRemove() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new PermissionController();
			const string parameter = "Template.Parameter";
			const string parameterJson = "[\"Template.Parameter\"]";
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid(),
				Name = "Old template name"
			};
			db.Setup(x => x.FindById<PermissionTemplate>(It.IsAny<Guid>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			ActionResult result = controller.UpdateTemplateParameter(new[] { parameter }, permissionTemplate.Id);
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.Update(It.Is<PermissionTemplate>(y => y.PermissionRules == parameterJson)), Times.Once);
		}

		[Test]
		public void UpdateTemplateParameter_RedirectToMessageWithError_WhenRecordNotFound() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new PermissionController();
			const string parameter = "Template.Parameter";
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid(),
				Name = "Old template name"
			};
			db.Setup(x => x.FindById<PermissionTemplate>(It.IsAny<Guid>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate>(), null));
			ActionResult result = controller.UpdateTemplateParameter(new[] { parameter }, permissionTemplate.Id);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void GetParameterTree_ResturnCorrectJson() {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			var controller = new PermissionController();
			var permissionTemplate = new PermissionTemplate {
				Id = Guid.NewGuid(),
				Name = "Old template name",
				PermissionRules = "[\"Login.Index\"]"
			};
			db.Setup(x => x.FindById<PermissionTemplate>(It.IsAny<Guid>())).
				Returns(new ResultCollection<PermissionTemplate>(new List<PermissionTemplate> { permissionTemplate }, null));
			ActionResult result = controller.GetParameterTree(Guid.NewGuid());
			var data = (JsTree3Node)((JsonResult)(result)).Data;
			bool selectedUpdatedNode = false;
			foreach (var jsTree3Node in data.children) {
				if (jsTree3Node.id != "Login") {
					continue;
				}
				foreach (var tree3Node in jsTree3Node.children) {
					if (tree3Node.id == "Login.Index") {
						selectedUpdatedNode = tree3Node.state.selected;
					}
				}
			}
			Assert.IsTrue(selectedUpdatedNode, "Method GetParameterTree not update status jsonNodeList");
		}


	}
}
