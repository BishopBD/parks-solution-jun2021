using Microsoft.AspNetCore.Mvc;
using OutingsApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OutingsApi.Controllers
{
    public class ScheduledOutingsController : ControllerBase
    {

        private readonly IWriteOutingsForProcessing _processor;

        public ScheduledOutingsController(IWriteOutingsForProcessing processor)
        {
            _processor = processor;
        }

        [HttpPost("scheduledoutings")]
        public async Task<ActionResult> ScheduleAnOuting([FromBody] PostOutingCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // ??? 
            // 1. Write it to our "inside" database. 
            //   "I own this."

            // 2. Actually process the the thing, do something with it...
            await _processor.SendOuting(request);
            // 3. Log it out somewhere for someone else to deal with later.
            // write to a message queue - for us that will kafka. (we will be a "producer", "consumer", "broker"
            return Accepted();
        }
    }

 

    public record PostOutingCreateRequest
    {
        [Required]
        public string ParkId { get; init; }
        [Required]
        public DateTime When { get; init; }
        [Required]
        public string Who { get; init; }
        [Required]
        public string Notes { get; init; }
        public int? NumberOfPeople { get; init; }

       
    }
}


/*
 {
	Park: "1",
	When: "8398983973",
	Who: "Brian",
	Notes: "This should be fun. Bring bug spray",
	maxNumberOfPeople?: number 

}
*/