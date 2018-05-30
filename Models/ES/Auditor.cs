using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nest;

namespace tt_apps_srs.Models
{
    public class Auditor: IAuditor
    {
        private readonly ElasticClient client;

        public Auditor(string connectionString)
        {
            var connectionConfiguration = new ConnectionSettings(new Uri(connectionString))
                                        .DefaultMappingFor<Audit>(i => i
                                                                        .IndexName("tt-apps-srs")
                                                                        .TypeName("db_audit"));

            client = new ElasticClient(connectionConfiguration);

        }

        public async void Create(Audit audit)
        {
            await client.IndexDocumentAsync(audit);
        }

    }

    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();

        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public Audit ToAudit()
        {
            var audit = new Audit();
            audit.Id = Guid.NewGuid();
            audit.TableName = TableName;
            audit.DateTime = DateTime.UtcNow;
            audit.KeyValues = KeyValues;
            audit.OldValues = OldValues.Count == 0 ? null : OldValues;
            audit.NewValues = NewValues.Count == 0 ? null : NewValues;
            return audit;
        }
    }
    public class Audit
    {
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }

        public Dictionary<string, object> KeyValues { get; set; }

        public Dictionary<string, object> OldValues { get; set; }
        public Dictionary<string, object> NewValues { get; set; }
    }
}
