using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HiSFS.Storage.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly ILogger<StorageController> _logger;


        public StorageController(ILogger<StorageController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            var size = files.Sum(x => x.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length <= 0)
                    continue;

                var filePath = Path.GetTempFileName();

                using var fs = System.IO.File.Create(filePath);
                await formFile.CopyToAsync(fs);
            }

            return Ok(new { count = files.Count, size });
        }
    }
}
