
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JustTrade
{
	using JustTrade.Controllers;
	using JustTrade.Tools.Security;

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new {
					controller = "Home",
					action = "Index",
					id = UrlParameter.Optional
				}
			);
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
			filters.Add(new PermissionFilerAttribute());
		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
		}
	}

	public class PermissionFilerAttribute : ActionFilterAttribute
	{

		private static HashSet<string> _freeMethods;
		private HashSet<string> FreeMethods {
			get {
				if (_freeMethods == null) {
					_freeMethods = new HashSet<string>();
					var controllerType = typeof(Controller);
					var types =
						AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(controllerType.IsAssignableFrom);
					foreach (var type in types) {
						var methods =
							type.GetMethods()
								.Where(x =>x.IsPublic && GetCustomAttributes(x).Any(y => y.TypeId.ToString().Contains("FreeAccess")));
						if (!methods.Any()) {
							continue;
						}
						var methodsList = methods.Select(methodInfo => methodInfo.Name).ToList();
						foreach (var methodItem in methodsList) {
							_freeMethods.Add(string.Concat(type.Name.Replace("Controller",""), ".", methodItem));
						}
					}
				}
				return _freeMethods;
			}
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			string controller = filterContext.RouteData.Values.First(x => x.Key == "controller").Value.ToString();
			string action = filterContext.RouteData.Values.First(x => x.Key == "action").Value.ToString();
			bool allow = IsAllowPermission(controller, action);
			/*if (!allow) {
				filterContext.Result =
						new RedirectToRouteResult(new RouteValueDictionary(new {
							controller = "Error",
							action = "Permission"
						}));
			}*/
			base.OnActionExecuting(filterContext);
		}

		private bool IsAllowPermission(string controller, string action) {
			string path = string.Concat(controller, ".", action);
			if (controller.Contains("Error")) {
				return true;
			}
			bool isFreeMethod = FreeMethods.Contains(path);
			if (isFreeMethod) {
				return true;
			}
			return JTSecurity.AccessIsAllowed(path);
		}

	}

}
