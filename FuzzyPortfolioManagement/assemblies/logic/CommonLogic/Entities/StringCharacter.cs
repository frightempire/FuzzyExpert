namespace CommonLogic.Entities
{
    public class StringCharacter
    {
        public StringCharacter(char symbol, int position)
        {
            Symbol = symbol;
            Position = position;
        }

        public char Symbol { get; }

        public int Position { get; }
    }
}
