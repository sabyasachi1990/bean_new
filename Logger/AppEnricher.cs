using Serilog.Core;
using Serilog.Events;
using System;
using System.Reflection;

namespace Logger
{
    public class AppEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            try
            {
                var applicationAssembly = Assembly.GetEntryAssembly();

                var name = applicationAssembly.GetName().Name;
                var version = applicationAssembly.GetName().Version;

                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationName", name));
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationVersion", version));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
