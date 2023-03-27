using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Data.Entities.System
{
    public class WebUser:IdentityUser<Guid>
    {
        public string FullName { get; set; }
        
    }
}
