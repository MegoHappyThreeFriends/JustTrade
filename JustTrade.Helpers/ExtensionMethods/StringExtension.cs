using System.Text;

namespace JustTrade.Helpers.ExtensionMethods
{
	using System.Security.Cryptography;

	public static class StringExtension
	{
		private static MD5 _md5Hash;

		public static bool NullOrEmpty(this string strData) {
			return string.IsNullOrEmpty(strData) || string.IsNullOrWhiteSpace(strData);
		}

		public static string GetHash(this string stringData) {
			if (_md5Hash == null) {
				_md5Hash = MD5.Create();
			}
			byte[] data = _md5Hash.ComputeHash(Encoding.UTF8.GetBytes(stringData));
			var sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++) {
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

		public static string GetHashPassword(this string stringData) {
			stringData = MixinChars(stringData);
			if (_md5Hash == null) {
				_md5Hash = MD5.Create();
			}
			byte[] data = _md5Hash.ComputeHash(Encoding.UTF8.GetBytes(stringData));
			var sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++) {
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}

		public static bool IsNullOrEmptyValue(this string item) {
			return string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item);
		}

		private static string MixinChars(string input) {
			const string chars = "☻~♦`!♣@#♠$%•^◘&*○(◙)♂ ♀♪♫";
			int curPosition = 0;
			int blockSumm = 0;
			string result= string.Empty;
			foreach (char inputChar in input) {
				var charItem = (byte)inputChar;
				blockSumm += charItem;
				if (blockSumm > 150) {
					result += chars[curPosition];
					curPosition = (curPosition >= chars.Length ? 0 : curPosition + 1);
					blockSumm -= 150;
				}
				result += inputChar;
			}
			return result;
		}

	}
}
