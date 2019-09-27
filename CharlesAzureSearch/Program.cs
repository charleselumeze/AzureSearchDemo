using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CharlesAzureSearch.Models;
using CsvHelper;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace CharlesAzureSearch
{
    class Program
    {
        private static readonly string azureSearchServiceName = "name";
        private static readonly string adminApiKey = "key";
        private static readonly string dataFileName = @"filepath";
        private static readonly string indexName = "indexname";
        static void Main(string[] args)
        {
            var serviceClient = CreateSearchServiceClient();
            CreateIndex(serviceClient);
            ImportData(serviceClient);
        }

        private static void ImportData(SearchServiceClient serviceClient)
        {
            List<RentingStats> rentingStats = new List<RentingStats>();

            using (var streamReader = new StreamReader(dataFileName))
            using (var csvReader = new CsvReader(streamReader))
            {
                rentingStats = csvReader.GetRecords<RentingStats>().ToList();
            }
         
            var actions = new List<IndexAction<RentingStats>>();
            foreach (var rentingStat in rentingStats)
            {
                actions.Add(IndexAction.Upload(rentingStat));
            }
            var batch = IndexBatch.New(actions);

            try
            {
                ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName);
                indexClient.Documents.Index(batch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private static void CreateIndex(SearchServiceClient serviceClient)
        {

            if (!serviceClient.Indexes.Exists(indexName))
            {
                var definition = new Index()
                {
                    Name = indexName,
                    Fields = FieldBuilder.BuildForType<RentingStats>()
                };
                serviceClient.Indexes.Create(definition);

            }
        }

        private static SearchServiceClient CreateSearchServiceClient()
        {
            SearchServiceClient serviceClient = new SearchServiceClient(azureSearchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }
    }
}
