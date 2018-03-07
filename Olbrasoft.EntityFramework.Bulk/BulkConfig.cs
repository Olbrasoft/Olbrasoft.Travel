using System.Collections.Generic;
using System.Reflection;
using System.Security.Policy;

namespace Olbrasoft.EntityFramework.Bulk
{
    public class BulkConfig
    {
        public bool PreserveInsertOrder { get; set; }

        public bool SetOutputIdentity { get; set; }

        public int BatchSize { get; set; } = 2000;

        public int? NotifyAfter { get; set; }

        public int? BulkCopyTimeout { get; set; }

        public bool EnableStreaming { get; set; }

        public bool UseTempDb { get; set; }

        public bool KeepIdentity { get; set; }

        public HashSet<string> IgnoreColumns { get; set; } = new HashSet<string>();
        public HashSet<string> IgnoreColumnsUpdate { get; set; } = new HashSet<string>();


    }
}
