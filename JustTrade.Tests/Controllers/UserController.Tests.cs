using NUnit.Framework;
using JustTrade.Controllers;
using JustTrade.Database;
using Moq;
using NHibernate;

namespace JustTrade.Tests
{
	using JustTrade.Controllers.Administration;

	[TestFixture]
	public class UserControllerTests
	{
	    protected Mock<ISession> CreateSessionMock()
        {
            Mock<ISession> session = new Mock<ISession>();
            Mock<ITransaction> transaction = new Mock<ITransaction>();
            session.Setup(x => x.BeginTransaction()).Returns(transaction.Object);
	        return session;
        }

	    [Test]
		public void Add ()
	    {
	        Mock<ISession> session = CreateSessionMock();
		    NHibernateHelper.SessionForTest = session.Object;
			var controller = new UserController ();
			User user = new User ();
		    //controller.Add(user);
            session.Verify(x=>x.SaveOrUpdate(It.Is<User>(i=>i.Equals(user))));
		}
	}
}

