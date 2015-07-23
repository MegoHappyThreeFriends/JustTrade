using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace JustTrade.Helpers
{
	using System.Linq;
	using System.Security.Cryptography;
	using System.Threading;
	using Newtonsoft.Json.Linq;

	public static class Lang
	{
		private static bool _loading = false;
		private static bool _loaded;
		private static readonly Hashtable _language = new Hashtable();
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
			while (_loading) {
				Thread.Sleep(100);
			}
			_loading = true;
			if (_loaded) {
				return;
			}
			_language.Clear();
			string neededLang = AppSettings.Lang;
			_filePath = AppSettings.Workspace + @"\Language\" + neededLang.ToLower() + ".json";
			if (!File.Exists(_filePath)) {
				throw new Exception("Language file not found");
			}

			using (TextReader reader = new StreamReader(_filePath)) {
				var data = reader.ReadToEnd();
				var language = JObject.Parse(data);
				foreach (var langItem in language) {
					if (langItem.Key == "LocaleInformation") {
						foreach (var item in langItem.Value.First.Parent) {
							if (!_language.ContainsKey(item.Path)) {
								_language.Add(item.Path, item.First.Value<string>());
							}
						}
						continue;
					}
					_language.Add(langItem.Key, langItem.Value.Value<string>());
				}
			}

			_loaded = true;
			_loading = false;
		}

		public static Dictionary<string, string> GetList() {
			if (!_loaded) {
				Load();
			}
			Dictionary<string, string> result = _language.Cast<DictionaryEntry>()
				.ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());
			result = (from item in result
					  orderby item.Key
					  select item).ToDictionary(x => x.Key, x => x.Value);
			return result;
		}

		public static string Get(string name) {
			if (!_loaded) {
				Load();
			}
			string value = name;
			if (_language.ContainsKey(name)) {
				value = _language[name].ToString();
			}
			return value;
		}

		public static void Save(Dictionary<string, string> dictionary) {
			if (!_loaded) {
				Load();
			}
			if (!dictionary.ContainsKey(LocaleName) ||
				!dictionary.ContainsKey(LocaleVersion)) {
				throw new KeyNotFoundException(LocaleVersion);
			}
			bool firstItem = true;
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
					if (!firstItem) {
						writer.Write(",\n");
					} else {
						firstItem = false;
					}
					writer.Write("\t\"{0}\":\"{1}\"", item.Key, item.Value);
				}
				writer.WriteLine("\n}");
			}
		}

		public static string GetInformation(string name) {
			if (!_loaded) {
				Load();
			}
			string key = string.Concat("LocaleInformation", name);
			if (!_language.ContainsKey(key)) {
				throw new KeyNotFoundException(key);
			}
			return _language[key].ToString();
		}

	}
}

