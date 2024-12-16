using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.RepositoryPattern
{
    public interface ICreditMemoModuleRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {

    }
}
