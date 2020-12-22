
using System;
using System.Collections.Generic;

namespace TAFL
{
    public class OPSGenerator
    {
        private Stack<int> _marks = new Stack<int>();
        private List<OPSElem> _OPS = new List<OPSElem>();
        private int _k => _OPS.Count;
        private Stack<int> _magazine = new Stack<int>();
        private Stack<int> _generator = new Stack<int>();
        private Lexeme _lexeme;
        
        private OPSGenerator(){}
        public static List<OPSElem> GenerateOPS(List<Lexeme> lexems)
        {
            var generator = new OPSGenerator();
            return generator._GenerateOPS(lexems);
        }

        private List<OPSElem> _GenerateOPS(List<Lexeme> lexems)
        {
            bool Step(Lexeme lexeme)
            {
                _lexeme = lexeme;
                if (_magazine.Count == 0) throw new Exception("Программа слишком длинная");
                _comands[_generator.Pop()](this);
                var current = _magazine.Pop();
                if (current >= 33)
                {
                    var ruleNumber = _table[current - 33, lexeme];
                    if (ruleNumber == -1) throw new Exception("Нет правила перехода");
                    var rule = _rules[ruleNumber];
                    for (int i = rule.Item1.Length - 1; i >= 0; i--)
                    {
                        _magazine.Push(rule.Item1[i]);
                        _generator.Push(rule.Item2[i]);
                    }
                }
                else
                {
                    if ((int) lexeme.Type != current) throw new Exception("Ожидалась другая лексема");
                    return true;
                }

                return false;
            }
            _magazine.Push(32);
            _magazine.Push(33);
            _generator.Push(0);
            _generator.Push(0);
            foreach (var lexeme in lexems)
                while (true)
                    if (Step(lexeme))
                        break;
            while (_generator.Count != 0)
            {
                var lexeme = new Lexeme(LexemeType.EoF, _lexeme.Line, _lexeme.End, _lexeme.End);
                Step(lexeme);
            }
            
            if (_magazine.Count != 0) throw new Exception("Программа закончилась преждевременно ");
            return _OPS;
        }

