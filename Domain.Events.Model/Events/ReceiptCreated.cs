using AppsWorld.ReceiptModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.Events
{
	public class ReceiptCreated:IDomainEvent
	{  
		public ReceiptModel ReceiptModel { get; private set; }
	
		public ReceiptCreated(ReceiptModel receiptModel)
		{
			ReceiptModel = receiptModel;
		}

	}
}
