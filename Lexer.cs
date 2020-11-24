using System.Collections.Generic;

namespace TAFL
{
    public static class Lexer
    {
        private enum State
        {
            S,
            Identifier,
            Number,
            Float,
            Summationn,   //сложение
            Subtraction,    //вычитание
            Multiplication,
            Division,
            Assignment,     //присваивание
            LeftRoundBracket,
            RightRoundgBracket,
            EndOfOperation,     // ;
            LeftSquareBracket,
            RightSquareBracket,
            Comma,          //запятая
            Not,        // !
            More,       //>
            Less,       //<
            Or,
            And,
            LeftCurleBracket,    // {
            RightCurleBracket,    // }
            EndSign,    //знак конца Ʇ (перевернутая Т) 
            Equality,   // ==
            NotEqual,   // !=
            MoreOrEqual,     // >=
            LessOrEqual,
            Z,
            Error
        }

        private enum CharType
        {
            Letter,
            Digit,
            Underline,
            EndSign,    //знак конца Ʇ (перевернутая Т) 
            Unknown,
            Space,
            Summation,   //сложение
            Subtraction,    //вычитание
            Multiplication,
            Division,
            Assignment,  //присваивание
            LeftRoundBracket,
            RightRoundgBracket,
            Semicolon,     // ;
            LeftSquareBracket,
            RightSquareBracket,
            Comma,          //запятая
            Not,        // !
            More,       //>
            Less,       //<
            Or,
            And,
            LeftCurleBracket,    // {
            RightCurleBracket,
            Z,
            Error
        }

        private static CharType TypeOf(char ch)
        {
            if (char.IsLetter(ch)) return CharType.Letter;
            if (char.IsDigit(ch)) return CharType.Digit;
            if (char.IsWhiteSpace(ch)) return CharType.Space;
            return ch switch
            {
                '+' => CharType.Summation,
                '-' => CharType.Subtraction,
                '*' => CharType.Multiplication,
                '/' => CharType.Division,
                '=' => CharType.Assignment,
                '(' => CharType.LeftRoundBracket,
                ')' => CharType.RightRoundgBracket,
                ';' => CharType.Semicolon,
                '[' => CharType.LeftSquareBracket,
                ']' => CharType.RightSquareBracket,
                ',' => CharType.Comma,
                '!' => CharType.Not,
                '>' => CharType.More,
                '<' => CharType.Less,
                '|' => CharType.Or,
                '&' => CharType.And,
                '{' => CharType.LeftCurleBracket,
                '}' => CharType.RightCurleBracket,
                '_' => CharType.Underline,
                 _  => CharType.Unknown,
            };
        }

        private delegate Lexeme LexemeBuilder(int line, int start, string word);

        private static LexemeBuilder[] _lexemeBuilders =
        {
            null,
            (line, start, word) => new Lexeme.Identifier(line, start, word), 
        };
        
        private static readonly State[,] _table =
        {
            {State.Identifier, State.Number},
            {State.Identifier, State.Identifier}
        };

        public static List<Lexeme> Analyze(IEnumerable<string> lines)
        {
            var lexemes = new List<Lexeme>();
            
            int lineNumber = 0;
            foreach (var line in lines)
            {
                lexemes.AddRange(AnalyzeLine(line, lineNumber));
                lineNumber++;
            }

            return lexemes;
        }

        public static List<Lexeme> AnalyzeLine(string line, int lineNumber)
        {
            var lexemes = new List<Lexeme>();
            
            State currentState = State.S;
            
            int start = 0;
            int end = 0;
            foreach (var ch in line)
            {
                end++;
                var nextState = _table[(int) currentState, (int) TypeOf(ch)];
                switch (nextState)
                {
                    case State.Error:
                        lexemes.Add(
                            new Lexeme.Error(lineNumber, start, "Ошибка")
                        );
                        currentState = State.S;
                        start = end;
                        break;
                    case State.Z:
                        lexemes.Add(
                            _lexemeBuilders[(int) currentState](lineNumber, start, line.Substring(start,end))
                        );
                        currentState = State.S;
                        start = end;
                        break;
                    default:
                        currentState = nextState;
                        break;
                }
            }

            if (currentState == State.S) return lexemes;
            switch (_table[(int) currentState, (int) CharType.EndSign])
            {
                case State.Z:
                    lexemes.Add(
                        _lexemeBuilders[(int) currentState](lineNumber, start, line.Substring(start, end))
                    );
                    break;
                case State.Error:
                    lexemes.Add(
                        new Lexeme.Error(lineNumber, start, "Ошибка")
                    );
                    break;
            }

            return lexemes;
        }
    }
}
