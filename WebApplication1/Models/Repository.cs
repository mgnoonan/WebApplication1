﻿using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;

namespace WebApplication1.Models
{
    public static class Repository<T>
    {
        private static string _endpointUrl = ConfigurationManager.AppSettings["EndpointUrl"];
        private static string _authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];

        //Use the Database if it exists, if not create a new Database
        private static Database ReadOrCreateDatabase()
        {
            var db = Client.CreateDatabaseQuery()
                            .Where(d => d.Id == DatabaseId)
                            .AsEnumerable()
                            .FirstOrDefault();

            if (db == null)
            {
                db = Client.CreateDatabaseAsync(new Database { Id = DatabaseId }).Result;
            }

            return db;
        }

        //Use the DocumentCollection if it exists, if not create a new Collection
        private static DocumentCollection ReadOrCreateCollection(string databaseLink)
        {
            var col = Client.CreateDocumentCollectionQuery(databaseLink)
                              .Where(c => c.Id == CollectionId)
                              .AsEnumerable()
                              .FirstOrDefault();

            if (col == null)
            {
                var collectionSpec = new DocumentCollection { Id = CollectionId };
                var requestOptions = new RequestOptions { OfferType = "S1" };

                col = Client.CreateDocumentCollectionAsync(databaseLink, collectionSpec, requestOptions).Result;
            }

            return col;
        }

        //Expose the "database" value from configuration as a property for internal use
        private static string databaseId;
        private static string DatabaseId
        {
            get
            {
                if (string.IsNullOrEmpty(databaseId))
                {
                    databaseId = ConfigurationManager.AppSettings["database"];
                }

                return databaseId;
            }
        }

        //Expose the "collection" value from configuration as a property for internal use
        private static string collectionId;
        private static string CollectionId
        {
            get
            {
                if (string.IsNullOrEmpty(collectionId))
                {
                    collectionId = ConfigurationManager.AppSettings["collection"];
                }

                return collectionId;
            }
        }

        //Use the ReadOrCreateDatabase function to get a reference to the database.
        private static Database database;
        private static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = ReadOrCreateDatabase();
                }

                return database;
            }
        }

        //Use the ReadOrCreateCollection function to get a reference to the collection.
        private static DocumentCollection collection;
        private static DocumentCollection Collection
        {
            get
            {
                if (collection == null)
                {
                    collection = ReadOrCreateCollection(Database.SelfLink);
                }

                return collection;
            }
        }

        //This property establishes a new connection to DocumentDB the first time it is used, 
        //and then reuses this instance for the duration of the application avoiding the
        //overhead of instantiating a new instance of DocumentClient with each request
        private static DocumentClient client;
        private static DocumentClient Client
        {
            get
            {
                if (client == null)
                {
                    string endpoint = ConfigurationManager.AppSettings["EndpointUrl"];
                    string authKey = ConfigurationManager.AppSettings["AuthorizationKey"];
                    Uri endpointUri = new Uri(endpoint);
                    client = new DocumentClient(endpointUri, authKey);
                }

                return client;
            }
        }

        public static void SaveDocument(T document)
        {
            try
            {
                var result = Client.CreateDocumentAsync(Collection.DocumentsLink, document).Result;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static IEnumerable<T> GetItems()
        {
            try
            {
                return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                    .AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }

            //return new List<T>().AsEnumerable();
        }

        public static IEnumerable<T> GetItems(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                        .Where(predicate)
                        .AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T GetItem(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return Client.CreateDocumentQuery<T>(Collection.DocumentsLink)
                            .Where(predicate)
                            .AsEnumerable()
                            .FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}