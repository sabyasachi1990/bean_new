using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
using AppsWorld.PaymentModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.PaymentModule.Infra;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using AppsWorld.PaymentModule.Infra.Resources;
namespace AppsWorld.PaymentModule.Service
{
    public class FeatureService : Service<Feature>, IFeatureService
    {
        private readonly IPaymentModuleRepositoryAsync<Feature> _featureRepository;

        public FeatureService(IPaymentModuleRepositoryAsync<Feature> featureRepository)
            : base(featureRepository)
        {
            this._featureRepository = featureRepository;
        }

    }
}
