using LinkFinder.DbWorker;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Threading.Tasks;
using System.Linq;

namespace LinkFinder.WebApp.Controllers
{
    public class ResultController : Controller
    {
        private readonly DatabaseWorker _dbWorker;
        public ResultController(DatabaseWorker dbWorker)
        {
            _dbWorker = dbWorker;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id, int page = 1 , int pageSize = 10)
        {
            var results = await _dbWorker.GetResultsAsync(id);
            var outputResults = results.OrderBy(p=>p.TimeResponse).ToPagedList(page,pageSize);

            ViewBag.OnlySitemap = results.Where(p => p.InHtml == false).ToList();
            ViewBag.OnlyHtml = results.Where(p => p.InSitemap == false).ToList();

            return View(outputResults);
        }
    }
}
