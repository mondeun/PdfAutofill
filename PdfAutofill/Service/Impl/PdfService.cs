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
        private Document _pdfDocument;
        private PdfReader _pdfReader;
        private PdfWriter _pdfWriter;
        private MemoryStream _memoryBuffer;
        private bool _writeMode;

        public void InitDocument(string url, bool writeMode)
        {
            _writeMode = writeMode;

            byte[] bytes;

            using (var client = new WebClient())
            {
                bytes = client.DownloadData(new Uri(url));
            }

            using (var memStream = new MemoryStream(bytes))
            {
                _pdfDocument = new Document();
                _pdfReader = new PdfReader(memStream);

                if (!writeMode)
                    return;

                _memoryBuffer = new MemoryStream();
                _pdfWriter = PdfWriter.GetInstance(_pdfDocument, memStream);
            }
        }

        public void InitDocument(PdfViewModel model, bool writeMode)
        {
            InitDocument(model.Url, writeMode);
        }

        public string FillPdf(PdfViewModel model)
        {
            var stamper = new PdfStamper(_pdfReader, _memoryBuffer);
            var form = stamper.AcroFields;

            foreach (var element in model.FieldsData)
            {
                if (form.Fields.ContainsKey(element.Key))
                {
                    form.SetField(element.Key, element.Value);
                }
            }

            _memoryBuffer.Seek(0, 0);

            var pdfBytes = _memoryBuffer.ToArray();

            stamper.Close();
            _pdfReader.Close();
            _pdfWriter.Dispose();



            return Convert.ToBase64String(pdfBytes);
        }

        public AcroFields GetAcroFields()
        {
            var fields = _pdfReader.AcroFields;

            if (!_writeMode)
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
