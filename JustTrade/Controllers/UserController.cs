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
			var existingUser = Repository<User>.Find(new RepoFiler("Login", user.Login));
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
			var existingUser = Repository<User>.FindById(user.Id).FirstOrDefault();
			var existingUserWithSameLogin = Repository<User>.Find(
				new RepoFiler("Login", user.Login), 
				new RepoFiler("id", user.Id,RepoFilerExpr.NotEq));
			if (existingUserWithSameLogin.Any()) {
				return GenerateErrorMessage(Lang.Get("User with same login already exist"), string.Empty);
			}
			if (existingUser == null) {
				return GenerateErrorMessage(Lang.Get("Required user not found"), string.Empty);
			}
			existingUser.IsSuperuser = user.IsSuperuser;
			existingUser.Login = user.Login;
			existingUser.Name = user.Name;
			existingUser.Password = user.Password.GetHashPassword();
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult Delete(string id) {

		}

		[HttpGet]
		public ActionResult GetItem(string id) {
			var findedUser = Repository<User>.FindById(new Guid(id));
			if (!findedUser.Any()) {
				return Json(JsonData.Create(false, "User not exist"), JsonRequestBehavior.AllowGet);
			}
			return Json(findedUser, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AddUpdateForm(string id) {
			id = (string.IsNullOrEmpty(id) ? Guid.Empty.ToString() : id);
			var findedUser = Repository<User>.FindById(new Guid(id)).FirstOrDefault();
			return PartialView("_AddUpdateForm", findedUser);
		}

		[HttpGet]
		public ActionResult List() {
			return PartialView("_List");
		}

		[HttpGet]
		public ActionResult JsonList() {
			var users = Repository<User>.Find();
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
