using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PdfAutofillNet.Service;
using PdfAutofillNet.Service.Impl;

namespace PdfAutofillNet.Controllers
{
    [System.Web.Mvc.Route("api/[controller]")]
    public class PdfController : ApiController
    {
        private readonly IPdfService _service = new PdfService();

        [System.Web.Mvc.HttpGet]
        public IHttpActionResult Get([FromUri(Name = "url")]string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                var fieldNames = _service.GetAcroFields(url)?.Fields?.Select(x => x.Key).ToList();
                if (fieldNames == null)
                {
                    ModelState.AddModelError("url", $"404 - Cannot find a pdf on {url}");
                    return BadRequest(ModelState);
                }
                if (fieldNames.Count <= 0)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return Ok(fieldNames);
            }

            ModelState.AddModelError("url", "Url is missing");
            return BadRequest(ModelState);
        }

        [System.Web.Mvc.HttpPost]
        public IHttpActionResult Post([FromBody]string html)
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
            return Ok(pdfData);
        }
    }
}
