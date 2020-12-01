using System;
using System.Threading;

namespace TAFL
{
    public abstract class Lexeme
    {
        public readonly int Line;
        public readonly int Start;
        public readonly int End;
        //public abstract Lexeme NextLexeme(char ch);
        public Lexeme(int line, int start, int end)
        {
            this.Line = line;
            this.Start = start;
            this.End = end;
        }

        public class Identifier: Lexeme
        {
            public readonly string Word;
            public Identifier(int line, int start, int end, string word): base(line, start, end) => 
                this.Word = word;

        }
        public class Const: Lexeme
        {

            public readonly double value;

            public Const(int line, int start, int end, string word) : base(line, start, end)
            {
                this.value = double.Parse(word);
            }
        }
        
        //Операции
        public class Summation: Lexeme{
            public Summation(int line, int start, int end) : base(line, start, end) {}
        }
        public class Subtraction: Lexeme{
            public Subtraction(int line, int start, int end) : base(line, start, end) {}
        }
        public class Multiplication: Lexeme{
            public Multiplication(int line, int start, int end) : base(line, start, end) {}
        }
        public class Division: Lexeme{
            public Division(int line, int start, int end) : base(line, start, end) {}
        }
        public class Assignment: Lexeme{
            public Assignment(int line, int start, int end) : base(line, start, end) {}
        }

        //Сравнения
        public class EqualTo: Lexeme{
            public EqualTo(int line, int start, int end) : base(line, start, end) {}
        }
        public class NotEqualTo: Lexeme{
            public NotEqualTo(int line, int start, int end) : base(line, start, end) {}
        }
        public class GreaterThan: Lexeme{
            public GreaterThan(int line, int start, int end) : base(line, start, end) {}
        }
        public class LessThan: Lexeme{
            public LessThan(int line, int start, int end) : base(line, start, end) {}
        }
        public class GreaterThanOrEqualTo: Lexeme{
            public GreaterThanOrEqualTo(int line, int start, int end) : base(line, start, end) {}
        }
        public class LessThanOrEqualTo: Lexeme{
            public LessThanOrEqualTo(int line, int start, int end) : base(line, start, end) {}
        }

        // Логические операции
        public class Or: Lexeme{
            public Or(int line, int start, int end) : base(line, start, end) {}
        }
        public class And: Lexeme{
            public And(int line, int start, int end) : base(line, start, end) {}
        }
        public class Not: Lexeme{
            public Not(int line, int start, int end) : base(line, start, end) {}
        }

        //Скобки
        public class LeftRoundBracket: Lexeme{
            public LeftRoundBracket(int line, int start, int end) : base(line, start, end) {}
        }
        public class RightRoundBracket: Lexeme{
            public RightRoundBracket(int line, int start, int end) : base(line, start, end) {}
        }
        public class LeftSquareBracket: Lexeme{
            public LeftSquareBracket(int line, int start, int end) : base(line, start, end) {}
        }
        public class RightSquareBracket: Lexeme{
            public RightSquareBracket(int line, int start, int end) : base(line, start, end) {}
        }
        public class LeftCurlyBracket: Lexeme{
            public LeftCurlyBracket(int line, int start, int end) : base(line, start, end) {}
        }
        public class RightCurlyBracket: Lexeme{
            public RightCurlyBracket(int line, int start, int end) : base(line, start, end) {}
        }

        //Разделители
        public class Semicolon: Lexeme{
            public Semicolon(int line, int start, int end) : base(line, start, end) {}
        }
        public class Comma: Lexeme{
            public Comma(int line, int start, int end) : base(line, start, end) {}
        }

        //Ключевые слова
        public class Read : Lexeme
        {
            public Read(int line, int start, int end) : base(line, start, end) {}
        }
        public class Write: Lexeme
        {
            public Write(int line, int start, int end) : base(line, start, end) {}
        }
        public class While: Lexeme
        {
            public While(int line, int start, int end) : base(line, start, end) {}
        }
        public class If: Lexeme
        {
            public If(int line, int start, int end) : base(line, start, end) {}
        }
        public class Else: Lexeme
        {
            public Else(int line, int start, int end) : base(line, start, end) {}
        }
        public class Var: Lexeme
        {
            public Var(int line, int start, int end) : base(line, start, end) {}
        }
        public class Fn: Lexeme
        {
            public Fn(int line, int start, int end) : base(line, start, end) {}
        }
        public class Return: Lexeme
        {
            public Return(int line, int start, int end) : base(line, start, end) {}
        }
        public class Error : Lexeme
        {
            public readonly string Message;

            public Error(int line, int start, int end, string message): base(line, start, end) => 
                this.Message = message;

        }
    }
}