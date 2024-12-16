using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;
using AppsWorld.MasterModule.Models;
using System.Data.SqlClient;
using System.Data;
using AppsWorld.Framework;

namespace AppsWorld.MasterModule.Service
{
    public class CommunicationService : Service<Communication>, ICommunication
    {
        private readonly IMasterModuleRepositoryAsync<Communication> _communicationRepository;
        private readonly IMasterModuleRepositoryAsync<Invoice> _invoiceRepository;
        private readonly IMasterModuleRepositoryAsync<CashSale> _cashSaleRepository;
        private readonly IMasterModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        private readonly IMasterModuleRepositoryAsync<Receipt> _receiptRepository;

        public CommunicationService(IMasterModuleRepositoryAsync<Communication> communicationRepository, IMasterModuleRepositoryAsync<Invoice> invoiceRepository, IMasterModuleRepositoryAsync<CashSale> cashSaleRepository, IMasterModuleRepositoryAsync<DebitNote> debitNoteRepository, IMasterModuleRepositoryAsync<Receipt> receiptRepository)
           : base(communicationRepository)
        {
            _communicationRepository = communicationRepository;
            _invoiceRepository = invoiceRepository;
            _cashSaleRepository = cashSaleRepository;
            _debitNoteRepository = debitNoteRepository;
            _receiptRepository = receiptRepository;
        }
        //public IQueryable<CommunicationModel> GetCommunications(Guid clientId)
        //{
        //    var communications = _communicationRepository.Queryable();
        //    var invoices = _invoiceRepository.Queryable();
        //    var cashsale = _cashSaleRepository.Queryable();
        //    var debitnote = _debitNoteRepository.Queryable();
        //    var receipt = _receiptRepository.Queryable();
        //    var communication = from com in communications
        //                        join i in invoices on com.InvoiceId equals i.Id
        //                        join cs in cashsale on com.InvoiceId equals cs.Id
        //                        join dn in debitnote on com.InvoiceId equals dn.Id
        //                        join re in receipt on com.InvoiceId equals re.Id
        //                        where (com.InvoiceId == clientId)
        //                        select new CommunicationModel()
        //                        {
        //                            Id = com.Id,
        //                            //CaseNumber = ca.CaseNumber,
        //                            Category = com.Category,
        //                            CommunicationType = com.CommunicationType,
        //                            CreatedDate = com.CreatedDate,
        //                            Date = com.Date,
        //                            Description = com.Description,
        //                            FromMail = com.FromMail,
        //                            //InvoiceFee = i.Fee,
        //                            InvoiceId = com.InvoiceId,
        //                            DocNo = i.DocNo,
        //                            InvoiceType = i.DocType,
        //                            ModifiedBy = com.ModifiedBy,
        //                            ModifiedDate = com.ModifiedDate,
        //                            Remarks = com.Remarks,
        //                            ReportPath = com.ReportPath,
        //                            SentBy = com.SentBy,
        //                            Status = com.Status,
        //                            Subject = com.Subject,
        //                            TemplateCode = com.TemplateCode,
        //                            TemplateId = com.TemplateId,
        //                            TemplateName = com.TemplateName,
        //                            ToMail = com.ToMail,
        //                            UserCreated = com.UserCreated
        //                        };
        //    return communication.AsQueryable();

