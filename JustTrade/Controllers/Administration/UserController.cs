namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tools;
	using JustTrade.Tools.Security;

	public class UserController : ControllerWithTools
	{
		[HttpGet]
		public ActionResult Index() {
			return PartialView("_Index");
		}

		[HttpPost]
		public ActionResult Add(User user, Guid[] permissionTemplates) {
			if(user.Login.NullOrEmpty() || user.Name.NullOrEmpty() || user.Password.NullOrEmpty()){
				return GenerateErrorMessage(Lang.Get("You must enter Login, Name and Password"), string.Empty);
			}
			ICollection<User> existingUser;
			using (var resultExistingUser = JTSecurity.Session.Db.Find<User>(new RepoFiler("Login", user.Login))) {
				existingUser = resultExistingUser;
			}
			if (existingUser.Any()) {
				return GenerateErrorMessage(Lang.Get("User with same login already exist"), string.Empty);
			}
			var newUser = new User() {
				Login = user.Login,
				Password = user.Password.GetHashPassword(),
				Name = user.Name,
				IsSuperuser = user.IsSuperuser
			};
			JTSecurity.Session.Db.Add(newUser);
			if (permissionTemplates == null) {
				return new EmptyResult();
			}
			return UpdatePermission(GetUserIdByLogin(user.Login), permissionTemplates);
		}

		[HttpPost]
		public ActionResult Update(User user, Guid[] permissionTemplates) {
			if (user.Login.NullOrEmpty() || user.Name.NullOrEmpty() || user.Password.NullOrEmpty()) {
				return GenerateErrorMessage(Lang.Get("You must enter Login, Name and Password"), string.Empty);
			}
			User existingUser;
			using (var existingUsers = JTSecurity.Session.Db.FindById<User>(user.Id)) {
				existingUser = existingUsers.FirstOrDefault();
			}
			if (existingUser == null) {
				return GenerateErrorMessage(Lang.Get("Required user not found"), string.Empty);
			}
			existingUser.IsSuperuser = user.IsSuperuser;
			existingUser.Login = user.Login;
			existingUser.Name = user.Name;
			existingUser.Password = user.Password.GetHashPassword();
			JTSecurity.Session.Db.Update(existingUser);
			return UpdatePermission(user.Id, permissionTemplates);
		}

		[HttpPost]
		public ActionResult Remove(Guid[] ids, bool? accepted) {
			ICollection<User> findedUsers;
			if (ids == null) {
				return GenerateErrorMessage(Lang.Get("Required user(s) not found"), string.Empty);
			}
			using (var rfindedUsers = JTSecurity.Session.Db.Find<User>(new RepoFiler("id", ids, RepoFilerExpr.In))) {
				findedUsers = rfindedUsers;
			}
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
			JTSecurity.Session.Db.Remove(findedUsers);
			return new EmptyResult();
		}


		[HttpGet]
		public ActionResult ShowAddUpdateForm(Guid? id) {
			var userPermissionItems = new List<UserPermissionItem>();

			using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>()) {
				userPermissionItems.AddRange(
					templates.Select(permissionTemplate => new UserPermissionItem() 
						{ Id = permissionTemplate.Id, TemplateName = permissionTemplate.Name, IsUse = false }));
			}

			User findedUser=null;
			if (id != null) {
				using (var findedUsers = JTSecurity.Session.Db.FindById<User>((Guid)id)) {
					findedUser = findedUsers.FirstOrDefault();

					if (findedUser != null) {
						foreach (var userPermissionBinding in findedUser.UserPermissionBindings) {
							foreach (var userPermissionItem in userPermissionItems) {
								if (userPermissionItem.Id == userPermissionBinding.PermissionTemplate.Id) {
									userPermissionItem.IsUse = true;
								}
							}
						}
					}

				}
			}
			ViewBag.PermissionList = userPermissionItems;
			return PartialView("_AddUpdateForm", findedUser);
		}
		
		[HttpGet]
		public ActionResult UsersJsonList() {
			using (var users = JTSecurity.Session.Db.Find<User>()) {
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

		#region Class: UserPermissionItem

		public class UserPermissionItem
		{
			public Guid Id {
				get;
				set;
			}

			public string TemplateName {
				get;
				set;
			}

			public bool IsUse {
				get;
				set;
			}
		}
		
		#endregion

		#region Methods: Private

		private Guid GetUserIdByLogin(string login) {
			using (var users = JTSecurity.Session.Db.Find<User>(new RepoFiler("Login", login))) {
				if (users.Any()) {
					return users.First().Id;
				}
			}
			throw new Exception("Error get user id by Login!");
		}

		private ActionResult UpdatePermission(Guid userId, Guid[] ids) {
			User user;
			List<PermissionTemplate> templateList;
			List<UserPermissionBinding> templateListToRemove;
			if (ids == null) {
				return new EmptyResult();
			}
			using (var users = JTSecurity.Session.Db.FindById<User>(userId)) {
				if (!users.Any()) {
					return GenerateErrorMessage(Lang.Get("Required user(s) not found"), string.Empty);
				}
				user = users.First();
				templateListToRemove = user.UserPermissionBindings.ToList();
			}
			JTSecurity.Session.Db.Remove(templateListToRemove);
			using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>(new RepoFiler("id", ids, RepoFilerExpr.In))) {
				templateList = templates.ToList();
			}
			var userPermissionBindingListToInsert = templateList.Select(x => new UserPermissionBinding() {
				User = user,
				PermissionTemplate = x
			});
			JTSecurity.Session.Db.Add(userPermissionBindingListToInsert);
			return new EmptyResult();
		}

		#endregion

	}
}
