using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JustTrade.Database;
using JustTrade.Tools;

namespace JastTrade.Controllers
{
	using JustTrade.Helpers.ExtensionMethods;

	public class AdminController : Controller
	{

		public ActionResult Index() {
			return View();
		}

		public ActionResult Database() {
			return PartialView();
		}

		public ActionResult GenerateDatabase() {
			try {
				NHibernateHelper.CreateDb();
				InsertDefaultData();
			} catch (Exception ex) {
				return Json(JsonData.Create(ex), JsonRequestBehavior.AllowGet);
			}
			return Json(JsonData.Create(true, "Database created"), JsonRequestBehavior.AllowGet);
		}

		public void InsertDefaultData() {
			var user = new User {
				Name = "demo",
				Login = "demo",
				Password = "demo".GetHashPassword(),
				IsSuperuser = true,
				Permitions = new List<Permition>
                { new Permition
                    {
                        Name = "Full permition"
                    }
                }
			};

			Repository<User>.Add(user);
		}

	}
}
