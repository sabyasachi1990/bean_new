using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.ReminderModule.Entities.V2Entities;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.ReminderModule.RepositoryPattern;
using AppsWorld.ReminderModule.RepositoryPattern.V2Repository;
using AppsWorld.ReminderModule.Models.Models;
using System.Runtime.InteropServices;

namespace AppsWorld.ReminderModule.Service.V2Service
{
    public class ReminderKService : Service<SOAReminderBatchListEntity>, IReminderKService
    {
        private readonly IReminderKModuleRepositoryAsync<SOAReminderBatchListEntity> _soaReminderBatchListRepository;
        private readonly IReminderKModuleRepositoryAsync<SOAReminderBatchListDetailsEntity> _soaReminderBatchListDetailsRepository;
        private readonly IReminderKModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        public ReminderKService(IReminderKModuleRepositoryAsync<SOAReminderBatchListEntity> soaReminderBatchListRepository,
            IReminderKModuleRepositoryAsync<SOAReminderBatchListDetailsEntity> soaReminderBatchListDetailsRepository,
            IReminderKModuleRepositoryAsync<BeanEntity> beanEntityRepository
            ) : base(soaReminderBatchListRepository)
        {
            _soaReminderBatchListRepository = soaReminderBatchListRepository;
            _soaReminderBatchListDetailsRepository = soaReminderBatchListDetailsRepository;
            _beanEntityRepository = beanEntityRepository;
        }
        public async Task<IQueryable<ReminderVMK>> GetReminderskNew(long companyId, DateTime? fromDate, DateTime? toDate, string type, string name)
        {

            IQueryable<ReminderVMK> lstReminders = null;
            if (name == "Dismiss")
            {
                return await Task.FromResult((from r in _soaReminderBatchListRepository.Queryable()
                                              //join rd in _soaReminderBatchListDetailsRepository.Queryable() on r.Id equals rd.MasterId
                                              //into rds
                                              //from rd in rds.DefaultIfEmpty()
                                              join client in _beanEntityRepository.Queryable() on r.DocumentId equals client.Id
                                              where r.CompanyId == companyId && r.IsDismiss == true || r.JobStatus == "Sent" && r.JobExecutedOn >= fromDate && r.JobExecutedOn <= toDate
                                              select new ReminderVMK()
                                              {
                                                  Id = r.Id,
                                                  EntityId = client.Id,
                                                  EntityName = client.Name,
                                                  CompanyId = client.CompanyId,
                                                  BalanceAmount = r.Custbalance,
                                                  Recipient = r.Recipient,
                                                  ReminderType = r.ReminderType,
                                                  ReminderName = r.Name,
                                                  CreatedDate = r.ModifiedDate,
                                                  RemainderDate = r.JobExecutedOn,
                                                  DismissOrSentDate = r.ModifiedDate,
                                                  UserCreated = r.ModifiedBy,
                                                  Status = (r.IsDismiss == null || r.IsDismiss == false) ? r.JobStatus : "Dismiss"
                                              }).OrderByDescending(x => (x.CreatedDate)).AsQueryable());
               
            }
            return await Task.FromResult((from r in _soaReminderBatchListRepository.Queryable()
                                          //join rd in _soaReminderBatchListDetailsRepository.Queryable() on r.Id equals rd.MasterId into rds
                                          //from rd in rds.DefaultIfEmpty()
                                          join client in _beanEntityRepository.Queryable() on r.DocumentId equals client.Id
                                          where r.CompanyId == companyId
                                          && r.IsDismiss != true
                                          && r.JobStatus != "Sent"
                                          && r.JobExecutedOn >= fromDate
                                          && r.JobExecutedOn <= toDate
                                          select new ReminderVMK
                                          {
                                              Id = r.Id,
                                              EntityId = client.Id,
                                              EntityName = client.Name,
                                              CompanyId = client.CompanyId,
                                              BalanceAmount = r.Custbalance,
                                              Recipient = r.Recipient,
                                              ReminderType = r.ReminderType,
                                              ReminderName = r.Name,
                                              RemainderDate = r.JobExecutedOn,
                                              CreatedDate = r.CreatedDate,
                                              DismissOrSentDate = r.ModifiedDate
                                          }).OrderByDescending(x => (x.CreatedDate)).AsQueryable());
            


        }


    }

}

