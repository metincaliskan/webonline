using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using WebControlApp.Models;
using WebControlApp.Models.Context;
using WebControlApp.Models.Entity;
using WebControlApp.Services;

namespace WebControlApp.Controllers
{
    public class HomeController : Controller
    {
        private Models.Context.WebContext _context;
        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public HomeController(Models.Context.WebContext context, ILogger<HomeController> logger)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            Logger.Log(NLog.LogLevel.Info, "Run");
            var List = _context.Set<WebAddress>().ToList();
            foreach (var item in List)
            {
                Timer tt = new Timer(new TimerCallback(isOnline), item, 1, item.Interval);
            }

            return View(List);
        }

        void isOnline(object obj)
        {
            try
            {
                bool _isOnline = WebAddressRun.Ping((obj as WebAddress));

                var optionsBuilder = new DbContextOptionsBuilder<WebContext>();
                optionsBuilder.UseSqlServer(Startup.ConnectionString);

                using (var context = new WebContext(optionsBuilder.Options))
                {
                    WebAddress EditModel = context.Set<WebAddress>().ToList().Where(x => x.WebAddressId == (obj as WebAddress).WebAddressId).FirstOrDefault();
                    EditModel.IsOnline = _isOnline;
                    Logger.Log(NLog.LogLevel.Info, string.Format("{0} - {1} - {2}", EditModel.Name, EditModel.Url, EditModel.IsOnline));
                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public IActionResult CreateWebSite(int? Id)
        {
            WebAddress Model = new WebAddress();

            if (Id.HasValue)
            {
                Model = _context.Set<WebAddress>().Where(x => x.WebAddressId == Id).SingleOrDefault();
            }
            return View(Model);
        }

        [HttpPost]
        public IActionResult CreateWebSite(int? Id, WebAddress Model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!Id.HasValue) _context.Add(Model); //Insert
                    else
                    { //Update
                        WebAddress EditModel = _context.Set<WebAddress>().SingleOrDefault(x => x.WebAddressId == Id);
                        EditModel.Interval = Model.Interval;
                        EditModel.IsDelete = Model.IsDelete;
                        EditModel.Name = Model.Name;
                        EditModel.Url = Model.Url;
                    }
                    _context.SaveChanges();
                    Logger.Log(NLog.LogLevel.Info, string.Format("Yeni uygulama eklendi."));
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CreateWebSite");
                throw;
            }

            return View();
        }

        public IActionResult Contact()
        {
            var List = _context.Set<StatusEmail>().ToList();
            return View(List);
        }

        public async Task<IActionResult> DeleteEmail(int? Id)
        {
            try
            {
                var Email = _context.Set<StatusEmail>().SingleOrDefault(x => x.StatusEmailId == Id);
                _context.Remove<StatusEmail>(Email);
                Logger.Log(NLog.LogLevel.Info, string.Format("Email silindi. Silinen : {0}", Email.Email));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "DeleteEmail : " + ex.Message);
            }

            return RedirectToAction(nameof(Contact));
        }

        public IActionResult CreateEmail(int? Id)
        {
            StatusEmail Model = new StatusEmail();

            if (Id.HasValue)
            {
                Model = _context.Set<StatusEmail>().Where(x => x.StatusEmailId == Id).SingleOrDefault();
            }

            return View(Model);
        }

        [HttpPost]
        public IActionResult CreateEmail(int? Id, StatusEmail Model)
        {
            if (ModelState.IsValid)
            {
                if (!Id.HasValue) _context.Add(Model); //Insert
                else
                { //Update
                    StatusEmail EditModel = _context.Set<StatusEmail>().SingleOrDefault(x => x.StatusEmailId == Id);
                    EditModel.Email = Model.Email;
                }
                Logger.Log(NLog.LogLevel.Info, string.Format("Email eklendi{0}", Model.Email));
                _context.SaveChanges();
                return RedirectToAction("Contact");
            }
            return View();
        }

        public JsonResult IsDeleteWeb(int Id)
        {
            WebAddress EditModel = _context.Set<WebAddress>().SingleOrDefault(x => x.WebAddressId == Id);
            EditModel.IsDelete = EditModel.IsDelete == false ? true : false;
            Logger.Log(NLog.LogLevel.Info, string.Format("Web uygulaması silindi.", EditModel.Name));
            bool Result = _context.SaveChanges() > 0;
            return Json(Result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            Logger.Log(NLog.LogLevel.Info, "Error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
