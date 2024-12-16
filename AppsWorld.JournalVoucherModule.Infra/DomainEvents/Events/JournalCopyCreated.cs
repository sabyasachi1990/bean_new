using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Models;
using Domain.Events;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public class JournalCopyCreated:IDomainEvent
    {
        public Journal JournalModel { get;private set; }
        public JournalCopyCreated(Journal journal)
        {
            JournalModel = journal;
        }
    }
}
