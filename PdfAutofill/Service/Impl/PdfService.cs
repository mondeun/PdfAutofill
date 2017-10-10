using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfAutofill.Model;

namespace PdfAutofill.Service.Impl
{
    public class PdfService : IPdfService
    {
        public byte[] CreatepdfFromHtml(string html)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();

                        using (var srHtml = new StringReader(html))
                        {
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        }
                        doc.Close();
                    }
                }

                return ms.ToArray();
            }
        }

        public byte[] FillPdf(PdfViewModel model)
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

        public AcroFields GetAcroFields(string url)
        {
            var pdfReader = new PdfReader(url);
            var fields = pdfReader.AcroFields;

            return fields;
        }
    }
}
