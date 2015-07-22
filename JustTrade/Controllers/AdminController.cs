namespace JustTrade.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using JustTrade.Database;
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tools;

	public class AdminController : Controller
	{

		[HttpGet]
		public ActionResult Index() {
			return View();
		}

		[HttpGet]
		public ActionResult Database() {
			return PartialView();
		}

		[HttpGet]
		public ActionResult GenerateDatabase() {
			try {
				NHibernateHelper.CreateDb();
				InsertDefaultData();
			} catch (Exception ex) {
				return Json(JsonData.Create(ex), JsonRequestBehavior.AllowGet);
			}
			return Json(JsonData.Create(true, "Database created"), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public void InsertDefaultData() {

			var user = new User {
				Name = "demo",
				Login = "demo",
				Password = "demo".GetHashPassword(),
				IsSuperuser = true,
				UserPermissionBindings = new List<UserPermissionBinding>() {
					new UserPermissionBinding() {
						PermissionTemplate = new PermissionTemplate() {
							Name = "TestProfile",
							PermissionRules = "bla bla"
						}
					}
				}
			};

			Repository<User>.Add(user);
		}

	}
}
