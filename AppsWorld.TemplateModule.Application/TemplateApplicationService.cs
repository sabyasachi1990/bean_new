using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.Models;
using AppsWorld.TemplateModule.Service;
using Mustache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Application
{
    public class TemplateApplicationService
    {

        private readonly IReceiptService _receiptService;
        private readonly IInvoiceService _invoiceEntityService;
        private readonly IBeanEmtityService _beanEntityService;
        private readonly IGenericTemplateService _genericTemlateService;


        public TemplateApplicationService(IReceiptService receiptService, IInvoiceService invoiceEntityService, IBeanEmtityService beanEntityService, IGenericTemplateService genericTemlateService)
        {
            _receiptService = receiptService;
            _invoiceEntityService = invoiceEntityService;
            _beanEntityService = beanEntityService;
            _genericTemlateService = genericTemlateService;
        }




        public TemplatesVM GenerateTemplates(TemplatesVM templateVM)
        {
            string result = null;
            Company company = _beanEntityService.GetServiceCompany(templateVM.CompanyId);
            BeanEntity entity = _beanEntityService.GetEntity(templateVM.EntityId);
            var genericTemplate = _genericTemlateService.GetGenerictemplate(templateVM.CompanyId, templateVM.TemplateType);
            List<Address> address = _beanEntityService.GetAddress(entity.Id);
            Bank bank = company.Bank.FirstOrDefault();
            Invoice invoice = entity.Invoices.Where(b => b.Id == templateVM.ScreenId && b.DocType == templateVM.TemplateType).FirstOrDefault();
            FormatCompiler compiler = new FormatCompiler();
            MustacheModel mustacheModel = new MustacheModel();
            FillServiceEntitiesEntity2MustacheModel(company, entity, address, bank, mustacheModel);
            switch (genericTemplate.TemplateType)
            {
                case "Invoice":
                    mustacheModel.Invoice = FillInvoiceTemplateEntity2Model(invoice, company, mustacheModel);
                    break;
                case "Credit Note":
                    mustacheModel.CreditNote = FillInvoiceTemplateEntity2Model(invoice, company, mustacheModel);
                    break;
                case "Receipt":
                    FillReceiptEntit2MustacheModel(templateVM, mustacheModel);
                    break;
                case "Statement Of Account":
                    FillSoaEntity2MustacheModel(templateVM, mustacheModel);
                    break;
            }
            Generator content = compiler.Compile(genericTemplate.TempletContent);
            result = content.Render(mustacheModel);
            templateVM.TemplateContent = result;


            return templateVM;
        }

        private static void FillServiceEntitiesEntity2MustacheModel(Company company, BeanEntity entity, List<Address> address, Bank bank, MustacheModel mustacheModel)
        {
            ServiceEntity serviceEntity = new ServiceEntity();
            if (entity != null)
            {
                serviceEntity.CompanyName = company.Name;
                serviceEntity.EntityName = entity.Name;
                serviceEntity.RegistrationNo = entity.GSTRegNo;
                serviceEntity.IdentificationType = entity.IdNo;
                mustacheModel.ServiceEntity = serviceEntity;
            }
            if (address.Count() > 0)
            {
                AddressBook mailingAddbook = address.Select(b => b.AddressBook).FirstOrDefault();
                serviceEntity.MailingAddress = ($" {mailingAddbook.UnitNo},{mailingAddbook.BlockHouseNo},{mailingAddbook.Street}");
                AddressBook registredAddbook = address.Select(b => b.AddressBook).LastOrDefault();
                serviceEntity.RegisteredAddress = ($" {registredAddbook.UnitNo},{registredAddbook.BlockHouseNo},{registredAddbook.Street}");
            }
            if (bank != null)
            {
                serviceEntity.BankName = bank.Name;
                serviceEntity.SWIFTCode = bank.SwiftCode;
                serviceEntity.BankAddress = bank.BankAddress;
                serviceEntity.AccountNumber = bank.AccountNumber;
                serviceEntity.AccountName = bank.AccountName;
            }
        }

        private void FillSoaEntity2MustacheModel(TemplatesVM templateVM, MustacheModel mustacheModel)
        {
            var journelLst = _invoiceEntityService.GetJournal(templateVM.EntityId);
            var receipt1 = _receiptService.Query(v => v.CompanyId == templateVM.CompanyId).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
            mustacheModel.StatementModel = new StatementModel();
            mustacheModel.receiptModel = new ReceiptsModel();
            mustacheModel.ReceiptDetailsModel = new List<ReceiptDetaislModel>();
            mustacheModel.StatementModel.StatementDate = DateTime.UtcNow.ToString("dd/MM/yyyy"); ;
            if (receipt1 != null)
            {

                mustacheModel.receiptModel.RecieptNo = receipt1.SystemRefNo;
                mustacheModel.receiptModel.RecieptDate = receipt1.CreatedDate.Value.ToString("dd/MM/yyyy");
            }

            foreach (var journel in journelLst)
            {
                ReceiptDetaislModel statementModel = new ReceiptDetaislModel();
                statementModel.DocType = journel.DocType;
                statementModel.DocNo = journel.DocNo;
                statementModel.Currency = journel.DocCurrency;
                statementModel.GrandTotal = journel.GrandDocCreditTotal != null ? journel.GrandDocCreditTotal : journel.GrandDocDebitTotal;
                statementModel.DocBalance = journel.BalanceAmount;
                mustacheModel.ReceiptDetailsModel.Add(statementModel);

            }
        }

        private void FillReceiptEntit2MustacheModel(TemplatesVM templateVM, MustacheModel mustacheModel)
        {
            var receipt = _receiptService.Query(v => v.Id == templateVM.ScreenId).Include(v => v.ReceiptDetails).Select().FirstOrDefault();
            mustacheModel.receiptModel = new ReceiptsModel();

            mustacheModel.receiptModel.RecieptDate = receipt.CreatedDate.Value.ToString("dd/MM/yyyy");
            ;
            mustacheModel.receiptModel.PaymentMode = receipt.ModeOfReceipt;
            mustacheModel.receiptModel.RecieptNo = receipt.SystemRefNo;
            mustacheModel.receiptModel.PaymentAmount = receipt.DocCurrency + receipt.ReceiptApplicationAmmount;
            mustacheModel.receiptModel.ReceiptDetaislModel = new List<ReceiptDetaislModel>();
            foreach (var receptDetail in receipt.ReceiptDetails.ToList())
            {
                ReceiptDetaislModel rDetailmodel = new ReceiptDetaislModel();
                rDetailmodel.DocNo = receptDetail.DocumentNo;
                rDetailmodel.DocType = receptDetail.DocumentType;
                rDetailmodel.DocTot = receipt.GrandTotal;
                rDetailmodel.PaymentAmt = receptDetail.ReceiptAmount;
                rDetailmodel.Currency = receptDetail.Currency;


                mustacheModel.receiptModel.ReceiptDetaislModel.Add(rDetailmodel);

            }
            mustacheModel.receiptModel.Total = mustacheModel.receiptModel.ReceiptDetaislModel.Select(q => q.PaymentAmt).Sum();
        }

        private InvoiceTemplateVM FillInvoiceTemplateEntity2Model(Invoice invoice, Company company, MustacheModel mustacheModel)
        {
            InvoiceTemplateVM templateVm = new InvoiceTemplateVM();
            templateVm.DocNo = invoice.DocNo;
            templateVm.DocDate = invoice.DocDate.ToString("dd/MM/yyyy");
            templateVm.DueDate = invoice.DueDate.Value.Date.ToString("dd/MM/yyyy");
            List<LineItem> lstLineItem = new List<LineItem>();
            List<TaxCodeVM> lstTaxCode = new List<TaxCodeVM>();
            foreach (var invoiceItem in invoice.InvoiceDetails)
            {
                TaxCode taxtcodendName = _invoiceEntityService.GetTaxCode(invoiceItem.TaxId);
                LineItem LineItem = new LineItem();
                LineItem.ItemCode = invoiceItem.ItemCode;
                LineItem.ItemDescription = invoiceItem.ItemDescription;
                LineItem.Quantity = invoiceItem.Qty;
                LineItem.UnitPrice = invoiceItem.UnitPrice;
                LineItem.Unit = invoiceItem.Unit;
                LineItem.TaxCode = taxtcodendName.Code;
                LineItem.Amount = invoiceItem.DocAmount;
                LineItem.Currency = invoiceItem.AmtCurrency;
                lstLineItem.Add(LineItem);

                TaxCodeVM taxCodeName = new TaxCodeVM();
                taxCodeName.TaxName = taxtcodendName.Name;
                taxCodeName.TaxRate = invoiceItem.TaxRate;
                taxCodeName.SubTotal = invoiceItem.BaseTotalAmount;
                lstTaxCode.Add(taxCodeName);

            }

            templateVm.Total = lstLineItem.Select(b => b.Amount).Sum();
            templateVm.SubTotal = lstTaxCode.Select(v => v.SubTotal).Sum();
            templateVm.IncludeGST = lstTaxCode.Select(v => v.SubTotal).Sum();
            templateVm.ExcludeGST = lstLineItem.Select(v => v.Amount).Sum();
            templateVm.GSTPayable = invoice.InvoiceDetails.Select(b => b.BaseTaxAmount).Sum();

            if (mustacheModel.Invoice != null)
            {
                mustacheModel.Invoice = new InvoiceTemplateVM();
                mustacheModel.Invoice.LineItem = new List<LineItem>();
                mustacheModel.Invoice.TaxCode = new List<TaxCodeVM>();
                mustacheModel.Invoice = templateVm;
                mustacheModel.Invoice.LineItem = lstLineItem;
                mustacheModel.Invoice.TaxCode = lstTaxCode;
            }
            else
            {
                mustacheModel.CreditNote = new InvoiceTemplateVM();
                mustacheModel.CreditNote.LineItem = new List<LineItem>();
                mustacheModel.CreditNote.TaxCode = new List<TaxCodeVM>();
                mustacheModel.CreditNote = templateVm;
                mustacheModel.CreditNote.LineItem = lstLineItem;
                mustacheModel.CreditNote.TaxCode = lstTaxCode;
            }

            return templateVm;
        }

    }
}
