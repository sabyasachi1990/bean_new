using AppsWorld.BankReconciliationModule.Application;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.BankWithdrawalModule.Application;
using AppsWorld.BankWithdrawalModule.Entities;
using ElasticClientWrapper;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AppsWorld.Bean.WebAPI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AppsWorld.Bean.WebAPI.App_Start.NinjectWebCommon), "Stop")]

namespace AppsWorld.Bean.WebAPI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Http;
    using AppsWorld.ReceiptModule.Application;
    using AppsWorld.ReceiptModule.Entities;
    using AppsWorld.ReceiptModule.RepositoryPattern;
    using AppsWorld.Bean.WebAPI.Helper;
    using AppsWorld.BillModule.Application;
    using AppsWorld.BillModule.RepositoryPattern;
    using AppsWorld.BillModule.Entities;
    using JournalVoucherModule.RepositoryPattern;
    using AppsWorld.RevaluationModule.Entities;
    using AppsWorld.RevaluationModule.RepositoryPattern;
    using AppsWorld.RevaluationModule.Service;
    using AppsWorld.PaymentModule.Application;
    using AppsWorld.PaymentModule.Service;
    using AppsWorld.PaymentModule.RepositoryPattern;
    using AppsWorld.PaymentModule.Entities;
    using AppsWorld.BankWithdrawalModule.RepositoryPattern;
    using AppsWorld.OpeningBalancesModule.Application;
    using AppsWorld.OpeningBalancesModule.RepositoryPattern;
    using AppsWorld.OpeningBalancesModule.Entities;
    using AppsWorld.CashSalesModule.Application;
    using AppsWorld.CashSalesModule.RepositoryPattern;
    using AppsWorld.CashSalesModule.Entities.Context;
    using AppsWorld.MasterModule.Application;
    using AppsWorld.MasterModule.RepositoryPattern;
    using AppsWorld.MasterModule.Entities;
    using AppsWorld.InvoiceModule.Application;
    using AppsWold.InvoiceModule.RepositoryPattern;
    using AppsWorld.InvoiceModule.Entities;
    using AppsWorld.InvoiceModule.RepositoryPattern;
    using AppsWorld.DebitNoteModule.Application;
    using AppsWorld.DebitNoteModule.RepositoryPattern;
    using AppsWorld.DebitNoteModule.Entities;
    using AppsWorld.BankTransferModule.Application;
    using AppsWorld.BankTransferModule.RepositoryPattern;
    using AppsWorld.BankTransferModule.Entities.Models.Context;
    using AppsWorld.CreditMemoModule.Application;
    using AppsWorld.CreditMemoModule.RepositoryPattern;
    using AppsWorld.CreditMemoModule.Entities;
    using GLClearingModule.Application;
    using GLClearingModule.RepositoryPattern;
    using GLClearingModule.Entities;
    using TemplateModule.Application;
    using TemplateModule.RepositoryPattern;
    using TemplateModule.Entities.Context;
    using AppsWorld.CashSalesModule.Application.V2;
    using AppsWorld.CashSalesModule.RepositoryPattern.V2;
    using AppsWorld.CashSalesModule.Entities.Context.V2;
    using AppsWorld.InvoiceModule.Application.V2;
    using AppsWorld.InvoiceModule.Entities.V2;
    using AppsWorld.InvoiceModule.RepositoryPattern.V2;
    using AppsWorld.CommonModule.Application.V2;
    using AppsWorld.CommonModule.RepositoryPattern.V2;
    using AppsWorld.CommonModule.Entities.V2;
    using AppsWorld.TemplateModule.Application.V2;
    using AppsWorld.TemplateModule.RepositoryPattern.V2;
    using AppsWorld.TemplateModule.Service.V2;
    using AppsWorld.DebitNoteModule.Application.V2;
    using AppsWorld.DebitNoteModule.RepositoryPattern.V2;
    using AppsWorld.DebitNoteModule.Entities.V2;
    using AppsWorld.BankTransferModule.Application.V2;
    using AppsWorld.BankTransferModule.RepositoryPattern.V2;
    using AppsWorld.BankTransferModule.Entities.V2;
    using RevaluationModule.Application.V2;
    using RevaluationModule.RepositoryPattern.V2;
    using RevaluationModule.Entities.V2;
    using ReceiptModule.Application.V2;
    using ReceiptModule.RepositoryPattern.V2;
    using ReceiptModule.Entities.Context.V2;
    using ReminderModule.Application;
    using ReminderModule.RepositoryPattern;
    using ReminderModule.Entities.Context;
    using ReminderModule.Entities.Entities;
    using MasterModule.Entities.Models;
    using AppsWorld.ReminderModule.Entities.Context.V2Context;
    using AppsWorld.ReminderModule.RepositoryPattern.V2Repository;
    using AppsWorld.JournalVoucherModule.RepositoryPattern.V3;
    using AppsWorld.JournalVoucherModule.Entities.Context.V3;
    using AppsWorld.JournalVoucherModule.Service.cs.V3.Journal;
    using AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal;
    using AppsWorld.JournalVoucherModule.Application.V3;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            RegisterReceiptServices(kernel);
            RegisterBillServices(kernel);
            RegisterBankReconciliationServices(kernel);
            RegisterJournalVoucherServices(kernel);
            RegisterRevaluationService(kernel);
            RegisterCommonServices(kernel);
            RegisterPaymentService(kernel);
            RegisterBankWithdrawalService(kernel);
            RegisterOpeningBalanceService(kernel);
            RegisterCashSaleService(kernel);
            RegisterMasterModule(kernel);
            RegisterInvoiceModule(kernel);
            RegisterDebitNoteService(kernel);
            RegisterBankTransfer(kernel);
            RegisterCreditMemo(kernel);
            RegisterGLClearing(kernel);
            RegisterTemplatesService(kernel);
            RegisterReminderService(kernel);

            #region New_Optimized_Block

            RegisterCashSaleKServices(kernel);
            RegisterInvoiceKService(kernel);
            RegisterCommonKService(kernel);
            RegisterInvoiceService(kernel);
            RegisterCashSaleMasterServices(kernel);
            ReceiptKApplicationService(kernel);
            RegisterTempletKServices(kernel);
            RegisterDebitNoteKService(kernel);
            RegisterTransferKService(kernel);
            RegisterRevaluationKService(kernel);
            RegisterRevaluationMasterService(kernel);
            RegisterCNApplicationService(kernel);

            RegisterReminderKService(kernel);
            RegisterJournalV3Service(kernel);
            #endregion New_Optimized_Block
        }

        private static void RegisterReceiptServices(IKernel kernel)
        {

            kernel.Bind<ReceiptApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IReceiptModuleDataContextAsync>().To<ReceiptContext>().InRequestScope();
            kernel.Bind<IReceiptModuleUnitOfWorkAsync>().To<ReceiptModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Receipt>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.Receipt>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Company>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Currency>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.Currency>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.TermsOfPayment>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.TermsOfPayment>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.TaxCode>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.TaxCode>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.ChartOfAccount>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.ControlCodeCategory>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.ControlCodeCategory>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.BeanEntity>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.GSTSetting>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.GSTSetting>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.ControlCode>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.ControlCode>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.ReceiptBalancingItem>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.ReceiptBalancingItem>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.FinancialSetting>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.FinancialSetting>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.CompanySetting>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.CompanySetting>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.MultiCurrencySetting>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.MultiCurrencySetting>>().InRequestScope();
            //kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Forex>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.Forex>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.AccountType>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.AccountType>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.ReceiptDetail>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.ReceiptDetail>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.AutoNumber>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.AutoNumberCompany>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Invoice>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.Invoice>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.InvoiceDetail>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.InvoiceDetail>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.DebitNote>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.DebitNote>>().InRequestScope();
            //kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.ReceiptGSTDetail>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.ReceiptGSTDetail>>().InRequestScope();
            //kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.BankReconciliationSetting>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.BankReconciliationSetting>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Journal>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.JournalDetail>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.BillCompact>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.BillCompact>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.CreditMemoCompact>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.CreditMemoCompact>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.CreditNoteApplicationCompact>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.CreditNoteApplicationCompact>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.CreditMemoApplicationCompact>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.CreditMemoApplicationCompact>>().InRequestScope();
            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<ReceiptModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();


            kernel.Bind<AppsWorld.ReceiptModule.Service.IReceiptService>().To<AppsWorld.ReceiptModule.Service.ReceiptService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.ICompanyService>().To<AppsWorld.ReceiptModule.Service.CompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.ICurrencyService>().To<AppsWorld.ReceiptModule.Service.CurrencyService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.ITermsOfPaymentService>().To<AppsWorld.ReceiptModule.Service.TermsOfPaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.ITaxCodeService>().To<AppsWorld.ReceiptModule.Service.TaxCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IChartOfAccountService>().To<AppsWorld.ReceiptModule.Service.ChartOfAccountService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IControlCodeCategoryService>().To<AppsWorld.ReceiptModule.Service.ControlCodeCategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IBeanEntityService>().To<AppsWorld.ReceiptModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IGSTSettingService>().To<AppsWorld.ReceiptModule.Service.GstSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IControlCodeService>().To<AppsWorld.ReceiptModule.Service.ControlCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IReceiptBalancingItemService>().To<AppsWorld.ReceiptModule.Service.ReceiptBalancingItemService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IFinancialSettingService>().To<AppsWorld.ReceiptModule.Service.FinancialSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.ICompanySettingService>().To<AppsWorld.ReceiptModule.Service.CompanySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IMultiCurrencySettingService>().To<AppsWorld.ReceiptModule.Service.MultiCurrencySettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.ReceiptModule.Service.IForexService>().To<AppsWorld.ReceiptModule.Service.ForexService>().InRequestScope();

            kernel.Bind<AppsWorld.ReceiptModule.Service.IReceiptDetailService>().To<AppsWorld.ReceiptModule.Service.ReceiptDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IAutoNumberCompanyService>().To<AppsWorld.ReceiptModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IAutoNumberService>().To<AppsWorld.ReceiptModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IInvoiceService>().To<AppsWorld.ReceiptModule.Service.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IDebitNoteService>().To<AppsWorld.ReceiptModule.Service.DebitNoteService>().InRequestScope();
            //kernel.Bind<AppsWorld.ReceiptModule.Service.IReceiptGSTDetailsService>().To<AppsWorld.ReceiptModule.Service.ReceiptGSTDetailsService>().InRequestScope();
            //kernel.Bind<AppsWorld.ReceiptModule.Service.IBankReconciliationSettingService>().To<AppsWorld.ReceiptModule.Service.BankReconciliationSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IJournalService>().To<AppsWorld.ReceiptModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.IJournalDetailService>().To<AppsWorld.ReceiptModule.Service.JournalDetailService>().InRequestScope();

            kernel.Bind<IReceiptModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.CompanyUser>>().To<ReceiptModuleRepository<AppsWorld.ReceiptModule.Entities.CompanyUser>>().InRequestScope();
        }

        private static void RegisterBillServices(IKernel kernel)
        {
            kernel.Bind<BillApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IBillModuleDataContextAsync>().To<BillContext>().InRequestScope();
            kernel.Bind<IBillModuleUnitOfWorkAsync>().To<BillModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.Bill>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.Bill>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.TermsOfPayment>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.TermsOfPayment>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.BeanEntity>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.GSTSetting>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.GSTSetting>>().InRequestScope();
            //kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.Forex>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.Forex>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.MultiCurrencySetting>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.MultiCurrencySetting>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.FinancialSetting>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.FinancialSetting>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.CompanySetting>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.CompanySetting>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.ControlCodeCategory>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.ControlCodeCategory>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.Currency>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.Currency>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.ControlCode>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.ControlCode>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.Company>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.TaxCode>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.TaxCode>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.BillDetail>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.BillDetail>>().InRequestScope();
            //kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.SegmentMaster>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.SegmentMaster>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.ChartOfAccount>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.ChartOfAccount>>().InRequestScope();
            //kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.BillGSTDetail>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.BillGSTDetail>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.AutoNumber>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.AutoNumberCompany>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<BillModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.Journal>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.PaymentDetail>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.PaymentDetail>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.Payment>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.Payment>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.CreditMemoApplication>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.CreditMemoApplication>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.CreditMemoApplicationDetail>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.CreditMemoApplicationDetail>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.CreditMemo>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.CreditMemo>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<Localization>>().To<BillModuleRepository<Localization>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.AccountType>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.AccountType>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.JournalDetail>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.ReceiptCompact>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.ReceiptCompact>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.ReceiptDetailCompact>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.ReceiptDetailCompact>>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.CommonForex>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.CommonForex>>().InRequestScope();
            //kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.BeanAutoNumber>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.BeanAutoNumber>>().InRequestScope();




            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.PeppolInboundInvoice>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.PeppolInboundInvoice>>().InRequestScope();




            kernel.Bind<AppsWorld.BillModule.Service.IBillService>().To<AppsWorld.BillModule.Service.BillService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ITermsOfPaymentService>().To<AppsWorld.BillModule.Service.TermsOfPaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IBeanEntityService>().To<AppsWorld.BillModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICompanySettingService>().To<AppsWorld.BillModule.Service.CompanySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICompanyService>().To<AppsWorld.BillModule.Service.CompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IGSTSettingService>().To<AppsWorld.BillModule.Service.GstSettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.BillModule.Service.IForexService>().To<AppsWorld.BillModule.Service.ForexService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IMultiCurrencySettingService>().To<AppsWorld.BillModule.Service.MultiCurrencySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IFinancialSettingService>().To<AppsWorld.BillModule.Service.FinancialSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IControlCodeCategoryService>().To<AppsWorld.BillModule.Service.ControlCodeCategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICurrencyService>().To<AppsWorld.BillModule.Service.CurrencyService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ITaxCodeService>().To<AppsWorld.BillModule.Service.TaxCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IControlCodeService>().To<AppsWorld.BillModule.Service.ControlCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IBillDetailService>().To<AppsWorld.BillModule.Service.BillDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IChartOfAccountService>().To<AppsWorld.BillModule.Service.ChartOfAccountService>().InRequestScope();
            //kernel.Bind<AppsWorld.BillModule.Service.ISegmentMasterService>().To<AppsWorld.BillModule.Service.SegmentMasterService>().InRequestScope();
            //kernel.Bind<AppsWorld.BillModule.Service.IBillGstDetailService>().To<AppsWorld.BillModule.Service.BillGstDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IAutoNumberCompanyService>().To<AppsWorld.BillModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IAutoNumberService>().To<AppsWorld.BillModule.Service.AutoNumberService>().InRequestScope();

            kernel.Bind<AppsWorld.BillModule.Service.IJournalService>().To<AppsWorld.BillModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IPaymentDetailService>().To<AppsWorld.BillModule.Service.PaymentDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICreditMemoApplicationDetailService>().To<AppsWorld.BillModule.Service.CreditMemoApplicationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICreditMemoService>().To<AppsWorld.BillModule.Service.CreditMemoService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICreditMemoApplicationService>().To<AppsWorld.BillModule.Service.CreditMemoApplicationService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IPaymentService>().To<AppsWorld.BillModule.Service.PaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ILocalizationService>().To<AppsWorld.BillModule.Service.LocalizationService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IAccountTypeService>().To<AppsWorld.BillModule.Service.AccountTypeService>().InRequestScope();
            kernel.Bind<IBillModuleRepositoryAsync<AppsWorld.BillModule.Entities.CompanyUser>>().To<BillModuleRepository<AppsWorld.BillModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.IJournalDetailService>().To<AppsWorld.BillModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BillModule.Service.ICommonForexService>().To<AppsWorld.BillModule.Service.CommonForexService>().InRequestScope();

        }

        private static void RegisterBankReconciliationServices(IKernel kernel)
        {
            kernel.Bind<BankReconciliationApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IBankReconciliationModuleDataContextAsync>().To<BankReconciliationContext>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleUnitOfWorkAsync>().To<BankReconciliationModuleUnitOfWork>().InRequestScope();
            //kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.AccountType>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.AccountType>>().InRequestScope();

            //kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.ChartOfAccount>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.ChartOfAccount>>().InRequestScope();

            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.BankReconciliation>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.BankReconciliation>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.BankReconciliationDetail>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.BankReconciliationDetail>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.Receipt>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.Receipt>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.FinancialSetting>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.FinancialSetting>>().InRequestScope();
            // kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.BeanEntity>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.Company>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.Journal>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.JournalDetail>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.JournalDetail>>().InRequestScope();

            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.Payment>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.Payment>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.Withdrawal>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.Withdrawal>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.BankTransfer>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.BankTransfer>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.CashSale>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.CashSale>>().InRequestScope();
            //kernel.Bind<AppsWorld.BankReconciliationModule.Service.IAccountTypeService>().To<AppsWorld.BankReconciliationModule.Service.AccountTypeService>().InRequestScope();
            //kernel.Bind<AppsWorld.BankReconciliationModule.Service.ICompanyService>().To<AppsWorld.BankReconciliationModule.Service.CompanyService>().InRequestScope();
            //kernel.Bind<AppsWorld.BankReconciliationModule.Service.IChartOfAccountService>().To<AppsWorld.BankReconciliationModule.Service.ChartOfAccountService>().InRequestScope();

            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IReceiptService>().To<AppsWorld.BankReconciliationModule.Service.ReceiptService>().InRequestScope();
            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IBankReconciliationService>().To<AppsWorld.BankReconciliationModule.Service.BankReconciliationService>().InRequestScope();
            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IBankReconciliationDetailService>().To<AppsWorld.BankReconciliationModule.Service.BankReconciliationDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.BankReconciliationModule.Service.IFinancialSettingService>().To<AppsWorld.BankReconciliationModule.Service.FinancialSettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.BankReconciliationModule.Service.IBeanEntityService>().To<AppsWorld.BankReconciliationModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IJournalService>().To<AppsWorld.BankReconciliationModule.Service.JournalService>().InRequestScope();


            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IPaymentService>().To<AppsWorld.BankReconciliationModule.Service.PaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IWithdrawalService>().To<AppsWorld.BankReconciliationModule.Service.WithdrawalService>().InRequestScope();
            kernel.Bind<AppsWorld.BankReconciliationModule.Service.IBankTransferService>().To<AppsWorld.BankReconciliationModule.Service.BankTransferService>().InRequestScope();

            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.CompanyUser>>().To<BankReconciliationModuleRepository<AppsWorld.BankReconciliationModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<IBankReconciliationModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<BankReconciliationModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();


        }

        private static void RegisterJournalVoucherServices(IKernel kernel)
        {
            kernel.Bind<JournalVoucherModule.Application.JournalApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IJournalVoucherModuleDataContextAsync>().To<JournalContext>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleUnitOfWorkAsync>().To<JournalVoucherModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Journal>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Company>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.JournalDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.JournalDetail>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.JournalGSTDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.JournalGSTDetail>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.AutoNumber>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.AutoNumber>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.AutoNumberCompany>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.AutoNumberCompany>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.BeanEntity>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Bill>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Bill>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.BillCreditMemo>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.BillCreditMemo>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.BillCreditMemoDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.BillCreditMemoDetail>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.BillDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.BillDetail>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.SegmentMaster>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.SegmentMaster>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.TaxCode>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.TaxCode>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.ChartOfAccount>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.ChartOfAccount>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Company>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Company>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CompanySetting>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CompanySetting>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.ControlCode>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.ControlCode>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.ControlCodeCategory>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.ControlCodeCategory>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Currency>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Currency>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.MultiCurrencySetting>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.MultiCurrencySetting>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<JournalVoucherModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.DebitNoteDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.DebitNoteDetail>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.AutoNumberCompany>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.AutoNumberCompany>>().InRequestScope();

            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.DebitNoteNote>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.DebitNoteNote>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.FinancialSetting>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.FinancialSetting>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Forex>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Forex>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.GSTSetting>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.GSTSetting>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.AutoNumber>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.AutoNumber>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CompanySetting>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CompanySetting>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CreditNoteApplication>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CreditNoteApplication>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CreditNoteApplicationDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CreditNoteApplicationDetail>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Invoice>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Invoice>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.DoubtfulDebtAllocation>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.DoubtfulDebtAllocation>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.DoubtfulDebtAllocationDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.DoubtfulDebtAllocationDetail>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.TermsOfPayment>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.TermsOfPayment>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.InvoiceDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.InvoiceDetail>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.BeanEntity>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.BeanEntity>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.SegmentDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.SegmentDetail>>().InRequestScope();
            //kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.JvActivityLog>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.JvActivityLog>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.DebitNote>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.DebitNote>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Withdrawal>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Withdrawal>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CashSale>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CashSale>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Receipt>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Receipt>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.ActivityHistory>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.ActivityHistory>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Payment>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Payment>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CreditMemo>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CreditMemo>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CompanyFeature>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CompanyFeature>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Feature>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Feature>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.PaymentDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.PaymentDetail>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.ReceiptDetail>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.ReceiptDetail>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.AccountType>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.AccountType>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.BankReconciliation>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.BankReconciliation>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Category>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Category>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.SubCategory>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.SubCategory>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Order>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Order>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.OpeningBalance>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.OpeningBalance>>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.Models.CommonForex>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.Models.CommonForex>>().InRequestScope();



            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IJournalService>().To<AppsWorld.JournalVoucherModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICompanyService>().To<AppsWorld.JournalVoucherModule.Service.CompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IJournalDetailService>().To<AppsWorld.JournalVoucherModule.Service.JournalDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.JournalVoucherModule.Service.IJournalGSTDetailService>().To<AppsWorld.JournalVoucherModule.Service.JournalGSTDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IFinancialSettingService>().To<AppsWorld.JournalVoucherModule.Service.FinancialSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IGSTSettingService>().To<AppsWorld.JournalVoucherModule.Service.GstSettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.JournalVoucherModule.Service.IForexService>().To<AppsWorld.JournalVoucherModule.Service.ForexService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICompanySettingService>().To<AppsWorld.JournalVoucherModule.Service.CompanySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IMultiCurrencySettingService>().To<AppsWorld.JournalVoucherModule.Service.MultiCurrencySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IAutoNumberCompanyService>().To<AppsWorld.JournalVoucherModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IAutoNumberService>().To<AppsWorld.JournalVoucherModule.Service.AutoNumberService>().InRequestScope();

            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IControlCodeCategoryService>().To<AppsWorld.JournalVoucherModule.Service.ControlCodeCategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ITaxCodeService>().To<AppsWorld.JournalVoucherModule.Service.TaxCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IChartOfAccountService>().To<AppsWorld.JournalVoucherModule.Service.ChartOfAccountService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICurrencyService>().To<AppsWorld.JournalVoucherModule.Service.CurrencyService>().InRequestScope();
            //kernel.Bind<AppsWorld.JournalVoucherModule.Service.ISegmentMasterService>().To<AppsWorld.JournalVoucherModule.Service.SegmentMasterService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IControlCodeService>().To<AppsWorld.JournalVoucherModule.Service.ControlCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICreditNoteApplicationService>().To<AppsWorld.JournalVoucherModule.Service.CreditNoteApplicationService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICreditNoteApplicationDetailService>().To<AppsWorld.JournalVoucherModule.Service.CreditNoteApplicationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IInvoiceService>().To<AppsWorld.JournalVoucherModule.Service.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IDoubtfulDebtAllocationService>().To<AppsWorld.JournalVoucherModule.Service.DoubtfulDebtAllocationService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IDoubtfulDebtAllocationDetailService>().To<AppsWorld.JournalVoucherModule.Service.DoubtfulDebtAllocationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IInvoiceDetailService>().To<AppsWorld.JournalVoucherModule.Service.InvoiceDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ITermsOfPaymentService>().To<AppsWorld.JournalVoucherModule.Service.TermsOfPaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IBeanEntityService>().To<AppsWorld.JournalVoucherModule.Service.BeanEntityService>().InRequestScope();
            //kernel.Bind<AppsWorld.JournalVoucherModule.Service.ISegmentDetailService>().To<AppsWorld.JournalVoucherModule.Service.SegmentDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.JournalVoucherModule.Service.IJvActivitylogService>().To<AppsWorld.JournalVoucherModule.Service.JvActivitylogService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IDebitNoteService>().To<AppsWorld.JournalVoucherModule.Service.DebitNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IWithdrawalService>().To<AppsWorld.JournalVoucherModule.Service.WithdrawalService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICashSaleService>().To<AppsWorld.JournalVoucherModule.Service.CashSaleService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IBillService>().To<AppsWorld.JournalVoucherModule.Service.BillService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IReceiptService>().To<AppsWorld.JournalVoucherModule.Service.ReciptService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IActivityHistoryService>().To<AppsWorld.JournalVoucherModule.Service.ActivityHistoryService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IPaymentService>().To<AppsWorld.JournalVoucherModule.Service.PaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICreditMemoService>().To<AppsWorld.JournalVoucherModule.Service.CreditMemoService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICompanyFeatureService>().To<AppsWorld.JournalVoucherModule.Service.CompanyFeatureService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IFeatureService>().To<AppsWorld.JournalVoucherModule.Service.FeatureService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IPaymentDetailService>().To<AppsWorld.JournalVoucherModule.Service.PaymentDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IReceiptDetailService>().To<AppsWorld.JournalVoucherModule.Service.ReceiptDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IAccountTypeService>().To<AppsWorld.JournalVoucherModule.Service.AccountTypeService>().InRequestScope();

            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICategoryService>().To<AppsWorld.JournalVoucherModule.Service.CategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ISubCategoryService>().To<AppsWorld.JournalVoucherModule.Service.SubCategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.IOrderService>().To<AppsWorld.JournalVoucherModule.Service.OrderService>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleRepositoryAsync<AppsWorld.JournalVoucherModule.Entities.CompanyUser>>().To<JournalVoucherModuleRepository<AppsWorld.JournalVoucherModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<AppsWorld.JournalVoucherModule.Service.ICommonForexService>().To<AppsWorld.JournalVoucherModule.Service.CommonForexService>().InRequestScope();


        }
        private static void RegisterRevaluationService(IKernel kernel)
        {
            kernel.Bind<RevaluationModule.Application.RevaluationApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IRevaluationModuleDataContextAsync>().To<AppsWorld.RevaluationModule.Entities.RevaluationContext>().InRequestScope();
            kernel.Bind<IRevaluationModuleUnitOfWorkAsync>().To<RevaluationModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.Revaluation>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.Revaluation>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.Journal>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.Journal>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.MultiCurrencySetting>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.MultiCurrencySetting>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.JournalDetail>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.JournalDetail>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.ChartOfAccount>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.FinancialSetting>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.FinancialSetting>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.Forex>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.Forex>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.GSTSetting>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.GSTSetting>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.RevalutionDetail>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.RevalutionDetail>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.BeanEntity>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.BeanEntity>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.Invoice>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.Invoice>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.DebitNote>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.DebitNote>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.Bill>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.Bill>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.Company>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.Company>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.AutoNumber>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.AutoNumber>>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.AutoNumberCompany>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.AutoNumberCompany>>().InRequestScope();


            kernel.Bind<AppsWorld.RevaluationModule.Service.IRevaluationService>().To<AppsWorld.RevaluationModule.Service.RevaluationService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IJournalService>().To<AppsWorld.RevaluationModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IMultiCurrencySettingService>().To<AppsWorld.RevaluationModule.Service.MultiCurrencySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IJournalDetailService>().To<AppsWorld.RevaluationModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IFinancialSettingService>().To<AppsWorld.RevaluationModule.Service.FinancialSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IGSTSettingService>().To<AppsWorld.RevaluationModule.Service.GstSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IForexService>().To<AppsWorld.RevaluationModule.Service.ForexService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IChartOfAccountService>().To<AppsWorld.RevaluationModule.Service.ChartOfAccountService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IRevaluationDetailService>().To<AppsWorld.RevaluationModule.Service.RevaluationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IBeanEntityService>().To<AppsWorld.RevaluationModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IInvoiceService>().To<AppsWorld.RevaluationModule.Service.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IDebitNoteService>().To<AppsWorld.RevaluationModule.Service.DebitNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IBillService>().To<AppsWorld.RevaluationModule.Service.BillService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IAutoNumberService>().To<AppsWorld.RevaluationModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.IAutoNumberCompanyService>().To<AppsWorld.RevaluationModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<IRevaluationModuleRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.CompanyUser>>().To<RevaluationModuleRepository<AppsWorld.RevaluationModule.Entities.Models.CompanyUser>>().InRequestScope();
        }

        private static void RegisterCommonServices(IKernel kernel)
        {

            kernel.Bind<CommonApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ICommonModuleDataContextAsync>().To<CommonContext>().InRequestScope();
            kernel.Bind<ICommonModuleUnitOfWorkAsync>().To<CommonModuleUnitOfWork>().InRequestScope();

            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Currency>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Currency>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ControlCodeCategory>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.ControlCodeCategory>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.GSTSetting>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.GSTSetting>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ControlCode>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.ControlCode>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.FinancialSetting>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.FinancialSetting>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.CompanySetting>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.CompanySetting>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.MultiCurrencySetting>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.MultiCurrencySetting>>().InRequestScope();
            //kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Forex>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Forex>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.AccountType>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.AccountType>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.AutoNumber>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.AutoNumberCompany>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.AutoNumberCompany>>().InRequestScope();
            //kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BankReconciliationSetting>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.BankReconciliationSetting>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.TaxCode>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.TaxCode>>().InRequestScope();
            //kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.SegmentMaster>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.SegmentMaster>>().InRequestScope();
            //kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.SegmentDetail>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.SegmentDetail>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Employee>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Employee>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.TermsOfPayment>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.TermsOfPayment>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Journal>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Item>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Item>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUser>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUser>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.DocRepository>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.DocRepository>>().InRequestScope();
            kernel.Bind<ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.DocumentHistory>>().To<CommonModuleRepository<AppsWorld.CommonModule.Entities.DocumentHistory>>().InRequestScope();

            kernel.Bind<AppsWorld.CommonModule.Service.ICompanyService>().To<AppsWorld.CommonModule.Service.CompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.ICurrencyService>().To<AppsWorld.CommonModule.Service.CurrencyService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IChartOfAccountService>().To<AppsWorld.CommonModule.Service.ChartOfAccountService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IControlCodeCategoryService>().To<AppsWorld.CommonModule.Service.ControlCodeCategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IBeanEntityService>().To<AppsWorld.CommonModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IGSTSettingService>().To<AppsWorld.CommonModule.Service.GstSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IControlCodeService>().To<AppsWorld.CommonModule.Service.ControlCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IFinancialSettingService>().To<AppsWorld.CommonModule.Service.FinancialSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.ICompanySettingService>().To<AppsWorld.CommonModule.Service.CompanySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IMultiCurrencySettingService>().To<AppsWorld.CommonModule.Service.MultiCurrencySettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.CommonModule.Service.IForexService>().To<AppsWorld.CommonModule.Service.ForexService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IAccountTypeService>().To<AppsWorld.CommonModule.Service.AccountTypeService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IAutoNumberCompanyService>().To<AppsWorld.CommonModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IAutoNumberService>().To<AppsWorld.CommonModule.Service.AutoNumberService>().InRequestScope();
            //kernel.Bind<AppsWorld.CommonModule.Service.IBankReconciliationSettingService>().To<AppsWorld.CommonModule.Service.BankReconciliationSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.ITaxCodeService>().To<AppsWorld.CommonModule.Service.TaxCodeService>().InRequestScope();
            //kernel.Bind<AppsWorld.CommonModule.Service.ISegmentMasterService>().To<AppsWorld.CommonModule.Service.SegmentMasterService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IEmployeeService>().To<AppsWorld.CommonModule.Service.EmployeeService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.ITermsOfPaymentService>().To<AppsWorld.CommonModule.Service.TermsOfPaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IJournalService>().To<AppsWorld.CommonModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.CommonModule.Service.IItemService>().To<AppsWorld.CommonModule.Service.ItemService>().InRequestScope();
            //kernel.Bind<AppsWorld.CommonModule.Service.IAccountTypeService>().To<AppsWorld.CommonModule.Service.AccountTypeService>().InRequestScope();



            //kernel.Bind<ICommonModuleRepositoryAsync<DocRepository>>().To<CommonModuleRepository<DocRepository>>().InRequestScope();
            //kernel.Bind<ICommonModuleRepositoryAsync<ReferenceFiles>>().To<CommonModuleRepository<ReferenceFiles>>().InRequestScope();
            // kernel.Bind<ReferenceFiles>().ToSelf().InRequestScope();

            kernel.Bind<AppsWorld.CommonModule.Service.IDocRepositoryService>().To<AppsWorld.CommonModule.Service.DocRepositoryService>().InRequestScope();
            //kernel.Bind<AppsWorld.CommonModule.Service.IReferenceFilesService>().To<AppsWorld.CommonModule.Service.ReferenceFilesService>().InRequestScope();


        }
        private static void RegisterPaymentService(IKernel kernel)
        {
            kernel.Bind<PaymentApplicationSevice>().ToSelf().InRequestScope();
            kernel.Bind<IPaymentModuleDataContextAsync>().To<PaymentContext>().InRequestScope();
            kernel.Bind<IPaymentModuleUnitOfWorkAsync>().To<PaymentModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.Payment>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.Payment>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.PaymentDetail>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.PaymentDetail>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.Bill>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.Bill>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<PaymentModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.AutoNumber>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.AutoNumberCompany>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.JournalDetail>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<PaymentModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<PaymentModuleRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.Feature>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.Feature>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.CompanyFeature>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.CompanyFeature>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.Journal>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.CreditMemoCompact>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.CreditMemoCompact>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.InvoiceCompact>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.InvoiceCompact>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.CreditNoteApplicationCompact>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.CreditNoteApplicationCompact>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.CreditMemoApplicationCompact>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.CreditMemoApplicationCompact>>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.DebitNoteCompact>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.DebitNoteCompact>>().InRequestScope();

            kernel.Bind<AppsWorld.PaymentModule.Service.IPaymentService>().To<AppsWorld.PaymentModule.Service.PaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IPaymentDetailService>().To<AppsWorld.PaymentModule.Service.PaymentDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IBillService>().To<AppsWorld.PaymentModule.Service.BillService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IAutoNumberService>().To<AppsWorld.PaymentModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IAutoNumberCompanyService>().To<AppsWorld.PaymentModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IJournalDetailService>().To<AppsWorld.PaymentModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IFeatureService>().To<AppsWorld.PaymentModule.Service.FeatureService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.ICompanyFeatureService>().To<AppsWorld.PaymentModule.Service.CompanyFeatureService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IJournalServices>().To<AppsWorld.PaymentModule.Service.JournalService>().InRequestScope();
            kernel.Bind<IPaymentModuleRepositoryAsync<AppsWorld.PaymentModule.Entities.CompanyUser>>().To<PaymentModuleRepository<AppsWorld.PaymentModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IInvoiceCompactService>().To<AppsWorld.PaymentModule.Service.InvoiceCompactService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.IDebitNoteCompactService>().To<AppsWorld.PaymentModule.Service.DebitNoteCompactService>().InRequestScope();
            kernel.Bind<AppsWorld.PaymentModule.Service.ICreditMemoCompactService>().To<AppsWorld.PaymentModule.Service.CreditMemoCompactService>().InRequestScope();
        }
        private static void RegisterBankWithdrawalService(IKernel kernel)
        {

            kernel.Bind<BankWithdrawalApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IBankWithdrawalModuleDataContextAsync>().To<BankWithdrawalContext>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleUnitOfWorkAsync>().To<BankWithdrawalUnitOfWork>().InRequestScope();

            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.BankWithdrawalModule.Entities.Withdrawal>>().To<BankWithdrawalRepository<AppsWorld.BankWithdrawalModule.Entities.Withdrawal>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.BankWithdrawalModule.Entities.WithdrawalDetail>>().To<BankWithdrawalRepository<AppsWorld.BankWithdrawalModule.Entities.WithdrawalDetail>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.BankWithdrawalModule.Entities.GSTDetail>>().To<BankWithdrawalRepository<AppsWorld.BankWithdrawalModule.Entities.GSTDetail>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.BankWithdrawalModule.Entities.AutoNumber>>().To<BankWithdrawalRepository<AppsWorld.BankWithdrawalModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.BankWithdrawalModule.Entities.AutoNumberCompany>>().To<BankWithdrawalRepository<AppsWorld.BankWithdrawalModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<BankWithdrawalRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity>>().To<BankWithdrawalRepository<AppsWorld.CommonModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<BankWithdrawalRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

            kernel.Bind<AppsWorld.BankWithdrawalModule.Service.IWithdrawalService>().To<AppsWorld.BankWithdrawalModule.Service.WithdrawalService>().InRequestScope();
            kernel.Bind<AppsWorld.BankWithdrawalModule.Service.IWithdrawalDetailService>().To<AppsWorld.BankWithdrawalModule.Service.WithdrawalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BankWithdrawalModule.Service.IGSTDetailService>().To<AppsWorld.BankWithdrawalModule.Service.GSTDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BankWithdrawalModule.Service.IAutoNumberService>().To<AppsWorld.BankWithdrawalModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.BankWithdrawalModule.Service.IAutoNumberCompanyService>().To<AppsWorld.BankWithdrawalModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<AppsWorld.BankWithdrawalModule.Entities.CompanyUser>>().To<BankWithdrawalRepository<AppsWorld.BankWithdrawalModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<IBankWithdrawalModuleRepositoryAsync<CommonModule.Entities.Company>>().To<BankWithdrawalRepository<CommonModule.Entities.Company>>().InRequestScope();
        }
        private static void RegisterOpeningBalanceService(IKernel kernel)
        {
            kernel.Bind<OpeningBalancesApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IOpeningBalancesModuleDataContextAsync>().To<OpeningBalancesContext>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleUnitOfWorkAsync>().To<OpeningBalancesModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.OpeningBalance>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.OpeningBalance>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.OpeningBalanceDetail>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.OpeningBalanceDetail>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.OpeningBalanceDetailLineItem>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.OpeningBalanceDetailLineItem>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<OpeningBalancesModuleRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.AutoNumber>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.AutoNumberCompany>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.Bill>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.Bill>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.Invoice>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.Invoice>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.CreditMemo>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.CreditMemo>>().InRequestScope();
            //kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.OpeningBalancesModule.Entities.BeanAutoNumber>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.BeanAutoNumber>>().InRequestScope();

            kernel.Bind<AppsWorld.OpeningBalancesModule.Service.IOpeningBalanceService>().To<AppsWorld.OpeningBalancesModule.Service.OpeningBalanceService>().InRequestScope();
            kernel.Bind<AppsWorld.OpeningBalancesModule.Service.IOpeningBalanceDetailService>().To<AppsWorld.OpeningBalancesModule.Service.OpeningBalanceDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.OpeningBalancesModule.Service.IOpeningBalanceDetailLineItemService>().To<AppsWorld.OpeningBalancesModule.Service.OpeningBalanceDetailLineItemService>().InRequestScope();
            kernel.Bind<AppsWorld.OpeningBalancesModule.Service.IAutoNumberService>().To<AppsWorld.OpeningBalancesModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.OpeningBalancesModule.Service.IAutoNumberCompanyService>().To<AppsWorld.OpeningBalancesModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.OpeningBalancesModule.Service.IBillService>().To<AppsWorld.OpeningBalancesModule.Service.BillService>().InRequestScope();


            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<OpeningBalancesModule.Entities.CompanyUser>>().To<OpeningBalancesModuleRepository<AppsWorld.OpeningBalancesModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<IOpeningBalancesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<OpeningBalancesModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
        }
        private static void RegisterCashSaleService(IKernel kernel)
        {
            kernel.Bind<AppsWorld.CashSalesModule.Application.CashSaleApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ICashSalesModuleDataContextAsync>().To<CashSalesContext>().InRequestScope();
            kernel.Bind<ICashSalesModuleUnitOfWorkAsync>().To<CashSalesUnitOfWork>().InRequestScope();

            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.Models.CashSale>>().To<CashSalesRepository<AppsWorld.CashSalesModule.Entities.Models.CashSale>>().InRequestScope();
            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.Models.CashSaleDetail>>().To<CashSalesRepository<AppsWorld.CashSalesModule.Entities.Models.CashSaleDetail>>().InRequestScope();
            //kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.Models.GSTDetail>>().To<CashSalesRepository<AppsWorld.CashSalesModule.Entities.Models.GSTDetail>>().InRequestScope();
            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.Models.AutoNumber>>().To<CashSalesRepository<AppsWorld.CashSalesModule.Entities.Models.AutoNumber>>().InRequestScope();
            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.Models.AutoNumberCompany>>().To<CashSalesRepository<AppsWorld.CashSalesModule.Entities.Models.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<CashSalesRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<CashSalesRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();

            kernel.Bind<AppsWorld.CashSalesModule.Service.ICashSalesService>().To<AppsWorld.CashSalesModule.Service.CashSalesService>().InRequestScope();
            kernel.Bind<AppsWorld.CashSalesModule.Service.ICashSalesDetailService>().To<AppsWorld.CashSalesModule.Service.CashSalesDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.CashSalesModule.Service.IGSTDetailService>().To<AppsWorld.CashSalesModule.Service.GSTDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.CashSalesModule.Service.IAutoNumberService>().To<AppsWorld.CashSalesModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.CashSalesModule.Service.IAutoNumberCompanyService>().To<AppsWorld.CashSalesModule.Service.AutoNumberCompanyService>().InRequestScope();

            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.Models.CompanyUser>>().To<CashSalesRepository<AppsWorld.CashSalesModule.Entities.Models.CompanyUser>>().InRequestScope();
            kernel.Bind<ICashSalesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<CashSalesRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

        }
        private static void RegisterMasterModule(IKernel kernel)
        {
            kernel.Bind<MasterModuleApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IMasterModuleDataContextAsync>().To<MasterModuleContext>().InRequestScope();
            kernel.Bind<IMasterModuleUnitOfWorkAsync>().To<MasterModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.BeanEntity>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.AccountType>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.AccountType>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Address>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Address>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.AddressBook>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.AddressBook>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CompanyFeature>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CompanyFeature>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.ControlCode>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.ControlCode>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.ControlCodeCategory>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.ControlCodeCategory>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Currency>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Currency>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Feature>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Feature>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.IdType>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.IdType>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.MultiCurrencySetting>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.MultiCurrencySetting>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.TermsOfPayment>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.TermsOfPayment>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Models.SSICCodes>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Models.SSICCodes>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.FinancialSetting>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.FinancialSetting>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Invoice>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Invoice>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.GSTSetting>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.GSTSetting>>().InRequestScope();
            //kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Forex>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Forex>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Journal>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.ChartOfAccount>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CompanySetting>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CompanySetting>>().InRequestScope();
            //kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.BankReconciliationSetting>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.BankReconciliationSetting>>().InRequestScope();
            //kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.SegmentDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.SegmentDetail>>().InRequestScope();
            //kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.SegmentDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.SegmentDetail>>().InRequestScope();
            //kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.SegmentMaster>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.SegmentMaster>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Item>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Item>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.TaxCode>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.TaxCode>>().InRequestScope();
            //kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.JournalLedger>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.JournalLedger>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.ModuleMaster>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.ModuleMaster>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CCAccountType>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CCAccountType>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.AccountTypeIdType>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.AccountTypeIdType>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.ActivityHistory>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.ActivityHistory>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.JournalDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.DebitNote>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.DebitNote>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CreditNoteApplication>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CreditNoteApplication>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Receipt>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Receipt>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CashSale>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CashSale>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CashSaleDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CashSaleDetail>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Contact>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Contact>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.ContactDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.ContactDetail>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.MediaRepository>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.MediaRepository>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.OpeningBalanceDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.OpeningBalanceDetail>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Communication>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Communication>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.InterCompanySetting>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.InterCompanySetting>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.InterCompanySettingDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.InterCompanySettingDetail>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.COAMapping>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.COAMapping>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.COAMappingDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.COAMappingDetail>>().InRequestScope();

            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.TaxCodeMapping>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.TaxCodeMapping>>().InRequestScope();

            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.TaxCodeMappingDetail>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.TaxCodeMappingDetail>>().InRequestScope();

            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.Models.CommonForex>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.Models.CommonForex>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<MasterModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<MasterModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
            kernel.Bind<IMasterModuleRepositoryAsync<AppsWorld.MasterModule.Entities.CompanyUser>>().To<MasterModuleRepository<AppsWorld.MasterModule.Entities.CompanyUser>>().InRequestScope();


            kernel.Bind<AppsWorld.MasterModule.Service.IBeanEntityService>().To<AppsWorld.MasterModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IAccountTypeService>().To<AppsWorld.MasterModule.Service.AccountTypeService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IAddressBookService>().To<AppsWorld.MasterModule.Service.AddressBookService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IAddressService>().To<AppsWorld.MasterModule.Service.AddressService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IControlCodeCategoryService>().To<AppsWorld.MasterModule.Service.ControlCodeCategoryService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IControlCodeService>().To<AppsWorld.MasterModule.Service.ControlCodeService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICurrencyService>().To<AppsWorld.MasterModule.Service.CurrencyService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IIdTypeService>().To<AppsWorld.MasterModule.Service.IdTypeService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IMultiCurrencySettingService>().To<AppsWorld.MasterModule.Service.MultiCurrencySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ITermsOfPaymentService>().To<AppsWorld.MasterModule.Service.TermsOfPaymentService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IFinancialSettingService>().To<AppsWorld.MasterModule.Service.FinancialSettingService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IInvoiceService>().To<AppsWorld.MasterModule.Service.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IGSTSettingService>().To<AppsWorld.MasterModule.Service.GSTSettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.MasterModule.Service.IForexService>().To<AppsWorld.MasterModule.Service.ForexService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IJournalService>().To<AppsWorld.MasterModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IChartOfAccountService>().To<AppsWorld.MasterModule.Service.ChartOfAccountService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICompanySettingService>().To<AppsWorld.MasterModule.Service.CompanySettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.MasterModule.Service.IBankReconciliationSettingService>().To<AppsWorld.MasterModule.Service.BankReconciliationSettingService>().InRequestScope();
            //kernel.Bind<AppsWorld.MasterModule.Service.ISegmentDetailService>().To<AppsWorld.MasterModule.Service.SegmentDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.MasterModule.Service.ISegmentMasterService>().To<AppsWorld.MasterModule.Service.SegmentMasterService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IItemService>().To<AppsWorld.MasterModule.Service.ItemService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ITaxCodeService>().To<AppsWorld.MasterModule.Service.TaxCodeService>().InRequestScope();
            //kernel.Bind<AppsWorld.MasterModule.Service.IJournalLedgerService>().To<AppsWorld.MasterModule.Service.JournalLedgerService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IModuleMosterService>().To<AppsWorld.MasterModule.Service.ModuleMasterService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICompanyFeatureService>().To<AppsWorld.MasterModule.Service.CompanyFeatureService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICCAccountTypeService>().To<AppsWorld.MasterModule.Service.CCAccountTypeService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IAccountTypeIdtypeService>().To<AppsWorld.MasterModule.Service.AccountTypeIdTypeService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IActivityHistoryService>().To<AppsWorld.MasterModule.Service.ActivityHistoryService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IJournalDetailService>().To<AppsWorld.MasterModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IDebitNoteService>().To<AppsWorld.MasterModule.Service.DebitNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICreditNoteApplicationService>().To<AppsWorld.MasterModule.Service.CreditNoteApplicationService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IReceiptService>().To<AppsWorld.MasterModule.Service.ReceiptService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IFeatureService>().To<AppsWorld.MasterModule.Service.FeatureService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICashSalesService>().To<AppsWorld.MasterModule.Service.CashSaleService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IContactService>().To<AppsWorld.MasterModule.Service.ContactService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IContactDetailService>().To<AppsWorld.MasterModule.Service.ContactDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IOpeningBalanceDetail>().To<AppsWorld.MasterModule.Service.OpeningBalanceDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICommunication>().To<AppsWorld.MasterModule.Service.CommunicationService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IInterCompanySettingService>().To<AppsWorld.MasterModule.Service.InterCompanySettingService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.IInterCompanySettingDetailService>().To<AppsWorld.MasterModule.Service.InterCompanySettingDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICOAMappingService>().To<AppsWorld.MasterModule.Service.COAMappingService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICOAMappingDetailService>().To<AppsWorld.MasterModule.Service.COAMappingDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ITaxCodeMappingService>().To<AppsWorld.MasterModule.Service.TaxCodeMappingService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ITaxCodeMappingDetailService>().To<AppsWorld.MasterModule.Service.TaxCodeMappingDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.MasterModule.Service.ICommonForexService>().To<AppsWorld.MasterModule.Service.CommonForexService>().InRequestScope();
        }

        private static void RegisterInvoiceModule(IKernel kernel)
        {
            kernel.Bind<AppsWorld.InvoiceModule.Application.InvoiceApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IInvoiceModuleDataContextAsync>().To<InvoiceModuleContext>().InRequestScope();
            kernel.Bind<IInvoiceModuleUnitOfWorkAsync>().To<InvoiceModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Invoice>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Invoice>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.InvoiceDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.InvoiceDetail>>().InRequestScope();
            //kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.InvoiceGSTDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.InvoiceGSTDetail>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.InvoiceNote>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.InvoiceNote>>().InRequestScope();
            //kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Provision>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Provision>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.CreditNoteApplication>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.CreditNoteApplication>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.CreditNoteApplicationDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.CreditNoteApplicationDetail>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.DoubtfulDebtAllocation>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.DoubtfulDebtAllocation>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.DoubtfulDebtAllocationDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.DoubtfulDebtAllocationDetail>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.DebitNote>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.DebitNote>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.DebitNoteDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.DebitNoteDetail>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.DebitNoteNote>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.DebitNoteNote>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.DebitNoteGSTDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.DebitNoteGSTDetail>>().InRequestScope();
            //kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.JournalLedger>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.JournalLedger>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity>>().To<InvoiceModuleRepository<AppsWorld.CommonModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.AutoNumber>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.AutoNumberCompany>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Receipt>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Receipt>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.ReceiptDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.ReceiptDetail>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.JournalDetail>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Journal>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<InvoiceModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.CommonModule.Entities.TermsOfPayment>>().To<InvoiceModuleRepository<AppsWorld.CommonModule.Entities.TermsOfPayment>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.PaymentCompact>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.PaymentCompact>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.PaymentDetailCompact>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.PaymentDetailCompact>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Models.CommonForex>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Models.CommonForex>>().InRequestScope();

            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Bank>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Bank>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Models.Address>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Models.Address>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.Models.AddressBook>>().To<InvoiceModuleRepository<AppsWorld.InvoiceModule.Entities.Models.AddressBook>>().InRequestScope();

            kernel.Bind<AppsWorld.InvoiceModule.Service.IInvoiceEntityService>().To<AppsWorld.InvoiceModule.Service.InvoiceEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.ICreditNoteApplicationDetailService>().To<AppsWorld.InvoiceModule.Service.CreditNoteApplicationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.ICreditNoteApplicationService>().To<AppsWorld.InvoiceModule.Service.CreditNoteApplicationService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IDoubtfulDebtallocationDetailService>().To<AppsWorld.InvoiceModule.Service.DoubtfulDebtallocationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IDoubtfulDebtAllocationService>().To<AppsWorld.InvoiceModule.Service.DoubtfulDebtAllocationService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IInvoiceDetailService>().To<AppsWorld.InvoiceModule.Service.InvoiceDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.InvoiceModule.Service.IInvoiceGSTDetailService>().To<AppsWorld.InvoiceModule.Service.InvoiceGSTDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IInvoiceNoteService>().To<AppsWorld.InvoiceModule.Service.InvoiceNoteService>().InRequestScope();
            //kernel.Bind<AppsWorld.InvoiceModule.Service.IProvisionService>().To<AppsWorld.InvoiceModule.Service.ProvisionService>().InRequestScope();
            //kernel.Bind<AppsWorld.InvoiceModule.Service.IJournalLedgerService>().To<AppsWorld.InvoiceModule.Service.JournalLedgerService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IDebitNoteService>().To<AppsWorld.InvoiceModule.Service.DebitNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IAutoNumberService>().To<AppsWorld.InvoiceModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IAutoNumberCompanyService>().To<AppsWorld.InvoiceModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IReceiptService>().To<AppsWorld.InvoiceModule.Service.ReceiptService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IReceiptDetailService>().To<AppsWorld.InvoiceModule.Service.ReceiptDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IJournalDetailService>().To<AppsWorld.InvoiceModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.IJournalService>().To<AppsWorld.InvoiceModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.ICommonForexService>().To<AppsWorld.InvoiceModule.Service.CommonForexService>().InRequestScope();

            kernel.Bind<IInvoiceModuleRepositoryAsync<InvoiceModule.Entities.CompanyUser>>().To<InvoiceModuleRepository<InvoiceModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<IInvoiceModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<InvoiceModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
        }
        private static void RegisterDebitNoteService(IKernel kernel)
        {
            kernel.Bind<AppsWorld.DebitNoteModule.Application.DebitNoteApplicationService>().ToSelf().InRequestScope();
            kernel.Bind<IDebitNoteModuleDataContextAsync>().To<AppsWorld.DebitNoteModule.Entities.DebitNoteContext>().InRequestScope();
            kernel.Bind<IDebitNoteModuleUnitOfWorkAsync>().To<DebitNoteModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.DebitNote>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.DebitNote>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.DebitNoteDetail>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.DebitNoteDetail>>().InRequestScope();
            //kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.DebitNoteGSTDetail>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.DebitNoteGSTDetail>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.DebitNoteNote>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.DebitNoteNote>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.AutoNumber>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.AutoNumberCompany>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.AutoNumberCompany>>().InRequestScope();
            //kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.Provision>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.Provision>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.Invoice>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.Invoice>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.InvoiceDetail>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.InvoiceDetail>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity>>().To<DebitNoteModuleRepository<AppsWorld.CommonModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.Receipt>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.Receipt>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.CreditNoteApplication>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.CreditNoteApplication>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.CreditNoteApplicationDetail>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.CreditNoteApplicationDetail>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.DoubtfulDebtAllocation>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.DoubtfulDebtAllocation>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.DoubtfulDebtAllocationDetail>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.DoubtfulDebtAllocationDetail>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.DebitNoteModule.Entities.ReceiptDetail>>().To<DebitNoteModuleRepository<AppsWorld.DebitNoteModule.Entities.ReceiptDetail>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.CommonModule.Entities.TermsOfPayment>>().To<DebitNoteModuleRepository<AppsWorld.CommonModule.Entities.TermsOfPayment>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<DebitNoteModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();

            kernel.Bind<AppsWorld.DebitNoteModule.Service.IDebitNoteService>().To<AppsWorld.DebitNoteModule.Service.DebitNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IDebitNoteDetailService>().To<AppsWorld.DebitNoteModule.Service.DebitNoteDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.DebitNoteModule.Service.IDebitNoteGstDetailService>().To<AppsWorld.DebitNoteModule.Service.DebitNoteGstDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IDebitNoteNoteService>().To<AppsWorld.DebitNoteModule.Service.DebitNoteNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IAutoNumberService>().To<AppsWorld.DebitNoteModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IAutoNumberCompanyService>().To<AppsWorld.DebitNoteModule.Service.AutoNumberCompanyService>().InRequestScope();
            //kernel.Bind<AppsWorld.DebitNoteModule.Service.IProvisionService>().To<AppsWorld.DebitNoteModule.Service.ProvisionService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IInvoiceService>().To<AppsWorld.DebitNoteModule.Service.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IInvoiceDetailService>().To<AppsWorld.DebitNoteModule.Service.InvoiceDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IReceiptService>().To<AppsWorld.DebitNoteModule.Service.ReceiptService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.ICreditNoteApplicationService>().To<AppsWorld.DebitNoteModule.Service.CreditNoteApplicationService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.ICreditNoteApplicationDetailService>().To<AppsWorld.DebitNoteModule.Service.CreditNoteApplicationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IDoubtfulDebtAllocationService>().To<AppsWorld.DebitNoteModule.Service.DoubtfulDebtAllocationService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IDoubtfulDebtallocationDetailService>().To<AppsWorld.DebitNoteModule.Service.DoubtfulDebtallocationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.DebitNoteModule.Service.IReceiptDetailService>().To<AppsWorld.DebitNoteModule.Service.ReceiptDetailService>().InRequestScope();
            //kernel.Bind<AppsWorld.CommonModule.Service.ITermsOfPaymentService>().To<AppsWorld.CommonModule.Service.TermsOfPaymentService>().InRequestScope();

            kernel.Bind<IDebitNoteMoluleRepositoryAsync<DebitNoteModule.Entities.CompanyUser>>().To<DebitNoteModuleRepository<DebitNoteModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<IDebitNoteMoluleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<DebitNoteModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
        }

        private static void RegisterBankTransfer(IKernel kernel)
        {
            kernel.Bind<BankTransferApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IBankTransferModuleDataContextAsync>().To<BankTransferContext>().InRequestScope();
            kernel.Bind<IBankTransferModuleUnitOfWorkAsync>().To<BankTransferModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.BankTransfer>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.BankTransfer>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.BankTransferDetail>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.BankTransferDetail>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.AutoNumber>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.AutoNumberCompany>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<BankTransferModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();

            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUser >> ().To<BankTransferModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUser>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<BankTransferModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<BankTransferModuleRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();

            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.Models.Invoice>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.Models.Invoice>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.Models.DebitNote>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.Models.DebitNote>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.Models.Bill>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.Models.Bill>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.Models.SettlementDetail>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.Models.SettlementDetail>>().InRequestScope();
            kernel.Bind<IBankTransferModuleRepositoryAsync<AppsWorld.BankTransferModule.Entities.Models.Journal>>().To<BankTransferModuleRepository<AppsWorld.BankTransferModule.Entities.Models.Journal>>().InRequestScope();



            kernel.Bind<AppsWorld.BankTransferModule.Service.IBankTransferService>().To<AppsWorld.BankTransferModule.Service.BankTransferService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IBankTransferDetailService>().To<AppsWorld.BankTransferModule.Service.BankTransferDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IAutoNumberService>().To<AppsWorld.BankTransferModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IAutoNumberCompanyService>().To<AppsWorld.BankTransferModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IInvoiceService>().To<AppsWorld.BankTransferModule.Service.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IDebitNoteService>().To<AppsWorld.BankTransferModule.Service.DebitNoteService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IBillService>().To<AppsWorld.BankTransferModule.Service.BillService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.ISettlementDetailService>().To<AppsWorld.BankTransferModule.Service.SettlementDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.BankTransferModule.Service.IJournalService>().To<AppsWorld.BankTransferModule.Service.JournalService>().InRequestScope();
        }
        public static void RegisterCreditMemo(IKernel kernel)
        {
            kernel.Bind<CreditMemoApplicationService>().ToSelf().InRequestScope();
            kernel.Bind<ICreditMemoModuleDataContextAsync>().To<CreditMemoModuleContext>().InRequestScope();
            kernel.Bind<ICreditMemoModuleUnitOfWorkAsync>().To<CreditMemoModuleUnitOfWork>().InRequestScope();

            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.CreditMemo>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.CreditMemo>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.CreditMemoDetail>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.CreditMemoDetail>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.CreditMemoApplication>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.CreditMemoApplication>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.CreditMemoApplicationDetail>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.CreditMemoApplicationDetail>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.AutoNumber>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.AutoNumberCompany>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.AutoNumberCompany>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.Bill>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.Bill>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.BillDetail>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.BillDetail>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<CreditMemoModuleRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.BeanEntity>>().To<CreditMemoModuleRepository<AppsWorld.CommonModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.TaxCode>>().To<CreditMemoModuleRepository<AppsWorld.CommonModule.Entities.TaxCode>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<CreditMemoModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.JournalDetail>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.Journal>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.Journal>>().InRequestScope();


            kernel.Bind<AppsWorld.CreditMemoModule.Service.ICreditMemoService>().To<AppsWorld.CreditMemoModule.Service.CreditMemoService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.ICreditMemoDetailService>().To<AppsWorld.CreditMemoModule.Service.CreditMemoDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.ICreditMemoApplicationService>().To<AppsWorld.CreditMemoModule.Service.CreditMemoApplicationService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.ICreditMemoApplicationDetailService>().To<AppsWorld.CreditMemoModule.Service.CreditMemoApplicationDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.IAutoNumberService>().To<AppsWorld.CreditMemoModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.IAutoNumberCompanyService>().To<AppsWorld.CreditMemoModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.IBillService>().To<AppsWorld.CreditMemoModule.Service.BillService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.IJournalDetailService>().To<AppsWorld.CreditMemoModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.CreditMemoModule.Service.IJournalService>().To<AppsWorld.CreditMemoModule.Service.JournalService>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CreditMemoModule.Entities.CompanyUser>>().To<CreditMemoModuleRepository<AppsWorld.CreditMemoModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<ICreditMemoModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<CreditMemoModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();
        }
        public static void RegisterGLClearing(IKernel kernel)
        {
            kernel.Bind<GLClearingApplicationService>().ToSelf().InRequestScope();
            kernel.Bind<IClearingModuleDataContextAsync>().To<GLClearingContext>().InRequestScope();
            kernel.Bind<IClearingModuleUnitOfWorkAsync>().To<ClearingModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.GLClearing>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.GLClearing>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.GLClearingDetail>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.GLClearingDetail>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.Journal>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.Journal>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Company>>().To<ClearingModuleRepository<AppsWorld.CommonModule.Entities.Company>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.CommonModule.Entities.ChartOfAccount>>().To<ClearingModuleRepository<AppsWorld.CommonModule.Entities.ChartOfAccount>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.JournalDetail>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.JournalDetail>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.AutoNumber>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.AutoNumber>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.AutoNumberCompany>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.AutoNumberCompany>>().InRequestScope();

            kernel.Bind<AppsWorld.GLClearingModule.Service.IClearingService>().To<AppsWorld.GLClearingModule.Service.ClearingService>().InRequestScope();
            kernel.Bind<AppsWorld.GLClearingModule.Service.IClearingDetailService>().To<AppsWorld.GLClearingModule.Service.ClearingDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.GLClearingModule.Service.IJournalService>().To<AppsWorld.GLClearingModule.Service.JournalService>().InRequestScope();
            kernel.Bind<AppsWorld.GLClearingModule.Service.IJournalDetailService>().To<AppsWorld.GLClearingModule.Service.JournalDetailService>().InRequestScope();
            kernel.Bind<AppsWorld.GLClearingModule.Service.IAutoNumberService>().To<AppsWorld.GLClearingModule.Service.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.GLClearingModule.Service.IAutoNumberCompanyService>().To<AppsWorld.GLClearingModule.Service.AutoNumberCompanyService>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.GLClearingModule.Entities.CompanyUser>>().To<ClearingModuleRepository<AppsWorld.GLClearingModule.Entities.CompanyUser>>().InRequestScope();
            kernel.Bind<IClearingModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<ClearingModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

        }
        private static void RegisterTemplatesService(IKernel kernel)
        {

            kernel.Bind<TemplateApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ITemplateModuleDataContextAsync>().To<TemplateContext>().InRequestScope();
            kernel.Bind<ITemplateModuleUnitOfWorkAsync>().To<TemplateModuleUnitOfWork>().InRequestScope();

            //entites
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.Bank>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.Bank>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.Address>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.Address>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.AddressBook>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.AddressBook>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.Invoice>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.Invoice>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.InvoiceDetail>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.InvoiceDetail>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.Receipt>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.Receipt>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.ReceiptDetail>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.ReceiptDetail>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.Company>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.Company>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.BeanEntity>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.BeanEntity>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.Journal>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.Journal>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.JournalDetail>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.JournalDetail>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.TaxCode>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.TaxCode>>().InRequestScope();
            kernel.Bind<ITemplateModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.GenericTemplate>>().To<TemplateModuleRepository<AppsWorld.TemplateModule.Entities.Models.GenericTemplate>>().InRequestScope();

            //service
            kernel.Bind<AppsWorld.TemplateModule.Service.IReceiptService>().To<AppsWorld.TemplateModule.Service.ReceiptService>().InRequestScope();
            kernel.Bind<AppsWorld.TemplateModule.Service.IInvoiceService>().To<AppsWorld.TemplateModule.Service.Invoiceservice>().InRequestScope();
            kernel.Bind<AppsWorld.TemplateModule.Service.IBeanEmtityService>().To<AppsWorld.TemplateModule.Service.BeanEntityService>().InRequestScope();
            kernel.Bind<AppsWorld.TemplateModule.Service.IGenericTemplateService>().To<AppsWorld.TemplateModule.Service.GenericTemplateService>().InRequestScope();

        }

        #region New_Optimized_Block
        private static void RegisterCashSaleKServices(IKernel kernel)
        {
            kernel.Bind<CashSaleKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ICashSalesKModuleDataContextAsync>().To<CashSaleKContext>().InRequestScope();
            kernel.Bind<ICashSalesKModuleUnitOfWorkAsync>().To<CashSalesKUnitOfWork>().InRequestScope();

            kernel.Bind<ICashSalesKModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.CashSaleK>>().To<CashSalesKRepository<AppsWorld.CashSalesModule.Entities.V2.CashSaleK>>().InRequestScope();
            kernel.Bind<ICashSalesKModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.BeanEntityK>>().To<CashSalesKRepository<AppsWorld.CashSalesModule.Entities.V2.BeanEntityK>>().InRequestScope();
            kernel.Bind<ICashSalesKModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.ChartOfAccountK>>().To<CashSalesKRepository<AppsWorld.CashSalesModule.Entities.V2.ChartOfAccountK>>().InRequestScope();
            kernel.Bind<ICashSalesKModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.CompanyK>>().To<CashSalesKRepository<AppsWorld.CashSalesModule.Entities.V2.CompanyK>>().InRequestScope();
            kernel.Bind<ICashSalesKModuleRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.CompanyUserK>>().To<CashSalesKRepository<AppsWorld.CashSalesModule.Entities.V2.CompanyUserK>>().InRequestScope();

            kernel.Bind<AppsWorld.CashSalesModule.Service.V2.ICashSalesKService>().To<AppsWorld.CashSalesModule.Service.V2.CashSalesKService>().InRequestScope();
        }

        private static void RegisterCashSaleMasterServices(IKernel kernel)
        {
            kernel.Bind<CashSalesModule.Application.V2.CashSaleApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ICashSalesMasterDataContextAsync>().To<CashSaleMasterContext>().InRequestScope();
            kernel.Bind<ICashSalesMasterUnitOfWorkAsync>().To<CashSalesMasterUnitOfWork>().InRequestScope();

            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.CashSale>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.CashSale>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.ChartOfAccountCompact>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.ChartOfAccountCompact>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.FinancialSettingCompact>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.FinancialSettingCompact>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.TaxCodeCompact>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.TaxCodeCompact>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.AutoNumberCompact>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.AutoNumberCompact>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.CompanyK>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.CompanyK>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.AutoNumberComptCompany>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.AutoNumberComptCompany>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.BeanEntityCompact>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.BeanEntityCompact>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.ItemCompact>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.ItemCompact>>().InRequestScope();
            kernel.Bind<ICashSalesMasterRepositoryAsync<AppsWorld.CashSalesModule.Entities.V2.CashSaleDetail>>().To<CashSalesMasterRepository<AppsWorld.CashSalesModule.Entities.V2.CashSaleDetail>>().InRequestScope();

            kernel.Bind<AppsWorld.CashSalesModule.Service.V2.ICashSalesService>().To<AppsWorld.CashSalesModule.Service.V2.CashSalesService>().InRequestScope();
            kernel.Bind<AppsWorld.CashSalesModule.Service.V2.IMasterCompactService>().To<AppsWorld.CashSalesModule.Service.V2.MasterCompactService>().InRequestScope();
            kernel.Bind<AppsWorld.CashSalesModule.Service.V2.IAutoNumberService>().To<AppsWorld.CashSalesModule.Service.V2.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.CashSalesModule.Service.V2.IItemCompactService>().To<AppsWorld.CashSalesModule.Service.V2.ItemCompactService>().InRequestScope();
        }

        private static void RegisterInvoiceKService(IKernel kernel)
        {
            kernel.Bind<InvoiceKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IInvoiceKModuleDataContextAsync>().To<InvoiceKModuleContext>().InRequestScope();
            kernel.Bind<IInvoiceKModuleUnitOfWorkAsync>().To<InvoiceKModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IInvoiceKModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.InvoiceK>>().To<InvoiceKModuleRepository<AppsWorld.InvoiceModule.Entities.V2.InvoiceK>>().InRequestScope();
            kernel.Bind<IInvoiceKModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.BeanEntityCompact>>().To<InvoiceKModuleRepository<AppsWorld.InvoiceModule.Entities.V2.BeanEntityCompact>>().InRequestScope();
            kernel.Bind<IInvoiceKModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CompanyCompact>>().To<InvoiceKModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<IInvoiceKModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CompanyUserCompact>>().To<InvoiceKModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CompanyUserCompact>>().InRequestScope();

            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.IInvoiceKService>().To<AppsWorld.InvoiceModule.Service.V2.InvoiceKService>().InRequestScope();

        }
        private static void ReceiptKApplicationService(IKernel kernel)
        {
            kernel.Bind<ReceiptKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IReceiptKModuleDataContextAsync>().To<ReceiptKModuleContext>().InRequestScope();
            kernel.Bind<IReceiptKModuleUnitOfWorkAsync>().To<ReceiptKModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IReceiptKModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.ReceiptK>>().To<ReceiptKModuleRepository<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.ReceiptK>>().InRequestScope();
            kernel.Bind<IReceiptKModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.BeanEntityCompact>>().To<ReceiptKModuleRepository<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.BeanEntityCompact>>().InRequestScope();
            kernel.Bind<IReceiptKModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.CompanyCompact>>().To<ReceiptKModuleRepository<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.CompanyCompact>>().InRequestScope();
            kernel.Bind<IReceiptKModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.CompanyUserCompact>>().To<ReceiptKModuleRepository<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.CompanyUserCompact>>().InRequestScope();
            kernel.Bind<IReceiptKModuleRepositoryAsync<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.ChartOfAccountCompact>>().To<ReceiptKModuleRepository<AppsWorld.ReceiptModule.Entities.Models.V2.Receipt.ChartOfAccountCompact>>().InRequestScope();
            kernel.Bind<AppsWorld.ReceiptModule.Service.V2.Receipt.K_Service.IReceiptKService>().To<AppsWorld.ReceiptModule.Service.V2.Receipt.K_Service.ReceiptKService>().InRequestScope();

        }
        private static void RegisterCommonKService(IKernel kernel)
        {
            kernel.Bind<CommonKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ICommonKModuleDataContextAsync>().To<CommonKContext>().InRequestScope();
            kernel.Bind<ICommonKModuleUnitOfWorkAsync>().To<CommonKModuleUnitOfWork>().InRequestScope();

            kernel.Bind<ICommonKModuleRepositoryAsync<AppsWorld.CommonModule.Entities.V2.BeanEntityK>>().To<CommonKModuleRepository<AppsWorld.CommonModule.Entities.V2.BeanEntityK>>().InRequestScope();
            kernel.Bind<ICommonKModuleRepositoryAsync<AppsWorld.CommonModule.Entities.V2.CompanyK>>().To<CommonKModuleRepository<AppsWorld.CommonModule.Entities.V2.CompanyK>>().InRequestScope();
            kernel.Bind<ICommonKModuleRepositoryAsync<AppsWorld.CommonModule.Entities.V2.CompanyUserK>>().To<CommonKModuleRepository<AppsWorld.CommonModule.Entities.V2.CompanyUserK>>().InRequestScope();
            kernel.Bind<ICommonKModuleRepositoryAsync<AppsWorld.CommonModule.Entities.V2.ChartOfAccountK>>().To<CommonKModuleRepository<AppsWorld.CommonModule.Entities.V2.ChartOfAccountK>>().InRequestScope();

        }

        private static void RegisterInvoiceService(IKernel kernel)
        {
            kernel.Bind<AppsWorld.InvoiceModule.Application.V2.InvoiceApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IInvoiceComptModuleDataContextAsync>().To<InvoiceCompactModuleContext>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleUnitOfWorkAsync>().To<InvoiceComptModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.Invoice>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.Invoice>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.BeanEntityCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.BeanEntityCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CompanyCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CompanyUserCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CompanyUserCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.InvoiceDetail>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.InvoiceDetail>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.InvoiceNoteCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.InvoiceNoteCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.FinancialSettingCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.FinancialSettingCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.ItemCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.ItemCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.AutoNumberCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.AutoNumberCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.AutoNumberComptCompany>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.AutoNumberComptCompany>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.TaxCodeCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.TaxCodeCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.ChartOfAccountCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.ChartOfAccountCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.TermsOfPaymentCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.TermsOfPaymentCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.ReceiptCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.ReceiptCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.AccountTypeCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.AccountTypeCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CurrencyCompact>>().To<InvoiceComptModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CurrencyCompact>>().InRequestScope();
            kernel.Bind<IInvoiceComptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<InvoiceComptModuleRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();


            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.IAutoNumberService>().To<AppsWorld.InvoiceModule.Service.V2.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.IInvoiceService>().To<AppsWorld.InvoiceModule.Service.V2.InvoiceService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.IItemCompactService>().To<AppsWorld.InvoiceModule.Service.V2.ItemCompactService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.IMasterCompactService>().To<AppsWorld.InvoiceModule.Service.V2.MasterCompactService>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.ICurrencyCompactService>().To<AppsWorld.InvoiceModule.Service.V2.CurrencyCompactService>().InRequestScope();
        }

        private static void RegisterTempletKServices(IKernel kernel)
        {
            kernel.Bind<TemplateCompactApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ITemplateCompactModuleDataContextAsync>().To<TemplateKContext>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleUnitOfWorkAsync>().To<TemplateCompactModuleUnitOfWork>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Address>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Address>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.AddressBook>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.AddressBook>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Bank>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Bank>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.BeanEntity>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.BeanEntity>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Company>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Company>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.CompanyTemplateSettings>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.CompanyTemplateSettings>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Invoice>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Invoice>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Journal>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Journal>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.GenericTemplate>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.GenericTemplate>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.JournalDetail>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.JournalDetail>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Receipt>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Receipt>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.ReceiptDetail>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.ReceiptDetail>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.TaxCode>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.TaxCode>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Template>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Template>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.InvoiceDetail>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.InvoiceDetail>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Localization>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Localization>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Contact>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Contact>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.ContactDetail>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.ContactDetail>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.CompanyUser>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.CompanyUser>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.GSTSetting>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.GSTSetting>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.ChartOfAccount>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.ChartOfAccount>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.TermsOfPayment>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.TermsOfPayment>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.IdType>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.IdType>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.DebitNote>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.DebitNote>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.DebitNoteDetail>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.DebitNoteDetail>>().InRequestScope();
            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.Item>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.Item>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.CashSale>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.CashSale>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.CashSaleDetail>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.CashSaleDetail>>().InRequestScope();

            kernel.Bind<ITemplateCompactModuleRepositoryAsync<AppsWorld.TemplateModule.Entities.Models.V2.MediaRepository>>().To<TemplateCompactModuleRepository<AppsWorld.TemplateModule.Entities.Models.V2.MediaRepository>>().InRequestScope();

            kernel.Bind<ITemplateService>().To<TemplateService>().InRequestScope();
        }

        private static void RegisterDebitNoteKService(IKernel kernel)
        {
            kernel.Bind<DebitNoteKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IDebitNoteKDataContextAsync>().To<DebitNoteKContext>().InRequestScope();
            kernel.Bind<IDebitNoteKUnitOfWorkAsync>().To<DebitNoteKUnitOfWork>().InRequestScope();

            kernel.Bind<IDebitNoteKRepositoryAsync<AppsWorld.DebitNoteModule.Entities.V2.DebitNoteK>>().To<DebitNoteKRepository<AppsWorld.DebitNoteModule.Entities.V2.DebitNoteK>>().InRequestScope();
            kernel.Bind<IDebitNoteKRepositoryAsync<AppsWorld.DebitNoteModule.Entities.V2.CompanyCompact>>().To<DebitNoteKRepository<AppsWorld.DebitNoteModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<IDebitNoteKRepositoryAsync<AppsWorld.DebitNoteModule.Entities.V2.CompanyUserCompact>>().To<DebitNoteKRepository<AppsWorld.DebitNoteModule.Entities.V2.CompanyUserCompact>>().InRequestScope();
            kernel.Bind<IDebitNoteKRepositoryAsync<AppsWorld.DebitNoteModule.Entities.V2.BeanEntityCompact>>().To<DebitNoteKRepository<AppsWorld.DebitNoteModule.Entities.V2.BeanEntityCompact>>().InRequestScope();

            kernel.Bind<AppsWorld.DebitNoteModule.Service.V2.IDebitNoteKService>().To<AppsWorld.DebitNoteModule.Service.V2.DebitNoteKService>().InRequestScope();
        }
        private static void RegisterTransferKService(IKernel kernel)
        {
            kernel.Bind<TransferKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<ITransferKDataContextAsync>().To<BankTransferKContext>().InRequestScope();
            kernel.Bind<ITransferKUnitOfWorkAsync>().To<TransferKUnitOfWork>().InRequestScope();

            kernel.Bind<ITransferKRepositoryAsync<AppsWorld.BankTransferModule.Entities.V2.BankTransferK>>().To<TransferKRepository<AppsWorld.BankTransferModule.Entities.V2.BankTransferK>>().InRequestScope();
            kernel.Bind<ITransferKRepositoryAsync<AppsWorld.BankTransferModule.Entities.V2.BankTransferDetailK>>().To<TransferKRepository<AppsWorld.BankTransferModule.Entities.V2.BankTransferDetailK>>().InRequestScope();
            kernel.Bind<ITransferKRepositoryAsync<AppsWorld.BankTransferModule.Entities.V2.CompanyCompact>>().To<TransferKRepository<AppsWorld.BankTransferModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<ITransferKRepositoryAsync<AppsWorld.BankTransferModule.Entities.V2.ChartOfAccountCompact>>().To<TransferKRepository<AppsWorld.BankTransferModule.Entities.V2.ChartOfAccountCompact>>().InRequestScope();

            kernel.Bind<AppsWorld.BankTransferModule.Service.V2.ITransferKService>().To<AppsWorld.BankTransferModule.Service.V2.TransferKService>().InRequestScope();
        }

        private static void RegisterRevaluationKService(IKernel kernel)
        {
            kernel.Bind<RevaluationKApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IRevaluationKDataContextAsync>().To<RevaluationKContext>().InRequestScope();
            kernel.Bind<IRevaluationKUnitOfWorkAsync>().To<RevaluationKUnitOfWork>().InRequestScope();

            kernel.Bind<IRevaluationKRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.RevaluationK>>().To<RevaluationKRepository<AppsWorld.RevaluationModule.Entities.V2.RevaluationK>>().InRequestScope();
            kernel.Bind<IRevaluationKRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.CompanyCompact>>().To<RevaluationKRepository<AppsWorld.RevaluationModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<IRevaluationKRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.CompanyUserCompact>>().To<RevaluationKRepository<AppsWorld.RevaluationModule.Entities.V2.CompanyUserCompact>>().InRequestScope();

            kernel.Bind<AppsWorld.RevaluationModule.Service.V2.IRevaluationKService>().To<AppsWorld.RevaluationModule.Service.V2.RevaluationKService>().InRequestScope();
            kernel.Bind<IRevaluationKRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().To<RevaluationKRepository<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail>>().InRequestScope();

        }

        private static void RegisterRevaluationMasterService(IKernel kernel)
        {
            kernel.Bind<RevaluationApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IRevaluationDataContextAsync>().To<AppsWorld.RevaluationModule.Entities.V2.RevaluationContext>().InRequestScope();
            kernel.Bind<IRevaluationUnitOfWorkAsync>().To<RevaluationUnitOfWork>().InRequestScope();

            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.Revaluation>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.Revaluation>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.CompanyCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.CompanyUserCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.CompanyUserCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.AutoNumberCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.AutoNumberCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.AutoNumberCompanyCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.AutoNumberCompanyCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.RevalutionDetail>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.RevalutionDetail>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.BeanEntityCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.BeanEntityCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.ChartOfAccountCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.ChartOfAccountCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.JournalDetailCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.JournalDetailCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.FinancialSettingCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.FinancialSettingCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.MultiCurrencySettingCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.MultiCurrencySettingCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.V2.JournalCompact>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.V2.JournalCompact>>().InRequestScope();
            kernel.Bind<IRevaluationRepositoryAsync<AppsWorld.RevaluationModule.Entities.Models.V2.CommonForex>>().To<RevaluationRepository<AppsWorld.RevaluationModule.Entities.Models.V2.CommonForex>>().InRequestScope();

            kernel.Bind<AppsWorld.RevaluationModule.Service.V2.IRevaluationService>().To<AppsWorld.RevaluationModule.Service.V2.RevaluationService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.V2.IAutoNumberService>().To<AppsWorld.RevaluationModule.Service.V2.AutoNumberService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.V2.IMasterCompactService>().To<AppsWorld.RevaluationModule.Service.V2.MasterCompactService>().InRequestScope();
            kernel.Bind<AppsWorld.RevaluationModule.Service.V2.Get_Save_Services.ICommonForexService>().To<AppsWorld.RevaluationModule.Service.V2.Get_Save_Services.CommonForexService>().InRequestScope();
        }

        private static void RegisterCNApplicationService(IKernel kernel)
        {
            //kernel.Bind<IInvoiceComptModuleDataContextAsync>().To<ReaderContext>().InRequestScope();
            kernel.Bind<AppsWorld.InvoiceModule.Application.V2.CNAApplicationService>().ToSelf().InRequestScope();
            //kernel.Bind<AppsWorld.InvoiceModule.Application.V3.ReaderAppilictionServices>().ToSelf().InRequestScope();


            //kernel.Bind<AppsWorld.InvoiceModule.Service.V3.IReaderServices>().To<AppsWorld.InvoiceModule.Service.V3.ReaderServices>().InRequestScope();

            kernel.Bind<IApplicationCompactModuleDataContextAsync>().To<CNApplicationModuleContext>().InRequestScope();
           
            kernel.Bind<IApplicationCompactModuleUnitOfWorkAsync>().To<ApplicationCompactModuleUnitOfWork>().InRequestScope();

            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CreditNoteApplication>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CreditNoteApplication>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CreditNoteApplicationDetail>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CreditNoteApplicationDetail>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.BeanEntityCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.BeanEntityCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.FinancialSettingCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.FinancialSettingCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.InvoiceCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.InvoiceCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.DebitNoteCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.DebitNoteCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.AutoNumberCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.AutoNumberCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.AutoNumberComptCompany>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.AutoNumberComptCompany>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.TaxCodeCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.TaxCodeCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CompanyCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CompanyCompact>>().InRequestScope();
            kernel.Bind<IApplicationCompactModuleRepositoryAsync<AppsWorld.InvoiceModule.Entities.V2.CompanyUserCompact>>().To<ApplicationCompactModuleRepository<AppsWorld.InvoiceModule.Entities.V2.CompanyUserCompact>>().InRequestScope();


            kernel.Bind<AppsWorld.InvoiceModule.Service.V2.ICNApplicationService>().To<AppsWorld.InvoiceModule.Service.V2.CNApplicationService>().InRequestScope();
          
      
        }

        #endregion New_Optimized_Block

        #region Reminders
        public static void RegisterReminderService(IKernel kernel)
        {
            kernel.Bind<ReminderApplicationService>().ToSelf().InRequestScope();

            kernel.Bind<IReminderModuleDataContextAsync>().To<ReminderContext>().InRequestScope();
            kernel.Bind<IReminderModuleUnitOfWorkAsync>().To<ReminderModuleUnitOfWork>().InRequestScope();

            //entites ControlCode
            kernel.Bind<IReminderModuleRepositoryAsync<AppsWorld.ReminderModule.Entities.Entities.SOAReminderBatchList>>().To<ReminderModuleRepository<AppsWorld.ReminderModule.Entities.Entities.SOAReminderBatchList>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<AppsWorld.ReminderModule.Entities.Entities.SOAReminderBatchListDetails>> ().To<ReminderModuleRepository<AppsWorld.ReminderModule.Entities.Entities.SOAReminderBatchListDetails>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.BeanEntity>>().To<ReminderModuleRepository<BillModule.Entities.BeanEntity>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.ControlCodeCategory>>().To<ReminderModuleRepository<BillModule.Entities.ControlCodeCategory>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.ControlCode>>().To<ReminderModuleRepository<BillModule.Entities.ControlCode>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<AppsWorld.ReminderModule.Entities.Entities.CompanyCompact>>().To<ReminderModuleRepository<AppsWorld.ReminderModule.Entities.Entities.CompanyCompact>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync< AppsWorld.ReminderModule.Entities.Entities.Bank>> ().To<ReminderModuleRepository< AppsWorld.ReminderModule.Entities.Entities.Bank>> ().InRequestScope();
            
            kernel.Bind<IReminderModuleRepositoryAsync<LocalizationCompact>>().To<ReminderModuleRepository<LocalizationCompact>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<GenericTemplateCompact>>().To<ReminderModuleRepository<GenericTemplateCompact>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.Address>>().To<ReminderModuleRepository<BillModule.Entities.Address>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.AddressBook>>().To<ReminderModuleRepository<BillModule.Entities.AddressBook>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.GSTSetting>>().To<ReminderModuleRepository<BillModule.Entities.GSTSetting>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<BillModule.Entities.IdType>>().To<ReminderModuleRepository<BillModule.Entities.IdType>>().InRequestScope();
            kernel.Bind<IReminderModuleRepositoryAsync<CommunicationCompact>>().To<ReminderModuleRepository<CommunicationCompact>>().InRequestScope();

            // service 
            kernel.Bind<AppsWorld.ReminderModule.Service.IReminderSaveService>().To<AppsWorld.ReminderModule.Service.ReminderSaveService>().InRequestScope();
        }
        #endregion

        public static void RegisterReminderKService(IKernel kernel)
        {

            kernel.Bind<IReminderKModuleDataContextAsync>().To<ReminderContextK>().InRequestScope();
            kernel.Bind<IReminderKModuleUnitOfWorkAsync>().To<ReminderKModuleUnitOfWork>().InRequestScope();

            //entites ControlCode
            kernel.Bind<IReminderKModuleRepositoryAsync<AppsWorld.ReminderModule.Entities.V2Entities.SOAReminderBatchListEntity>>().To<ReminderKModuleRepository<AppsWorld.ReminderModule.Entities.V2Entities.SOAReminderBatchListEntity>>().InRequestScope();
            kernel.Bind<IReminderKModuleRepositoryAsync<AppsWorld.ReminderModule.Entities.V2Entities.SOAReminderBatchListDetailsEntity>>().To<ReminderKModuleRepository<AppsWorld.ReminderModule.Entities.V2Entities.SOAReminderBatchListDetailsEntity>>().InRequestScope();
            kernel.Bind<IReminderKModuleRepositoryAsync<AppsWorld.ReminderModule.Entities.V2Entities.BeanEntity>>().To< ReminderKModuleRepository<AppsWorld.ReminderModule.Entities.V2Entities.BeanEntity>> ().InRequestScope();



            // service 
            kernel.Bind<AppsWorld.ReminderModule.Service.V2Service.IReminderKService>().To<AppsWorld.ReminderModule.Service.V2Service.ReminderKService>().InRequestScope();
        }

        public static void RegisterJournalV3Service(IKernel kernel)
        {

            kernel.Bind<JournalApplicationServiceV3>().ToSelf().InRequestScope();
            kernel.Bind<IJournalVoucherModuleDataContextAsyncV3>().To<JournalV3Context>().InRequestScope();
            kernel.Bind<IJournalVoucherModuleUnitOfWorkAsyncV3>().To<JournalVoucherModuleUnitOfWorkV3>().InRequestScope();

            //entites ControlCode
            kernel.Bind<IJournalVoucherModuleRepositoryAsyncV3<AccountTypeV3>>().To<JournalVoucherModuleRepositoryV3<AccountTypeV3>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsyncV3<CategoryV3>>().To<JournalVoucherModuleRepositoryV3<CategoryV3>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsyncV3<SubCategoryV3>>().To<JournalVoucherModuleRepositoryV3<SubCategoryV3>>().InRequestScope();

            kernel.Bind<IJournalVoucherModuleRepositoryAsyncV3<ChartOfAccountV3>>().To<JournalVoucherModuleRepositoryV3<ChartOfAccountV3>>().InRequestScope();

            // service 

            kernel.Bind<IJournalServiceV3>().To<JournalServiceV3>().InRequestScope();
        }

    }
}
