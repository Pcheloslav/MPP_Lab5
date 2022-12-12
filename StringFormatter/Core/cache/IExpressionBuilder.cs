using System.Linq.Expressions;

namespace Core.Cache
{
    internal interface IExpressionBuilder
    {
        Expression<Func<object, string>> Build(Type targetType, string expression);
    }
}
