﻿using AppsWorld.InvoiceModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra
{
    public class InvoiceCreated : IDomainEvent
    {
        public InvoiceModel Invoice { get; private set; }
        public InvoiceCreated(InvoiceModel invoice)
        {
            Invoice = invoice;
        }
    }
}