namespace Core
{
    public interface IStringFormatter
    {
        string FormatString(string template, object target);
    }
}
