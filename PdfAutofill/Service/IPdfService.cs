using System.Collections.Generic;
using System.IO;
using iTextSharp.text.pdf;
using PdfAutofill.Model;

namespace PdfAutofill.Service
{
    public interface IPdfService
    {
        void InitDocument(string url, bool writeMode);
        void InitDocument(PdfViewModel model, bool writeMode);
        byte[] FillPdf(PdfViewModel model);
        AcroFields GetAcroFields(string url);
    }
}