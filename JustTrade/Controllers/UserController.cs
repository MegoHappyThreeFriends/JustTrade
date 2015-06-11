using System;
using System.Web.Mvc;
using JustTrade.Database;
using System.Linq;
using JustTrade.Tools;
using JustTrade.Helpers.ExtensionMethods;

namespace JustTrade.Controllers
{
	using JustTrade.Helpers;

	public class UserController : ControllerWithTools
	{
		public ActionResult Index() {
			return View();
		}

		[HttpPost]
		public ActionResult Add(User user) {
			if(user.Login.NullOrEmpty() || user.Name.NullOrEmpty() || user.Password.NullOrEmpty()){
				return GenerateErrorMessage(Lang.Get("You must enter Login, Name and Password"), string.Empty);
			}
			var existingUser = Repository<User>.FindBy("Login", user.Login);
			if (existingUser != null) {
				return GenerateErrorMessage(Lang.Get("User with same login already exist"), string.Empty);
			}
			var newUser = new User() {
				Login = user.Login,
				Password = user.Password.GetHashPassword(),
				Name = user.Name,
				IsSuperuser = user.IsSuperuser
			};
			Repository<User>.Add(newUser);
			return new EmptyResult();
		}

		//TODO: реализовать в репозитории выбор коллекции с простым фильтром.
		[HttpPost]
		public ActionResult Update(User user) {
			if (user.Login.NullOrEmpty() || user.Name.NullOrEmpty() || user.Password.NullOrEmpty()) {
				return GenerateErrorMessage(Lang.Get("You must enter Login, Name and Password"), string.Empty);
			}
			var existingUser = Repository<User>.FindById(user.Id);
			var existingUserWithSameLogin = Repository<User>.FindBy("Login", user.Login);
			if (existingUserWithSameLogin.Id != user.Id && ) {
				return GenerateErrorMessage(Lang.Get("User with same login already exist"), string.Empty);
			}

			return new EmptyResult();
		}


		[HttpGet]
		public ActionResult GetItem(string id) {
			var findedUser = Repository<User>.FindById(new Guid(id));
			if (findedUser == null) {
				return Json(JsonData.Create(false, "User not exist"), JsonRequestBehavior.AllowGet);
			}
			return Json(findedUser, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AddUpdateForm(string id) {
			id = (string.IsNullOrEmpty(id) ? Guid.Empty.ToString() : id);
			var findedUser = Repository<User>.FindById(new Guid(id));
			return PartialView("_AddUpdateForm", findedUser);
		}

		[HttpGet]
		public ActionResult List() {
			return PartialView("_List");
		}

		[HttpGet]
		public ActionResult JsonList() {
			var users = Repository<User>.GetAll();
			var userList = new {
				data = users.Select(x => new {
					x.Id,
					x.Name,
					x.Login,
					x.IsSuperuser
				}).ToList()
			};
			return Json(userList, JsonRequestBehavior.AllowGet);
		}

	}
}
