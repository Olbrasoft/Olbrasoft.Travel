namespace Olbrasoft.Travel.EAN
{
    public class ParserFactory : IParserFactory
    {
        public IParser<TEan> Create<TEan>(string firstLine) where TEan : class, new()
        {
            return new Parser<TEan>(firstLine);
        }
    }
}