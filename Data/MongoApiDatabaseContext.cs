using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mongoAPI.Models;

    public class MongoApiDatabaseContext : DbContext
    {
        public MongoApiDatabaseContext (DbContextOptions<MongoApiDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<mongoAPI.Models.Item> Item { get; set; }
    }
