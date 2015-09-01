using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public void ShowAddUpdateSettings_RedirectToError_WhenIdIsNull() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var settings = new SettingsSection() {
				Name = "Section",
			};
			db.Setup(x => x.Find<SettingsSection>())
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settings }, null));
			var controller = new SettingsController();
			var result = controller.ShowAddUpdateSettings(null);
			List<SettingsSection> sectionList = controller.ViewBag.SectionList;
			Assert.AreEqual(sectionList[0].Name, settings.Name);
		}

	}
}
