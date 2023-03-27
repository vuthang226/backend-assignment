using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Web.Data.EF
{
    public class AngularDbContextFactory:IDesignTimeDbContextFactory<AngularDbContext>
    {
        public AngularDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("AngularDb");

            var optionsBuilder = new DbContextOptionsBuilder<AngularDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new AngularDbContext(optionsBuilder.Options);


        }

        
    }
}
