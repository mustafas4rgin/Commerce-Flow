using CommerceFLow.Services.Auth.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CommerceFLow.Services.Auth.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=ep-blue-tooth-a4iog2sy-pooler.us-east-1.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_9etL2cNKUAyu; SSL Mode=VerifyFull; Channel Binding=Require;");

            return new AppDbContext(optionsBuilder.Options);
        }
        
    }