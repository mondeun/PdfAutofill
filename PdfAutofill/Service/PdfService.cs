using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using iText.Layout;
using PdfAutofill.Model;

namespace PdfAutofill.Service
{
    public class PdfService
    {
        public string FillPdf(PdfViewModel model)
        {
            byte[] bytes;
            var base64 = string.Empty;            

            using (var client = new WebClient())
            {
                bytes = client.DownloadData(new Uri(model.Url));
            }

            var memStream = new MemoryStream(bytes);
            var pdfReader = new PdfReader(memStream);
            // var pdfWriter = new PdfWriter();

            var pdfDoc = new PdfDocument(pdfReader);

            var fields = GetAcroFields(pdfDoc);

            

            return fields.ToString();
        }

        public List<KeyValuePair<string, PdfFormField>> GetAcroFields(PdfDocument doc)
        {
            var fields = new List<KeyValuePair<string, PdfFormField>>();

            foreach (var field in PdfAcroForm.GetAcroForm(doc, false).GetFormFields())
            {
                fields.Add(field);
            }

            return fields;
        }
    }
}
