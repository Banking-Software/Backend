using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroFinance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwaggerController : ControllerBase
    {
        private readonly ISwaggerProvider _swaggerProvider;

        public SwaggerController(ISwaggerProvider swaggerProvider)
        {
            _swaggerProvider = swaggerProvider;
        }

        [HttpGet("getall")]
        [SwaggerOperation(OperationId = "GetSwaggerJson")]
        [Produces("application/json")]
        public IActionResult GetSwaggerJson()
        {
            var swagger = _swaggerProvider.GetSwagger("v1");
            // var apiNames = new List<string>();
            // foreach (var path in swagger.Paths)
            // {
            //     apiNames.Add(path.Key);
            // }

            return Ok(swagger);
        }
    }
}