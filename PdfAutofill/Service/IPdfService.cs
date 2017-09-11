using System.Collections.Generic;
using iText.Forms.Fields;
using PdfAutofill.Model;

namespace PdfAutofill.Service
{
    public interface IPdfService
    {
        void InitDocument(string url);
        void InitDocument(PdfViewModel model);
        string FillPdf(PdfViewModel model);
        List<KeyValuePair<string, PdfFormField>> GetAcroFields();
    }
}