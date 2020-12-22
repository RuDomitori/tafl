using System;
using System.IO;

namespace TAFL
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(args[0]);
            var res = Lexer.Analyze(lines);
            var ops = OPSGenerator.GenerateOPS(res);
            
            Interpreter.Interpret(ops);
        }
    }
}