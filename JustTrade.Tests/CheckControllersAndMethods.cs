namespace JustTrade.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using JustTrade.Controllers;
	using NUnit.Framework;

	[TestFixture]
	class CheckControllersAndMethods
	{
		[Test]
		public void CheckPublicMethodInControllersUsingAttributeGetOrPost() {
			var home = new HomeController();
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
			Assert.IsFalse(methodsWithoutAttributes.Any(),string.Format(
					"Has been found public methods without attributes! " +
					"Fix this error to prevent security hole. \nMethods: {0}",
					String.Join("\n,", methodsWithoutAttributes)));
		}
	}
}
