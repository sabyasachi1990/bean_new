namespace AppsWorld.ReceiptModule.Infra
{
    public static class ReceiptState
    {
        public const string NotPaid = "Not Paid";
        public const string Paid = "Paid";
        public const string FullyApplied = "Fully Applied";
        public const string Void = "Void";
        public const string Created = "Created";
        public const string Posted = "Posted";
        public const string Cleared = "Cleared";
    }
    public static class InvoiceState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string PartialApplied = "Partial Applied";
        public const string FullyPaid = "Fully Paid";
        public const string FullyApplied = "Fully Applied";
        public const string Deleted = "Deleted";
        public const string Void = "Void";
        public const string Not_Applied = "Not Applied";
    }
}
