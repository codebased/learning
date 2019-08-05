using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly TelemetryClient _insightsClient;

        public ValuesController(TelemetryClient insightsClient)
        {
            _insightsClient = insightsClient;
            _insightsClient.TrackPageView("Values Controller");

        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _insightsClient.TrackEvent("justforfun");
            _insightsClient.TrackTrace("Someone came here with food", Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("exception")]
        public ActionResult ThrowException()
        {
            try
            {
                throw new System.Exception();
            }
            catch (Exception ex)
            {
                _insightsClient.TrackException(ex);
            }

            return Ok();
        }
    }
}
