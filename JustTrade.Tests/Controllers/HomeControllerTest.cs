using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using JustTrade;
using JustTrade.Controllers;
using JustTrade.Database;

namespace JustTrade.Tests
{
	[TestFixture]
	public class HomeControllerTest
	{
		[Test]
		public void Index ()
		{
			// Arrange
			var controller = new HomeController ();
			controller.Get ();

			NHibernateHelper.SessionForTest = null;

			// Act
			var result = (ViewResult)controller.Index ();

			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			var expectedVersion = mvcName.Version.Major;
			var expectedRuntime = isMono ? "Mono" : ".NET";

			// Assert
			Assert.AreEqual (expectedVersion, result.ViewData ["Version"]);
			Assert.AreEqual (expectedRuntime, result.ViewData ["Runtime"]);
		}
	}
}
