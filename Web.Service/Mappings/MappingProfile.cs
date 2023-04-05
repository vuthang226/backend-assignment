using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Data.Entities.System;
using Web.ViewModel.System;

namespace Web.Service.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<WebUser, UserVm>();
        }
    }
}
