using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Infra
{
    public static class CreditMemoState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
        public const string FullyApplied = "Fully Applied";
        public const string PartialApplied = "Partial Applied";
        public const string NotApplied = "Not Applied";
        public const string Cleared = "Cleared";
    }
}
