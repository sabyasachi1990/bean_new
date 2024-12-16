namespace AppsWorld.Framework
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
        }
		public static class ReceiptState
		{
			public const string NotPaid = "Not Paid";
			public const string Paid = "Paid";
			public const string FullyApplied = "Fully Applied";
			public const string Void = "Void";

		}

    public static class JournalState
    {
        public const string Parked = "Parked";
        public const string Posted = "Posted";
        public const string Reversed = "Reversed";
        public const string Void = "Void";
		public const string Recurring = "Recurring";

    }
    public static class PaymentState
    {
        public const string Created = "Created";
        public const string Cleared = "Cleared";
        public const string Void = "Void";
    }
    public static class WithdrawalState
    {
        public const string Created = "Created";
        public const string Void = "Void";
    }
    public static class CashSaleStatus
    {
        public const string Created = "Created";
        public const string Void = "Void";
        public const string FullyPaid = "Fully Paid";
    }
}
