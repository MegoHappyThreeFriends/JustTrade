﻿using System;
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

		internal static void Load() {
			if (_loaded) {
				return;
			}
			string neededLang = AppSettings.Lang;
			var filePath = AppSettings.Workspace + @"\Language\" + neededLang.ToLower()+".json";
			if (!File.Exists(filePath)) {
				throw new Exception("Language file not found");
			}
			using (TextReader reader = new StreamReader(filePath)) {
				var data = reader.ReadToEnd();
				_language = JObject.Parse(data);
			}
			_loaded = true;
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

		public static string GetInformation(string name) {
			if (!_loaded) {
				Load();
			}
			return _language["LocaleInformation"][name].ToString();
		}

	}
}

