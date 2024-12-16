using System;
using System.Collections.Generic;

namespace AppsWorld.Framework
{

    public class LookUpCategory<T>
    {
        public LookUpCategory()
        {
            List<LookUp<T>> Lookups = new List<LookUp<T>>();
            List<LookUpGuid<T>> LookUps = new List<LookUpGuid<T>>();
            List<LookUpUser<T>> LookUpUsers = new List<LookUpUser<T>>();
        }
        public string CategoryName { get; set; }
        public string Code { get; set; }
        public string DefaultValue { get; set; }
        public long Id { get; set; }
      
        public List<LookUp<T>> Lookups { get; set; }
        public List<LookUpGuid<T>> LookUps { get; set; }
        public List<LookUpUser<T>> LookUpUsers { get; set; }
    }
    public class LookUp<T>
    {
        public T Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public int? RecOrder { get; set; }
        public double? TOPValue { get; set; }
		public string Currency { get; set; }
    }
    public class LookUpGuid<T>
    {
        public T Code { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
    }
    public class LookUpUser<T>
    {
        public T Username { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? RecOrder { get; set; }
    }
	public class TaxCodeLookUp<T>
	{
		public T Code { get; set; }
		public long Id { get; set; }
		public string Name { get; set; }
		public int? RecOrder { get; set; }
		public double? TaxRate { get; set; }
        public string TaxIdCode { get; set; }
		public string TaxType { get; set; }
	}
	public class LookUpCompany<T>
	{
		public T Code { get; set; }
		public long Id { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }

	}
}
