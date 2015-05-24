using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using JustTrade.Helpers;
using JustTrade.Database;

namespace JastTrade.Controllers
{
	public class AdminController : Controller
    {
		private string ReplaceTemplateVariable(string host, string database, string user, 
			string password, string fileContent)
		{
			string file = fileContent;
			file = file.Replace ("[host]", host);
			file = file.Replace ("[database]", database);
			file = file.Replace ("[user]", user);
			file = file.Replace ("[password]", password);
			return file;
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
			return Json (System.IO.Directory.GetFiles (@".\bin\ConfigurationTemplates\").
				Select(x=>Path.GetFileName(x).Replace(".cfg.xml","")), JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public object CreateDatabase(string databaseType, string host, string database, string user, string password){
			string file = string.Format (@".\bin\ConfigurationTemplates\{0}.cfg.xml", databaseType);
			if (!System.IO.File.Exists (file)) {
				throw new FileNotFoundException (file);
			}
			string fileContent = System.IO.File.ReadAllText (file);
			fileContent = ReplaceTemplateVariable (host,database,user,password, fileContent);
			System.IO.File.WriteAllText (@".\bin\hibernate.cfg.xml", fileContent);
			//try{
			throw new FileNotFoundException("asdfdsgfdsg");
				//NHibernateHelper.CreateDb();
			//}
			//catch(Exception ex) {
			return Json( new { result = "error", details = "asfdsa" }, JsonRequestBehavior.AllowGet);
			//}
			return Json( new { result = "success", details = Lang.Get("Database_created") }, JsonRequestBehavior.AllowGet);
		}

    }
}
