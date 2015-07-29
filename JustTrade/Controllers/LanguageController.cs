
namespace JustTrade.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Web.Mvc;
	using JustTrade.Helpers;
	using JustTrade.Tools;
	using JustTrade.Tools.Attributes;
	using Newtonsoft.Json.Linq;

	public class LanguageData
	{
		public string Name {
			get;
			set;
		}

		public string Version {
			get;
			set;
		}

		public string Value {
			get;
			set;
		}
	}

	internal class LanguageItem
	{
		public string Key {
			get;
			set;
		}

		public string Value {
			get;
			set;
		}
	}


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
			return PartialView(languageItemList);
		}

		[HttpPost]
		public ActionResult SaveLanguage(LanguageData language) {
			try {
				var languageDictionary =
				JArray.Parse(language.Value).ToObject<List<LanguageItem>>().ToDictionary(x => x.Key, x => x.Value);
				var sortedDictionary =
					(from item in languageDictionary
					 orderby item.Key
					 select item).ToDictionary(x => x.Key, x => x.Value);
				sortedDictionary.Add(Lang.LocaleName, language.Name);
				sortedDictionary.Add(Lang.LocaleVersion, language.Version);
				Lang.Save(sortedDictionary);
				
			} catch (Exception ex) {
				return GenerateErrorMessage(
					string.Format(Lang.Get("Error save language dictionary data. Info: {0}"),ex.Message), 
					string.Empty);
			}
			return new EmptyResult();
		}

	}
}