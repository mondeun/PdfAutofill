﻿using iTextSharp.text.pdf;
using PdfAutofillNet.Models;

namespace PdfAutofillNet.Service
{
    public interface IPdfService
    {
        byte[] CreatePdfFromHtml(string html);
        byte[] FillPdf(PdfViewModel model);
        AcroFields GetAcroFields(string url);
    }
}