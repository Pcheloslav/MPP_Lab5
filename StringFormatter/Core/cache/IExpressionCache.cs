namespace Core.Cache
{
    internal interface IExpressionCache
    {
        string Evaluate(object target, string expression);
    }
}
