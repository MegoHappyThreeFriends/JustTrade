
namespace JustTrade.Controllers
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Web.Mvc;
	using JustTrade.Helpers;
	using JustTrade.Tools;

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

	}
}