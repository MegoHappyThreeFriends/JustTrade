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
		public void AddSection_ReturnCorrectData() {
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
			controller.AddSection(settingsSection.Name);
			db.Verify(x => x.Add(It.Is<SettingsSection>(y => y.Name == settingsSection.Name)), Times.Once);
		}

		[Test]
		public void Add_RedirectToMessageWithError_WhenInputParameterIsEmpty() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var result = controller.Add(null, "value", Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
			result = controller.Add("name", null, Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
			result = controller.Add("name", "value", Guid.Empty);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Add_RedirectToMessageWithError_WhenTryDuplicateEntryInOneSection() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();

			var settingsSection = new SettingsSection {
				Name = "Section",
				Id = Guid.NewGuid()
			};
			var settings = new Settings {
				Name = "Setting",
				Value = "SettingValue",
				Section = settingsSection
			};
			db.Setup(x => x.Find<Settings>(It.IsAny<RepoFiler>()))
				.Returns(new ResultCollection<Settings>(new List<Settings>() { settings }, null));
			var result = controller.Add("name", "value", settingsSection.Id);
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Add_ReturnEmptyResultAndCallAddMethod_WhenAllDataIsCorrect() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var settingsSection = new SettingsSection {
				Name = "Section",
				Id = Guid.NewGuid()
			};
			var settings = new Settings {
				Name = "Setting",
				Value = "SettingValue",
				Section = settingsSection
			};
			db.Setup(x => x.Find<Settings>(It.IsAny<RepoFiler>()))
				.Returns(new ResultCollection<Settings>(new List<Settings>() { settings }, null));
			db.Setup(x => x.FindById<SettingsSection>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settingsSection }, null));
			var result = controller.Add("name", "value", Guid.NewGuid());
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.Add(
				It.Is<Settings>(y => y.Name == "name" && y.Value == "value" && y.Section.Id == settingsSection.Id)));
		}

		[Test]
		public void Update_RedirectToMessageWithError_WhenInputParameterIsEmpty() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var result = controller.Update(Guid.NewGuid(), null, "value", Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
			result = controller.Update(Guid.NewGuid(), "name", null, Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
			result = controller.Update(Guid.NewGuid(), "name", "value", Guid.Empty);
			CheckRedirectToMessageWithError(result);
			result = controller.Update(Guid.Empty, "name", "value", Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Update_RedirectToMessageWithError_WhenSettingsRecordNotFound() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			db.Setup(x => x.FindById<Settings>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<Settings>(new List<Settings>() {
				}, null));
			var result = controller.Update(Guid.NewGuid(), "name", "value", Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
		}


		[Test]
		public void Update_RedirectToMessageWithError_WhenSettingsSectionRecordNotFound() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var settingsSection = new SettingsSection {
				Name = "Section",
				Id = Guid.NewGuid()
			};
			var settings = new Settings {
				Name = "Setting",
				Value = "SettingValue",
				Section = settingsSection
			};
			db.Setup(x => x.FindById<SettingsSection>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() {
				}, null));

			db.Setup(x => x.FindById<Settings>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<Settings>(new List<Settings>() { settings }, null));

			var result = controller.Update(Guid.NewGuid(), "name", "value", Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Update_ReturnEmptyResultAndCallUpdateAndDeleteUnusedSection_WhenInputDataIsCorrect() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var settingsSection = new SettingsSection {
				Name = "Section",
				Id = Guid.NewGuid(),
				Settings = new List<Settings>()
			};
			var settingsSectionWithSettings = new SettingsSection {
				Name = "SectionWithSettings",
				Id = Guid.NewGuid(),
				Settings = new List<Settings>() { new Settings() }
			};
			var settings = new Settings {
				Name = "Setting",
				Value = "SettingValue",
				Section = settingsSection
			};
			db.Setup(x => x.FindById<SettingsSection>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settingsSection }, null));
			db.Setup(x => x.FindById<Settings>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<Settings>(new List<Settings>() { settings }, null));
			db.Setup(x => x.Find<SettingsSection>())
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() { settingsSection, settingsSectionWithSettings }, null));
			var result = controller.Update(Guid.NewGuid(), "name", "value", Guid.NewGuid());
			Assert.IsTrue(result is EmptyResult);
			db.Verify(x => x.Update(It.Is<Settings>(y => y.Name == "name" && y.Value == "value" && y.Section.Id == settingsSection.Id)));
			db.Verify(x => x.RemoveList(It.Is<List<SettingsSection>>(y => y.Count == 1 && y[0].Id == settingsSection.Id), false));
		}

		[Test]
		public void Remove_RedirectToMessageWithError_WhenItemNotFound() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			db.Setup(x => x.FindById<Settings>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<Settings>(new List<Settings>() {
				}, null));
			var result = controller.Remove(Guid.NewGuid());
			CheckRedirectToMessageWithError(result);
		}

		[Test]
		public void Remove_CallRemoveMathod_WhenAllDataCorrect() {
			var session = MockTools.SetupSession();
			var db = session.SetupDb();
			MockTools.SetupLanguage();
			MockTools.SetupDefaultAppSettings();
			var controller = new SettingsController();
			var settingsSection = new SettingsSection {
				Name = "Section",
				Id = Guid.NewGuid()
			};
			var settings = new Settings {
				Id = Guid.NewGuid(),
				Name = "Setting",
				Value = "SettingValue",
				Section = settingsSection
			};
			db.Setup(x => x.Find<SettingsSection>())
				.Returns(new ResultCollection<SettingsSection>(new List<SettingsSection>() {
				}, null));

			db.Setup(x => x.FindById<Settings>(It.IsAny<Guid>(), false))
				.Returns(new ResultCollection<Settings>(new List<Settings>() { settings }, null));

			var result = controller.Remove(Guid.NewGuid());
			db.Verify(x => x.Remove(It.Is<Settings>(y => y.Id == settings.Id), false));
			Assert.IsTrue(result is EmptyResult);
		}


	}
}
