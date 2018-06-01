using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tt_apps_srs.Models
{
    public interface IAuditor
    {
        void Create(Audit audit);
        Task<IEnumerable<Audit>> GetAllEntriesAsync(object object_id, ushort from, ushort number_of_entries);

        Task<ObjectTimestamps> CreatedUpdatedDatesAsync<T>(object object_id);

    }

    public class ObjectTimestamps{

        public DateTime? LastUpdatedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
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

        public Audit()
        {
            this.InitiatedBy = "Owner";
        }
        public Guid Id { get; set; }
        public string TableName { get; set; }

        public string InitiatedBy { get; set; }
        public DateTime DateTime { get; set; }

        public Dictionary<string, object> KeyValues { get; set; }

        public Dictionary<string, object> OldValues { get; set; }
        public Dictionary<string, object> NewValues { get; set; }
    }
}
