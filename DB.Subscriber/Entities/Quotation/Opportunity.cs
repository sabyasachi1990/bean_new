namespace DB.Subscriber.Entities.Quotation
{
    using Repository.Pattern.Ef6;
    using System;

    public partial class Opportunity : Entity
    {
        public Opportunity()
        {
        }

        public Guid Id { get; set; }

        public string Stage { get; set; }
    }
}
