namespace AppsWorld.PaymentModule.Infra
{
    public static class PaymentState
    {
        public const string Posted = "Posted";
        public const string Cleared = "Cleared";
        public const string Void = "Void";
    }
    public static class InvoiceState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
        public const string Deleted = "Deleted";
    }
    public static class BillNoteState
    {
        public const string NotPaid = "Not Paid";
        public const string PartialPaid = "Partial Paid";
        public const string FullyPaid = "Fully Paid";
        public const string Void = "Void";
    }
    public static class CreditMemoState
    {
        public const string Not_Applied = "Not Applied";
        public const string Partial_Applied = "Partial Applied";
        public const string Fully_Applied = "Fully Applied";
        public const string Void = "Void";
    }
    //public static class CreditNoteState
    //{
    //    public const string Not_Applied = "Not Applied";
    //    public const string Partial_Applied = "Partial Applied";
    //    public const string Fully_Applied = "Fully Applied";
    //    public const string Void = "Void";
    //}

}
