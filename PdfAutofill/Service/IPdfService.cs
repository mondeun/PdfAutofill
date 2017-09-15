using iTextSharp.text.pdf;
using PdfAutofill.Model;

namespace PdfAutofill.Service
{
    public interface IPdfService
    {
        byte[] FillPdf(PdfViewModel model);
        AcroFields GetAcroFields(string url);
    }
}