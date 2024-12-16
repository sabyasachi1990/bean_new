using AppsWorld.CommonModule.Infra;
using AppsWorld.ReminderModule.Entities.Entities;
using AppsWorld.ReminderModule.Models.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;

namespace AppsWorld.ReminderModule.Service
{
    public interface IReminderSaveService : IService<SOAReminderBatchList>
    {
        Task<IQueryable<ReminderVMK>> GetReminderskNew(long companyId, DateTime? fromDate, DateTime? toDate, string type, string name);
        LookUpCategory<string> GetByCategoryCodeCategoryByCursorName(long companyId, string categoryCode);
        void UpdateSOAReminderBatchList(SOAReminderBatchList reminderBatchList);
        SOAReminderBatchList GetReminderBatchList(Guid id);
        LocalizationCompact GetLocalizationByCompanyId(long companyId);
        string GetLocalizationShotDate(long companyId);
        GenericTemplateCompact GetgenerictemplateById(Guid templateId);
        BeanEntity GetClientById(Guid clientId);
        List<Address> GetAddress(Guid id);
        CompanyCompact GetServiceCompanyForSOA(long? documentId);
        List<CompanyCompact> GetListOfServiceCompanyForSOA(List<long?> serCompIds);
        List<Bank> GetListOfBanks(List<long?> serviceCompIds);
        List<Address> GetAddressForCompany(long id);
        List<Address> GetListAddressForCompany(List<long?> serviceCompanyIds);
		string GetGSTnumber(long? companyId);
        Dictionary<long?, string> GetListGSTnumber(List<long?> serviceCompIds);
		string GetIdType(long? idTypeId);
        BeanEntity GetEntity(Guid entityId);
        void InsertCommunication(CommunicationCompact communicationNew);
        GenericTemplateCompact GetgenerictemplateByIdForPreview(long companyId, string templateName);
    }
}
