using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class JVViewModel
    {
        public Guid Id { get; set; }
        public string SystemReferenceNo { get; set; }
        public string DocType { get; set; }
        public string Type { get; set; }
    }
}
