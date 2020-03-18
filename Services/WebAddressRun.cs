using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebControlApp.Models.Context;
using WebControlApp.Models.Entity;

namespace WebControlApp.Services
{
    public class WebAddressRun
    {
        public static bool Ping(WebAddress Model)
        {
            bool Status = true;
            try
            {
                var ping = new System.Net.NetworkInformation.Ping();

                var result = ping.Send(Model.Url);

                if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                {
                    Status = false;

                    var optionsBuilder = new DbContextOptionsBuilder<WebContext>();
                    optionsBuilder.UseSqlServer(Startup.ConnectionString);

                    using (var context = new WebContext(optionsBuilder.Options))
                    {
                        var ListEmail = context.Set<StatusEmail>().ToList();

                        foreach (var item in ListEmail)
                        {
                            new EmailRun().SendEmail(item.Email, Model.Name, Model.Url);
                        }

                        context.SaveChanges();
                    }

                }

                return Status;
            }
            catch
            {
                return false;
            }
        }

   

    }
}
