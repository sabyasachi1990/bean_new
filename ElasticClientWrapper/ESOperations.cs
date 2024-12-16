using Nest;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Elasticsearch.Net;

namespace ElasticClientWrapper
{
    public class ESOperations : IESOperations
    {
        private ElasticClient client;
        string ElasticSearchURL;
        bool isElasticEnabled;
        public ESOperations()
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("ElasticSearchSettings");
            ElasticSearchURL = section["ElasticSearchServer"];
            isElasticEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ElasticEnabled"]);
        }

        #region SmartSearch
        public object SmartSearch(string searchCriteria, string context)
        {
            try
            {
                //Helpers.Helper.logger.Information("ElasticSearchController on SmartSearch");
                var node = new Uri(ElasticSearchURL);

                var settings = new ConnectionSettings(
                    node,
                    defaultIndex: context
                );

                var client = new ElasticClient(settings);
                string queryString = "*" + searchCriteria + "*";
                context = context.ToLower();
                switch (context)
                {
                    //case "accounttype":
                    //    return client.Search<GSIndex<AccountType>>(s => s.From(0).Size(10).
                    //        Query(q => q.QueryString(qs => qs.OnFields(f => f.GSId).Query(queryString)))).
                    //        Documents.AsQueryable().AsEnumerable().ToList().Select(s => s.Index).Where(l => l.Status == RecordStatusEnum.Active).AsEnumerable().ToList();

                   
                    default: return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public object GlobalSearch(string searchCriteria, int startIndex, int noOfRecords)
        {
            try
            {
                //Helpers.Helper.logger.Information("ElasticSearchController on GlobalSearch ");

                var node = new Uri(ElasticSearchURL);

                var settings = new ConnectionSettings(
                    node
                );

                var elasticClient = new ElasticClient(settings);
               // var queryString = "*" + searchCriteria + "*";
                var queryString = "*" + searchCriteria + "*";


                var results = elasticClient.Search<dynamic>(s => s.From(startIndex).Size(noOfRecords).AllIndices().AllTypes()
                    .Query(q => q.QueryString(qs => qs.Query(queryString)))).Documents.ToList();

                var lstGlobalSearch = new List<GlobalSearch>();
                foreach (var result in results)
                {
                    try
                    {
                        var index = result["index"];
                        //string companyId = index["companyId"] ?? "";
                        //string userCreated = index["userCreated"] ?? "";
                        //string createdDate = index["createdDate"] ?? "";
                        //string modifiedDate = index["modifiedDate"] ?? "";
                        //string strHeadLine = result["headline"] ?? "";
                        //string strUrl = result["uRL"] ?? "";
                        //string type = strUrl.Split(Convert.ToChar("/"))[1];
                        //string strDescription = result["description"] ?? "";
                        //lstGlobalSearch.Add(new GlobalSearch
                        //{
                        //    CompanyId = companyId,
                        //    UserCreated = userCreated,
                        //    CreatedDate = createdDate,
                        //    ModifiedDate = modifiedDate,
                        //    Type = type,
                        //    Headline = strHeadLine,
                        //    URL = strUrl,
                        //    Description = strDescription
                        //});

                        if (index != null)
                        {
                            string companyId = index[0]["Value"]["CompanyId"] ?? "";
                            string userCreated = index[0]["Value"]["UserCreated"] ?? "";
                            string createdDate = index[0]["Value"]["CreatedDate"] ?? "";
                            string modifiedDate = index[0]["Value"]["ModifiedDate"] ?? "";
                            string strHeadLine = result["headline"] ?? "";
                            string strUrl = result["uRL"] ?? "";
                            string type = strUrl.Split(Convert.ToChar("/"))[1];
                            string strDescription = result["description"] ?? "";
                            lstGlobalSearch.Add(new GlobalSearch
                            {
                                CompanyId = companyId,
                                UserCreated = userCreated,
                                CreatedDate = createdDate,
                                ModifiedDate = modifiedDate,
                                Type = type,
                                Headline = strHeadLine,
                                URL = strUrl,
                                Description = strDescription
                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                }
                return lstGlobalSearch;
            }

            catch (Exception)
            {

                throw;
            }

        }

        public bool SaveInES<T>(T entity, string indexId, string indexName, string indexHeadline, string indexUrl, string indexDescription, int status)
        {
            if (!isElasticEnabled)
            {
                return true;
            }
            var node = new Uri(ElasticSearchURL);

            var settings = new ConnectionSettings(
                        node, indexName.ToLower()
                    );

            var elasticClient = new ElasticClient(settings);

            var gsIndex = new GSIndex<T>
            {
                GSId = indexId,
                Index = entity,
                Headline = indexHeadline,
                URL = indexUrl,
                Description = indexDescription
                //status = status
            };
            if (entity != null)
            {
                elasticClient.Update<GSIndex<T>, object>(u => u.Id(gsIndex.GSId).Doc(gsIndex).Upsert(gsIndex));
                return true;
            }

            return false;
            /*Any other operations goes here */
        }


        public object AutoComplete(string indexName, string input, string indexField,int noOfRecords)
        {
            var node = new Uri("http://appsworldelk.cloudapp.net:9200");
            var connSettings = new ConnectionSettings(node);
            connSettings.SetDefaultIndex(indexName);
            connSettings.ThrowOnElasticsearchServerExceptions();

            var elasticClient = new ElasticClient(connSettings);

            var resp = elasticClient.Search<dynamic>(s => s
               .AllTypes()
               .From(0)
               .Take(noOfRecords)
               .Query(qry => qry
                   .Bool(b => b
                   .Must(m => m
                       .Wildcard(qs => qs
                           .OnField(indexField)
                           .Value("*" + input + "*"))))));
            return resp.Documents.ToList();
        }
        public void TEst<T>()
        {

        }


        public object GlobalSearch(string queryString, int startIndex, int noOfRecords, string companyIds)
        {
            try
            {
                //Helpers.Helper.logger.Information("ElasticSearchController on GlobalSearch ");
                var node = new Uri(ElasticSearchURL);
                var settings = new ConnectionSettings(
                    node
                );
                var elasticClient = new ElasticClient(settings);
                var companyIdss = companyIds ;
                var results = elasticClient.Search<dynamic>(s => s.From(startIndex).Size(noOfRecords).AllIndices().AllTypes()
                    .Query(q => q.QueryString(qs => qs.Query(queryString)))
                    .Query(p => p.Term("companyId", companyIdss))).Documents.ToList();
                

                var lstGlobalSearch = new List<GlobalSearch>();

                for (int i = 0; i < results.Count; i++)
                {
                    var result = results[i];
                    try
                    {
                        var index = result["index"];
                        if (index != null)
                        {
                            string companyId = index[0]["Value"]["CompanyId"] ?? "";
                            string userCreated = index[0]["Value"]["UserCreated"] ?? "";
                            string createdDate = index[0]["Value"]["CreatedDate"] ?? "";
                            string modifiedDate = index[0]["Value"]["ModifiedDate"] ?? "";
                            string strHeadLine = result["headline"] ?? "";
                            string strUrl = result["uRL"] ?? "";
                            string type = strUrl.Split(Convert.ToChar("/"))[1];
                            string strDescription = result["description"] ?? "";
                            lstGlobalSearch.Add(new GlobalSearch
                            {
                                CompanyId = companyId,
                                UserCreated = userCreated,
                                CreatedDate = createdDate,
                                ModifiedDate = modifiedDate,
                                Type = type,
                                Headline = strHeadLine,
                                URL = strUrl,
                                Description = strDescription
                            });
                        }
                    }
                    catch (Exception exception)
                    {
                        throw;
                    }
                }
                return lstGlobalSearch;
            }

            catch (Exception)
            {

                throw;
            }

        }
    }
}
