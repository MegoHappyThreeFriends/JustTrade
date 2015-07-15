using System;
using System.Reflection;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace JustTrade.Helpers
{
	using Newtonsoft.Json.Linq;

	public static class Lang
	{
		private static bool _loaded;
		private static JObject _language;
		private static string _filePath;

		public static string LocaleName {
			get {
				return "LocaleInformation.LocaleName";
			}
		}

		public static string LocaleVersion {
			get {
				return "LocaleInformation.Version";
			}
		}

		internal static void Load() {
			if (_loaded) {
				return;
			}
			string neededLang = AppSettings.Lang;
			_filePath = AppSettings.Workspace + @"\Language\" + neededLang.ToLower() + ".json";
			if (!File.Exists(_filePath)) {
				throw new Exception("Language file not found");
			}
			using (TextReader reader = new StreamReader(_filePath)) {
				var data = reader.ReadToEnd();
				_language = JObject.Parse(data);
			}
			_loaded = true;
		}

		public static IDictionary GetList() {
			if (!_loaded) {
				Load();
			}
			var langDict = new Dictionary<string, string>();
			foreach (var langItem in _language) {
				if (langItem.Key == "LocaleInformation") {
					foreach (var item in langItem.Value.First.Parent) {
						langDict.Add(item.Path, item.First.Value<string>());
					}
					continue;
				}
				langDict.Add(langItem.Key, langItem.Value.Value<string>());
			}
			return langDict;
		}

		public static string Get(string name) {
			if (!_loaded) {
				Load();
			}
			string value = name;
			try
			{
				value = _language[name].ToString();
			}
			catch (Exception ex)
			{
				
			}
			return value;
		}

		public static void Save(Dictionary<string, string> dictionary) {
			if (!dictionary.ContainsKey(LocaleName) ||
				!dictionary.ContainsKey(LocaleVersion)) {
					throw new KeyNotFoundException(LocaleVersion);
			}
			using (var writer = new StreamWriter(_filePath)) {
				writer.WriteLine("{");
				writer.WriteLine("\t \"LocaleInformation\": {");
				writer.WriteLine("\t\t\"LocaleName\": \"{0}\",", dictionary[LocaleName]);
				writer.WriteLine("\t\t\"Version\": \"{0}\"", dictionary["LocaleInformation.Version"]);
				writer.WriteLine("\t},");
				foreach (var item in dictionary) {
					if (item.Key == LocaleName || item.Key == LocaleVersion) {
						continue;
					}
					writer.WriteLine("\t\"{0}\":\"{1}\",",item.Key, item.Value);
				}
				writer.WriteLine("}");
			}
		}

		public static string GetInformation(string name) {
			if (!_loaded) {
				Load();
			}
			return _language["LocaleInformation"][name].ToString();
		}

	}
}

