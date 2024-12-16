using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra.Resources
{
    public class InvoiceState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
        public const string NotApplied = "Not Applied";
        public const string Posted = "Posted";
        public const string Parked = "Parked";
        public const string Deleted = "Deleted";
        public const string Recurring = "Recurring";
        public const string Cleared = "Cleared";
        public const string Reset = "Reset";
    }
}
