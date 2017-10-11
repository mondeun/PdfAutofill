using System.Collections.Generic;

namespace PdfAutofillNet.Models
{
    public class PdfViewModel
    {
        public string Url { get; set; }

        public Dictionary<string, string> FieldsData { get; set; }
    }
}
