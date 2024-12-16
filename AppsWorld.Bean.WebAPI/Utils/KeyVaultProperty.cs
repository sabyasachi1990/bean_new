using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsWorld.Bean.WebAPI.Utils
{
    public class KeyVaultProperty
    {
        public static string SecretKey { get; set; }
        public static string SkipAuthPassword { get; set; }
        public static string AppsWorldDBContext { get; set; }
        public static string HangFireContext { get; set; }
        public static string AzureStorage { get; set; }
    }
}