using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class ControlCodeCategoryService : Service<ControlCodeCategory>, IControlCodeCategoryService
    {
        private readonly IMasterModuleRepositoryAsync<ControlCodeCategory> _controlCodeCategoryRepository;
        private readonly IControlCodeService _controlCodeRepository;
        public ControlCodeCategoryService(IMasterModuleRepositoryAsync<ControlCodeCategory> controlCodeCategoryRepository, IControlCodeService controlCodeRepository)
            : base(controlCodeCategoryRepository)
        {
            _controlCodeCategoryRepository = controlCodeCategoryRepository;
            _controlCodeRepository = controlCodeRepository;
        }

        public LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == RecordStatusEnum.Active).Include(c => c.ControlCodes).Select().FirstOrDefault();
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                //List<ControlCode> lstCont = _controlCodeRepository.GetControlCodesByCatId(controlcodeCategory.Id);
                if (CategoryCode != "VendorType")
                {
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new LookUp<string>()
                    {
                        Code = x.CodeKey,
                        Name = x.CodeValue,
                        Id = x.Id,
                        RecOrder = x.RecOrder
                    }).OrderBy(x => x.RecOrder).ToList();
                }
                else
                {
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active && c.CodeValue != "CPF Board").Select(x => new LookUp<string>()
                    {
                        Code = x.CodeKey,
                        Name = x.CodeValue,
                        Id = x.Id,
                        RecOrder = x.RecOrder
                    }).OrderBy(x => x.RecOrder).ToList();
                }
            }
            return lookUpCategory;
        }


        public async Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long companyId, string categoryCode)
        {
            try
            {
                var controlCodeCategory = await Task.Run(() => _controlCodeCategoryRepository
                .Query(a => a.CompanyId == companyId && a.ControlCodeCategoryCode == categoryCode && a.Status == RecordStatusEnum.Active)
                .Include(c => c.ControlCodes).Select()
                .FirstOrDefault());

                if (controlCodeCategory != null)
                {
                    var filteredControlCodes = categoryCode != "VendorType"
                        ? controlCodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active)
                        : controlCodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active && c.CodeValue != "CPF Board");

                    var lookUpCategory = new LookUpCategory<string>
                    {
                        CategoryName = controlCodeCategory.ControlCodeCategoryDescription,
                        DefaultValue = controlCodeCategory.DefaultValue,
                        Code = controlCodeCategory.ControlCodeCategoryCode,
                        Id = controlCodeCategory.Id,
                        Lookups = filteredControlCodes
                            .Select(x => new LookUp<string>
                            {
                                Code = x.CodeKey,
                                Name = x.CodeValue,
                                Id = x.Id,
                                RecOrder = x.RecOrder
                            })
                            .OrderBy(x => x.RecOrder)
                            .ToList()
                    };

                    return lookUpCategory;
                }
                else
                {
                    return new LookUpCategory<string>();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<LookUp<string>> GetInactiveControlCodeAsync(long companyId, string categoryCode, string controlCodeKey)
        {
            var controlCodeCategory = await Task.Run(()=> _controlCodeCategoryRepository
                .Query(a => a.CompanyId == companyId && a.ControlCodeCategoryCode == categoryCode && a.Status == RecordStatusEnum.Active).Select()
                .FirstOrDefault());

            if (controlCodeCategory != null)
            {
                var controlCode = await Task.Run(() => _controlCodeRepository
                    .Query(a => a.ControlCategoryId == controlCodeCategory.Id && a.Status == RecordStatusEnum.Inactive && a.CodeKey == controlCodeKey).Select().FirstOrDefault());
                if (controlCode != null)
                {
                    var existingLookup = (await GetByCategoryCodeCategoryAsync(companyId, categoryCode)).Lookups.FirstOrDefault(a => a.Id == controlCode.Id);

                    if (existingLookup == null)
                    {
                        var lookUp = new LookUp<string>
                        {
                            Code = controlCode.CodeKey,
                            Name = controlCode.CodeValue,
                            Id = controlCode.Id,
                            RecOrder = controlCode.RecOrder
                        };
                        (await GetByCategoryCodeCategoryAsync(companyId, categoryCode)).Lookups.Add(lookUp);
                        return lookUp;
                    }
                }
            }

            return null;
        }
        public LookUp<string> GetInactiveControlcode(long CompanyId, string CategoryCode, string controlCodeKey)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
            if (controlcodeCategory != null)
            {
                var controlCode =
                    _controlCodeRepository.Query(a => a.ControlCategoryId == controlcodeCategory.Id && a.Status == RecordStatusEnum.Inactive && a.CodeKey == controlCodeKey).Select().FirstOrDefault();
                if (controlCode != null)
                {
                    LookUp<string> lookUp = new LookUp<string>();
                    var lu = this.GetByCategoryCodeCategory(CompanyId, CategoryCode).Lookups.Where(a => a.Id == controlCode.Id).FirstOrDefault();
                    if (lu == null)
                    {
                        lookUp.Code = controlCode.CodeKey;
                        lookUp.Name = controlCode.CodeValue;
                        lookUp.Id = controlCode.Id;
                        lookUp.RecOrder = controlCode.RecOrder;
                        this.GetByCategoryCodeCategory(CompanyId, CategoryCode).Lookups.Add(lookUp);
                    }
                    return lookUp;
                }
            }
            return null;
        }

        public ControlCodeCategory GetcontrolCodecategory(long companyId, string control_Codes_CommunicationType)
        {
            return _controlCodeCategoryRepository.Query(a => a.ControlCodeCategoryCode == control_Codes_CommunicationType && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Include(z => z.ControlCodes).Select().FirstOrDefault();

        }

        public FrameWork.LookUpCategory<string> GetByCategoryCodeCategory1(long companyId, string CategoryCode)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == companyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == RecordStatusEnum.Active).Include(c => c.ControlCodes).Select().FirstOrDefault();
            var lookUpCategory = new FrameWork.LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new FrameWork.LookUp<string>()
                {
                    Code = x.CodeKey,
                    Name = x.CodeValue,
                    Id = x.Id,
                    RecOrder = x.RecOrder
                }).AsEnumerable().OrderBy(x => x.RecOrder).ToList();
            }
            return lookUpCategory;
        }




        public async Task<FrameWork.LookUpCategory<string>> GetByCategoryCodeCategory1Asnc(long companyId, string CategoryCode)
        {
            var controlCodeCategory = await Task.Run(() => _controlCodeCategoryRepository.Query(a => a.CompanyId == companyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == RecordStatusEnum.Active).Include(c => c.ControlCodes).Select().FirstOrDefault());
            var lookUpCategory = new FrameWork.LookUpCategory<string>();
            if (controlCodeCategory != null)
            {
                lookUpCategory.CategoryName = controlCodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlCodeCategory.DefaultValue;
                lookUpCategory.Code = controlCodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlCodeCategory.Id;
                lookUpCategory.Lookups = controlCodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new FrameWork.LookUp<string>()
                {
                    Code = x.CodeKey,
                    Name = x.CodeValue,
                    Id = x.Id,
                    RecOrder = x.RecOrder
                }).AsQueryable().OrderBy(x => x.RecOrder).ToList();
            }
            return lookUpCategory;
        }
    }
}
