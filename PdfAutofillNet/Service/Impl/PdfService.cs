using System;
using System.IO;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PdfAutofillNet.Models;

namespace PdfAutofillNet.Service.Impl
{
    public class PdfService : IPdfService
    {
        public byte[] CreatePdfFromHtml(string html)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document(PageSize.A4, 10f, 10f, 10f, 5f))
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        using (var srHtml = new StringReader(html))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        }
                        doc.Close();
                    }
                }

                return ms.ToArray();
            }
        }

        public byte[] FillPdf(PdfViewModel model)
        {
            try
            {
                var pdfReader = new PdfReader(model.Url);
                using (var memStream = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(pdfReader, memStream))
                    {
                        var form = stamper.AcroFields;

                        foreach (var element in model.FieldsData)
                        {
                            if (form.Fields.ContainsKey(element.Key))
                            {
                                form.SetField(element.Key, element.Value);
                            }
                        }
                    }
                    pdfReader.Close();
                    return memStream.ToArray();
                }
            }
            catch (WebException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public AcroFields GetAcroFields(string url)
        {
            try
            {
                var pdfReader = new PdfReader(url);

                return pdfReader.AcroFields;
            }
            catch (WebException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
