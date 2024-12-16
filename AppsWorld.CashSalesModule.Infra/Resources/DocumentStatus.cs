namespace AppsWorld.CashSalesModule.Infra
{
    public static class CashSaleStatus
    {
        public const string Created = "Created";
        public const string Void = "Void";
        public const string FullyPaid = "Fully Paid";
        public const string Cleared = "Cleared";
    }
    public static class InvoiceState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
    }
    public static class RecordStatus
    {
        public const string Deleted = "Deleted";
        public const string Modified = "Modified";
        public const string Added = "Added";
    }

}
