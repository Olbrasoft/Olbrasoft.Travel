using System.Collections.Generic;

namespace Olbrasoft.Travel.EAN
{
    public abstract class BaseParser<TEan> : IParser<TEan> where TEan : class, new()
    {
        public abstract bool TryParse(string line, out TEan entita);
        
        public IEnumerable<TEan> Parse(IEnumerable<string> lines)
        {
            var entities = new Queue<TEan>();
            foreach (var line in lines)
            {
                if (TryParse(line, out var entity))
                {
                    entities.Enqueue(entity);
                }
            }

            return entities;
        }
    }
}