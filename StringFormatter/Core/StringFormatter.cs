using Core.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new();

        private IExpressionCache _cache = new ExpressionCache();
        private const int _letters = 1, _ocb = 4, _ccb = 5, _dig = 2, _osb = 6, _csb = 7, _und = 3, _error = 0;

        private int[,] MatrixTransitions =
        {
            {0 ,0, 0, 0, 0, 0, 0, 0},
            {0 ,1, 1, 1, 2, 3, 1, 1},
            {0, 4, 0, 0, 1, 0, 0, 0},
            {0, 0, 0, 0, 0, 1, 0, 0},
            {0, 4, 4, 4, 0, 1, 5, 0},
            {0, 0, 6, 0, 0, 0, 0, 0},
            {0, 0, 6, 0, 0, 0, 0, 7},
            {0, 0, 0, 0, 0, 1, 0, 0},
        };

        private int WhatSymbol(char c)
        {
            if (c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == ' ')
                return _letters;
            else if (c >= '0' && c <= '9')
                return _dig;
            else if (c == '{')
                return _ocb;
            else if (c == '}')
                return _ccb;
            else if (c == '[')
                return _osb;
            else if (c == ']')
                return _csb;
            else if (c == '_')
                return _und;
            return _error;
        }

        public string Format(string template, object target)
        {
            int startPos = 0;
            int condition = 1;
            int prevCondition;
            string res = "";
            for (int i = 0; i < template.Length; i++)
            {
                prevCondition = condition;
                condition = MatrixTransitions[condition, WhatSymbol(template[i])];

                switch (condition)
                {
                    case 0:
                        throw new ArgumentException($"Invalid template, position {i}");
                    case 1:
                        if (prevCondition == 4 || prevCondition == 7)
                        {
                            res += _cache.Evaluate(target, template[startPos..i]); 
                        }
                        else
                        {
                            res += template[i];
                        }
                        break;
                    case 4:
                        if (prevCondition == 2)
                        {
                            startPos = i;
                        }
                        break;
                }
            }
            if (condition==1)
                return res;
            else
                throw new ArgumentException($"Invalid template, position {template.Length - 1}");
        }
      
    }
}
