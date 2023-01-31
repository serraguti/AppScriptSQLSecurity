using AppScriptSQLSecurity.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using AppScriptSQLSecurity.Models;
using AppScriptSQLSecurity.Filters;

namespace AppScriptSQLSecurity.Controllers
{
    [AuthorizeUsers]
    public class TimersController : Controller
    {
        RepositoryTimers repo;
        private IWebHostEnvironment Environment;

        public TimersController(RepositoryTimers repo
            , IWebHostEnvironment environment)
        {
            this.repo = repo;
            this.Environment = environment;
        }

        public IActionResult Index()
        {
            List<TiemposEventos> tiempos = this.repo.GetTimers();
            return View(tiempos);
        }

        [HttpPost]
        public IActionResult Index(string accion)
        {
            //string fileName = "SoloDatosTimers.sql";
            string fileName = "SoloDatosTimersBarcelona.sql";
            string path = Path.Combine(this.Environment.WebRootPath, "files/") + fileName;
            string readContents;
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                readContents = streamReader.ReadToEnd();
            }
            this.repo.ExecuteScript(readContents);
            List<TiemposEventos> tiempos = this.repo.GetTimers();
            return View(tiempos);
        }

        public IActionResult IncreaseTimers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IncreaseTimers(int minutes)
        {
            this.repo.IncreaseMinutesTimers(minutes);
            return RedirectToAction("Index");
        }
    }
}
