using AppsWorld.MasterModule.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
   public  class MultiCurrencySettingCreated:IDomainEvent
    {
        public MultiCurrencySetting MultiCurrencySetting { get; set; }
        public MultiCurrencySettingCreated(MultiCurrencySetting multiCurrencySetting)
        {
            MultiCurrencySetting = multiCurrencySetting;
        }
    }
}
