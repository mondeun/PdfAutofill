using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using iText.Kernel.Pdf;
using NUnit.Framework;
using PdfAutofill.Model;
using PdfAutofill.Service;

namespace PdfAutofill.Tests
{
    [TestFixture]
    public class PdfServiceTests
    {
        private PdfService _sut;
        private PdfViewModel _pdf;
        private PdfDocument _pdfDocument;

        [SetUp]
        public void Setup()
        {
            _sut = new PdfService();

            _pdf = new PdfViewModel
            {
                Url = "https://www.lantmateriet.se/globalassets/fastigheter/andra-agare/blanketter-och-information/blanketter/anmalan_fornyelse_av_inskrivning.pdf",
                FieldsData = new List<string>()
            };

            byte[] bytes;

            using (var client = new WebClient())
            {
                bytes = client.DownloadData(new Uri(_pdf.Url));
            }

            var memStream = new MemoryStream(bytes);
            var pdfReader = new PdfReader(memStream);

            _pdfDocument = new PdfDocument(pdfReader);
        }

        [Test]
        public void ReceivePdfWithForms_ReturnListOfFields()
        {

            var result = _sut.GetAcroFields(_pdfDocument);

            foreach (var pair in result)
            {
                Console.WriteLine(pair.Key);
            }

            Assert.AreEqual(23, result.Count);
            Assert.True(result.Count(x => x.Key == "Adress") == 1);
        }
    }
}
