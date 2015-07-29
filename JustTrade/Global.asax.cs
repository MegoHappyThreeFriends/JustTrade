
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JustTrade
{
	using JastTrade;
	using JustTrade.Controllers;

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
		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			string controller = filterContext.RouteData.Values.First(x => x.Key == "controller").Value.ToString();
			string action = filterContext.RouteData.Values.First(x => x.Key == "action").Value.ToString();
			List<string> permissionList = null;
			if (UserSession.CurrentSession != null) {
				permissionList = UserSession.CurrentSession.PermissionList;
			}

			// default permission list
			/*if (permissionList == null) {
				permissionList = new List<string>() {
					"Login.Index",
					"Login.Login",
					"Message.Index",
					"Message.SendReport",
					"Language.GetLanguageJson"
				};
			}

			if (controller != "Error") {
				if (!permissionList.Contains(string.Concat(controller, ".", action))) {
					filterContext.Result =
						new RedirectToRouteResult(new RouteValueDictionary(new {
							controller = "Error",
							action = "Permission"
						}));
				}
			}*/

			base.OnActionExecuting(filterContext);
		}
	}

}
