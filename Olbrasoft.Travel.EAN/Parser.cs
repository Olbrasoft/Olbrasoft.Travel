using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Olbrasoft.Travel.EAN
{
    public class Parser<TEan> : IParser<TEan> where TEan : class, new()
    {
        protected readonly string[] PropertiesNames;

        public Parser(string firstLine)
        {
            PropertiesNames = Split(firstLine);

            if (PropertiesNames.Length != typeof(TEan).GetProperties().Length)
                throw new ArgumentException(nameof(firstLine));
        }

        private static string[] Split(string s)
        {
            return s.Split('|');
        }


        public bool TryParse(string line, out TEan entita)
        {
            var items = Split(line);
            if (items.Count() != PropertiesNames.Length)
                throw new ArgumentException($"Count properties { nameof(items)} not corespondent with {nameof(PropertiesNames)}!");

            entita = new TEan();
            var counter = 0;
            foreach (var propertiesName in PropertiesNames)
            {
                var prop = entita.GetType().GetProperty(propertiesName);
                if (prop == null)
                {
                    return false;
                }
                prop.SetValue(entita, Convert.ChangeType(items[counter], prop.PropertyType), null);

                counter++;
            }

            return Validator.TryValidateObject(entita, new ValidationContext(entita), null, true);

        }

        public IEnumerable<TEan> Parse(IEnumerable<string> lines)
        {
            var entities = new HashSet<TEan>();
            foreach (var line in lines)
            {
                if (TryParse(line, out var entity))
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }
    }
}
