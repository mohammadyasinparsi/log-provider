using System;
using System.Collections.Generic;
using Kernel.LogProvider.SerilogProvider;
using Microsoft.AspNetCore.Mvc;
using Kernel.LogProvider.SerilogProvider;


namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogProvider _logger;

        public TestController(ILogProvider logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Log(Test test)
        {
            try
            {
                int testNumber = new Random().Next(0, int.MaxValue);
                var ex = new Exception($"Test number {testNumber}"
                    , new Exception($"inner exception of test number: {testNumber}"));
                throw ex;
            }
            catch (Exception e)
            {
                var payload = new Dictionary<string, string>
                {
                    { "Payload 1", "Payload Value 1" },
                    { "Payload 2", "Payload Value 2" },
                    { "Payload 3", "Payload Value 3" }
                };
                _logger.LogError(nameof(TestController), e
                    , payload);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult LogGet()
        {
            try
            {
                int testNumber = new Random().Next(0, int.MaxValue);
                var ex = new Exception($"Test number {testNumber}"
                    , new Exception($"inner exception of test number: {testNumber}"));
                throw ex;
            }
            catch (Exception e)
            {
                var payload = new Dictionary<string, string>
                {
                    { "Payload 1", "Payload Value 1" },
                    { "Payload 2", "Payload Value 2" },
                    { "Payload 3", "Payload Value 3" }
                };
                _logger.LogError(nameof(TestController), e, payload);
            }

            return Ok();
        }

        [HttpPost("2")]
        public IActionResult Log2([FromForm] Test test)
        {
            try
            {
                int testNumber = new Random().Next(0, int.MaxValue);
                var ex = new Exception($"Test number {testNumber}"
                    , new Exception($"inner exception of test number: {testNumber}"));
                throw ex;
            }
            catch (Exception e)
            {
                var payload = new Dictionary<string, string>
                {
                    { "Payload 1", "Payload Value 1" },
                    { "Payload 2", "Payload Value 2" },
                    { "Payload 3", "Payload Value 3" }
                };
                _logger.LogError(nameof(TestController), e
                    , payload);
            }

            return Ok();
        }
    }

    public class Test
    {
        public string typeOne { get; set; }
        public string typeTwo { get; set; }
    }
}