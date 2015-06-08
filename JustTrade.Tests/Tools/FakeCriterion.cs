using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;

namespace JustTrade.Tests.Tools
{
    class FakeCriterion : ICriterion
    {
        public SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            throw new NotImplementedException();
        }

        public TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            throw new NotImplementedException();
        }

        public IProjection[] GetProjections()
        {
            throw new NotImplementedException();
        }
    }
}
