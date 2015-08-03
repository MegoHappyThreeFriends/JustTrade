namespace JustTrade.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Web.Mvc;
	using jsTree3.Models;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Tools;
	using JustTrade.Tools.Security;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public class PermissionController : ControllerWithTools
	{

		[HttpGet]
		public ActionResult Index() {
			return PartialView("_Index");
		}

		[HttpGet]
		public ActionResult ShowAddUpdateTemplate(Guid?[] ids) {
			PermissionTemplate template = null;
			if (ids != null) {
				using (var templateList = 
					JTSecurity.Session.Db.Find<PermissionTemplate>(new RepoFiler("id", ids, RepoFilerExpr.In))) {
					template = templateList.FirstOrDefault();
				}
			}
			return PartialView("_AddUpdateTemplate", template);
		}

		[HttpGet]
		public ActionResult GetTemlateList() {
			using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>()) {
				List<PermissionTemplate> templateList = templates.ToList();
				return PartialView("_TemplateList", templateList);
			}
		}

		[HttpPost]
		public ActionResult AddTemplate(string name) {
			using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>(new RepoFiler("Name", name))) {
				if (templates.Any()) {
					return GenerateErrorMessage(Lang.Get("Found a duplicate entry"), Lang.Get("Check the name of the template to duplicate from the list!"));
				}
			}
			try {
				JTSecurity.Session.Db.Add(new PermissionTemplate {
					Name = name
				});
			} catch (Exception ex) {
				return GenerateErrorMessage("Error adding template", ex);
			}
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult UpdateTemplate(Guid id, string name) {
			PermissionTemplate item;
			using (var templates = JTSecurity.Session.Db.FindById<PermissionTemplate>(id)) {
				if (!templates.Any()) {
					return GenerateErrorMessage(Lang.Get("Record not found"), "");
				}
				item = templates.First();
				item.Name = name;
			}
			try {
				JTSecurity.Session.Db.Update(item);
			} catch (Exception ex) {
				return GenerateErrorMessage("Error adding template", ex);
			}
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult RemoveTemplates(Guid[] ids) {
			try {
				PermissionTemplate[] permissionTemplate;
				using (var templates = JTSecurity.Session.Db.Find<PermissionTemplate>(new RepoFiler("id", ids, RepoFilerExpr.In))) {
					permissionTemplate = templates.ToArray();
				}
				if (permissionTemplate.Any()) {
					JTSecurity.Session.Db.RemoveList(permissionTemplate);
				}
			} catch (Exception ex) {
				return GenerateErrorMessage("Error removing template(s)", ex);
			}
			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult UpdateTemplateParameter(string[] parameters, Guid templateId) {
			PermissionTemplate updateItem;
			using (var existingItems = JTSecurity.Session.Db.FindById<PermissionTemplate>(templateId)) {
				if (!existingItems.Any()) {
					return GenerateErrorMessage(Lang.Get("Required item not found"), string.Empty);
				}
				updateItem = existingItems.First();
			}
			var json = (parameters == null ? "" : JsonConvert.SerializeObject(parameters.Where(x => x.Contains("."))));
			updateItem.PermissionRules = json;
			try {
				JTSecurity.Session.Db.Update(updateItem);
			} catch (Exception ex) {
				return GenerateErrorMessage(Lang.Get("Error update item"), ex);
			}
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult GetParameterTree(Guid templateId) {
			return GetPermitionTreeInJsonData(templateId);
		}

		#region Methods: Private

		private JsonResult GetPermitionTreeInJsonData(Guid templateId) {
			List<string> parameters = null;
			using (var permission = JTSecurity.Session.Db.FindById<PermissionTemplate>(templateId)) {
				if (permission.Any()) {
					PermissionTemplate item = permission.First();
					if (!string.IsNullOrEmpty(item.PermissionRules)) {
						parameters = JArray.Parse(item.PermissionRules).ToObject<string[]>().ToList();
					}
				}
			}
			if (parameters == null) {
				parameters = new List<string>();
			}
			var root = new JsTree3Node {
				id = Guid.Empty.ToString(),
				text = "Controllers",
				state = new State(true, false, false)
			};
			var children = new List<JsTree3Node>();
			var paramaterDic = GetTemplateParametersList();
			foreach (var item in paramaterDic) {
				var nodeName = item.Key.Replace("Controller", "");
				var node = JsTree3Node.NewNode(nodeName);
				node.text = nodeName;
				node.state = new State(false, false, false);
				node.icon = "";
				foreach (var parameterItem in item.Value) {
					string methodNodeName = string.Concat(nodeName, ".", parameterItem);
					var newNode = JsTree3Node.NewNode(methodNodeName);
					newNode.text = parameterItem;
					newNode.icon = "glyphicon glyphicon-leaf";
					bool isSelected = parameters.Contains(methodNodeName);
					newNode.state = new State(false, false, isSelected);
					node.children.Add(newNode);
				}
				children.Add(node);
			}
			root.children = children;
			return Json(root, JsonRequestBehavior.AllowGet);
		}

		private Dictionary<string, List<string>> GetTemplateParametersList() {
			var resultDic = new Dictionary<string, List<string>>();
			var controllerType = typeof(Controller);
			var types =
				AppDomain.CurrentDomain.GetAssemblies().
				SelectMany(s => s.GetTypes()).Where(controllerType.IsAssignableFrom);
			foreach (var type in types) {
				var methods = type.GetMethods().Where(x => x.IsPublic &&
					Attribute.GetCustomAttributes(x).Any(y =>
						y.TypeId.ToString().Contains("HttpGetAttribute") ||
						y.TypeId.ToString().Contains("HttpPostAttribute")
					));
				IEnumerable<MethodInfo> methodInfos = methods as MethodInfo[] ?? methods.ToArray();
				if (!methodInfos.Any()) {
					continue;
				}
				var methodsList = methodInfos.Select(methodInfo => methodInfo.Name).ToList();
				resultDic.Add(type.Name, methodsList);
			}
			return resultDic;
		}

		#endregion

	}
}