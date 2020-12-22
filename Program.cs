using System;
using System.IO;

namespace TAFL
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                Console.WriteLine("Не указан файл с кодом");
                return;
            }
            if (!(new FileInfo(args[0]).Exists)) {
                Console.WriteLine("Файл не найден");
                return;
            }
            var lines = File.ReadAllLines(args[0]);
            var res = Lexer.Analyze(lines);
            var ops = OPSGenerator.GenerateOPS(res);
            
            Interpreter.Interpret(ops);
        }
    }
}