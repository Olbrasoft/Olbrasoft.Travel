namespace Olbrasoft.Travel.EAN
{
    public interface IParserFactory
    {
        IParser<TEan> Create<TEan>(string firstLine) where TEan : class, new();
    }
}
