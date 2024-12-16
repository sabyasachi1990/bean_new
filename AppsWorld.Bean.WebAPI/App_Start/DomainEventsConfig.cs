using Autofac;
using Domain.Events;
using Domain.Events.Autofac;
//using Domain.Events.Model.EventHandlers;
//using Domain.Events.Model.Events;
using Domain.Events.Autofac.Extensions;
using AppsWorld.JournalVoucherModule.Infra;

using AppsWorld.RevaluationModule.Infra;
using AppsWorld.PaymentModule.Infra;
//using Domain.Events.Model.EventHandlers;
//using Domain.Events.Model.Events;
using AppsWorld.BankWithdrawalModule.Infra;
using AppsWorld.OpeningBalancesModule.Infra;
using AppsWorld.CashSalesModule.Infra;
using AppsWorld.DebitNoteModule.Infra;
using AppsWorld.InvoiceModule.Infra;
using AppsWorld.MasterModule.Infra;
using AppsWorld.BankTransferModule.Infra;
using AppsWorld.BankReconciliationModule.Infra;
using AppsWorld.GLClearingModule.Infra;

namespace AppsWorld.Bean.WebAPI
{
    public class DomainEventsConfig
    {
		public static void RegisterDomainEvents()
		{
            var builder = new ContainerBuilder();
            //builder.RegisterDomainEventHandler<BillCreated, BillCreatedHandler>();
            //builder.RegisterDomainEventHandler<BillUpdated, BillUpdatedHandler>();
            //builder.RegisterDomainEventHandler<ReceiptCreated1, ReceiptCreate1Handler>();
            //builder.RegisterDomainEventHandler<ReceiptUpdated, ReceiptUpdatedHandler>();
            builder.RegisterDomainEventHandler<JournalUpdated, JournalUpdatedHandler>();
            builder.RegisterDomainEventHandler<JournalCreated, JournalCreatedHandler>();
            builder.RegisterDomainEventHandler<ReversalUpdated, ReversalUpdatedHandler>();
            builder.RegisterDomainEventHandler<JournalCopyCreated, JournalCopyCreatedHandler>();
            //builder.RegisterDomainEventHandler<BillCreated, BillCreatedHandler>();
            //builder.RegisterDomainEventHandler<BillUpdated, BillUpdatedHandler>();
            //builder.RegisterDomainEventHandler<ReceiptCreated, ReceiptCreatedHandler>();
            //builder.RegisterDomainEventHandler<ReceiptUpdated, ReceiptUpdatedHandler>();
            //builder.RegisterDomainEventHandler<CreditNoteDocumentVoidUpdated, CreditNoteDocumentVoidUpdatedHandler>();
            builder.RegisterDomainEventHandler<RevaluationCreated, RevaluationCreatedHandler>();
            builder.RegisterDomainEventHandler<RevaluationUpdated, RevaluationUpdatedHandler>();
            builder.RegisterDomainEventHandler<PaymentCreated, PaymentCreatedHandler>();
            builder.RegisterDomainEventHandler<PaymentUpdated, PaymentUpdatedHandler>();
            builder.RegisterDomainEventHandler<WithdrawalCreated, WithdrawalCreatedHandler>();
            builder.RegisterDomainEventHandler<WithdrawalUpdated, WithdrawalUpdatedHandler>();
            builder.RegisterDomainEventHandler<OpeningBalanceUpdated, OpeningBalanceUpdatedHandler>();
            builder.RegisterDomainEventHandler<OpeningBalanceCreated, OpeningBalanceCreatedHandler>();
            builder.RegisterDomainEventHandler<CashSaleCreated, CashSaleCreatedHandler>();
            builder.RegisterDomainEventHandler<CashSaleUpdated, CashSaleUpdatedHandler>();
            builder.RegisterDomainEventHandler<DepositCreated, DepositCreatedHandler>();
            builder.RegisterDomainEventHandler<DepositUpdated, DepositUpdatedHandler>();
            builder.RegisterDomainEventHandler<DebitNoteCreated, DebitNoteCreatedHandller>();
            builder.RegisterDomainEventHandler<DebitNoteUpdated, DebitNoteUpdatedHandler>();
            builder.RegisterDomainEventHandler<DebitNoteStatusChanged, DebitNoteStatusChangedHandler>();
            builder.RegisterDomainEventHandler<InvoiceCreated, InvoiceCreatedHandler>();
            builder.RegisterDomainEventHandler<InvoiceUpdated, InvoiceUpdatedHandler>();
            builder.RegisterDomainEventHandler<InvoiceStatusChanged, InvoiceStatusChangedHandler>();
            builder.RegisterDomainEventHandler<InvoiceDocStatusChanged, InvoiceDocStatusChangedHandler>();
            builder.RegisterDomainEventHandler<CreditNoteCreated, CreditNoteCreatedHandler>();
            builder.RegisterDomainEventHandler<CreditNoteUpdated, CreditNoteUpdatedHadler>();
            builder.RegisterDomainEventHandler<CreditNoteStatusChanged, CreditNoteStatusChangedHandler>();
            builder.RegisterDomainEventHandler<CreditNoteDocStatusChanged, CreditNoteDocStatusChangedHandler>();
            builder.RegisterDomainEventHandler<DoubtfulDebtCreated, DoubtfulDebtCreatedHandler>();
            builder.RegisterDomainEventHandler<DoubtfulDebtUpdated, DoubtfulDebitUpdatedHandler>();
            builder.RegisterDomainEventHandler<DoubtfulDebtStatusChanged, DoubtfulSatusChangedHandler>();
            //builder.RegisterDomainEventHandler<JournalLedgerCreated, JournalLedgerCreatedHandler>();
            //builder.RegisterDomainEventHandler<JournalLedgerUpdated, JournalLedgerUpdatedHandler>();


            builder.RegisterDomainEventHandler<BeanEntityCreated, BeanEntityCreatedHandler>();
            builder.RegisterDomainEventHandler<BeanEntityUpdated, BeanEntityUpdatedHandler>();
            builder.RegisterDomainEventHandler<BeanEntityStatusChanged, BeanEntityStatusChangedHandler>();
            builder.RegisterDomainEventHandler<QuickBeanEntityCreated, QuickBeanEntityCreatedHandler>();
            builder.RegisterDomainEventHandler<SegmentMasterCreated, SegmentMasterCreatedHandler>();
            builder.RegisterDomainEventHandler<SegmentMasterUpdated, SegmentMasterUpdatedHandler>();
            builder.RegisterDomainEventHandler<AllowableDisallowableUpdated, AllowableDisallowableUpdatedHandler>();
            builder.RegisterDomainEventHandler<FinancialSettingCreated, FinancialSettingCreatedHandller>();
            builder.RegisterDomainEventHandler<FinancialSettingUpdated, FinancialSettingUpdatedHandller>();
            builder.RegisterDomainEventHandler<GSTReportingCurrencyCreated, GSTReportingCurrencyCreatedHandler>();
            builder.RegisterDomainEventHandler<GSTSettingUpdated, GSTSettingUpdatedHandler>();
            builder.RegisterDomainEventHandler<GSTSettingCreated, GSTSettingCreatedHandler>();
            builder.RegisterDomainEventHandler<MultiCurrencySettingCreated, MultiCurrencySettingCreatedHandler>();
            builder.RegisterDomainEventHandler<MultiCurrencySettingUpdated, MultiCurrencySettingUpdatedHandler>();
            builder.RegisterDomainEventHandler<DeRegristationCreated, DeRegristationCreatedHandler>();
            builder.RegisterDomainEventHandler<FinancialYearEndChangedCreated, FinancialYearEndChangedCreatedHandller>();
            builder.RegisterDomainEventHandler<ItemCreated, ItemCreatedHandler>();
            builder.RegisterDomainEventHandler<ItemStatusChanged, ItemStatusChangedHandler>();
            builder.RegisterDomainEventHandler<ItemUpdated, ItemUpdatedHandler>();
            builder.RegisterDomainEventHandler<ForexCreated, ForexCreatedHandller>();
            builder.RegisterDomainEventHandler<ForexUpdated, ForexUpdatedHandller>();
            builder.RegisterDomainEventHandler<ChartOfAccountCreated, ChartOfAccountCreatedHandler>();
            builder.RegisterDomainEventHandler<ChartOfAccountUpdated, ChartOfAccountUpdatedHandler>();
            builder.RegisterDomainEventHandler<ChartOfAccountStatusChanged,CartOfAccountStatusChangedHandler>();
            builder.RegisterDomainEventHandler<CashPaymentCreated, CashPaymentCreatedHandler>();
            builder.RegisterDomainEventHandler<CashPaymentUpdated, CashPaymentUpdatedHandler>();
            builder.RegisterDomainEventHandler<BankTransferCreated, BankTransferCreatedHandler>();
            builder.RegisterDomainEventHandler<BankTransferUpdated, BankTransferUpdatedHandler>();
            builder.RegisterDomainEventHandler<BankReconciliationCreated, BankReconciliationCreatedHandler>();
            builder.RegisterDomainEventHandler<BankReconciliationUpdated, BankReconciliationUpdatedHandler>();

            //builder.RegisterDomainEventHandler<CreditMemoCreated, CreditMemoCreatedHandler>();
            //builder.RegisterDomainEventHandler<CreditMemoUpdated, CreditMemoUpdatedHadler>();
            //builder.RegisterDomainEventHandler<CreditMemoDocStatusChanged, CreditMemoDocStatusChangedHandler>();
            //builder.RegisterDomainEventHandler<CreditMemoStatusChanged, CreditMemoStatusChangedHandler>();
            builder.RegisterDomainEventHandler<ClearingCreated, ClearingCreatedHandler>();
            builder.RegisterDomainEventHandler<ClearingUpdated, ClearingUpdatedHandler>();

            var container = builder.Build();
            DomainEventChannel.Dispatcher = new AutofacDomainEventDispatcher(container);
        }
    }
}