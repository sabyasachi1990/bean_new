using AppsWorld.MasterModule.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
	public class AllowableDisallowableUpdated : IDomainEvent
	{
		public CompanySetting CompanySetting { get; set; }

		public AllowableDisallowableUpdated(CompanySetting companySetting)
		{
			CompanySetting = companySetting;
		}
	}
}
