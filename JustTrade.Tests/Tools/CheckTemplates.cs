using System;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace JastTrade.Tests
{
	[TestFixture]
	public class CheckTemplates
	{
		private bool ExistTemplatePropesties(string fileContent)
		{
			if (fileContent.Contains ("[host]") ||
				fileContent.Contains ("[dabatase]") ||
				fileContent.Contains ("[user]") ||
				fileContent.Contains ("[password]")) {
				return true;
			}
			return false;
		}

		[Test]
		public void CheckFileContext()
		{
			StringBuilder builder = new StringBuilder ();
			var templates = Directory.GetFiles (@".\ConfigurationTemplates\");
			foreach (var template in templates) 
			{
				if (!ExistTemplatePropesties (File.ReadAllText (template))) {
					builder.AppendFormat ("Not found template variable block in file:{0}\n",Path.GetFileName(template));
				}
			}
			Assert.IsTrue (builder.Length <= 0, builder.ToString ());
		}

	}
}

