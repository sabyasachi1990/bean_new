using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace AppsWorld.CommonModule.Entities.Mapping
{
    public class BeanEntityMap : EntityTypeConfiguration<BeanEntity>
    {
        public BeanEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            //this.Property(t => t.IdNo)
            //    .HasMaxLength(50);

            //this.Property(t => t.GSTRegNo)
            //    .HasMaxLength(50);

            //this.Property(t => t.CustTOP)
            //    .HasMaxLength(50);

            //this.Property(t => t.CustCurrency)
            //    .HasMaxLength(5);

            //this.Property(t => t.CustNature)
            //    .HasMaxLength(25);

            this.Property(t => t.VenTOP)
                .HasMaxLength(50);

            this.Property(t => t.VenCurrency)
                .HasMaxLength(5);

            //this.Property(t => t.VenNature)
            //    .HasMaxLength(25);

            //this.Property(t => t.ResBlockHouseNo)
            //    .HasMaxLength(100);

            //this.Property(t => t.ResStreet)
            //    .HasMaxLength(100);

            //this.Property(t => t.ResUnitNo)
            //    .HasMaxLength(100);

            //this.Property(t => t.ResBuildingEstate)
            //    .HasMaxLength(100);

            //this.Property(t => t.ResCity)
            //    .HasMaxLength(256);

            //this.Property(t => t.ResPostalCode)
            //    .HasMaxLength(10);

            //this.Property(t => t.ResState)
            //    .HasMaxLength(256);

            //this.Property(t => t.ResCountry)
            //    .HasMaxLength(256);

            //this.Property(t => t.Remarks)
            //    .HasMaxLength(1000);

            //this.Property(t => t.UserCreated)
            //    .HasMaxLength(254);

            //this.Property(t => t.ModifiedBy)
            //    .HasMaxLength(254);

            //this.Property(t => t.Communication)
            //    .HasMaxLength(1000);

            //this.Property(t => t.VendorType)
            //    .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Entity", "Bean");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.TypeId).HasColumnName("TypeId");
            //this.Property(t => t.IdTypeId).HasColumnName("IdTypeId");
            //this.Property(t => t.IdNo).HasColumnName("IdNo");
            this.Property(t => t.GSTRegNo).HasColumnName("GSTRegNo");
            this.Property(t => t.IsCustomer).HasColumnName("IsCustomer");
            //this.Property(t => t.CustTOPId).HasColumnName("CustTOPId");
            //this.Property(t => t.CustTOP).HasColumnName("CustTOP");
            //this.Property(t => t.CustTOPValue).HasColumnName("CustTOPValue");
            //this.Property(t => t.CustCreditLimit).HasColumnName("CustCreditLimit");
            //this.Property(t => t.CustCurrency).HasColumnName("CustCurrency");
            this.Property(t => t.CustNature).HasColumnName("CustNature");
            this.Property(t => t.IsVendor).HasColumnName("IsVendor");
            this.Property(t => t.VenTOPId).HasColumnName("VenTOPId");
            this.Property(t => t.VenTOP).HasColumnName("VenTOP");
            //this.Property(t => t.VenTOPValue).HasColumnName("VenTOPValue");
            this.Property(t => t.VenCurrency).HasColumnName("VenCurrency");
            this.Property(t => t.VenNature).HasColumnName("VenNature");
            //this.Property(t => t.AddressBookId).HasColumnName("AddressBookId");
            //this.Property(t => t.IsLocal).HasColumnName("IsLocal");
            //this.Property(t => t.ResBlockHouseNo).HasColumnName("ResBlockHouseNo");
            //this.Property(t => t.ResStreet).HasColumnName("ResStreet");
            //this.Property(t => t.ResUnitNo).HasColumnName("ResUnitNo");
            //this.Property(t => t.ResBuildingEstate).HasColumnName("ResBuildingEstate");
            //this.Property(t => t.ResCity).HasColumnName("ResCity");
            //this.Property(t => t.ResPostalCode).HasColumnName("ResPostalCode");
            //this.Property(t => t.ResState).HasColumnName("ResState");
            //this.Property(t => t.ResCountry).HasColumnName("ResCountry");
            //this.Property(t => t.RecOrder).HasColumnName("RecOrder");
            //this.Property(t => t.Remarks).HasColumnName("Remarks");
            //this.Property(t => t.UserCreated).HasColumnName("UserCreated");
            //this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            //this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            //this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            //this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.VenCreditLimit).HasColumnName("VenCreditLimit");
            //this.Property(t => t.Communication).HasColumnName("Communication");
            //this.Property(t => t.VendorType).HasColumnName("VendorType");
            this.Property(t => t.IsShowPayroll).HasColumnName("IsShowPayroll");
            this.Property(t => t.COAId).HasColumnName("COAId");
            this.Property(t => t.TaxId).HasColumnName("TaxId");
            this.Property(t => t.CreditLimitValue).HasColumnName("CreditLimitValue");
            this.Property(t => t.ServiceEntityId).HasColumnName("ServiceEntityId");



            this.Property(t => t.PeppolDocumentId).HasColumnName("PeppolDocumentId");
        }
    }
}
