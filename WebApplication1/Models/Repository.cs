using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Configuration;

namespace WebApplication1.Models
{
    public class Repository
    {
        private static string _endpointUrl = ConfigurationManager.AppSettings["EndpointUrl"];
        private static string _authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];

        public async Task<Database> CreateOrGetDatabase(DocumentClient client, string databaseID)
        {
            var databases = client.CreateDatabaseQuery().Where(db => db.Id == databaseID).ToList();

            if (databases.Any())
                return databases.First();

            return await client.CreateDatabaseAsync(new Database { Id = databaseID });
        }

        public async Task<DocumentCollection> CreateOrGetCollection(DocumentClient client, Database database, string collectionID)
        {
            var collections = client.CreateDocumentCollectionQuery(database.CollectionsLink)
                                .Where(col => col.Id == collectionID).ToList();

            if (collections.Any())
                return collections.First();

            return await client.CreateDocumentCollectionAsync(database.CollectionsLink,
                new DocumentCollection
                {
                    Id = collectionID
                });
        }

        public async Task SaveBrowserAgent(string agent, string ipAddress, Uri referrer, string pageType)
        {
            // Create a new instance of the DocumentClient
            var client = new DocumentClient(new Uri(_endpointUrl), _authorizationKey);

            var database = await CreateOrGetDatabase(client, "browsers");

            var collection = await CreateOrGetCollection(client, database, "agents");

            // Add test document
            var document = new Agent
            {
                id = Guid.NewGuid().ToString(),
                BrowserAgent = agent,
                IpAddress = ipAddress,
                Referrer = referrer == null ? "" : referrer.ToString(),
                PageType = pageType,
                Timestamp = DateTime.Now
            };

            try
            {
                var newDocument = await client.CreateDocumentAsync(collection.DocumentsLink, document);
                var code = newDocument.StatusCode;
            }
            catch(Exception ex)
            {
                //TODO
            }
        }
    }
}