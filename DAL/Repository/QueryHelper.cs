using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    internal static class QueryHelper
    {
        private static string GetPath<TResult, TObject>(Expression<Func<TObject, object>> memberExpression)
        {
            var bodyString = memberExpression.Body.ToString();
            string path = bodyString.Remove(0, memberExpression.Parameters[0].Name.Length + 1);
            return path;
        }

        public static DbQuery<TResult> IncludeAsDbQuery<TResult, TObject>(this DbQuery<TResult> query, Expression<Func<TObject, object>> memberExpression)
        {
            string includePath = GetPath<TResult, TObject>(memberExpression);
            return query.Include(includePath);
        }

        public static DbQuery<TResult> IncludeManyAsDbQuery<TResult, TObject>(this DbQuery<TResult> query, params Expression<Func<TObject, object>>[] memberExpression)
        {
            foreach (var expr in memberExpression)
            {
                query = query.IncludeAsDbQuery(expr);
            }

            return query;
        }
    }
}
