using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JustTrade.Database;
using JustTrade.Helpers;
using JustTrade.Tools.Security;

namespace JustTrade.Controllers.Administration
{
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
			if (string.IsNullOrEmpty(name)) {
				return GenerateErrorMessage("Error adding settings section", "Name cannot be empty");
			}
			Guid sectionId;
			try
			{
				JTSecurity.Session.Db.Add(new SettingsSection() { Name = name });
				using (var sections = JTSecurity.Session.Db.Find<SettingsSection>(new RepoFiler("Name", name)))
				{
					if (!sections.Any())
					{
						return GenerateErrorMessage("Error adding settings section", "Not found added object");
					}
					sectionId = sections.First().Id;
				}
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
				var sectionRemoveList = new List<SettingsSection>();
				using (var sections = JTSecurity.Session.Db.Find<SettingsSection>())
				{
					sectionRemoveList.AddRange(sections.Where(section => section.Settings.Count == 0));
				}
				if (sectionRemoveList.Any()) {
					JTSecurity.Session.Db.RemoveList(sectionRemoveList);
				}
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
	                item = settingslist.First();
                }
				JTSecurity.Session.Db.Remove(item);
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error removing settings", ex);
			}
			try
			{
				var sectionRemoveList = new List<SettingsSection>();
				using (var sections = JTSecurity.Session.Db.Find<SettingsSection>())
				{
					sectionRemoveList.AddRange(sections.Where(section => section.Settings.Count == 0));
				}
				if (sectionRemoveList.Any()) {
					JTSecurity.Session.Db.RemoveList(sectionRemoveList);
				}
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage("Error removing section without settings", ex);
			}
            return new EmptyResult();
		}

	}

}