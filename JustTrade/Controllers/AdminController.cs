using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using JustTrade.Helpers;

namespace JastTrade.Controllers
{
	public class AdminController : Controller
    {
		private void ReplaceTemplateVariable(string host, string database, string user, 
			string password, ref string fileContent)
		{
			string file = fileContent;
			file = file.Replace ("[host]", host);
			file = file.Replace ("[database]", database);
			file = file.Replace ("[user]", user);
			file = file.Replace ("[password]", password);
		}



        public ActionResult Index()
        {
            return View ();
        }

		public ActionResult Database()
		{
			return PartialView ();
		}

		[HttpGet]
		public object GetDatabaseTypeList(){
			return Json (System.IO.Directory.GetFiles (@".\bin\ConfigurationTemplates\").Select(x=>Path.GetFileName(x)), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public object CreateDatabase(string databaseType, string host, string database, string user, string password){
			string file = string.Format (@".\ConfigurationTemplates\{0}.xml", databaseType);
			if (!System.IO.File.Exists (file)) {
				throw new FileNotFoundException (file);
			}
			string fileContent = System.IO.File.ReadAllText (file);
			ReplaceTemplateVariable (host,database,user,password, ref fileContent);
			return new { a = "asdsaf" };
		}

    }
}
