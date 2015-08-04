namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web.Mvc;
	using Helpers;
	using Tools;
	using Tools.Attributes;

	public class LanguageController : ControllerWithTools
	{
		[FreeAccess]
		[HttpGet]
		public string GetLanguageJson() {
			string neededLang = AppSettings.Lang;
			var filePath = AppSettings.Workspace + @"\Language\" + neededLang.ToLower() + ".json";
			if (!System.IO.File.Exists(filePath)) {
				throw new Exception("Language file not found");
			}
			using (TextReader reader = new StreamReader(filePath)) {
				return reader.ReadToEnd();
			}
		}

		[HttpGet]
		public ActionResult Index() {
			Dictionary<string, string> languageItemList = Lang.GetList();
			return PartialView("../Administrator/Language/_Index", languageItemList);
		}

		[HttpPost]
		public ActionResult Save(Dictionary<string, string> langDictionary, string name, string version) {
			try {
				var sortedDictionary =
						(from item in langDictionary
						 orderby item.Key
						 select item).ToDictionary(x => x.Key, x => x.Value);
				sortedDictionary.Add(Lang.LocaleName, name);
				sortedDictionary.Add(Lang.LocaleVersion, version);
				Lang.Save(sortedDictionary);
			} catch (Exception ex) {
				return GenerateErrorMessage(
					string.Format(Lang.Get("Error save language dictionary data.")), ex);
			}
			return new EmptyResult();
		}

	}
}