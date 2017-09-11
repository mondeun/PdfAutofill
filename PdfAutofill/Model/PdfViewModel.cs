using System.Collections.Generic;

namespace PdfAutofill.Model
{
    public class PdfViewModel
    {
        public string Url { get; set; }

        public Dictionary<string, string> FieldsData { get; set; }
    }
}
