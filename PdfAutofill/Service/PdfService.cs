using System;
using System.Net;
using iText.Kernel.Pdf;
using PdfAutofill.Model;

namespace PdfAutofill.Service
{
    public class PdfService
    {
        public string FillPdf(PdfViewModel model)
        {
            byte[] bytes = { };
            var base64 = string.Empty;
            var pdfDoc = new PdfDocument(new PdfReader(string.Empty), new PdfWriter(string.Empty));
            

            using (var client = new WebClient())
            {
                bytes = client.DownloadData(new Uri(model.Url));
            }


            return base64;
        }
    }
}
