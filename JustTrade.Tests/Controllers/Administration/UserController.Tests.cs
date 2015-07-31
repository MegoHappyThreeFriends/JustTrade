namespace JustTrade.Tests.Controllers.Administration
{
	using System;
	using System.Collections.Generic;
	using JustTrade.Controllers.Administration;
	using JustTrade.Database;
	using JustTrade.Helpers;
	using JustTrade.Tests.Tools;
	using Moq;
	using NHibernate;
	using NUnit.Framework;

	[TestFixture]
	public class UserControllerTests
	{
	    

	    [Test]
		public void Add ()
	    {
			MockTools.SetupLanguage();
			MockTools.SetupDefaultSysSettings();
	        Mock<ISession> session =  MockTools.CreateSessionMock();
		    Mock<ICriteria> creteriaMock =  session.SetupCriteria();
			
		    NHibernateHelper.SessionForTest = session.Object;
			var controller = new UserController ();
			var user = new User () {
				Login = "login",
				Name = "name",
				Password = "password"
			};

			creteriaMock.Setup(x => x.List<User>()).Returns(new List<User>());

		    controller.Add(user,new []{ Guid.Empty });

			creteriaMock.Setup(x => x.List<User>()).Returns(new List<User>() { user });

            session.Verify(x=>x.SaveOrUpdate(It.Is<User>(i=>i.Equals(user))));
		}
	}
}

