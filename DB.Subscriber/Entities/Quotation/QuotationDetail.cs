namespace DB.Subscriber.Entities.Quotation
{
    using System;
    using Repository.Pattern.Ef6;

    public partial class QuotationDetail : Entity
    {
        public Guid Id { get; set; }

        public Guid MasterId { get; set; }

        public Guid OpportunityId { get; set; }

        public int? Revision { get; set; }

        public bool? IsModified { get; set; }

        public int? RecOrder { get; set; }

        public string Remarks { get; set; }

        public string UserCreated { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public short? Version { get; set; }

    }
}
