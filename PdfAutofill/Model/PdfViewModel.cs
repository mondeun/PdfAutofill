using System.Collections.Generic;

namespace PdfAutofill.Model
{
    public class PdfViewModel
    {
        public string Url { get; set; }

        public List<string> FieldsData { get; set; }
    }
}
