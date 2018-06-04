using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tt_apps_srs.Models
{
    public class Auditor: IAuditor
    {
        private readonly ElasticClient _client;

        public Auditor(string connectionString)
        {
            var connectionConfiguration = new ConnectionSettings(new Uri(connectionString))
                                        .DefaultMappingFor<Audit>(i => i
                                                                        .IndexName("tt-apps-srs")
                                                                        .TypeName("db_audit"));

            _client = new ElasticClient(connectionConfiguration);

        }

        public async void Create(Audit audit)
        {
            await _client.IndexDocumentAsync(audit);
        }

        public async Task<IEnumerable<Audit>> GetAllEntriesAsync(object object_id, ushort from = 0, ushort number_of_entries = 10)
        {
            var searchResponse = await _client.SearchAsync<Audit>(s =>
                                                                s.Query(q => q
                                                                           .Term(t => t
                                                                              .Field("keyValues.Id")
                                                                              .Value(object_id.ToString())
                                                                           )
                                                                           
                                                                 )
                                                                 .Sort(ss => ss
                                                                        .Descending( d => d.DateTime)
                                                                 )
                                                                 .From( from )
                                                                 .Size( number_of_entries)
                                 );

            return searchResponse.Documents;
        }

        public async Task<ObjectTimestamps> CreatedUpdatedDatesAsync<T>(object object_id)
        {
            ObjectTimestamps retval;

            // Compute the table name from the Type parameter
            // 1. Get the name of the type as string
            // 2. Extract the last portion of type; format is usually namespace.model
            // 3. Pluralize the model name because table name in db are pluralized and 
            //      hence the table name in the audit entry will be pluralized as well
            string tableName = typeof(T).ToString();
            tableName = tableName.Substring(tableName.LastIndexOf('.') + 1);
            tableName += "s";

            var searchResponse = await _client.SearchAsync<Audit>(s =>
                                                                s.Query(q => q
                                                                            .Bool( b => b
                                                                                        .Must( mt => mt
                                                                                                .Match( mch => mch
                                                                                                                .Field( f => f.TableName)
                                                                                                                .Query(tableName))
                                                                                         )
                                                                                         .Filter( fltr => fltr
                                                                                                .Term( t => t
                                                                                                    .Field("keyValues.Id")
                                                                                                    .Value(object_id.ToString())))))
                                                                    .Aggregations( agg => agg
                                                                                    . Max("LastUpdatedOn", mx => mx.Field( f => f.DateTime))
                                                                                    .Min("CreatedOn", mn => mn.Field( f => f.DateTime))
                                                                    )
                               );

            try
            {
                var objectAggregations = searchResponse.Aggregations;

                retval = new ObjectTimestamps
                {
                    LastUpdatedOn = Convert.ToDateTime(objectAggregations.Min("CreatedOn").ValueAsString),
                    CreatedOn = Convert.ToDateTime(objectAggregations.Max("LastUpdatedOn").ValueAsString)

                };

            }
            catch {
                retval = new ObjectTimestamps
                {
                    LastUpdatedOn = null,
                    CreatedOn = null

                };
            };

            return retval;
        }
    }

 
}
