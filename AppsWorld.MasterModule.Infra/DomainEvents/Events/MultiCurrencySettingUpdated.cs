using AppsWorld.MasterModule.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
   
    public class MultiCurrencySettingUpdated : IDomainEvent
    {
        public MultiCurrencySetting MultiCurrencySetting { get;  private set; }
        public MultiCurrencySettingUpdated(MultiCurrencySetting multiCurrencySetting)
        {
            MultiCurrencySetting = multiCurrencySetting;
        }
    }
}
