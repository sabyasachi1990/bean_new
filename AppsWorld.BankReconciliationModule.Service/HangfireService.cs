using AppaWorld.Bean;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.Infra;
using AppsWorld.BankReconciliationModule.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
    public class HangfireService
    {

        BankReconciliationContext appsworldcontext = new BankReconciliationContext();
        public void DifferentDocumentsClearingdateUpdation(BankReconciliationModel bankReconciliation, string ConnectionString, List<DocumentHistoryModel> lstDocHistoryModel)
        {
            if (bankReconciliation.BankReconciliationDetailModels != null && bankReconciliation.BankReconciliationDetailModels.Any())
            {
                UpdateAllDocuments(bankReconciliation, bankReconciliation.CompanyId, true, ConnectionString, lstDocHistoryModel);
            }
        }
        public void UpdateDates(List<BankReconciliationDetailModel> details, long companyId, bool isBankReconcile)
        {

            foreach (var BrDetails in details)
            {
                JournalDetail journal = new JournalDetail();
                //if (BrDetails.DocumentType == "Transfer")
                //        journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.IsWithdrawal == BrDetails.Withdrawal).FirstOrDefault();
                //else
                if (BrDetails.DocumentType == "Transfer" || BrDetails.DocumentType == "Bill Payment" || BrDetails.DocumentType == "Receipt")
                {
                    journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.COAId == BrDetails.COAId).FirstOrDefault();
                }
                else
                    journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.DocumentDetailId == new Guid() && a.COAId == BrDetails.COAId).FirstOrDefault();
                //journal = appsworldcontext.JournalDetails.Where(a => a.Id == BrDetails.JournalId).FirstOrDefault();
                List<Journal> lstJournal = appsworldcontext.Journals.Where(x => x.DocNo == BrDetails.DocRefNo && x.CompanyId == companyId).ToList();
                if (journal != null)
                {
                    if (BrDetails.ClearingDate != null)
                    {
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}',ClearingStatus='{2}' WHERE Id = '{3}'", "Bean.JournalDetail", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), "Cleared", journal.Id));
                        if (lstJournal.Any())
                        {
                            foreach (var journal1 in lstJournal)
                            {
                                if (journal1.DocSubType != "Opening Bal")
                                    appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}',ClearingStatus='{2}',ModifiedBy='{3}',ModifiedDate='{4}'  WHERE Id = '{5}'", "Bean.Journal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), "Cleared", "System", String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, journal1.Id));
                            }
                        }
                    }
                    else
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL,IsBankReconcile =NULL,ClearingStatus=NULL  WHERE Id = '{1}'", "Bean.JournalDetail", journal.Id));
                    if (isBankReconcile)
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET IsBankReconcile = '{1}'  WHERE Id = '{2}'", "Bean.JournalDetail", 1, journal.Id));

                }
                if (journal != null)
                //if (journal.DocumentDetailId == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    if (BrDetails.DocumentType == "Receipt")
                    {
                        Receipt reciept = appsworldcontext.Receipts.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (reciept != null)
                        {
                            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Receipt", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/* DateTime.UtcNow*/, "System", reciept.Id));
                        }
                    }
                    //ModifiedDate=GETUTCDATE()
                    //if (BrDetails.DocumentType == "Journal")
                    //{
                    //    Receipt reciept = appsworldcontext.Receipts.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                    //    if (reciept != null)
                    //    {
                    //        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', ModifiedDate=GETUTCDATE()  WHERE Id = '{2}'", "Bean.Journal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), reciept.Id));
                    //    }
                    //}
                    else if (BrDetails.DocumentType == "Bill Payment")
                    {
                        var paym = appsworldcontext.Payments.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (paym != null)
                        {
                            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Payment", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", paym.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Deposit" || BrDetails.DocumentType == "Cash Payment")
                    {
                        var withdraw = appsworldcontext.WithDrawals.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Withdrawal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Withdrawal")
                    {
                        var withdraw = appsworldcontext.WithDrawals.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Withdrawal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Cash Sale")
                    {
                        var withdraw = appsworldcontext.CashSales.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.CashSale", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Transfer")
                    {
                        string status = null;
                        var banktran = appsworldcontext.BankTransfers.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).Include(a => a.BankTransferDetails).FirstOrDefault();
                        Guid detailId = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Id).FirstOrDefault();
                        string type = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Type).FirstOrDefault();

                        if (banktran != null)
                        {
                            //string type = BrDetails.isWithdrawl == true ? "Withdrawal" : "Deposit";
                            //string type=banktran.ty

                            if (BrDetails.ClearingDate != null)
                            {
                                if (banktran.BankTransferDetails.Where(a => a.BankClearingDate != null && a.COAId != BrDetails.COAId).Count() == 1)
                                {
                                    status = "Cleared";
                                }
                                else
                                    status = "Posted";

                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}' WHERE Id = '{2}' and Type = '{3}'", "Bean.BankTransferDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, /*banktran.Id*/detailId, type));
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.BankTransfer", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null,/* BrDetails.ClearingStatus*/status, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", banktran.Id));
                            }
                            else
                            {
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL WHERE Id = '{1}' and Type = '{2}'", "Bean.BankTransferDetail", /*banktran.Id*/detailId, type));
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL,DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.BankTransfer", /* BrDetails.ClearingStatus*/"Posted", String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", banktran.Id));
                            }
                        }
                    }
                    else if (journal.DocSubType == "Opening Bal")
                    {
                        //var paym = appsworldcontext.Payments.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        var openingBalanceDetail = appsworldcontext.OpeningBalanceDetails.Where(a => a.OpeningBalanceId == BrDetails.DocumentId && a.COAId == journal.COAId).FirstOrDefault();
                        if (openingBalanceDetail != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}', ClearingState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.OpeningBalanceDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow) /*DateTime.UtcNow*/, BrDetails.ModifiedBy, openingBalanceDetail.Id));
                        }
                    }
                }
                appsworldcontext.SaveChanges();
            }

        }

        public void UpdateAllDocuments(BankReconciliationModel brModel, long companyId, bool isBankReconcile, string ConnectionString, List<DocumentHistoryModel> lstDocHistoryModel)
        {

            foreach (var BrDetails in brModel.BankReconciliationDetailModels)
            {
                JournalDetail journal = new JournalDetail();
                //if (BrDetails.DocumentType == "Transfer")
                //        journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.IsWithdrawal == BrDetails.Withdrawal).FirstOrDefault();
                //else
                //journal = appsworldcontext.JournalDetails.Where(a => a.Id == BrDetails.JournalId).FirstOrDefault();
                if (BrDetails.DocumentType == "Transfer" || BrDetails.DocumentType == "Bill Payment" || BrDetails.DocumentType == "Receipt")
                {
                    journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.COAId == BrDetails.COAId).FirstOrDefault();
                }
                else
                    journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.DocumentDetailId == new Guid() && a.COAId == BrDetails.COAId && a.Id == BrDetails.JournalDetailId).FirstOrDefault();

                List<Journal> lstJournal = appsworldcontext.Journals.Where(x => x.DocNo == BrDetails.DocRefNo && x.CompanyId == companyId && x.DocumentState!= BRConstants.Void).ToList();
                if (journal != null)
                {
                    if (BrDetails.ClearingDate != null)
                    {
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}',ClearingStatus='{2}',ReconciliationDate='{3}',ReconciliationId='{4}'  WHERE Id = '{5}'", "Bean.JournalDetail", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), "Cleared", String.Format("{0:MM/dd/yyyy}", brModel.StatementDate), brModel.Id, journal.Id));
                        if (lstJournal.Any())
                        {
                            foreach (var journal1 in lstJournal)
                            {
                                if (journal1.DocSubType != "Opening Bal")
                                    appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}',ClearingStatus='{2}',ModifiedBy='{3}',ModifiedDate='{4}'  WHERE Id = '{5}'", "Bean.Journal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), "Cleared", "System", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow.ToString(), journal1.Id));
                            }
                        }
                    }
                    else
                    {
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL,ClearingStatus=NULL,ReconciliationDate=NULL,ReconciliationId=NULL,IsBankReconcile=NULL  WHERE Id = '{1}'", "Bean.JournalDetail", journal.Id));
                        if (lstJournal.Any())
                        {
                            foreach (var journal1 in lstJournal)
                            {
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL,ClearingStatus=NULL,ModifiedBy='{1}',ModifiedDate='{2}'  WHERE Id = '{3}'", "Bean.Journal", "System", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow.ToString(), journal1.Id));
                            }
                        }
                    }
                    if (isBankReconcile && BrDetails.ClearingDate != null)
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET IsBankReconcile = '{1}'  WHERE Id = '{2}'", "Bean.JournalDetail", 1, journal.Id));
                }
                if (journal != null)
                {
                    if (BrDetails.DocumentType == "Receipt")
                    {
                        Receipt reciept = appsworldcontext.Receipts.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (reciept != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Receipt", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : string.Empty, BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", reciept.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL,DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Receipt", BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted",/* String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", reciept.Id));
                        }
                    }
                    //ModifiedDate=GETUTCDATE()
                    //if (BrDetails.DocumentType == "Journal")
                    //{
                    //    Receipt reciept = appsworldcontext.Receipts.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                    //    if (reciept != null)
                    //    {
                    //        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', ModifiedDate=GETUTCDATE()  WHERE Id = '{2}'", "Bean.Journal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), reciept.Id));
                    //    }
                    //}
                    else if (BrDetails.DocumentType == "Bill Payment")
                    {
                        var paym = appsworldcontext.Payments.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (paym != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Payment", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", paym.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Payment", BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", paym.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Deposit" || BrDetails.DocumentType == "Cash Payment")
                    {
                        var withdraw = appsworldcontext.WithDrawals.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Withdrawal", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null
                                , BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Withdrawal", BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));

                            //DateTime? date = BrDetails.ClearingDate;
                            //appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '"+ date + "', DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Withdrawal"
                            //    , BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, BrDetails.ModifiedBy, withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Withdrawal")
                    {
                        var withdraw = appsworldcontext.WithDrawals.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Withdrawal", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : string.Empty, BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted",/* String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Withdrawal", BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Cash Sale")
                    {
                        var withdraw = appsworldcontext.CashSales.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.CashSale", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.CashSale", BrDetails.ClearingStatus != null ? BrDetails.ClearingStatus : "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Transfer")
                    {
                        string status = null;
                        var banktran = appsworldcontext.BankTransfers.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).Include(a => a.BankTransferDetails).FirstOrDefault();
                        Guid detailId = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Id).FirstOrDefault();
                        string type = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Type).FirstOrDefault();

                        if (banktran != null)
                        {
                            //string type = BrDetails.isWithdrawl == true ? "Withdrawal" : "Deposit";
                            //string type=banktran.ty

                            if (BrDetails.ClearingDate != null)
                            {
                                if (banktran.BankTransferDetails.Where(a => a.BankClearingDate != null && a.COAId != BrDetails.COAId).Count() == 1)
                                {
                                    status = "Cleared";
                                }
                                else
                                    status = "Posted";

                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}' WHERE Id = '{2}' and Type = '{3}'", "Bean.BankTransferDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, /*banktran.Id*/detailId, type));
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.BankTransfer", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null,/* BrDetails.ClearingStatus*/status, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", banktran.Id));
                            }
                            else
                            {
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL WHERE Id = '{1}' and Type = '{2}'", "Bean.BankTransferDetail", /*banktran.Id*/detailId, type));
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL,DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.BankTransfer", /* BrDetails.ClearingStatus*/"Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", banktran.Id));
                            }
                        }
                    }
                    //else if (BrDetails.DocumentType == "Transfer")
                    //{
                    //    string status = null;
                    //    var banktran = appsworldcontext.BankTransfers.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).Include(a => a.BankTransferDetails).FirstOrDefault();
                    //    Guid detailId = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Id).FirstOrDefault();


                    //    if (banktran != null)
                    //    {
                    //        string type = BrDetails.isWithdrawl == true ? "Withdrawal" : "Deposit";

                    //        if (BrDetails.ClearingDate != null)
                    //        {
                    //            if (banktran.BankTransferDetails.Where(a => a.BankClearingDate != null && a.COAId != BrDetails.COAId).Count() == 1)
                    //            {
                    //                status = "Cleared";
                    //            }
                    //            else
                    //                status = "Posted";

                    //            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}' WHERE Id = '{2}' and Type = '{3}'", "Bean.BankTransferDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, /*banktran.Id*/detailId, type));
                    //            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.BankTransfer", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null,/* BrDetails.ClearingStatus*/status, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", banktran.Id));
                    //        }
                    //        else
                    //        {
                    //            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL WHERE Id = '{1}' and Type = '{2}'", "Bean.BankTransferDetail", /*banktran.Id*/detailId, type));
                    //            appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL,DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.BankTransfer", /* BrDetails.ClearingStatus*/"Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", banktran.Id));
                    //        }
                    //    }
                    //}
                    else if (journal.DocSubType == "Opening Bal")
                    {
                        //var paym = appsworldcontext.Payments.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        var openingBalanceDetail = appsworldcontext.OpeningBalanceDetails.Where(a => a.OpeningBalanceId == BrDetails.DocumentId && a.COAId == journal.COAId).FirstOrDefault();
                        if (openingBalanceDetail != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}', ClearingState='{2}',ModifiedDate='{3}',ModifiedBy='{4}',ReconciliationDate='{5}',ReconciliationId='{6}'  WHERE Id = '{7}'", "Bean.OpeningBalanceDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, "Reconciled", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", brModel.StatementDate, brModel.Id, openingBalanceDetail.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL, ClearingState=NULL,  ModifiedDate='{1}',ModifiedBy='{2}',ReconciliationDate=NULL,ReconciliationId=NULL  WHERE Id = '{3}'", "Bean.OpeningBalanceDetail", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", openingBalanceDetail.Id));
                        }
                    }
                }
                appsworldcontext.SaveChanges();

                #region Document_Hstory
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(brModel.Id, brModel.CompanyId, BrDetails.DocumentId.Value, BrDetails.DocumentType, BrDetails.DocSubType, BrDetails.ClearingDate != null ? "Cleared" : "Posted", string.Empty, BrDetails.Ammount.Value, 0, 0, "System", string.Empty, brModel.BankReconciliationDate, 0,0);
                    if (lstdocumet.Any())
                        //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                        lstDocHistoryModel.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {

                }

                #endregion Document_Hstory

            }
        }



        public void UpdateClearingDates(List<BankReconciliationDetailModel> details, long companyId, bool isBankReconcile, string ConnectionStrings)
        {
            Guid Ids = Guid.NewGuid();

            foreach (var BrDetails in details)
            {
                JournalDetail journal = new JournalDetail();
                //if (BrDetails.DocumentType == "Transfer")
                //        journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.IsWithdrawal == BrDetails.Withdrawal).FirstOrDefault();
                //else
                if (BrDetails.DocumentType == "Transfer" || BrDetails.DocumentType == "Bill Payment" || BrDetails.DocumentType == "Receipt")
                {
                    journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.COAId == BrDetails.COAId).FirstOrDefault();
                }
                else
                    journal = appsworldcontext.JournalDetails.Where(a => a.DocumentId == BrDetails.DocumentId && a.DocumentDetailId == new Guid() && a.COAId == BrDetails.COAId && a.Id == BrDetails.JournalDetailId).FirstOrDefault();
                //journal = appsworldcontext.JournalDetails.Where(a => a.Id == BrDetails.JournalId).FirstOrDefault();
                List<Journal> lstJournal = appsworldcontext.Journals.Where(x => x.DocNo == BrDetails.DocRefNo && x.CompanyId == companyId).ToList();
                if (journal != null)
                {
                    if (BrDetails.ClearingDate != null)
                    {
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}',ClearingStatus='{2}' WHERE Id = '{3}'", "Bean.JournalDetail", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), "Cleared", journal.Id));
                        if (lstJournal.Any())
                        {
                            foreach (var journal1 in lstJournal)
                            {
                                if (journal1.DocSubType != "Opening Bal")
                                    appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}',ClearingStatus='{2}',ModifiedBy='{3}',ModifiedDate='{4}'  WHERE Id = '{5}'", "Bean.Journal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), "Cleared", "System", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, journal1.Id));
                            }
                        }
                    }
                    else
                    {
                        appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL,ClearingStatus=NULL,IsBankReconcile=NULL  WHERE Id = '{1}'", "Bean.JournalDetail", journal.Id));
                        if (lstJournal.Any())
                        {
                            foreach (var journal1 in lstJournal)
                            {
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL,ClearingStatus=NULL,ModifiedBy='{1}',ModifiedDate='{2}'  WHERE Id = '{3}'", "Bean.Journal", "System", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow.ToString(), journal1.Id));
                            }
                        }
                    }

                    //if (isBankReconcile)
                    //    appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET IsBankReconcile = '{1}'  WHERE Id = '{2}'", "Bean.JournalDetail", 1, journal.Id));

                }
                if (journal != null)
                //if (journal.DocumentDetailId == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    if (BrDetails.DocumentType == "Receipt")
                    {
                        Receipt reciept = appsworldcontext.Receipts.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (reciept != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Receipt", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", reciept.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL,DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Receipt", "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", reciept.Id));

                        }
                    }

                    else if (BrDetails.DocumentType == "Bill Payment")
                    {
                        var paym = appsworldcontext.Payments.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (paym != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Payment", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)/*DateTime.UtcNow*/, "System", paym.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Payment", "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", paym.Id));

                        }
                    }
                    else if (BrDetails.DocumentType == "Deposit" || BrDetails.DocumentType == "Cash Payment")
                    {
                        var withdraw = appsworldcontext.WithDrawals.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Withdrawal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Withdrawal", "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Withdrawal")
                    {
                        var withdraw = appsworldcontext.WithDrawals.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.Withdrawal", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.Withdrawal", "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Cash Sale")
                    {
                        var withdraw = appsworldcontext.CashSales.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        if (withdraw != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}', DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.CashSale", String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate), BrDetails.ClearingStatus, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL, DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.CashSale", "Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", withdraw.Id));
                        }
                    }
                    else if (BrDetails.DocumentType == "Transfer")
                    {
                        string status = null;
                        var banktran = appsworldcontext.BankTransfers.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).Include(a => a.BankTransferDetails).FirstOrDefault();
                        Guid detailId = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Id).FirstOrDefault();
                        string type = banktran.BankTransferDetails.Where(a => a.COAId == BrDetails.COAId && a.ServiceCompanyId == BrDetails.ServiceEntityId && a.BankTransferId == banktran.Id).Select(a => a.Type).FirstOrDefault();

                        if (banktran != null)
                        {
                            //string type = BrDetails.isWithdrawl == true ? "Withdrawal" : "Deposit";
                            //string type=banktran.ty

                            if (BrDetails.ClearingDate != null)
                            {
                                if (banktran.BankTransferDetails.Where(a => a.BankClearingDate != null && a.COAId != BrDetails.COAId).Count() == 1)
                                {
                                    status = "Cleared";
                                }
                                else
                                    status = "Posted";

                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}' WHERE Id = '{2}' and Type = '{3}'", "Bean.BankTransferDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, /*banktran.Id*/detailId, type));
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = '{1}',DocumentState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.BankTransfer", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null,/* BrDetails.ClearingStatus*/status, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", banktran.Id));
                            }
                            else
                            {
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL WHERE Id = '{1}' and Type = '{2}'", "Bean.BankTransferDetail", /*banktran.Id*/detailId, type));
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET BankClearingDate = NULL,DocumentState='{1}',ModifiedDate='{2}',ModifiedBy='{3}'  WHERE Id = '{4}'", "Bean.BankTransfer", /* BrDetails.ClearingStatus*/"Posted", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", banktran.Id));
                            }
                        }
                    }
                    else if (journal.DocSubType == "Opening Bal")
                    {
                        //var paym = appsworldcontext.Payments.Where(a => a.Id == BrDetails.DocumentId && a.CompanyId == companyId).FirstOrDefault();
                        var openingBalanceDetail = appsworldcontext.OpeningBalanceDetails.Where(a => a.OpeningBalanceId == BrDetails.DocumentId && a.COAId == journal.COAId).FirstOrDefault();
                        if (openingBalanceDetail != null)
                        {
                            if (BrDetails.ClearingDate != null)
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = '{1}', ClearingState='{2}',ModifiedDate='{3}',ModifiedBy='{4}'  WHERE Id = '{5}'", "Bean.OpeningBalanceDetail", BrDetails.ClearingDate != null ? String.Format("{0:MM/dd/yyyy}", BrDetails.ClearingDate) : null, BrDetails.ClearingStatus, /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/ DateTime.UtcNow, "System", openingBalanceDetail.Id));
                            else
                                appsworldcontext.Database.ExecuteSqlCommand(string.Format("UPDATE {0} SET ClearingDate = NULL, ClearingState=NULL,  ModifiedDate='{1}',ModifiedBy='{2}',ReconciliationDate=NULL,ReconciliationId=NULL  WHERE Id = '{3}'", "Bean.OpeningBalanceDetail", /*String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)*/DateTime.UtcNow, "System", openingBalanceDetail.Id));
                        }
                    }
                }
                appsworldcontext.SaveChanges();
                #region Document_Hstory
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(Ids, companyId, BrDetails.DocumentId.Value, BrDetails.DocumentType, BrDetails.DocSubType, BrDetails.ClearingDate != null ? "Cleared" : "Posted", string.Empty, BrDetails.Ammount.Value, 0, 0, "System", string.Empty, DateTime.UtcNow, 0,0);
                    if (lstdocumet.Any())
                        AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionStrings);
                }
                catch (Exception ex)
                {

                }

                #endregion Document_Hstory

            }

        }

    }
}
