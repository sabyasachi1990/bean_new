using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Models;
using Domain.Events;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public class JournalCreated : IDomainEvent
    {
        public JournalModel JournalModel { get; private set; }
        public JournalCreated(JournalModel journalModel)
        {
            JournalModel = journalModel;
        }
    }
}
