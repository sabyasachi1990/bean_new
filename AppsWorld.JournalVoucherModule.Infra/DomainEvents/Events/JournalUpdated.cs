using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Models;
using Domain.Events;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public class JournalUpdated :IDomainEvent
    {
        public JournalModel JournalModel { get; private set; }
        public JournalUpdated(JournalModel journalModel)
        {
            JournalModel = journalModel;
        }
    }
}
