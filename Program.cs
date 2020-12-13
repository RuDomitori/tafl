using System;

namespace TAFL
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var res = Lexer.Analyze(new[]
            {
                "fn avi(_+==_)",
                "asd 0 / 0.-"
            });

            foreach (var lexeme in res)
            {
                Console.WriteLine(lexeme.Type);
            }
        }
    }
}