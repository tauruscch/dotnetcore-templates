using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Serilog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<int> Get()
        {
            _logger.LogTrace("Get Test 1");
            _logger.LogInformation("Get Test 2");
            return Enumerable.Range(1, 5);
        }

        [HttpPost]
        public Person Post(Person model)
        {
            _logger.LogInformation("Post Test");
            return model;
        }

        [HttpPut]
        public Person Put(Person model)
        {
            _logger.LogInformation("Put Test");
            throw new Exception("抛出个测试");
        }
    }

    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
