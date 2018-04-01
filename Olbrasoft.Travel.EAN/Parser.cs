using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Olbrasoft.Travel.EAN
{
    public class Parser<TEan> : BaseParser<TEan>, IParser<TEan> where TEan : class, new()
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


        public override bool TryParse(string line, out TEan entita)
        {
            var items = Split(line);
            if (items.Count() != PropertiesNames.Length)
                throw new ArgumentException($"Count properties { nameof(items)} not corespondent with {nameof(PropertiesNames)}!");

            entita = new TEan();
            var counter = 0;
            foreach (var propertiesName in PropertiesNames)
            {
                var property = entita.GetType().GetProperty(propertiesName);
                if (property == null)
                {
                    return false;
                }

                var t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (!string.IsNullOrEmpty(items[counter]))
                {
                    if (t != typeof(bool))
                    {
                        property.SetValue(entita, Convert.ChangeType(items[counter], t,
                            CultureInfo.InvariantCulture.NumberFormat), null);
                    }
                    else
                    {
                        if (items[counter] == "1") property.SetValue(entita, true);
                    }
                }

                counter++;
            }

            return Validator.TryValidateObject(entita, new ValidationContext(entita), null, true);

        }

       
    }





}


