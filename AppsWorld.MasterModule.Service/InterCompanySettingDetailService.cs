using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class InterCompanySettingDetailService : Service<InterCompanySettingDetail>, IInterCompanySettingDetailService
    {
        private readonly IMasterModuleRepositoryAsync<InterCompanySettingDetail> _InterCompanySettingDetailRepository;

        public InterCompanySettingDetailService(IMasterModuleRepositoryAsync<InterCompanySettingDetail> InterCompanySettingDetailRepository)
            : base(InterCompanySettingDetailRepository)
        {
            _InterCompanySettingDetailRepository = InterCompanySettingDetailRepository;

        }

        public InterCompanySettingDetail GetInterCompanyDetails(long? serviceCompanyId, Guid interCompanyDetailId)
        {
            return _InterCompanySettingDetailRepository.Queryable().Where(s => s.ServiceEntityId == serviceCompanyId && s.Id == interCompanyDetailId).FirstOrDefault();
        }
        public List<InterCompanySettingDetail> GetListOfInterCompanySetttingDetail(List<Guid> interCompanySettingDetailids)
        {
            return _InterCompanySettingDetailRepository.Query(a => interCompanySettingDetailids.Contains(a.Id)).Select().ToList();
        }
    }
}
