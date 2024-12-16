using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Entities.Models
{
    public class SettlementDetail : Entity
    {
        public Guid Id { get; set; }
        public Guid? BankTransferId { get; set; }
        public string SettlemetType { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocumentType { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentState { get; set; }
        public string Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public long? ServiceCompanyId { get; set; }
        public decimal? DocumentAmmount { get; set; }
        public decimal? AmmountDue { get; set; }
        public decimal? SettledAmount { get; set; }
        public int RecOrder { get; set; }
    }
}
