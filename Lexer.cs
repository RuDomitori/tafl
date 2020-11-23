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
            Z,
            Error
        }

        private enum CharType
        {
            Letter,
            Digit,
            Underscores,
            End,
            Unknown
        }

        private static CharType TypeOf(char ch)
        {
            if (char.IsLetter(ch)) return CharType.Letter;
            if (char.IsDigit(ch)) return CharType.Digit;
            return ch switch
            {
                '_' => CharType.Underscores,
                _ => CharType.Unknown
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
            switch (_table[(int) currentState, (int) CharType.End])
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