using AppsWorld.JournalVoucherModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.Events
{
   public  class JournalUpdated:IDomainEvent
    {
        public JournalModel JournalModel { get; private set; }

        public JournalUpdated(JournalModel journalModel)
        {
            JournalModel = journalModel;
        }
    }
}
