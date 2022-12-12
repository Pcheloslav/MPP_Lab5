using System.Linq.Expressions;

namespace Core.cache
{
    internal interface IExpressionBuilder
    {
        Expression<Func<object, string>> Build(Type targetType, string expression);
    }
}
