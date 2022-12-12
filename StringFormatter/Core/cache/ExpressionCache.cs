using System.Collections.Concurrent;

namespace Core.Cache
{
    internal class ExpressionCache : IExpressionCache
    {
        private readonly IExpressionBuilder _expressionBuilder = new ExpressionBuilder();
        private readonly IDictionary<string, Func<object, string>> _cache =
            new ConcurrentDictionary<string, Func<object, string>>();

        public string Evaluate(object target, string expression)
        {
            var expressionKey = $"{target.GetType()}.{expression}";
            if (!_cache.TryGetValue(expressionKey, out var funcLambda))
            {
                var funcLambdaExpr = _expressionBuilder.Build(target.GetType(), expression);
                funcLambda = funcLambdaExpr.Compile();
                _cache.TryAdd(expressionKey, funcLambda);
            }
            return funcLambda(target);
        }
    }
}
