using System.Configuration;

namespace ElasticClientWrapper
{
    public static class Settings
    {
        public static string Alias
        {
            get
            {
                //return "customer_product_mapping";
                return "";
            }
        }

        public static string ElasticSearchServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ElasticsearchServer"];
            }
        }
        /*Below method is for reference to fetch DBConnectionString if any */
        public static string NorthwndConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["northwnd"].ConnectionString;
            }
        }
    }
}