        //}
        public IQueryable<CommunicationModel> GetCommunications(Guid clientId, string ConnectionString)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            string query = "Select ent.Name as EntityName, ent.Id as EntityId,com.Id,com.LeadId, com.InvoiceId, com.Category,com.CommunicationType,com.CreatedDate,com.Date,com.Description,com.FromMail,com.ModifiedBy, com.ModifiedDate, com.Remarks,com.ReportPath,com.AzurePath,com.FileName,com.FilePath, com.SentBy, com.Status, com.Subject, com.TemplateCode, com.TemplateId, com.TemplateName, com.ToMail, com.UserCreated     From common.Communication com  inner  join Bean.Entity ent  on com.LeadId = ent.Id where com.LeadId='" + clientId + "'";
            //"select enti.Name as EntityName, Communications.* from (Select com.Id,com.LeadId, com.InvoiceId, com.Category, com.CommunicationType, inv.EntityId, com.CreatedDate, com.Date, com.Description, com.FromMail, inv.DocNo, inv.DocType, com.ModifiedBy, com.ModifiedDate, com.Remarks, com.ReportPath, com.AzurePath, com.FileName, com.FilePath, com.SentBy, com.Status, com.Subject, com.TemplateCode, com.TemplateId, com.TemplateName, com.ToMail, com.UserCreated     From common.Communication com     inner join Bean.Invoice  inv  on com.InvoiceId = inv.Id    Union     select com.Id,com.LeadId, com.InvoiceId, com.Category, com.CommunicationType, debt.EntityId, com.CreatedDate, com.Date, com.Description, com.FromMail, debt.DocNo, 'Debit Note', com.ModifiedBy, com.ModifiedDate, com.Remarks, com.ReportPath, com.AzurePath, com.FileName, com.FilePath, com.SentBy, com.Status, com.Subject, com.TemplateCode, com.TemplateId, com.TemplateName, com.ToMail, com.UserCreated     from common.Communication com     inner join Bean.DebitNote debt  on com.InvoiceId = debt.Id Union select com.Id,com.LeadId, com.InvoiceId, com.Category, com.CommunicationType, cas.EntityId, com.CreatedDate, com.Date, com.Description, com.FromMail, cas.DocNo, cas.DocType, com.ModifiedBy, com.ModifiedDate, com.Remarks, com.ReportPath, com.AzurePath, com.FileName, com.FilePath, com.SentBy, com.Status, com.Subject, com.TemplateCode, com.TemplateId, com.TemplateName, com.ToMail, com.UserCreated     from common.Communication com     inner join Bean.CashSale cas  on com.InvoiceId = cas.Id Union  select com.Id,com.LeadId, com.InvoiceId, com.Category, com.CommunicationType, res.EntityId, com.CreatedDate, com.Date, com.Description, com.FromMail, res.DocNo, 'Receipt', com.ModifiedBy, com.ModifiedDate, com.Remarks, com.ReportPath, com.AzurePath, com.FileName, com.FilePath, com.SentBy, com.Status, com.Subject, com.TemplateCode, com.TemplateId, com.TemplateName, com.ToMail, com.UserCreated     from common.Communication com     inner join Bean.Receipt res  on com.InvoiceId = res.Id Union  select com.Id,com.LeadId, com.InvoiceId, com.Category, com.CommunicationType, ent.Id, com.CreatedDate, com.Date, com.Description, com.FromMail, '', 'Statement Of Account', com.ModifiedBy, com.ModifiedDate, com.Remarks, com.ReportPath, com.AzurePath, com.FileName, com.FilePath, com.SentBy, com.Status, com.Subject, com.TemplateCode, com.TemplateId, com.TemplateName, com.ToMail, com.UserCreated     from common.Communication com     inner  join Bean.Entity ent  on com.InvoiceId = ent.Id)     Communications inner join Bean.Entity enti  on enti.Id = Communications.EntityId where Communications.LeadId ='" + clientId + "'";
            con = new SqlConnection(ConnectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
            using (cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dtSchema = new DataTable();
                //DataTable dt = new DataTable();
                List<CommunicationModel> listCols = new List<CommunicationModel>();
                dtSchema.Load(dr);


                if (dtSchema.Rows.Count > 0)
                {
                    foreach (DataRow drow in dtSchema.Rows)
                    {
                        //string columnName = System.Convert.ToString(drow["ColumnName"]);
                        CommunicationModel communicationModel = new CommunicationModel();
                        communicationModel.Id = (Guid)drow["Id"];
                        communicationModel.Category = drow["Category"].ToString();
                        communicationModel.CommunicationType = drow["CommunicationType"].ToString();
                        communicationModel.CreatedDate = (DateTime?)drow["CreatedDate"];
                        communicationModel.Date = (DateTime?)drow["Date"];
                        communicationModel.Description = drow["Description"].ToString();
                        communicationModel.FromMail = drow["FromMail"].ToString();
                        communicationModel.InvoiceId = (Guid)drow["InvoiceId"];
                        //communicationModel.DocNo = drow["DocNo"].ToString();
                        communicationModel.ModifiedBy = drow["ModifiedBy"].ToString();
                        communicationModel.ModifiedDate = drow["ModifiedDate"].ToString();
                        communicationModel.Remarks = drow["Remarks"].ToString();
                        communicationModel.ReportPath = drow["ReportPath"].ToString();
                        communicationModel.SentBy = drow["SentBy"].ToString();
                        communicationModel.Status = (RecordStatusEnum)drow["Status"];
                        communicationModel.Subject = drow["Subject"].ToString();
                        communicationModel.TemplateCode = drow["TemplateCode"].ToString();
                        communicationModel.TemplateId = (drow["TemplateId"] == DBNull.Value) ? (Guid?)null : (Guid?)drow["TemplateId"];
                        communicationModel.TemplateName = drow["TemplateName"].ToString();
                        communicationModel.ToMail = drow["ToMail"].ToString();
                        communicationModel.UserCreated = drow["UserCreated"].ToString();
                        communicationModel.FileName = drow["FileName"].ToString();
                        communicationModel.DisplayFileName = drow["FileName"].ToString();
                        communicationModel.AzurePath = drow["AzurePath"].ToString();
                        communicationModel.FilePath = drow["FilePath"].ToString();
                        communicationModel.EntityName = drow["EntityName"].ToString();
                        communicationModel.EntityId = (Guid)drow["EntityId"];
                        communicationModel.ScreenName = drow["TemplateName"].ToString();

                        listCols.Add(communicationModel);
                        //dt.Columns.Add(communicationModel);
                    }
                }

                return listCols.AsQueryable();
            }

        }
    }
}
