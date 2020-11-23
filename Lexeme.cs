using System.Threading;

namespace TAFL
{
    public abstract class Lexeme
    {
        public readonly int Line;
        public readonly int Start;
        //public abstract Lexeme NextLexeme(char ch);
        public Lexeme(int line, int start)
        {
            this.Line = line;
            this.Start = start;
        }
        
        public class Identifier: Lexeme
        {
            public readonly string Word;
            public Identifier(int line, int start, string word): base(line, start) => 
                this.Word = word;
            
        }
        public class Const: Lexeme{
        
        }
        public class KeyWord: Lexeme{
        
        }

        //Операции
        public class Summation: Lexeme{
        
        }
        public class Subtraction: Lexeme{
        
        }
        public class Multiplication: Lexeme{
        
        }
        public class Division: Lexeme{
        
        }
        public class Assignment: Lexeme{
        
        }

        //Сравнения
        public class EqualTo: Lexeme{
        
        }
        public class NotEqualTo: Lexeme{
        
        }
        public class GreaterThan: Lexeme{
        
        }
        public class LessThan: Lexeme{
        
        }
        public class GreaterThanOrEqualTo: Lexeme{
        
        }
        public class LessThanOrEqualTo: Lexeme{
        
        }

        // Логические операции
        public class Or: Lexeme{
        
        }
        public class And: Lexeme{
        
        }
        public class Not: Lexeme{
        
        }

        //Скобки
        public class LeftRoundBracket: Lexeme{
        
        }
        public class RightRoundBracket: Lexeme{
        
        }
        public class LeftSquareBracket: Lexeme{
        
        }
        public class RightSquareBracket: Lexeme{
        
        }
        public class LeftCurlyBracket: Lexeme{
        
        }
        public class RightCurlyBracket: Lexeme{
        
        }

        public class Semicolon: Lexeme{
        
        }
        public class Comma: Lexeme{
        
        }
        public class ReturnType: Lexeme{
        
        }

        public class Error : Lexeme
        {
            public readonly string Message;

            public Error(int line, int start, string message): base(line, start) => 
                this.Message = message;

        }
    }
    
        
    
}