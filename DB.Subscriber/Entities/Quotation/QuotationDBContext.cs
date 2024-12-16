namespace DB.Subscriber.Entities.Quotation
{
    using Repository.Pattern.Ef6;
    using DB.Subscriber.RepositoryPattern.Quotation;

    public partial class QuotationDBContext : DataContext , IQuotationDBDataContextAsync
    {
        public QuotationDBContext()
            : base("name=AppsWorldDBContext")
        {
        }

        public virtual Opportunity Opportunities { get; set; }
        public virtual QuotationDetail QuotationDetails { get; set; }
       

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
           
        //}
    }
}
