using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Data.Entities.System;

namespace Web.Data.Configurations
{
    public class WebRoleConfiguration : IEntityTypeConfiguration<WebRole>
    {
        public void Configure(EntityTypeBuilder<WebRole> builder)
        {
            builder.ToTable("web_Role");
        }
    }
}
