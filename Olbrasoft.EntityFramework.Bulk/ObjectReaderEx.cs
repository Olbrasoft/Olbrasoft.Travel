using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Reflection;
using FastMember;
using Microsoft.SqlServer.Types;

namespace Olbrasoft.EntityFramework.Bulk
{
    internal class ObjectReaderEx : ObjectReader // Overridden to fix ShadowProperties in FastMember library
    {
        private readonly HashSet<string> _shadowProperties;
        private readonly DbContext _context;
        private string[] _members;
        private FieldInfo _current;

        
        public static ObjectReaderEx Create<T>(IEnumerable<T> source, HashSet<string> shadowProperties, DbContext context, params string[] members)
        {
            bool hasShadowProp = shadowProperties.Count > 0;
            return  (hasShadowProp ? 
                new ObjectReaderEx(typeof(T), source, shadowProperties, context, members) : Create(source, members));
        }

        /// <summary>
        /// Creates a new ObjectReader instance for reading the supplied data
        /// </summary>
        /// <param name="source">The sequence of objects to represent</param>
        /// <param name="members">The members that should be exposed to the reader</param>
        public new static ObjectReaderEx Create<T>(IEnumerable<T> source, params string[] members)
        {
            return new ObjectReaderEx(typeof(T), source, members);
        }

        public ObjectReaderEx(Type type, IEnumerable source, HashSet<string> shadowProperties, DbContext context, params string[] members) : base(type, source, members)
        {
            _shadowProperties = shadowProperties;
            _context = context;
            _members = members;
            _current = typeof(ObjectReader).GetField("current", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public ObjectReaderEx(Type type, IEnumerable source, params string[] members) : base(type, source, members)
        {
            _members = members;
            _shadowProperties= new HashSet<string>();
        }

        public override object GetValue(int i)
        {
            var value = base.GetValue(i);

            if (value is DbGeography dbgeo)
            {
                var chars = new SqlChars(dbgeo.WellKnownValue.WellKnownText);
                return SqlGeography.STGeomFromText(chars, dbgeo.CoordinateSystemId);
            }

            if (value is DbGeometry dbgeom)
            {
                var chars = new SqlChars(dbgeom.WellKnownValue.WellKnownText);
                return SqlGeometry.STGeomFromText(chars, dbgeom.CoordinateSystemId);
            }

            return value;

        }

        public override object this[string name]
        {
            get
            {
                if (_shadowProperties.Contains(name))
                {
                    var current = _current.GetValue(this);
                    return _context.Entry(current).Property(name).CurrentValue;
                }
                return base[name];
            }
        }

        public override object this[int i]
        {
            get
            {
                var name = _members[i];
                return this[name];
            }
        }

        

       
    }
}
