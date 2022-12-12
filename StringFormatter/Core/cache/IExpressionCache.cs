namespace Core.cache
{
    internal interface IExpressionCache
    {
        string Evaluate(object target, string expression);
    }
}
