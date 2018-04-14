using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Olbrasoft.EntityFramework.Bulk;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DAL.EntityFramework;
using Attribute = Olbrasoft.Travel.EAN.DTO.Property.Attribute;

namespace Olbrasoft.Travel.EAN.Import
{
    class AttributesImporter : Importer
    {
        public AttributesImporter(IProvider provider, IFactoryOfRepositories factoryOfRepositories, SharedProperties sharedProperties, ILoggingImports logger)
            : base(provider, factoryOfRepositories, sharedProperties, logger)
        {

        }

        private Queue<Travel.EAN.DTO.Property.Attribute> _attributes = new Queue<Attribute>();

        protected override void RowLoaded(string[] items)
        {
            var attribute = new Attribute
            {
                AttributeID= int.Parse(items[0]),
                LanguageCode= items[1],
                AttributeDesc = items[2],
                Type= items[3],
                SubType= items[4]
            };

            _attributes.Enqueue(attribute);
        }

        public override void Import(string path)
        {
            LoadData(path);

            using (var ctx= new Travel.DAL.EntityFramework.TravelContext())
            {
                ctx.BulkInsert(_attributes);
            }
        }
    }
}
