namespace JustTrade.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using jsTree3.Models;
	using JustTrade.Tools;

	public class PermissionController : ControllerWithTools
	{
		[HttpGet]
		public ActionResult Index() {



			var controllerType = typeof(Controller);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => controllerType.IsAssignableFrom(p));
			foreach (var type in types) {
				var methods = type.GetMethods().Where(x => x.IsPublic && x.IsStatic == false
				&& !x.Name.Contains("get_") && !x.Name.Contains("set_")
				&& !x.Name.Contains("Dispose") && !x.Name.Contains("ToString")
				&& !x.Name.Contains("Equals") && !x.Name.Contains("GetHashCode")
				&& !x.Name.Contains("GetType"));

			}


			


			return PartialView("_Index");
		}

		[HttpGet]
		public JsonResult GetJsTree3Data() {
			var root = new JsTree3Node() // Create our root node and ensure it is opened
			{
				id = Guid.NewGuid().ToString(),
				text = "Controllers",
				state = new State(true, false, false)
			};

			var controllerType = typeof(Controller);
			var children = new List<JsTree3Node>();
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => controllerType.IsAssignableFrom(p));
			foreach (var type in types) {
				var node = JsTree3Node.NewNode(Guid.NewGuid().ToString());
				node.text = type.Name;
				node.state = new State(true, false, false);
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
					var n = JsTree3Node.NewNode(Guid.NewGuid().ToString());
					n.text = methodInfo.Name;
					n.icon = "glyphicon glyphicon-leaf";
					n.state = new State(true, false, false);
					node.children.Add(n);
				}
				children.Add(node);
			}

			

			// Create a basic structure of nodes
			/*var children = new List<JsTree3Node>();
			for (int i = 0; i < 5; i++) {
				var node = JsTree3Node.NewNode(Guid.NewGuid().ToString());
				node.state = new State(IsPrime(i), false, false);

				for (int y = 0; y < 5; y++) {
					node.children.Add(JsTree3Node.NewNode(Guid.NewGuid().ToString()));
				}

				children.Add(node);
			}*/

			// Add the sturcture to the root nodes children property
			root.children = children;
			 

			// Return the object as JSON
			return Json(root, JsonRequestBehavior.AllowGet);
		}

		static bool IsPrime(int n) {
			if (n > 1) {
				return Enumerable.Range(1, n).Where(x => n % x == 0)
								 .SequenceEqual(new[] { 1, n });
			}

			return false;
		}

	}
}