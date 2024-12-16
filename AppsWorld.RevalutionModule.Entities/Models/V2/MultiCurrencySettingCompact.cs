using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class MultiCurrencySettingCompact : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public Nullable<bool> Revaluation { get; set; }
    }
}
