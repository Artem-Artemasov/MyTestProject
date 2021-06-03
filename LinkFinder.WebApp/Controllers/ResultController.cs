using LinkFinder.DbWorker;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(int id)
        {
            var results = await _dbWorker.GetResultsAsync(id);

            return View(results);
        }
    }
}
