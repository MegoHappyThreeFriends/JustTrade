namespace JustTrade.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Web.Mvc;
	using jsTree3.Models;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Helpers.ExtensionMethods;
	using JustTrade.Tools;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public class PermissionController : ControllerWithTools
	{

		[HttpGet]
		public ActionResult Index() {
			CheckPublicMethodUsingAttribute();
			return PartialView("_Index");
		}

		[HttpPost]
		public ActionResult AddTemplate() {

			return null;
		}

		[HttpGet]
		public ActionResult AddUpdateTemplate(Guid?[] ids) {
			PermissionTemplate template = null;
			if (ids != null) {
				using (var templateList = Repository<PermissionTemplate>.Find(new RepoFiler("id", ids, RepoFilerExpr.In))) {
					template = templateList.FirstOrDefault();
				}
			}
			return PartialView("_AddUpdateTemplate", template);
		}

		[HttpPost]
		public ActionResult UpdateTemplateParameter(string[] parameters, Guid templateId) {
			PermissionTemplate updateItem;
			using (var existingItems = Repository<PermissionTemplate>.FindById(templateId)) {
				if (!existingItems.Any()) {
					return GenerateErrorMessage(Lang.Get("Required item not found"), string.Empty);
				}
				updateItem = existingItems.First();
			}
			var json = JsonConvert.SerializeObject(parameters.Select(x => x.Contains(".")));
			updateItem.PermissionRules = json;
			try {
				Repository<PermissionTemplate>.Update(updateItem);
			} catch (Exception ex) {
				return GenerateErrorMessage(Lang.Get("Error update item"), ex);
			}
			return new EmptyResult();
		}

		[HttpGet]
		public ActionResult GetParameterTree(Guid templateId) {
			return GetPermitionTreeInJsonData(templateId);
		}

		private JsonResult GetPermitionTreeInJsonData(Guid templateId) {
			List<string> parameters = null;
			using (var permission = Repository<PermissionTemplate>.FindById(templateId)) {
				if (permission.Any()) {
					parameters = JArray.Parse(permission.First().PermissionRules).ToObject<string[]>().ToList();
				}
			}
			if (parameters == null) {
				parameters = new List<string>();
			}
			var root = new JsTree3Node() {
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

			/*foreach (var type in types) {
				var nodeName = type.Name.Replace("Controller", "");
				var node = JsTree3Node.NewNode(nodeName.GetHash());
				node.text = nodeName;
				node.state = new State(false, false, false);
				node.icon = "";
				var methods = type.GetMethods().Where(x => x.IsPublic && x.IsStatic == false
				&& !x.Name.Contains("get_") && !x.Name.Contains("set_")
				&& !x.Name.Contains("Dispose") && !x.Name.Contains("ToString")
				&& !x.Name.Contains("Equals") && !x.Name.Contains("GetHashCode")
				&& !x.Name.Contains("GetType"));
				if (!methods.Any()) {
					continue;
				}
				foreach (var methodInfo in methods) {
					string methodNodeName = string.Concat(nodeName, ".", methodInfo.Name);
					var newNode = JsTree3Node.NewNode(methodNodeName.GetHash());
					newNode.text = methodInfo.Name;
					newNode.icon = "glyphicon glyphicon-leaf";
					bool isSelected = parameters.Contains(methodNodeName);
					newNode.state = new State(false, false, isSelected);
					node.children.Add(newNode);
				}
				children.Add(node);
			}*/

			root.children = children;
			return Json(root, JsonRequestBehavior.AllowGet);
		}

		private Dictionary<string, List<string>> GetTemplateParametersList() {
			var resultDic = new Dictionary<string, List<string>>();
			var controllerType = typeof(Controller);
			var types =
				AppDomain.CurrentDomain.GetAssemblies().
				SelectMany(s => s.GetTypes()).Where(p => controllerType.IsAssignableFrom(p));
			foreach (var type in types) {
				var methods = type.GetMethods().Where(x => x.IsPublic &&
					Attribute.GetCustomAttributes(x).Any(y =>
						y.TypeId.ToString().Contains("HttpGetAttribute") ||
						y.TypeId.ToString().Contains("HttpPostAttribute")
					));
				if (!methods.Any()) {
					continue;
				}
				var methodsList = methods.Select(methodInfo => methodInfo.Name).ToList();
				resultDic.Add(type.Name, methodsList);
			}
			return resultDic;
		}


		private void CheckPublicMethodUsingAttribute() {
			var methodsWithoutAttributes = new List<string>();
			var controllerType = typeof(Controller);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(controllerType.IsAssignableFrom);
			foreach (var type in types) {
				var methods1 = type.GetMethods().Where(x => x.IsPublic && x.IsStatic == false
				&& !x.Name.Contains("get_") && !x.Name.Contains("set_")
				&& !x.Name.Contains("Dispose") && !x.Name.Contains("ToString")
				&& !x.Name.Contains("Equals") && !x.Name.Contains("GetHashCode")
				&& !x.Name.Contains("GetType"));
				var methods2 = type.GetMethods().Where(x => x.IsPublic &&
					Attribute.GetCustomAttributes(x).Any(y =>
						y.TypeId.ToString().Contains("HttpGetAttribute") ||
						y.TypeId.ToString().Contains("HttpPostAttribute")
					));
				foreach (var methodInfo in methods1) {
					bool exist = false;
					foreach (var info in methods2) {
						if (methodInfo.Name == info.Name) {
							exist = true;
						}
					}
					if (!exist) {
						methodsWithoutAttributes.Add(string.Concat(type.Name, "/", methodInfo.Name));
					}
				}
			}
			if (methodsWithoutAttributes.Any()) {
				throw new Exception(string.Format(
					"Has been found public methods without attributes! " +
					"Fix this error to prevent security hole. Methods: {0}",
					String.Join("\n,", methodsWithoutAttributes)));
			}

		}

	}
}