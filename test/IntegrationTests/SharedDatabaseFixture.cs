using AutoMapper;
using CA.Application.Profiles;
using CA.Identity.DbContexts;
using CA.Infrastructure.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharedDatabaseSetup;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class SharedDatabaseFixture : IDisposable
    {

        private static IConfigurationProvider _configuration;
        public static IConfigurationProvider configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>();
                    });
                }
                return _configuration;
            }
        }
        private static IMapper _mapper;
        public static IMapper mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper = configuration.CreateMapper();
                }
                return _mapper;
            }

        }

        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private string dbNameSDADbContext = "IntegrationTestsDatabase.db";
        private string dbNameIdentityDbContext = "IntegrationTestsDatabase.dbIdentity";

        public SharedDatabaseFixture()
        {
            ConnectionSDADbContext = new SqliteConnection($"Filename={dbNameSDADbContext}");

            SeedSDADbContext();

            ConnectionSDADbContext.Open();


            ConnectionIdentityDbContext = new SqliteConnection($"Filename={dbNameIdentityDbContext}");

            SeedIdentityDbContext();

            ConnectionIdentityDbContext.Open();
        }

        public DbConnection ConnectionSDADbContext { get; }
        public DbConnection ConnectionIdentityDbContext { get; }

        public CustomDbContext CreateSDAContext(DbTransaction? transaction = null)
        {
            var context = new CustomDbContext(new DbContextOptionsBuilder<CustomDbContext>().UseSqlite(ConnectionSDADbContext).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }
        public IdentityDbContext CreateIdentityDbContext(DbTransaction? transaction = null)
        {
            var context = new IdentityDbContext(new DbContextOptionsBuilder<IdentityDbContext>().UseSqlite(ConnectionIdentityDbContext).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void SeedSDADbContext()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateSDAContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        DatabaseSetup.SeedDataCustomDbContext(context);
                    }

                    _databaseInitialized = true;
                }
            }
        }
        private void SeedIdentityDbContext()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateIdentityDbContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        DatabaseSetup.SeedDataIdentityDbContext(context);
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose()
        {
            ConnectionSDADbContext.Dispose();
            ConnectionIdentityDbContext.Dispose();
        }
    }
}
