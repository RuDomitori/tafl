using System;

namespace TAFL
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = Lexer.Analyze(new[]
            {
                "fn t(a, b, c, d) while(a > 0){" +
                "var ed;" +
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