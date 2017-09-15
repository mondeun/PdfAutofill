using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfAutofill.Model;

namespace PdfAutofill.Service.Impl
{
    public class PdfService : IPdfService
    {
        private MemoryStream _memoryBuffer;

        public void InitDocument(string url, bool writeMode)
        {
            

            _memoryBuffer = new MemoryStream();
        }

        public void InitDocument(PdfViewModel model, bool writeMode)
        {
            InitDocument(model.Url, writeMode);
        }

        public byte[] FillPdf(PdfViewModel model)
        {

            var pdfReader = new PdfReader(model.Url);
            using (var memStream = new MemoryStream())
            {
                using (var stamper = new PdfStamper(pdfReader, memStream))
                {
                    var form = stamper.AcroFields;

                    foreach (var element in model.FieldsData)
                    {
                        if (form.Fields.ContainsKey(element.Key))
                        {
                            form.SetField(element.Key, element.Value);
                        }
                    }
                }
                pdfReader.Close();
                return memStream.ToArray();
            }
        }

        public AcroFields GetAcroFields(string url)
        {
            var pdfReader = new PdfReader(url);
            var fields = pdfReader.AcroFields;

            return fields;
        }
    }
}
