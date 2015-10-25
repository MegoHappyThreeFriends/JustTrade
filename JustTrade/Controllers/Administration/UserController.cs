namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using Database;
	using Helpers;
	using Helpers.ExtensionMethods;
	using Tools;
	using Tools.Security;

	public class UserController : ControllerWithTools
	{
		[HttpGet]
		public ActionResult Index() {
			return PartialView("../Administrator/User/_Index");
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
			// check ip range count 
			try
			{
				user.AllowIPAdress.ParceIpAddresses();
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage(ex.Message, ex.StackTrace + "\n\n" + ex.InnerException);
			}
			var newUser = new User {
				Login = user.Login,
				Password = user.Password.GetHashPassword(),
				Name = user.Name,
				IsSuperuser = user.IsSuperuser,
				AllowIPAdress = user.AllowIPAdress
			};
			
            JTSecurity.Session.Db.Add(newUser);
			if (permissionTemplates == null) {
				return new EmptyResult();
			}
			return UpdatePermission(newUser.Id, permissionTemplates);
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
			// check ip range count 
			try
			{
				user.AllowIPAdress.ParceIpAddresses();
			}
			catch (Exception ex)
			{
				return GenerateErrorMessage(ex.Message, ex.StackTrace + "\n\n" + ex.InnerException);
			}
			existingUser.IsSuperuser = user.IsSuperuser;
			existingUser.Login = user.Login;
			existingUser.Name = user.Name;
			existingUser.AllowIPAdress = user.AllowIPAdress;
			existingUser.Password = user.Password.GetHashPassword();
			JTSecurity.Session.Db.Update(existingUser);
			return UpdatePermission(user.Id, permissionTemplates);
		}

		[HttpPost]
		public ActionResult Remove(Guid[] ids) {
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
			JTSecurity.Session.Db.RemoveList(findedUsers);
			return new EmptyResult();
		}


		[HttpGet]
		public ActionResult ShowAddUpdateForm(Guid? id) {
			var userPermissionItems = new List<UserPermissionItem>();

			using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>()) {
				userPermissionItems.AddRange(
					templates.Select(permissionTemplate => new UserPermissionItem { Id = permissionTemplate.Id, TemplateName = permissionTemplate.Name, IsUse = false }));
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
			return PartialView("../Administrator/User/_AddUpdateForm", findedUser);
		}
		
		[HttpGet]
		public ActionResult UsersJsonList() {
			using (var users = JTSecurity.Session.Db.Find<User>()) {
				var userList = new {
					data = users.Select(x => new {
						x.Id,
						x.Name,
						x.Login,
						x.AllowIPAdress
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
			if (templateListToRemove.Any())
			{
				JTSecurity.Session.Db.RemoveList(templateListToRemove);
			}
			using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>(new RepoFiler("id", ids, RepoFilerExpr.In))) {
				templateList = templates.ToList();
			}
			List<UserPermissionBinding> userPermissionBindingListToInsert = templateList.Select(x => new UserPermissionBinding() {
				User = user,
				PermissionTemplate = x
			}).ToList();
			if (userPermissionBindingListToInsert.Any())
			{
				JTSecurity.Session.Db.AddList(userPermissionBindingListToInsert);
			}
			return new EmptyResult();
		}

		#endregion

	}
}
