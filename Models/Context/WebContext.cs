using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebControlApp.Models.Entity;

namespace WebControlApp.Models.Context
{
    public class WebContext : DbContext
    {
        public WebContext(DbContextOptions<WebContext> options) : base(options)
        {
        }

        DbSet<WebAddress> WebAdress { get; set; }
        DbSet<StatusEmail> StatusEmail { get; set; }
    }
}
