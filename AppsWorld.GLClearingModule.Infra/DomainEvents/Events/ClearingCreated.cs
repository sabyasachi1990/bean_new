using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.GLClearingModule.Models;
using Domain.Events;

namespace AppsWorld.GLClearingModule.Infra
{
    public class ClearingCreated:IDomainEvent
    {
        public ClearingModel ClearingModel { get; private set; }
        public ClearingCreated(ClearingModel clearingModel)
        {
            ClearingModel = clearingModel;
        }
    }
}
