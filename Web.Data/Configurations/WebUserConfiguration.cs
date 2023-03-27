using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Data.Entities.System;

namespace Web.Data.Configurations
{
    public class WebUserConfiguration : IEntityTypeConfiguration<WebUser>
    {
        public void Configure(EntityTypeBuilder<WebUser> builder)
        {
            builder.ToTable("web_User");
            
            
        }
    }
}
