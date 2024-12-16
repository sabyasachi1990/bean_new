using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.RevaluationModule.RepositoryPattern.V2;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.V2;
using System.Data.SqlClient;
using System.Data;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public class AutoNumberService : Service<AutoNumberCompact>, IAutoNumberService
    {
        private readonly IRevaluationRepositoryAsync<AutoNumberCompact> _autoNumberepository;
        private readonly IRevaluationRepositoryAsync<AutoNumberCompanyCompact> _autoNumbeCompanyRepository;

        public AutoNumberService(IRevaluationRepositoryAsync<AutoNumberCompact> autoNumberRepository, IRevaluationRepositoryAsync<AutoNumberCompanyCompact> autoNumbeCompanyRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
            this._autoNumbeCompanyRepository = autoNumbeCompanyRepository;
        }

        public AutoNumberCompact GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public string GetAutoNumberPreview(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(e => e.Preview).FirstOrDefault();
        }
        public bool? GetAutoNumberFlag(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(x => x.IsEditable).FirstOrDefault();
        }
        public List<AutoNumberCompanyCompact> GetAutoNumberCompany(Guid AutoNumberId)
        {
            return _autoNumbeCompanyRepository.Query(a => a.AutonumberId == AutoNumberId).Select().ToList();
        }
        public AutoNumberCompanyCompact GetAutoCompany(Guid AutoNumberId)
        {
            return _autoNumbeCompanyRepository.Query(a => a.AutonumberId == AutoNumberId).Select().FirstOrDefault();
        }

        public string GetAutonumber(long companyId, string entityType, string connectionString)
        {
            string docNo = null;
            //bool? isEditable;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Common_GenerateDocNo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        cmd.Parameters.AddWithValue("@CursorName", "Bean Cursor");
                        cmd.Parameters.AddWithValue("@EntityType", entityType);
                        cmd.Parameters.AddWithValue("@IsAdd", false);
                        cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@DocNo"].Direction = ParameterDirection.Output;
                        //cmd.Parameters.Add("@IsDocNoEditable", SqlDbType.Bit);
                        //cmd.Parameters["@IsDocNoEditable"].Direction = ParameterDirection.Output;
                        cmd.ExecuteScalar();
                        docNo = cmd.Parameters["@DocNo"].Value != DBNull.Value ? Convert.ToString(cmd.Parameters["@DocNo"].Value) : null;

                        //isEditable = cmd.Parameters["@IsDocNoEditable"].Value != DBNull.Value ? Convert.ToBoolean(cmd.Parameters["@IsDocNoEditable"].Value) : (bool?)null;
                        //isEdit = isEditable;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return docNo;
        }
    }
}
