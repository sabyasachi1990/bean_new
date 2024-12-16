using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Domain.Events;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public class JournalVoidCreated:IDomainEvent
    {
        public Journal JournalModel { get; private set; }
        public JournalVoidCreated(Journal journal)
        {
            JournalModel = journal;
        }
    }
}
