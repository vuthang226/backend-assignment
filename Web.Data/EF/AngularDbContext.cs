using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Data.Configurations;
using Web.Data.Entities.System;

namespace Web.Data.EF
{
    public class AngularDbContext:IdentityDbContext<WebUser,WebRole,Guid>
    {
        public AngularDbContext (DbContextOptions options):base (options)
        {

        }
        protected override void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("WebUserClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("WebUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("WebUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("WebRoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("WebUserTokens").HasKey(x => x.UserId);
        }
    }
}
