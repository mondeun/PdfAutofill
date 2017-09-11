using Microsoft.AspNetCore.Mvc;
using PdfAutofill.Model;

namespace PdfAutofill.Controllers
{
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        [HttpPost]
        public string Post([FromBody]PdfViewModel model)
        {
            var service = new Service.PdfService();

            var pdf = service.FillPdf(model);
            
            // TODO Exception handling and validation

            return pdf;
        }
    }
}
