using System;
using System.Reflection;
using System.Linq;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace JustTrade.Helpers
{
	public static class Lang
	{
		private static bool _loaded;
		private static Hashtable _language;

		internal static void Load()
		{
			if (_loaded) { 
				return;
			}
			string neededLang = AppSettings.Lang;
			_language = new Hashtable ();
			string selectedResourceName = string.Empty;
			List<string> resourceName = Assembly.GetExecutingAssembly ().GetManifestResourceNames ().ToList ();
			foreach (var item in resourceName) {
				var langName = item.Replace ("JustTrade.Helpers.Language.", string.Empty).Replace (".lang", string.Empty);
				if (neededLang == langName) {
					selectedResourceName = item;
					break;
				}
				throw new Exception ("Language not found");
			}

			var assembly = Assembly.GetExecutingAssembly();
			using (Stream stream = assembly.GetManifestResourceStream(selectedResourceName))
			using (TextReader reader = new StreamReader(stream))
			{
				_language = ParseLangFile (reader);
			}

			_loaded = true;
		}

		internal static Hashtable ParseLangFile(TextReader reader){
			var htable = new Hashtable ();

			// read file header (ignoring)
			reader.ReadLine ();

			int lineCount=1;
			string buf;
			while ((buf = reader.ReadLine ()) != null) {
				buf = buf.Trim ();
				if (buf.Length < 2) {
					continue;
				}
				string[] langItem = buf.Split (new char[]{ ':', '=' });
				if (langItem.Length != 2) {
					throw new Exception ( string.Format( "Incorrect language item. Line number:{0}. Line data: {1} ", lineCount, buf));
				}
				htable.Add (langItem[0].Trim(),langItem[1].Trim());
				lineCount++;
			}

			return htable;
		}


		public static string Get(string name){
			Load ();
			if (!_language.ContainsKey (name)) {
				return name.Replace ('_', ' ');
			}
			string data = _language [name].ToString ();
			return data;
		}

	}
}

