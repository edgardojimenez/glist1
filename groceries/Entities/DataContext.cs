using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Groceries.Entities {

    public partial class DataContext : DbContext {

        public const string ConnectionString = "name=GroceriesConnection";

        #region Constructors

        public DataContext()
            : base(ConnectionString) {
        }

        public DataContext(string connectionString)
            : base(connectionString) {
        }

        #endregion

        #region DbSet Properties

        public DbSet<Product> Product { get; set; }
        public DbSet<Groceries> Groceries { get; set; }
        
        #endregion
    }
}