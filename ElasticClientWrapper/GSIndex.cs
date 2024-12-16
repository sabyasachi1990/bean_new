namespace ElasticClientWrapper
{
    public class GSIndex<T>
    {
        public T Index { get; set; }
        public string GSId { get; set; }
        //public string CompanyId { get; set; }
        public string Headline { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }

        //public int status { get; set; }
    }
}
