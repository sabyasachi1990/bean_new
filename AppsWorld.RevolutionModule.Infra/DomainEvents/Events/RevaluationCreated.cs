using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.RevaluationModule.Models;

namespace AppsWorld.RevaluationModule.Infra
{
    public class RevaluationCreated:IDomainEvent
    {
        public RevaluationSaveModel RevaluationModel { get; private set; }
        public RevaluationCreated(RevaluationSaveModel revaluationModel)
        {
            RevaluationModel = revaluationModel;
        }
    }
}
