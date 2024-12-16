using AppsWorld.GLClearingModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Infra
{
    public class ClearingUpdated:IDomainEvent
    {
        public ClearingModel ClearingModel { get; private set; }
        public ClearingUpdated(ClearingModel clearingModel)
        {
            ClearingModel = clearingModel;
        }
    }
}
