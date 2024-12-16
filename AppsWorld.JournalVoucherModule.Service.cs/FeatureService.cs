using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.Framework;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace AppsWorld.JournalVoucherModule.Service
{
    public class FeatureService : Service<Feature>, IFeatureService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Feature> _featureRepository;

        public FeatureService(IJournalVoucherModuleRepositoryAsync<Feature> featureRepository)
            : base(featureRepository)
        {
            this._featureRepository = featureRepository;
        }

    }
}
