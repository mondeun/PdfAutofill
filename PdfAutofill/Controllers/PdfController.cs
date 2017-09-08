using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Forms;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using PdfAutofill.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PdfAutofill.Controllers
{
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        // POST api/values
        [HttpPost]
        public void Post([FromBody]PdfViewModel model)
        {
            // TODO
        }
    }
}
