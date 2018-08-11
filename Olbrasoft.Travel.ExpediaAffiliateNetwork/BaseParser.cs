using System.Collections.Generic;

namespace Olbrasoft.Travel.ExpediaAffiliateNetwork
{
    public abstract class BaseParser<TEan> : IParser<TEan> where TEan : class, new()
    {
        public abstract TEan Parse(string[] items);

        public abstract bool TryParse(string line, out TEan entita);
        
        public IEnumerable<TEan> ParseAll(IEnumerable<string> lines)
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