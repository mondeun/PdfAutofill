using System;
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
                var fieldNames = _service.GetAcroFields(url)?.Fields?.Select(x => x.Key).ToList();
                if (fieldNames?.Count <= 0)
                    return NoContent();
                
                return Ok(fieldNames);
            }

            ModelState.AddModelError("url", "Url is missing");
            return BadRequest(ModelState);
        }

        [HttpPost("new")]
        public IActionResult PostHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                ModelState.AddModelError("html", "Cannot generate pdf from nothing");
                return BadRequest(ModelState);
            }
            var pdfData = _service.CreatepdfFromHtml(html);
            if (pdfData.LongLength <= 0)
            {
                ModelState.AddModelError("pdf", "Could not generate a pdf correctly");
                return BadRequest(ModelState);
            }

            Response.ContentType = "text/plain";
            return Ok(Convert.ToBase64String(pdfData));
        }

        [HttpPost("fill")]
        public IActionResult PostExistingPdf([FromBody]PdfViewModel model)
        {
            var pdfData = _service.FillPdf(model);

            Response.ContentType = "text/plain";

            return Ok(Convert.ToBase64String(pdfData));
        }
    }
}
