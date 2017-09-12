using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PdfAutofill.Model;
using PdfAutofill.Service;
using PdfAutofill.Service.Impl;

namespace PdfAutofill.Controllers
{
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        private readonly IPdfService _service = new PdfService();

        [HttpGet]
        public IActionResult Get([FromHeader(Name = "url")]string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                _service.InitDocument(url, false);

                return Ok(_service.GetAcroFields()?.Select(x => x.Key).ToList());
            }

            ModelState.AddModelError("url", "Url is missing");
            return BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult Post([FromBody]PdfViewModel model)
        {
            _service.InitDocument(model, true);

            var pdf = _service.FillPdf(model);

            Response.ContentType = "application/pdf";

            return Ok(pdf.Item1);
        }
    }
}
