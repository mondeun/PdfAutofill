using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf;
using NUnit.Framework;
using PdfAutofill.Model;
using PdfAutofill.Service;
using PdfAutofill.Service.Impl;

namespace PdfAutofill.Tests
{
    [TestFixture]
    public class PdfServiceTests
    {
        private IPdfService _sut;
        private PdfViewModel _pdf;

        [SetUp]
        public void Setup()
        {
            _sut = new PdfService();

            _pdf = new PdfViewModel
            {
                Url = "https://www.lantmateriet.se/globalassets/fastigheter/andra-agare/blanketter-och-information/blanketter/anmalan_fornyelse_av_inskrivning.pdf",
                FieldsData = new Dictionary<string, string>
                {
                    {"Adress", "storgatan 4"},
                    {"Önskas", "It works"},
                    {"Test", "This won't show"}
                }
            };
        }

        [Test]
        public void ReceivePdfWithForms_ReturnListOfFields()
        {
            _sut.InitDocument(_pdf.Url, false);
            var result = _sut.GetAcroFields(_pdf.Url);

            foreach (var pair in result.Fields)
            {
                Console.WriteLine(pair.Key);
            }

            Assert.IsNotNull(result);
            Assert.AreEqual(23, result.Fields.Count);
            Assert.True(result.Fields.Count(x => x.Key == "Adress") == 1);
        }

        [Test]
        public void ReceivePdfWithoutForms_ReturnNoContent()
        {
            _pdf.Url =
                "https://www.jur.lu.se/WEB.nsf/(MenuItemByDocId)/ID872C0E240C0068BBC1257D2E00261EF5/$FILE/Lathund%20PDF.pdf";

            _sut.InitDocument(_pdf.Url, false);

            var result = _sut.GetAcroFields(_pdf.Url);

            Assert.IsEmpty(result.Fields);
        }

        [Test]
        public void WriteToAcroformForm_RetrieveFilledOutPdf()
        {
            _sut.InitDocument(_pdf.Url, true);

            var pdf = _sut.FillPdf(_pdf);
            var reader = new PdfReader(pdf);
            var result = reader.AcroFields;


            foreach (var pair in result.Fields)
            {

                Console.WriteLine($"{pair.Key} : {reader.AcroFields.GetField(pair.Key)}");
            }

            Assert.IsNotNull(result);
            Assert.AreEqual("storgatan 4",
                reader.AcroFields.GetField(result.Fields.Keys.FirstOrDefault(x => x == "Adress")));
        }
    }
}
