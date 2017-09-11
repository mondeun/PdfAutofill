using System.Collections.Generic;
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
        public List<string> Get([FromHeader(Name = "url")]string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                _service.InitDocument(url);

                return _service.GetAcroFields().Select(x => x.Key).ToList();
            }

            return null;
        }

        [HttpPost]
        public string Post([FromBody]PdfViewModel model)
        {
            _service.InitDocument(model);

            var pdf = _service.FillPdf(model);
            
            // TODO Exception handling and validation

            return pdf;
        }
    }
}
