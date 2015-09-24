using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JustTrade.Database;
using JustTrade.Helpers;
using JustTrade.Tools.Security;

namespace JustTrade.Controllers.Administration
{
	using JustTrade.Helpers.ExtensionMethods;
	using Tools;

	public class SettingsController : ControllerWithTools
	{
		[HttpGet]
		public ActionResult Index()
		{
			return PartialView("../Administrator/Settings/_Index");
		}

		[HttpGet]
		public ActionResult GetSettingsList()
		{
			var list = new List<SettingsSection>();
			using (var sections = JTSecurity.Session.Db.Find<SettingsSection>())
			{
				foreach (var section in sections)
				{
					var settings = section.Settings;
					var cloneSection = section;
					cloneSection.Settings = settings.ToList();
                    list.Add(cloneSection);
				}
			}
			return PartialView("../Administrator/Settings/_SettingsList", list);
		}

		[HttpGet]
		public ActionResult ShowAddUpdateSettings(Guid? id) {
			ViewBag.SectionList = null;
			Settings settings = null;
			if (id != null)
			{
				using (var templateList = JTSecurity.Session.Db.FindById<Settings>((Guid)id))
				{
					settings = templateList.FirstOrDefault();
				}
			}
			using (var sections = JTSecurity.Session.Db.Find<SettingsSection>())
			{
				ViewBag.SectionList = sections.ToList();
			}
            return PartialView("../Administrator/Settings/_AddUpdateSettings", settings);
		}

		[HttpPost]
		public ActionResult AddSection(string name)
		{
			if (name.IsNullOrEmptyValue()) {
				return GenerateErrorMessage("Error adding settings section", "Name cannot be empty");
			}
			Guid sectionId;
			try {
				var settingSection = new SettingsSection() { Name = name };
				JTSecurity.Session.Db.Add(settingSection);
				sectionId = settingSection.Id;
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error adding settings section", ex);
			}
			return Content(sectionId.ToString());
		}

		[HttpPost]
		public ActionResult Add(string name, string value, Guid sectionId)
		{
			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value) || sectionId == Guid.Empty) {
				return GenerateErrorMessage("Error adding settings","Name, Value or Section is empty");
			}
			var newSettings = new Settings()
			{
				Name = name,
				Value = value,
				Section = null
			};
			using (var settingslist = JTSecurity.Session.Db.Find<Settings>(new RepoFiler("Name", name)))
			{
				if (settingslist.Any(x => x.Section.Id == sectionId))
				{
					return GenerateErrorMessage(Lang.Get("Found a duplicate entry"), Lang.Get("Check the name of the settings to duplicate from the list!"));
				}
			}
			using (var sections = JTSecurity.Session.Db.FindById<SettingsSection>(sectionId))
			{
				if (!sections.Any())
				{
					return GenerateErrorMessage(Lang.Get("Record not found"), "Section not found");
				}
				newSettings.Section = sections.First();
			}
			try
			{
				JTSecurity.Session.Db.Add(newSettings);
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error adding settings", ex);
			}
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult Update(Guid id, string name, string value, Guid sectionId)
		{
			if (name.IsNullOrEmptyValue() || value.IsNullOrEmptyValue() || sectionId == Guid.Empty || id == Guid.Empty) {
				return GenerateErrorMessage("Error updating settings", "Name, Value or Section is empty");
			}
			Settings newSettings;
			using (var settingslist = JTSecurity.Session.Db.FindById<Settings>(id))
			{
				if (!settingslist.Any())
				{
					return GenerateErrorMessage(Lang.Get("Record not found"), "");
				}
				newSettings = settingslist.First();
				newSettings.Name = name;
				newSettings.Value = value;
				if (newSettings.Section.Id != sectionId)
				{
					newSettings.Section = null;
				}
			}
			if (newSettings.Section == null)
			{
				using (var sections = JTSecurity.Session.Db.FindById<SettingsSection>(sectionId))
				{
					if (!sections.Any())
					{
						return GenerateErrorMessage(Lang.Get("Record not found"), "Section {"+ sectionId + "} not found");
					}
					newSettings.Section = sections.First();
				}
			}

			try
			{
				JTSecurity.Session.Db.Update(newSettings);
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error adding settings", ex);
			}

			try
			{
				RemoveUnuesdSection();
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error removing section without settings", ex);
			}

			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult Remove(Guid id)
		{
			try
			{
				Settings item;
                using (var settingslist = JTSecurity.Session.Db.FindById<Settings>(id))
                {
					if (!settingslist.Any()) {
						return GenerateErrorMessage(Lang.Get("Record not found"), "");
					}
	                item = settingslist.First();
                }
				JTSecurity.Session.Db.Remove(item);
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error removing settings", ex);
			}
			try {
				RemoveUnuesdSection();
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error removing section without settings", ex);
			}
            return new EmptyResult();
		}

		#region Methods: Private

		private void RemoveUnuesdSection() {
			var sectionRemoveList = new List<SettingsSection>();
			using (var sections = JTSecurity.Session.Db.Find<SettingsSection>()) {
				sectionRemoveList.AddRange(sections.Where(section => section.Settings.Count == 0));
			}
			if (sectionRemoveList.Any()) {
				JTSecurity.Session.Db.RemoveList(sectionRemoveList);
			}
		}

		#endregion


	}

}