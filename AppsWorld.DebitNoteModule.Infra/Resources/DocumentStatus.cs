using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Infra
{
    public static class DebitNoteState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
    }
    public static class InvoiceState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
    }
}
