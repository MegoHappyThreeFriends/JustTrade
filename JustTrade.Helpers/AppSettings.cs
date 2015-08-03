using System;
using System.Configuration;

namespace JustTrade.Helpers
{
	using System.Collections.Generic;

	public static class AppSettings
	{
		static AppSettings() {
			MockSettings = null;
		}

		internal static Dictionary<string, string> MockSettings {
			get;
			set;
		}

		public static string Lang => Setting<string>("lang");

		public static string Workspace => Setting<string>("workspace");

		public static string MailHost => Setting<string>("mail_host");

		public static int MailPort => Setting<int>("mail_port");

		public static bool MailSSL => Setting<bool>("mail_ssl");

		public static string MailUser => Setting<string>("mail_user");

		public static string MailPassword => Setting<string>("mail_password");

		private static T Setting<T>(string name) {
			if (MockSettings != null) {
				return (T)Convert.ChangeType(MockSettings[name], typeof(T));
			}
			string value = ConfigurationManager.AppSettings[name];

			if (value == null) {
				throw new Exception($"Could not find setting '{name}',");
			}

			return (T)Convert.ChangeType(value, typeof(T));
		}
	}
}

