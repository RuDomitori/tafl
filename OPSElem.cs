namespace TAFL
{
    public class OPSElem
    {
        public readonly int Type;
        public readonly string Word;
        public readonly double Value;

        public OPSElem(int type, string word = null, double value = 0)
        {
            Type = type;
            Word = word;
            Value = value;
        }
        
    }
}