﻿using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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

                var fieldNames = _service.GetAcroFields()?.Fields?.Select(x => x.Key).ToList();
                if (fieldNames?.Count <= 0)
                    return NoContent();

                return Ok(fieldNames);
            }

            ModelState.AddModelError("url", "Url is missing");
            return BadRequest(ModelState);
        }

        [HttpPost]
        public string Post([FromBody]PdfViewModel model)
        {
            _service.InitDocument(model, true);

            var pdfData = _service.FillPdf(model);

            Response.ContentType = "text/plain";

            return pdfData;
        }
    }
}
