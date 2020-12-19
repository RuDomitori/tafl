using System;
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
            Float1,
            Float,
            Summation,   //сложение
            Subtraction,    //вычитание
            Multiplication,
            Division,
            Assignment,     //присваивание
            LeftRoundBracket,
            RightRoundBracket,
            EndOfOperation,     // ;
            LeftSquareBracket,
            RightSquareBracket,
            Comma,          //запятая
            Not,        // !
            GreaterThan,       //>
            LessThan,       //<
            Or,
            And,
            LeftCurlyBracket,    // {
            RightCurlyBracket,    // }
            EqualTo,   // ==
            NotEqualTo,   // !=
            GreaterThanOrEqualTo,     // >=
            LessThanOrEqualTo,
            Z,
            Error,
            End,
        }

        private enum CharType
        {
            Letter,
            Digit,
            Space,
            Underscore, // _
            Summation,   // +
            Subtraction,    // -
            Multiplication, // *
            Division,  // /
            Assignment,  // =
            LeftRoundBracket, // (
            RightRoundBracket, // )
            Semicolon,     // ;
            LeftSquareBracket, // [
            RightSquareBracket, // ]
            Comma,          // ,
            ExclamationMark,        // !
            Greater,       // >
            Less,       // <
            VerticalSlash, // |
            Ampersand, // &
            LeftCurlyBracket,    // {
            RightCurlyBracket, // }
            Dot, // .
            End,    // EoF
            Unknown
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
                ')' => CharType.RightRoundBracket,
                ';' => CharType.Semicolon,
                '[' => CharType.LeftSquareBracket,
                ']' => CharType.RightSquareBracket,
                ',' => CharType.Comma,
                '!' => CharType.ExclamationMark,
                '>' => CharType.Greater,
                '<' => CharType.Less,
                '|' => CharType.VerticalSlash,
                '&' => CharType.Ampersand,
                '{' => CharType.LeftCurlyBracket,
                '}' => CharType.RightCurlyBracket,
                '_' => CharType.Underscore,
                '.' => CharType.Dot,
                 _  => CharType.Unknown,
            };
        }

        private delegate Lexeme SemanticAction(int line, int start, int end, string word);

        private static readonly SemanticAction[] SemanticActions =
        {
            null,
            (line, start, end, word) => word switch
            {
                "read" => new Lexeme(LexemeType.Read, line, start, end),
                "write" => new Lexeme(LexemeType.Write, line, start, end),
                "while" => new Lexeme(LexemeType.While, line, start, end),
                "if" => new Lexeme(LexemeType.If, line, start, end),
                "else" => new Lexeme(LexemeType.Else, line, start, end),
                "fn" => new Lexeme(LexemeType.Fn, line, start, end),
                "return" => new Lexeme(LexemeType.Return, line, start, end),
                "var" => new Lexeme(LexemeType.Var, line, start, end),
                _ => new Lexeme(LexemeType.Identifier, line, start, end, word)
            },
            (line, start, end, word) => new Lexeme(LexemeType.Const,line, start, end, value: Double.Parse(word)),
            (line, start, end, word) => new Lexeme(LexemeType.Const,line, start, end, value: Double.Parse(word)),
            (line, start, end, word) => new Lexeme(LexemeType.Summation,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Subtraction,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Multiplication,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Division,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Assignment,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.LeftRoundBracket,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.RightRoundBracket,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Semicolon,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.LeftSquareBracket,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.RightSquareBracket,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Comma,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Not,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.GreaterThan,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.LessThan,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Or,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.And,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.LeftCurlyBracket,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.RightCurlyBracket,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.EqualTo,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.NotEqualTo,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.GreaterThanOrEqualTo,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.LessThanOrEqualTo,line, start, end),
            (line, start, end, word) => new Lexeme(LexemeType.Error,line, start, end, "Неверно задано число"),
            (line, start, end, word) => new Lexeme(LexemeType.Error, line, start, end, "Символ неопознан")
        };

        private static readonly (int, int, bool)[,] _table =
        {
            {( 1, 0, false ),	( 2, 0, false ),	( 27, 0, false ),	( 1, 0, false ),	( 5, 0, false ),	( 6, 0, false ),	( 7, 0, false ),	( 8, 0, false ),	( 9, 0, false ),	( 10, 0, false ),	( 11, 0, false ),	( 12, 0, false ),	( 13, 0, false ),	( 14, 0, false ),	( 15, 0, false ),	( 16, 0, false ),	( 17, 0, false ),	( 18, 0, false ),	( 19, 0, false ),	( 20, 0, false ),	( 21, 0, false ),	( 22, 0, false ),	( 28, 26, false ),	( 27, 0, false ),	( 28, 27, false )},
            {( 1, 0, false ),	( 1, 0, false ),	( 27, 1, false ),	( 1, 0, false ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, true ),	( 27, 1, false ),	( 28, 27, false )},
            {( 27, 2, true ),	( 2, 0, false ),	( 27, 2, false ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 27, 2, true ),	( 3, 0, false ),	( 27, 2, false ),	( 28, 27, false )},
            {( 28, 26, true ),	( 4, 0, false ),	( 28, 26, false ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, true ),	( 28, 26, false ),	( 28, 26, false )},
            {( 27, 3, true ),	( 4, 0, false ),	( 27, 3, false ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, true ),	( 27, 3, false ),	( 28, 27, false )},
            {( 27, 4, true ),	( 27, 4, true ),	( 27, 4, false ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, true ),	( 27, 4, false ),	( 28, 27, false )},
            {( 27, 5, true ),	( 27, 5, true ),	( 27, 5, false ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, true ),	( 27, 5, false ),	( 28, 27, false )},
            {( 27, 6, true ),	( 27, 6, true ),	( 27, 6, false ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, true ),	( 27, 6, false ),	( 28, 27, false )},
            {( 27, 7, true ),	( 27, 7, true ),	( 27, 7, false ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, true ),	( 27, 7, false ),	( 28, 27, false )},
            {( 27, 8, true ),	( 27, 8, true ),	( 27, 8, false ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 23, 0, false ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, true ),	( 27, 8, false ),	( 28, 27, false )},
            {( 27, 9, true ),	( 27, 9, true ),	( 27, 9, false ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, true ),	( 27, 9, false ),	( 28, 27, false )},
            {( 27, 10, true ),	( 27, 10, true ),	( 27, 10, false ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, true ),	( 27, 10, false ),	( 28, 27, false )},
            {( 27, 11, true ),	( 27, 11, true ),	( 27, 11, false ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, true ),	( 27, 11, false ),	( 28, 27, false )},
            {( 27, 12, true ),	( 27, 12, true ),	( 27, 12, false ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, true ),	( 27, 12, false ),	( 28, 27, false )},
            {( 27, 13, true ),	( 27, 13, true ),	( 27, 13, false ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, true ),	( 27, 13, false ),	( 28, 27, false )},
            {( 27, 14, true ),	( 27, 14, true ),	( 27, 14, false ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, true ),	( 27, 14, false ),	( 28, 27, false )},
            {( 27, 15, true ),	( 27, 15, true ),	( 27, 15, false ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 24, 0, false ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, true ),	( 27, 15, false ),	( 28, 27, false )},
            {( 27, 16, true ),	( 27, 16, true ),	( 27, 16, false ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 25, 0, false ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, true ),	( 27, 16, false ),	( 28, 27, false )},
            {( 27, 17, true ),	( 27, 17, true ),	( 27, 17, false ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 26, 0, false ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, true ),	( 27, 17, false ),	( 28, 27, false )},
            {( 27, 18, true ),	( 27, 18, true ),	( 27, 18, false ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, true ),	( 27, 18, false ),	( 28, 27, false )},
            {( 27, 19, true ),	( 27, 19, true ),	( 27, 19, false ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, true ),	( 27, 19, false ),	( 28, 27, false )},
            {( 27, 20, true ),	( 27, 20, true ),	( 27, 20, false ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, true ),	( 27, 20, false ),	( 28, 27, false )},
            {( 27, 21, true ),	( 27, 21, true ),	( 27, 21, false ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, true ),	( 27, 21, false ),	( 28, 27, false )},
            {( 27, 22, true ),	( 27, 22, true ),	( 27, 22, false ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, true ),	( 27, 22, false ),	( 28, 27, false )},
            {( 27, 23, true ),	( 27, 23, true ),	( 27, 23, false ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, true ),	( 27, 23, false ),	( 28, 27, false )},
            {( 27, 24, true ),	( 27, 24, true ),	( 27, 24, false ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, true ),	( 27, 24, false ),	( 28, 27, false )},
            {( 27, 25, true ),	( 27, 25, true ),	( 27, 25, false ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, true ),	( 27, 25, false ),	( 28, 27, false )},
        };

        private static (int, int, bool) _GetCell(State state, CharType charType) => _table[(int) state, (int) charType];
        
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
            State nextState;
            int actionNumber;
            bool b;
            Lexeme lexeme;
            
            for(int i = 0; i < line.Length; i++)
            {
                var charType = TypeOf(line[i]);
                var tuple = _GetCell(currentState, charType);
                (nextState, actionNumber, b) = ((State) tuple.Item1, tuple.Item2, tuple.Item3);
                
                lexeme = SemanticActions[actionNumber]
                    ?.Invoke(
                        lineNumber,
                        start, 
                        i,
                        line.Substring(start, i - start)
                        );
                if (lexeme != null) lexemes.Add(lexeme);
                
                if (b) i--;
                
                switch (nextState)
                {
                    case State.Error:
                        goto case State.Z;
                    case State.Z:
                        currentState = State.S;
                        start = i;
                        break;
                    default:
                        currentState = nextState;
                        break;
                }
            }
            
            (_, actionNumber, _) = _GetCell(currentState, CharType.End);
            
            lexeme = SemanticActions[actionNumber]?.Invoke(
                lineNumber,
                start,
                line.Length,
                line.Substring(start, line.Length - start)
                );
            if (lexeme != null) lexemes.Add(lexeme);

            return lexemes;
        }
    }
}
