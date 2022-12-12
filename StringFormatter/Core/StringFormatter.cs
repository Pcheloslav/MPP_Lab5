using System.Text;
using Core.Cache;

namespace Core
{
    public class StringFormatter : IStringFormatter
    {
        private static readonly StringFormatter s_singleton = new();

        private const int _error = 0, _letters = 1, _dig = 2, _und = 3, _ocb = 4, _ccb = 5, _osb = 6, _csb = 7;

        private readonly IExpressionCache _cache = new ExpressionCache();
        private readonly int[,] _transitions =
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 1, 1, 2, 3, 1, 1 },
            { 0, 4, 0, 0, 1, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 4, 4, 4, 0, 1, 5, 0 },
            { 0, 0, 6, 0, 0, 0, 0, 0 },
            { 0, 0, 6, 0, 0, 0, 0, 7 },
            { 0, 0, 0, 0, 0, 1, 0, 0 }
        };

        public static string Format(string template, object target)
        {
            return s_singleton.FormatString(template, target);
        }

        private StringFormatter()
        {
        }

        public string FormatString(string template, object target)
        {
            int currentState = 1, previousState, expressionStart = 0;
            var builder = new StringBuilder(template.Length);
            for (int i = 0; i < template.Length; i++)
            {
                if (Char.IsWhiteSpace(template[i]))
                {
                    builder.Append(template[i]);
                    continue;
                }

                previousState = currentState;
                currentState = _transitions[currentState, GetTransitionStep(template[i])];

                switch (currentState)
                {
                    case 1:
                        if (previousState == 4 || previousState == 7)
                        {
                            // Completed collecting the property/field (4) or indexed property/field (7)
                            builder.Append(_cache.Evaluate(target, template[expressionStart..i])); 
                        }
                        else
                        {
                            builder.Append(template[i]);
                        }
                        break;
                    case 4:
                        if (previousState == 2)
                        {
                            // Start collecting property/field or indexed property/field
                            expressionStart = i;
                        }
                        break;
                    case 0:
                        {
                            throw new ArgumentException($"Invalid template character '{template[i]}' at position {i}");
                        }
                }
            }
            if (currentState == 1)
            {
                return builder.ToString();
            }
            else
            {
                throw new ArgumentException($"Invalid template at position {template.Length - 1}");
            }
        }

        private static int GetTransitionStep(char c)
        {
            if (c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z')
            {
                return _letters;
            }
            else if (c >= '0' && c <= '9')
            {
                return _dig;
            }
            else if (c == '{')
            {
                return _ocb;
            }
            else if (c == '}')
            {
                return _ccb;
            }
            else if (c == '[')
            {
                return _osb;
            }
            else if (c == ']')
            {
                return _csb;
            }
            else if (c == '_')
            {
                return _und;
            }
            return _error;
        }
    }
}
