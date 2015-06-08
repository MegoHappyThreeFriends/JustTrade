using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace JustTrade.Tests.Tools
{
    public class FakeCriteria : ICriteria
    {
        public Dictionary<string, object> ExpressionParameter { get; set; }
        public List<object> Data { get; set; }

        public FakeCriteria()
        {
            Data = new List<object>();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public ICriteria SetProjection(params IProjection[] projection)
        {
            throw new NotImplementedException();
        }

        public ICriteria Add(ICriterion expression)
        {
            var param = (SimpleExpression) expression;
            ExpressionParameter.Add(param.PropertyName, param.Value);
            return this;
        }

        public ICriteria AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetFetchMode(string associationPath, FetchMode mode)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetLockMode(LockMode lockMode)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetLockMode(string alias, LockMode lockMode)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateAlias(string associationPath, string alias)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateAlias(string associationPath, string alias, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateAlias(string associationPath, string alias, JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateCriteria(string associationPath)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateCriteria(string associationPath, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateCriteria(string associationPath, string alias)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateCriteria(string associationPath, string alias, JoinType joinType)
        {
            throw new NotImplementedException();
        }

        public ICriteria CreateCriteria(string associationPath, string alias, JoinType joinType, ICriterion withClause)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetResultTransformer(IResultTransformer resultTransformer)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetMaxResults(int maxResults)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetFirstResult(int firstResult)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetFetchSize(int fetchSize)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetTimeout(int timeout)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetCacheable(bool cacheable)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetCacheRegion(string cacheRegion)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetComment(string comment)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetFlushMode(FlushMode flushMode)
        {
            throw new NotImplementedException();
        }

        public ICriteria SetCacheMode(CacheMode cacheMode)
        {
            throw new NotImplementedException();
        }

        public IList List()
        {
            throw new NotImplementedException();
        }

        public object UniqueResult()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Future<T>()
        {
            throw new NotImplementedException();
        }

        public IFutureValue<T> FutureValue<T>()
        {
            throw new NotImplementedException();
        }

        public ICriteria SetReadOnly(bool readOnly)
        {
            throw new NotImplementedException();
        }

        public void List(IList results)
        {
            throw new NotImplementedException();
        }

        public IList<T> List<T>()
        {
            var prop = ExpressionParameter.First().Key;
            var val = (T)ExpressionParameter.First().Value;
            return (from item in Data 
                    where val.Equals(item.GetType().GetProperty(prop).GetValue(item)) 
                    select val)
                    .ToList();
        }

        public T UniqueResult<T>()
        {
            throw new NotImplementedException();
        }

        public void ClearOrders()
        {
            throw new NotImplementedException();
        }

        public ICriteria GetCriteriaByPath(string path)
        {
            throw new NotImplementedException();
        }

        public ICriteria GetCriteriaByAlias(string alias)
        {
            throw new NotImplementedException();
        }

        public Type GetRootEntityTypeIfAvailable()
        {
            throw new NotImplementedException();
        }

        public string Alias
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnlyInitialized
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }
    }
}
