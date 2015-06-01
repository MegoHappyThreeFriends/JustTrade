using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.Query.Dynamic;
using JastTrade;
using JustTrade.Database;
using Moq;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;
using NUnit.Framework;

namespace JustTrade.Tests.Tools
{

    public static class Database
    {
        public static void MockNHibernate()
        {
			

            //var m = new Substitute.For<ISession>();


            //NHibernateHelper.SessionForTest = (ISession)m.MockInstance;
        }



    }
}
