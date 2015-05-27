using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using JastTrade;
using JastTrade.Controllers;
using JustTrade.Database;
using JustTrade.Tools;
using System.Runtime.CompilerServices;

namespace JastTrade.Tests
{
	[TestFixture]
	public class HomeControllerTest
	{
		[Test]
		public void Index ()
		{
			// Arrange
			var controller = new HomeController ();

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


        [Test]
        public void Add_ReturnJsonSuccess ()
        {
            //NHibernateHelper.SessionForTest = null;

            var controller = new HomeController ();
            var user = new User();
            user.Login = "unit_test";
            user.Password = "unit_test";
            var result = controller.Add(user);
            //Assert.IsTrue(result == JsonData(true));


        }


	}




}
