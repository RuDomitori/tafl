using System;

namespace TAFL
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = Lexer.Analyze(new[]
            {
                "fn a(a,b) while a > 0 {" +
                "var a;" +
                "}"
            });
            foreach (var lexeme in res)
            {
                Console.WriteLine(lexeme.Type);
            }
            var ops = OPSGenerator.GenerateOPS(res);
            
            foreach (var elem in ops)
            {
                Console.WriteLine(elem.Type);
            }
        }
    }
}