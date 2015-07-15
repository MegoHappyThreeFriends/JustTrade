
namespace JustTrade.Controllers
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Web.Mvc;
	using JustTrade.Helpers;
	using JustTrade.Tools;

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

	public class LanguageController : ControllerWithTools
	{

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

		public ActionResult Index() {
			IDictionary languageItemList = Lang.GetList();
			return PartialView(languageItemList);
		}

		[HttpPost]
		public ActionResult SaveLanguage(LanguageData language) {
			List<string> parameters = language.Value.Split('|').ToList();
			var languageDictionary = new Dictionary<string, string>();
			foreach (var parameter in parameters.Where(x=>x.Contains("~"))) {
				languageDictionary.Add(parameter.Split('~')[0], parameter.Split('~')[1]);
			}
			var sortedDictionary = 
				(from item in languageDictionary orderby item.Key select item).ToDictionary(x=>x.Key,x=>x.Value);
			sortedDictionary.Add(Lang.LocaleName, language.Name);
			sortedDictionary.Add(Lang.LocaleVersion, language.Version);
			Lang.Save(sortedDictionary);
			return new EmptyResult();
		}



	}
}