using System;
using System.Collections.Generic;
using System.Linq;
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
                FieldsData = new List<string>()
            };

            _sut.InitDocument(_pdf.Url);
        }

        [Test]
        public void ReceivePdfWithForms_ReturnListOfFields()
        {

            var result = _sut.GetAcroFields();

            foreach (var pair in result)
            {
                Console.WriteLine(pair.Key);
            }

            Assert.AreEqual(23, result.Count);
            Assert.True(result.Count(x => x.Key == "Adress") == 1);
        }
    }
}
