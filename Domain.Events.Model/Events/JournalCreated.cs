using AppsWorld.JournalVoucherModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.Events
{
   public  class JournalCreated:IDomainEvent
    {
        public JournalModel JournalModel { get; private set; }

        public JournalCreated(JournalModel journalModel)
        {
            JournalModel = journalModel;
        }
    }
}
