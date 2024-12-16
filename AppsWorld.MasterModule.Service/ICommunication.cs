using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface ICommunication : IService<Communication>
    {

        IQueryable<CommunicationModel> GetCommunications(Guid clientId,string connectionSting);
    }
}
