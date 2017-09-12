using System.Collections.Generic;
using iText.Forms.Fields;
using PdfAutofill.Model;

namespace PdfAutofill.Service
{
    public interface IPdfService
    {
        void InitDocument(string url, bool writeMode);
        void InitDocument(PdfViewModel model, bool writeMode);
        (string, int) FillPdf(PdfViewModel model);
        IDictionary<string, PdfFormField> GetAcroFields();
    }
}