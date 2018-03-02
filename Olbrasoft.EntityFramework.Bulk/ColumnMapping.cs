﻿
using System.Diagnostics;

namespace Olbrasoft.EntityFramework.Bulk
{
    [DebuggerDisplay("NameOnObject = {NameOnObject} NameInDatabase = {NameInDatabase}")]
    public class ColumnMapping
    {
        public string NameOnObject { get; set; }
        public string StaticValue { get; set; }
        public string NameInDatabase { get; set; }
        public string DataType { get; set; }
        public bool IsPrimaryKey { get; set; }
    }
}
