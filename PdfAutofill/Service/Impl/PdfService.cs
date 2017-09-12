using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                _pdfReader = new PdfReader(memStream);

                if (writeMode)
                {
                    _memoryBuffer = new MemoryStream();
                    _pdfWriter = new PdfWriter(_memoryBuffer);
                    _pdfDocument = new PdfDocument(_pdfReader, _pdfWriter);
                }
                else
                    _pdfDocument = new PdfDocument(_pdfReader);
            }
        }

        public void InitDocument(PdfViewModel model, bool writeMode)
        {
            InitDocument(model.Url, writeMode);
        }

        public (string, int) FillPdf(PdfViewModel model)
        {
            var fields = GetAcroFields();

            foreach (var element in model.FieldsData)
            {
                if (fields.ContainsKey(element.Key))
                {
                    fields[element.Key].SetValue(element.Value);
                }
            }

            _memoryBuffer.Seek(0, 0);
            var base64 = Convert.ToBase64String((_pdfWriter.GetOutputStream() as MemoryStream).ToArray());
            var size = _memoryBuffer.ToArray().Length;

            Close();
            _memoryBuffer.Dispose();
            _pdfWriter.Dispose();

            return (base64, size);
        }

        public IDictionary<string, PdfFormField> GetAcroFields()
        {
            var fields = PdfAcroForm.GetAcroForm(_pdfDocument, false)?.GetFormFields();

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
