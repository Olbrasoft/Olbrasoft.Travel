using System.Collections.Generic;

namespace Olbrasoft.Travel.EAN
{
    public interface IParser<TEan> where TEan : class, new()
    {
        bool TryParse(string line, out TEan entita);
        IEnumerable<TEan> Parse(IEnumerable<string> lines);
    }
}