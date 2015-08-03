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

		public static string Lang {
			get {
				return Setting<string>("lang");
			}
		}

		public static string Workspace {
			get {
				return Setting<string>("workspace");
			}
		}

		public static string MailHost {
			get {
				return Setting<string>("mail_host");
			}
		}

		public static int MailPort {
			get {
				return Setting<int>("mail_port");
			}
		}

		public static bool MailSSL {
			get {
				return Setting<bool>("mail_ssl");
			}
		}

		public static string MailUser {
			get {
				return Setting<string>("mail_user");
			}
		}

		public static string MailPassword {
			get {
				return Setting<string>("mail_password");
			}
		}
		
		private static T Setting<T>(string name) {
			if (MockSettings != null) {
				return (T)Convert.ChangeType(MockSettings[name], typeof(T));
			}
			string value = ConfigurationManager.AppSettings[name];

			if (value == null) {
				throw new Exception(String.Format("Could not find setting '{0}',", name));
			}

			return (T)Convert.ChangeType(value, typeof(T));
		}
	}
}

