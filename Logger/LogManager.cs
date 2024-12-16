using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Linq;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;

namespace Logger
{
    public class LogManager
    {
        private static ILogger logger;
        private  const string customTemplate = "{Timestamp:yyyy-MMM-dd HH:mm:ss}[{Level}{Message}{NewLine}{Exception}]";
        /*Below Main method is for testing it by converting library into console app. Will be detatched shortly */
        public static void Main(string[] args)
        {
            //string elasticsearchURL = ConfigurationManager.AppSettings["elasticsearch"];
            logger = new LoggerConfiguration()
                 .ReadFrom.AppSettings()
                 .Enrich.With(new AppEnricher())
				 .WriteTo.RollingFile(@"C:\temp\Log-{Date}.txt", outputTemplate: customTemplate)
				 //.WriteTo.RollingFile("myapp-{Date}.txt",outputTemplate:customTemplate)
                 //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchURL)){AutoRegisterTemplate = true})
                 .CreateLogger();

            var library = typeof(Log).Assembly.GetName();
            //logger.Verbose("Verbose log test");
            logger.Information(" Hello {User} from {@Library}!", Environment.UserName, new { library.Name, Version = library.Version.ToString() });

        }

        public static ILogger getRollingFileSink(){
            logger = new LoggerConfiguration()
                 .ReadFrom.AppSettings()
                 .Enrich.With(new AppEnricher())
                 .WriteTo.RollingFile("myapp-{Date}.txt",outputTemplate:customTemplate)                
                 .CreateLogger();
            return logger;
        }
        public static ILogger getFileSink()
        {
            logger = new LoggerConfiguration()
                 .ReadFrom.AppSettings()
                 .Enrich.With(new AppEnricher())
                 .WriteTo.File("myapp.txt", outputTemplate: customTemplate)
                 .CreateLogger();
            return logger;
        }
        public static ILogger getElasticsearchSink()
        {
			NameValueCollection EsSection = (NameValueCollection)ConfigurationManager.GetSection("ElasticSearchSettings");
			string strElasticSearchServer = EsSection["ElasticSearchServer"];
			string customTemplates = "[{Message}{NewLine}{Exception}]";
			logger = new LoggerConfiguration()
				 .ReadFrom.AppSettings()
				 .Enrich.With(new AppEnricher())
				 .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(strElasticSearchServer))
				 {
					 AutoRegisterTemplate = false,
					 InlineFields = false,
					 IndexFormat = "testlogbean-{0:yyyy.MM.dd}",
					 MinimumLogEventLevel = Serilog.Events.LogEventLevel.Verbose,
					 TypeName = "Bean",

				 })
				  .WriteTo.RollingFile(@"C:\temp\Log-{Month}-bean.txt", outputTemplate: customTemplates)
				 .CreateLogger();
			//string elasticsearchURL = ConfigurationManager.AppSettings["elasticsearch"];

			return logger;
			
        }
    }

    public static class SeriLogExtensions
    {
        public static void ZInfo(this ILogger logger, string eventID, string messageTemplate, params object[] propertyValues)
        {
            var allProps = new object[] { eventID }.Concat(propertyValues).ToArray();
            logger.Information("<{EventID:l}> " + messageTemplate, allProps);
        }

        public static void ZInfo(this ILogger logger, object data, params object[] propertyValues)
        {
            logger.Information("this is data: {Data}",data);
        }

        public static void ZInfo(this ILogger logger, string message, object data, params object[] propertyValues)
        {
            logger.Information(message + " {Data}", data);
        }

        public static void ZDebug(this ILogger logger, object data, params object[] propertyValues)
        {
            logger.Debug("this is data: {Data}", data);
        }

        public static void ZCritical(this ILogger logger, object data, params object[] propertyValues)
        {
            logger.Fatal("{Data}", data);
        }
        
        public static ILogger ZWith(this ILogger logger, string transactionid, object value)
        {
            return logger.ForContext(transactionid, value, true);
        }

        public static ILogger ZHere(this ILogger logger, [CallerMemberName()] string methodname = null)
        {
            return logger.ForContext("method", methodname);
        }
    }   
}
