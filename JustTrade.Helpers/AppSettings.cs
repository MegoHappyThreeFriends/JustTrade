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

		public static string Lang
        {
            get
            {
                return Setting<string>("lang");
            }
        }

		public static string Workspace {
			get {
				return Setting<string>("workspace");
			}
		}

        private static T Setting<T>(string name)
        {
	        if (MockSettings != null) {
				return (T)Convert.ChangeType(MockSettings[name], typeof(T));
	        }
	        string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}

