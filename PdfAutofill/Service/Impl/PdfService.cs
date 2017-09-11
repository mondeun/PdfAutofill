using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using PdfAutofill.Model;

namespace PdfAutofill.Service.Impl
{
    public class PdfService : IPdfService
    {
        private PdfDocument _pdfDocument;
        private PdfReader _pdfReader;

        public void InitDocument(string url)
        {
            byte[] bytes;

            using (var client = new WebClient())
            {
                bytes = client.DownloadData(new Uri(url));
            }

            var memStream = new MemoryStream(bytes);
            _pdfReader = new PdfReader(memStream);

            _pdfDocument = new PdfDocument(_pdfReader);

            memStream.Close();
        }

        public void InitDocument(PdfViewModel model)
        {
            InitDocument(model.Url);
        }

        public string FillPdf(PdfViewModel model)
        {
            var base64 = string.Empty;            
            
            var fields = GetAcroFields();

            Close();
            return fields.ToString();
        }

        public List<KeyValuePair<string, PdfFormField>> GetAcroFields()
        {
            var fields = new List<KeyValuePair<string, PdfFormField>>();

            foreach (var field in PdfAcroForm.GetAcroForm(_pdfDocument, false).GetFormFields())
            {
                fields.Add(field);
            }

            Close();
            return fields;
        }

        private void Close()
        {
            _pdfDocument.Close();
            _pdfReader.Close();
        }
    }
}
