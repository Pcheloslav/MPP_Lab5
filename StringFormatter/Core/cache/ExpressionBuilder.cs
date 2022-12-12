using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.cache
{
    internal class ExpressionBuilder : IExpressionBuilder
    {
        public Expression<Func<object, string>> Build(Type targetType, string expression)
        {
            var propAndIndex = expression.Split(new char[] { '[', ']' });
            var isArrayExpression = propAndIndex.Length > 1;
            var propOrFieldName = propAndIndex[0];

            var targetProps = targetType.GetProperties();
            var targetFields = targetType.GetFields();
            if (targetProps.Where(p => p.Name == propOrFieldName).Any() ||
                targetFields.Where(f => f.Name == propOrFieldName).Any())
            {
                var targetParamExpr = Expression.Parameter(typeof(object), "target");
                var targetExpr = Expression.TypeAs(targetParamExpr, targetType);

                Expression propOrFieldExpr = isArrayExpression
                    ? Expression.ArrayAccess(
                        Expression.PropertyOrField(targetExpr, propOrFieldName),
                        Expression.Constant(int.Parse(propAndIndex[1]), typeof(int)))
                    : Expression.PropertyOrField(targetExpr, propOrFieldName);

                var toStringExpr = Expression.Call(propOrFieldExpr, "ToString", null, null);
                return Expression.Lambda<Func<object, string>>(toStringExpr, targetParamExpr);
            }
            else
            {
                throw new ArgumentException($"Object of type '{targetType}'" +
                    $" does not contain property/field '{expression}'");
            }
        }
    }
}
