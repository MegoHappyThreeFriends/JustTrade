﻿using System;
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
			if (existingUser.Any()) {
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
			Repository<User>.Update(existingUser);
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult Remove(Guid[] ids, bool? accepted) {
			var findedUsers = Repository<User>.Find(new RepoFiler("id", ids, RepoFilerExpr.In));
			if (ids.Length != findedUsers.Count) {
				return GenerateErrorMessage(Lang.Get("Required user(s) not found"), string.Empty);
			}
			accepted = accepted != null;
			if (accepted == false) {
				var idsArray = string.Join(", ", Array.ConvertAll(ids, x => string.Format("\"{0}\"", x)));
				var data = string.Format("{{\"ids\":[{0}], \"accepted\":\"true\"}}", idsArray);
				var names = string.Join(", ", findedUsers.Select(x => string.Format("{0}", x.Name)).ToArray());
				return GenerateConfirmMessage(
					string.Format(Lang.Get("Are you sure you want to remove a user ({0}) ?"), names),
					string.Empty, "User/Remove", data);
			}
			Repository<User>.Remove(findedUsers);
			return new EmptyResult();
		}

		public ActionResult AddUpdateForm(Guid? id) {
			User findedUser=null;
			if (id != null) {
				findedUser = Repository<User>.FindById((Guid)id).FirstOrDefault();
			}
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
