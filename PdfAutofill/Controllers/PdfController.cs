using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PdfAutofill.Model;
using PdfAutofill.Service;

namespace PdfAutofill.Controllers
{
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        [HttpGet]
        public List<string> Get([FromHeader(Name = "url")]string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                var service = new PdfService();
                service.InitDocument(url);

                return service.GetAcroFields().Select(x => x.Key).ToList();
            }

            return null;
        }

        [HttpPost]
        public string Post([FromBody]PdfViewModel model)
        {
            var service = new PdfService();
            service.InitDocument(model);

            var pdf = service.FillPdf(model);
            
            // TODO Exception handling and validation

            return pdf;
        }
    }
}
