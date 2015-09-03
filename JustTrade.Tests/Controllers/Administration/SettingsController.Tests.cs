using System;
using System.Collections.Generic;
using System.Linq;

namespace JustTrade.Tests.Controllers.Administration
{
	using System.Web.Mvc;
	using JustTrade.Controllers.Administration;
	using JustTrade.Database;
	using JustTrade.Tests.Tools;
	using Moq;
	using NUnit.Framework;

	[TestFixture]
	class SettingsControllerTests : BaseTests
	{
		[Test]
		public void GetSettingsList_ReturnCorrectList() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var settings = new SettingsSection() {
				Name = "Section",
				Settings = new List<Settings>() {
					new Settings() {
						Name = "Settings",
						Value = "Value"
					}
				}
			};
			db.Setup(x => x.Find<SettingsSection>())
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settings }, null));
			var controller = new SettingsController();
			var result = controller.GetSettingsList();
			var model = ((ViewResultBase)(result)).ViewData.Model as List<SettingsSection>;
			if (model == null || !model.Any()) {
				Assert.Fail("Return incorrect model");
			}
			var item = model.First();
			Assert.AreEqual(item.Name, settings.Name);
			Assert.AreEqual(item.Settings[0].Name, settings.Settings[0].Name);
			Assert.AreEqual(item.Settings[0].Value, settings.Settings[0].Value);
		}

		[Test]
		public void ShowAddUpdateSettings_ReturnCorrectDataWithModelNull_IfParameterNull() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var settingsSection = new SettingsSection() {
				Name = "Section",
			};
			db.Setup(x => x.Find<SettingsSection>())
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settingsSection }, null));
			var controller = new SettingsController();
			var result = controller.ShowAddUpdateSettings(null);
			Assert.AreEqual(((ViewResultBase)(result)).Model, null);
			List<SettingsSection> sectionList = controller.ViewBag.SectionList;
			Assert.AreEqual(sectionList[0].Name, settingsSection.Name);
		}

		[Test]
		public void ShowAddUpdateSettings_ReturnCorrectModel_IfParameterExist() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var settingsSection = new SettingsSection {
				Name = "Section",
			};
			var settings = new Settings {
				Name = "Setting",
				Value = "SettingValue"
			};
			db.Setup(x => x.Find<SettingsSection>())
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settingsSection }, null));
			db.Setup(x => x.FindById<Settings>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<Settings>(new List<Settings>() { settings }, null));
			var controller = new SettingsController();
			var result = controller.ShowAddUpdateSettings(Guid.Empty);
			var model = ((ViewResultBase)(result)).Model;
			Assert.AreEqual(((Settings)model).Name, settings.Name);
			Assert.AreEqual(((Settings)model).Value, settings.Value);
			List<SettingsSection> sectionList = controller.ViewBag.SectionList;
			Assert.AreEqual(sectionList[0].Name, settingsSection.Name);
		}

		[Test]
		public void AddSection_RedirectToMessageWithError_WhenParameterIsNull() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var result = controller.AddSection(null);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void ShowAddUpdateSettings_ReturnCorrectData() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var settingsSection = new SettingsSection {
				Id = Guid.NewGuid(),
				Name = "Section",
			};
			db.Setup(x => x.Find<SettingsSection>(It.IsAny<RepoFiler>()))
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settingsSection }, null));
			var result = controller.AddSection(settingsSection.Name);
			db.Verify(x => x.Add(It.Is<SettingsSection>(y=>y.Name == settingsSection.Name)),Times.Once);
			Assert.AreEqual(((ContentResult)(result)).Content, settingsSection.Id.ToString());
		}

	}
}
