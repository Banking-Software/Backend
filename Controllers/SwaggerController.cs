using MicroFinance.Helper;
using Microsoft.AspNetCore.Mvc;
using NepaliCalendar;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroFinance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwaggerController : ControllerBase
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private readonly INepaliCalendarFormat _nepaliCalendarFormat;

        public SwaggerController(ISwaggerProvider swaggerProvider, INepaliCalendarFormat nepaliCalendarFormat)
        {
            _swaggerProvider = swaggerProvider;
            _nepaliCalendarFormat=nepaliCalendarFormat;
        }

        [HttpGet("getall")]
        [SwaggerOperation(OperationId = "GetSwaggerJson")]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetSwaggerJson()
        {

            var swagger = _swaggerProvider.GetSwagger("v1");
            DateTime EngDate1 = DateTime.Now;  
            string nepaliDate = await _nepaliCalendarFormat.ConvertEnglishDateToNepali(EngDate1);
            System.Console.WriteLine(nepaliDate);

            
            // Console.WriteLine($"IsValid: {NepaliCalendar.Convert.Now.BSIsValid()}");
            // var apiNames = new List<string>();
            // foreach (var path in swagger.Paths)
            // {
            //     apiNames.Add(path.Key);
            // }

            return Ok(nepaliDate);
        }
    }
}