        #region Переходы и команды генерации
        private static int[,] _table =
        {
            {3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  9,  0,  4,  5,  6,  7,  0,  2,  1,  8,  0, },
            {-1,  -1,  -1,  -1,  -1,  -1,  11,  12,  -1,  -1,  10,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {-1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  13,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {17,  18,  -1,  14,  -1,  -1,  -1,  16,  -1,  20,  19,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  15,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {-1,  -1,  -1,  -1,  -1,  -1,  21,  -1,  -1,  22,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {23,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  24,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {-1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  27,  -1,  -1,  -1,  25,  26,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  28,  0,  0,  0,  0, },
            {-1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  29,  -1,  -1,  -1,  31,  30,  -1,  -1,  -1,  -1,  -1, },
            {-1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  32,  -1,  -1,  -1,  33,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  34,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {38,  39,  0,  35,  0,  0,  0,  37,  0,  0,  40,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  36,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  41,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {42,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  43,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {47,  48,  -1,  44,  -1,  -1,  -1,  46,  -1,  -1,  49,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  45,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  50,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {54,  55,  -1,  51,  -1,  -1,  -1,  53,  -1,  -1,  56,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  52,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  57,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {61,  62,  -1,  58,  -1,  -1,  -1,  60,  -1,  -1,  63,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  59,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  68,  69,  64,  65,  66,  67,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {73,  74,  -1,  70,  -1,  -1,  -1,  72,  -1,  -1,  75,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  71,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  76,  77,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {81,  82,  -1,  78,  -1,  -1,  -1,  80,  -1,  -1,  83,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  79,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  84,  85,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {89,  90,  -1,  86,  -1,  -1,  -1,  88,  -1,  -1,  91,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  87,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {93,  94,  -1,  -1,  -1,  -1,  -1,  92,  -1,  -1,  95,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, },
            {0,  0,  0,  0,  0,  0,  0,  96,  0,  0,  97,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {101,  102,  0,  98,  0,  0,  0,  100,  0,  0,  103,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  99,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  104,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
            {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
        };

        private static (int[], int[])[] _rules =
        {
            (new int[] { }, new int[] { }),
            (new int[] {30, 0, 7, 38, 8, 40, 64, 33}, new int[] {0, 1, 23, 0, 24, 38, 32, 35}),
            (new int[] {29, 0, 37, 33}, new int[] {0, 1, 27, 0}),
            (new int[] {0, 34, 33}, new int[] {1, 0, 0}),
            (new int[] {24, 47, 44, 9, 33}, new int[] {0, 0, 18, 0, 0}),
            (new int[] {25, 45, 9, 33}, new int[] {0, 0, 20, 0}),
            (new int[] {26, 49, 35, 33}, new int[] {36, 0, 33, 37}),
            (new int[] {27, 49, 43, 41, 33}, new int[] {0, 0, 33, 0, 35}),
            (new int[] {31, 36, 33}, new int[] {0, 0, 0}),
            (new int[] {22, 33, 23, 33}, new int[] {29, 0, 30, 0}),
            (new int[] {10, 49, 11, 48, 6, 49, 9}, new int[] {0, 0, 21, 0, 0, 0, 8}),
            (new int[] {6, 49, 9}, new int[] {0, 0, 8}),
            (new int[] {7, 62, 8, 9}, new int[] {25, 0, 26, 28}),
            (new int[] {22, 33, 23}, new int[] {29, 0, 30}),
            (new int[] {3, 60, 58, 56, 54, 52, 50, 9}, new int[] {0, 0, 5, 0, 0, 0, 0, 31}),
            (new int[] {21, 60, 58, 56, 54, 52, 50, 9}, new int[] {0, 0, 17, 0, 0, 0, 0, 31}),
            (new int[] {7, 49, 8, 58, 56, 54, 52, 50, 9}, new int[] {0, 0, 0, 0, 0, 0, 0, 0, 31}),
            (new int[] {0, 61, 58, 56, 54, 52, 50, 9}, new int[] {1, 0, 0, 0, 0, 0, 0, 31}),
            (new int[] {1, 58, 56, 54, 52, 50, 9}, new int[] {2, 0, 0, 0, 0, 0, 31}),
            (new int[] {10, 49, 11, 58, 56, 54, 52, 50, 9}, new int[] {0, 0, 22, 0, 0, 0, 0, 0, 31}),
            (new int[] {9}, new int[] {32}),
            (new int[] {6, 49, 9}, new int[] {0, 0, 8}),
            (new int[] {9}, new int[] {28}),
            (new int[] {0, 39}, new int[] {1, 0}),
            (new int[] {12, 0, 39}, new int[] {0, 1, 0}),
            (new int[] {26, 49, 35, 64}, new int[] {36, 0, 33, 37}),
            (new int[] {27, 49, 43, 41, 64}, new int[] {0, 0, 33, 0, 35}),
            (new int[] {22, 33, 23}, new int[] {29, 0, 30}),
            (new int[] {28, 42}, new int[] {34, 0}),
            (new int[] {22, 33, 23}, new int[] {29, 0, 30}),
            (new int[] {27, 49, 43, 41, 64}, new int[] {0, 0, 33, 0, 35}),
            (new int[] {26, 49, 35, 64}, new int[] {36, 0, 33, 37}),
            (new int[] {22, 33, 23}, new int[] {29, 0, 30}),
            (new int[] {26, 49, 35, 64}, new int[] {36, 0, 33, 37}),
            (new int[] {12, 47, 44}, new int[] {0, 0, 18}),
            (new int[] {3, 60, 58, 56, 54, 52, 50, 46}, new int[] {0, 0, 5, 0, 0, 0, 0, 19}),
            (new int[] {21, 60, 58, 56, 54, 52, 50, 46}, new int[] {0, 0, 15, 0, 0, 0, 0, 19}),
            (new int[] {7, 49, 8, 58, 56, 54, 52, 50, 46}, new int[] {0, 0, 0, 0, 0, 0, 0, 0, 19}),
            (new int[] {0, 61, 58, 56, 54, 52, 50, 46}, new int[] {1, 0, 0, 0, 0, 0, 0, 19}),
            (new int[] {1, 58, 56, 54, 52, 50, 46}, new int[] {2, 0, 0, 0, 0, 0, 19}),
            (new int[] {10, 49, 11, 58, 56, 54, 52, 50, 46}, new int[] {0, 0, 22, 0, 0, 0, 0, 0, 19}),
            (new int[] {12, 49, 46}, new int[] {0, 0, 19}),
            (new int[] {0, 48}, new int[] {1, 0}),
            (new int[] {10, 49, 11, 48}, new int[] {0, 0, 21, 0}),
            (new int[] {3, 60, 58, 56, 54, 52, 50}, new int[] {0, 0, 5, 0, 0, 0, 0}),
            (new int[] {21, 60, 58, 56, 54, 52, 50}, new int[] {0, 0, 15, 0, 0, 0, 0}),
            (new int[] {7, 49, 8, 58, 56, 54, 52, 50}, new int[] {0, 0, 0, 0, 0, 0, 0, 0}),
            (new int[] {0, 61, 58, 56, 54, 52, 50}, new int[] {1, 0, 0, 0, 0, 0, 0}),
            (new int[] {1, 58, 56, 54, 52, 50}, new int[] {2, 0, 0, 0, 0, 0}),
            (new int[] {10, 49, 11, 58, 56, 54, 52, 50}, new int[] {0, 0, 22, 0, 0, 0, 0, 0}),
            (new int[] {19, 51, 50}, new int[] {0, 0, 15}),
            (new int[] {3, 60, 58, 56, 54, 52}, new int[] {0, 0, 5, 0, 0, 0}),
            (new int[] {21, 60, 58, 56, 54, 52}, new int[] {0, 0, 17, 0, 0, 0}),
            (new int[] {7, 49, 8, 58, 56, 54, 52}, new int[] {0, 0, 0, 0, 0, 0, 0}),
            (new int[] {0, 61, 58, 56, 54, 52}, new int[] {1, 0, 0, 0, 0, 0}),
            (new int[] {1, 58, 56, 54, 52}, new int[] {2, 0, 0, 0, 0}),
            (new int[] {10, 49, 11, 58, 56, 54, 52}, new int[] {0, 0, 22, 0, 0, 0, 0}),
            (new int[] {20, 53, 52}, new int[] {0, 0, 16}),
            (new int[] {3, 60, 58, 56, 54}, new int[] {0, 0, 5, 0, 0}),
            (new int[] {21, 60, 58, 56, 54}, new int[] {0, 0, 17, 0, 0}),
            (new int[] {7, 49, 8, 58, 56, 54}, new int[] {0, 0, 0, 0, 0, 0}),
            (new int[] {0, 61, 58, 56, 54}, new int[] {1, 0, 0, 0, 0}),
            (new int[] {1, 58, 56, 54}, new int[] {2, 0, 0, 0}),
            (new int[] {10, 49, 11, 58, 56, 54}, new int[] {0, 0, 22, 0, 0, 0}),
            (new int[] {15, 55, 54}, new int[] {0, 0, 11}),
            (new int[] {16, 55, 54}, new int[] {0, 0, 12}),
            (new int[] {17, 55, 54}, new int[] {0, 0, 13}),
            (new int[] {18, 55, 54}, new int[] {0, 0, 14}),
            (new int[] {13, 55, 54}, new int[] {0, 0, 9}),
            (new int[] {14, 55, 54}, new int[] {0, 0, 10}),
            (new int[] {3, 60, 58, 56}, new int[] {0, 0, 5, 0}),
            (new int[] {21, 60, 58, 56}, new int[] {0, 0, 17, 0}),
            (new int[] {7, 49, 8, 58, 56}, new int[] {0, 0, 0, 0, 0}),
            (new int[] {0, 61, 58, 56}, new int[] {1, 0, 0, 0}),
            (new int[] {1, 58, 56}, new int[] {2, 0, 0}),
            (new int[] {10, 49, 11, 58, 56}, new int[] {0, 0, 22, 0, 0}),
            (new int[] {2, 57, 56}, new int[] {0, 0, 3}),
            (new int[] {3, 57, 56}, new int[] {0, 0, 4}),
            (new int[] {3, 60, 58}, new int[] {0, 0, 5}),
            (new int[] {21, 60, 58}, new int[] {0, 0, 17}),
            (new int[] {7, 49, 8, 58}, new int[] {0, 0, 0, 0}),
            (new int[] {0, 61, 58}, new int[] {1, 0, 0}),
            (new int[] {1, 58}, new int[] {2, 0}),
            (new int[] {10, 49, 11, 58}, new int[] {0, 0, 22, 0}),
            (new int[] {4, 59, 58}, new int[] {0, 0, 6}),
            (new int[] {5, 59, 58}, new int[] {0, 0, 7}),
            (new int[] {3, 60, 64}, new int[] {0, 0, 5}),
            (new int[] {21, 60, 64}, new int[] {0, 0, 17}),
            (new int[] {7, 49, 8}, new int[] {0, 0, 0}),
            (new int[] {0, 61}, new int[] {1, 0}),
            (new int[] {1}, new int[] {2}),
            (new int[] {10, 49, 11}, new int[] {0, 0, 22}),
            (new int[] {7, 49, 8}, new int[] {0, 0, 0}),
            (new int[] {0, 61}, new int[] {1, 0}),
            (new int[] {1}, new int[] {2}),
            (new int[] {10, 49, 11}, new int[] {0, 0, 22}),
            (new int[] {7, 62, 8}, new int[] {25, 0, 26}),
            (new int[] {10, 49, 11, 48}, new int[] {0, 0, 21, 0}),
            (new int[] {3, 60, 58, 56, 54, 52, 50, 63}, new int[] {0, 0, 5, 0, 0, 0, 0, 0}),
            (new int[] {21, 60, 58, 56, 54, 52, 50, 63}, new int[] {0, 0, 15, 0, 0, 0, 0, 0}),
            (new int[] {7, 49, 8, 58, 56, 54, 52, 50, 63}, new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0}),
            (new int[] {0, 61, 58, 56, 54, 52, 50, 63}, new int[] {1, 0, 0, 0, 0, 0, 0, 0}),
            (new int[] {1, 58, 56, 54, 52, 50, 63}, new int[] {2, 0, 0, 0, 0, 0, 0}),
            (new int[] {10, 49, 11, 58, 56, 54, 52, 50, 63}, new int[] {0, 0, 22, 0, 0, 0, 0, 0, 0}),
            (new int[] {12, 49, 63}, new int[] {0, 0, 0}),
        };

        private delegate void Comand(OPSGenerator con);

        private Comand[] _comands =
        {
            (con) => { },
            (con) => con._OPS.Add(new OPSElem(1, con._lexeme.Word)),
            (con) => con._OPS.Add(new OPSElem(2, value: con._lexeme.Value)),
            (con) => con._OPS.Add(new OPSElem(3)),
            (con) => con._OPS.Add(new OPSElem(4)),
            (con) => con._OPS.Add(new OPSElem(5)),
            (con) => con._OPS.Add(new OPSElem(6)),
            (con) => con._OPS.Add(new OPSElem(7)),
            (con) => con._OPS.Add(new OPSElem(8)),
            (con) => con._OPS.Add(new OPSElem(9)),
            (con) => con._OPS.Add(new OPSElem(10)),
            (con) => con._OPS.Add(new OPSElem(11)),
            (con) => con._OPS.Add(new OPSElem(12)),
            (con) => con._OPS.Add(new OPSElem(13)),
            (con) => con._OPS.Add(new OPSElem(14)),
            (con) => con._OPS.Add(new OPSElem(15)),
            (con) => con._OPS.Add(new OPSElem(16)),
            (con) => con._OPS.Add(new OPSElem(17)),
            (con) => con._OPS.Add(new OPSElem(18)),
            (con) => con._OPS.Add(new OPSElem(19)),
            (con) => con._OPS.Add(new OPSElem(20)),
            (con) => con._OPS.Add(new OPSElem(21)),
            (con) => con._OPS.Add(new OPSElem(22)),
            (con) => con._OPS.Add(new OPSElem(23)),
            (con) => con._OPS.Add(new OPSElem(24)),
            (con) => con._OPS.Add(new OPSElem(25)),
            (con) => con._OPS.Add(new OPSElem(26)),
            (con) => con._OPS.Add(new OPSElem(27)),
            (con) => con._OPS.Add(new OPSElem(28)),
            (con) => con._OPS.Add(new OPSElem(29)),
            (con) => con._OPS.Add(new OPSElem(30)),
            (con) => con._OPS.Add(new OPSElem(31)),
            (con) => con._OPS.Add(new OPSElem(32)),
            (con) =>
            {
                con._marks.Push(con._k);
                con._OPS.Add(new OPSElem(0));
                con._OPS.Add(new OPSElem(33));
            },
            (con) =>
            {
                con._OPS[con._marks.Pop()] = new OPSElem(2, value: con._k + 2);
                con._marks.Push(con._k);
                con._OPS.Add(new OPSElem(0));
                con._OPS.Add(new OPSElem(34));
            },
            (con) =>
            {
                con._OPS[con._marks.Pop()] = new OPSElem(2, value: con._k);
            },
            (con) =>
            {
                con._marks.Push(con._k);
            },
            (con) =>
            {
                con._OPS[con._marks.Pop()] = new OPSElem(2, value: con._k + 2);
                con._OPS.Add(new OPSElem(2, value:con._marks.Pop()));
                con._OPS.Add(new OPSElem(34));
            },
            (con) => {
                con._marks.Push(con._k);
                con._OPS.Add(new OPSElem(0));
                con._OPS.Add(new OPSElem(34));
            },
        };
        #endregion
    }
}