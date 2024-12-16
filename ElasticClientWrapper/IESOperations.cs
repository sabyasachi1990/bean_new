namespace ElasticClientWrapper
{
    public interface IESOperations
    {
        object SmartSearch(string searchCriteria, string context);
        object GlobalSearch(string searchCriteria, int startIndex, int noOfRecords);
        bool SaveInES<T>(T entity, string indexId, string indexName, string indexHeadline, string indexUrl, string indexDescription, int status);

        object AutoComplete(string indexName, string input, string indexField, int noOfRecords);


        object GlobalSearch(string searchCriteria, int startIndex, int noOfRecords, string companyId);//modified by sreenivas
    }
}
