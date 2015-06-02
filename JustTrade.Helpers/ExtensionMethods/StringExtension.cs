using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustTrade.Database;

namespace JustTrade.Helpers.ExtensionMethods
{
	using System.Security.Cryptography;

	public static class StringExtension
	{
		private static MD5 md5Hash;

		public static string GetHash(this string stringData) {
			if (md5Hash == null) {
				md5Hash = MD5.Create();
			}
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(stringData));
			var sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++) {
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

		public static string GetHashPassword(this string stringData) {
			stringData = string.Format("{0}_{1}_{0}", "~`!@#$%^&*() ", stringData);
			if (md5Hash == null) {
				md5Hash = MD5.Create();
			}
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(stringData));
			var sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++) {
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

	}
}
