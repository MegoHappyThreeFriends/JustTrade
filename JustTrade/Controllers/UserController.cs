using System;
using System.Web.Mvc;
using JustTrade.Database;

namespace JustTrade.Controllers
{
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tools;

	public class UserController : Controller
	{
		public ActionResult Index() {
			return View();
		}

		[HttpPost]
		public ActionResult Add(User user) {
			var existingUser = Repository<User>.FindBy("Login", user.Login);
			if (existingUser != null) {
				return Json(JsonData.Create(true,"User with same login already exist"));
			}
			var newUser = new User() {
				Login = user.Login,
				Password = user.Password.GetHashPassword(),
				Name = user.Name,
				IsSuperuser = user.IsSuperuser
			};
			Repository<User>.Add(newUser);
			return Json(JsonData.Create(true));
		}

		[HttpGet]
		public ActionResult GetItem(string id) {
			var findedUser = Repository<User>.FindById(new Guid(id));
			if (findedUser == null) {
				return Json(JsonData.Create(false, "User not exist"), JsonRequestBehavior.AllowGet);
			}

			
			return Json(findedUser, JsonRequestBehavior.AllowGet);
		}

	}
}
