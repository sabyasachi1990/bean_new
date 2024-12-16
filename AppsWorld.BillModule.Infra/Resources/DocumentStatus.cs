namespace AppsWorld.BillModule.Infra
{
    public static class InvoiceState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
    }

    public static class BillNoteState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
        public const string Cleared = "Cleared";
    }
}
